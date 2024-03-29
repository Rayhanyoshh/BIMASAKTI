﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GLT00600Common.DTOs;
using GLT00600Model;
using GLT00600Model.ViewModel;
using GLTR00100COMMON;
using GLTR00100FRONT;
using GLTR00100MODEL;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Lookup_GSModel.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace GLT00600Front
{
    public partial class GLT00600JournalEntry : R_Page
    {
        private GLT00600ViewModel _JournalListViewModel = new();
        private R_Conductor _conductorRef;
        private R_Grid<GLT00600JournalGridDTO> _gridRef;
        private R_Grid<GLT00600JournalGridDetailDTO> _gridDetailRef;
        private R_ConductorGrid _conductorGridDetailRef;

        [Inject] IClientHelper clientHelper { get; set; }

        private R_TextBox loCrefNo;

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _JournalListViewModel.GetGsmCompany();
                await _JournalListViewModel.GetSystemParam();
                await _JournalListViewModel.GetCurrencyList();
                await _JournalListViewModel.GetStatusList();
                await _JournalListViewModel.GetCenterList();

                _JournalListViewModel.lcLocalCurrency = _JournalListViewModel.CompanyCollection.CLOCAL_CURRENCY_CODE;
                _JournalListViewModel.lcBaseCurrency = _JournalListViewModel.CompanyCollection.CBASE_CURRENCY_CODE;
                VAR_GL_SYSTEM_PARAMDTO systemparamData = new VAR_GL_SYSTEM_PARAMDTO()
                {
                    CSTART_PERIOD = _JournalListViewModel.CurrentPeriodStartCollection.CSTART_DATE,
                    CCURRENT_PERIOD_MM = _JournalListViewModel.SystemParamCollection.CCURRENT_PERIOD_MM,
                    CCURRENT_PERIOD_YY = _JournalListViewModel.SystemParamCollection.CSTART_PERIOD_YY,
                    CSOFT_PERIOD_MM = _JournalListViewModel.SystemParamCollection.CSOFT_PERIOD_MM,
                    CSOFT_PERIOD_YY = _JournalListViewModel.SystemParamCollection.CSOFT_PERIOD_YY
                };

                await _JournalListViewModel.GetCurrentPeriodStart(systemparamData);
                _JournalListViewModel.RefreshCurrencyRate();

                _JournalListViewModel.Data.CCREATE_BY = _JournalListViewModel.Data.CUPDATE_BY = clientHelper.UserId;
                _JournalListViewModel.Data.DCREATE_DATE = _JournalListViewModel.Data.DUPDATE_DATE = DateTime.Now;

                GLT00600JournalGridDTO pcParam = (GLT00600JournalGridDTO)poParameter;

                if (pcParam.CREC_ID != null)
                {
                    _JournalListViewModel.Journal = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(poParameter);
                    _JournalListViewModel.JournalEntity.CREC_ID = _JournalListViewModel.Journal.CREC_ID;
                    _JournalListViewModel.LcCrecID = _JournalListViewModel.Journal.CREC_ID;
                    _JournalListViewModel.GetJournalDetailList();
                    _conductorRef.R_GetEntity(_JournalListViewModel.Journal.CREC_ID);

                    _JournalListViewModel.EnableDept = _JournalListViewModel.EnableDelete =
                        _JournalListViewModel.EnableEdit = _JournalListViewModel.EnablePrint =
                            _JournalListViewModel.EnableCopy = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Form
        private async Task JournalForm_GetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _JournalListViewModel.GetJournal(_JournalListViewModel.Journal);
                eventArgs.Result = _JournalListViewModel.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task JournalForm_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(eventArgs.Data);
                var loDetailListData = _gridDetailRef.DataSource.ToList();
                loParam.DetailList = new List<GLT00600JournalGridDetailDTO>(loDetailListData);

                await _JournalListViewModel.SaveJournal(loParam, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _JournalListViewModel.Journal;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalForm_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var data = (GLT00600DTO)eventArgs.Data;

            data.CDEPT_CODE = _JournalListViewModel.lcDeptCode;
            data.CDEPT_NAME = _JournalListViewModel.lcDeptName;

            _JournalListViewModel.allStatusData = _JournalListViewModel.allStatusData
                .Where(item => !string.IsNullOrEmpty(item.CCODE))
                .ToList();

            _JournalListViewModel.Drefdate = _JournalListViewModel.Ddocdate = DateTime.Now;
            data.CCREATE_BY = data.CUPDATE_BY = clientHelper.UserId;
            data.CUPDATE_DATE = data.CCREATE_DATE = DateTime.Now.ToLongDateString();
            await _JournalListViewModel.RefreshCurrencyRate();
            data.CLOCAL_CURRENCY_CODE = _JournalListViewModel.lcLocalCurrency;
            data.CBASE_CURRENCY_CODE = _JournalListViewModel.lcBaseCurrency;
            _JournalListViewModel.JournaDetailListTemp = _JournalListViewModel.JournaDetailList;
            _JournalListViewModel.JournaDetailList = new();

            if (_JournalListViewModel.allStatusData != null)
                data.CSTATUS = _JournalListViewModel.allStatusData.FirstOrDefault()?.CCODE;

            if (_JournalListViewModel.currencyData != null)
                data.CCURRENCY_CODE = _JournalListViewModel.currencyData.FirstOrDefault()?.CCURRENCY_CODE;

            data.CCREATE_BY = data.CUPDATE_BY = clientHelper.UserId;
            data.DCREATE_DATE = data.DUPDATE_DATE = DateTime.Now;
        }
        public async Task JournalForm_RSaving(R_SavingEventArgs eventArgs)
        {
            var loParam = (GLT00600DTO)eventArgs.Data;
            var loEx = new R_Exception();

            try
            {
                ((GLT00600DTO)eventArgs.Data).CREF_DATE = _JournalListViewModel.Drefdate.ToString("yyyyMMdd");
                ((GLT00600DTO)eventArgs.Data).CDOC_DATE = _JournalListViewModel.Ddocdate.ToString("yyyyMMdd");
                //await _JournalListViewModel.RefreshCurrencyRate();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private void ValidationFormGLT00600JournalEntry(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loParam = (GLT00600DTO)eventArgs.Data;
                if (eventArgs.ConductorMode != R_eConductorMode.Normal)
                {
                    if (loParam.CREF_NO == null && _JournalListViewModel.TransactionCodeCollection.LINCREMENT_FLAG == false)
                    {
                        loEx.Add("", @_localizer["_validationReferenceRequired"]);
                    }

                    if (_JournalListViewModel.Drefdate < DateTime.ParseExact(_JournalListViewModel.CurrentPeriodStartCollection.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture))
                    {
                        loEx.Add("", @_localizer["_validationReferenceDateCannot"]);
                    }



                    if (_JournalListViewModel.Ddocdate != null && _JournalListViewModel.Ddocdate < DateTime.ParseExact(_JournalListViewModel.CurrentPeriodStartCollection.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture))
                    {
                        loEx.Add("", @_localizer["_validationDocumentDateCannot"]);
                    }

                    if (loParam.CDOC_NO == null && _JournalListViewModel.Ddocdate != null)
                    {
                        loEx.Add("", @_localizer["_validationInputDocumentNo"]);
                    }

                    if (string.IsNullOrEmpty(loParam.CDEPT_CODE))
                    {
                        loEx.Add("", @_localizer["_validationDeptMandatory"]);
                    }

                    if (string.IsNullOrEmpty(loParam.CTRANS_DESC))
                    {
                        loEx.Add("", @_localizer["_validationDescMandatory"]);
                    }
                    if ((loParam.NDEBIT_AMOUNT > 0 || loParam.NCREDIT_AMOUNT > 0) && loParam.NDEBIT_AMOUNT != loParam.NCREDIT_AMOUNT)
                    {
                        loEx.Add("", @_localizer["_validationTotalDebitTotalAmount"]);
                    }

                    if (loParam.NPRELIST_AMOUNT > 0 && loParam.NPRELIST_AMOUNT != loParam.NDEBIT_AMOUNT)
                    {
                        loEx.Add("", @_localizer["_validationJournalAmount"]);
                    }

                    if (loParam.NLBASE_RATE <= 0)
                    {
                        loEx.Add("", @_localizer["_validationLocalCurrencyBase"]);
                    }

                    if (loParam.NLCURRENCY_RATE <= 0)
                    {
                        loEx.Add("", @_localizer["_validationLocalCurrencyRate"]);
                    }

                    if (loParam.NBBASE_RATE <= 0)
                    {
                        loEx.Add("", @_localizer["_validationBaseCurrencyBase"]);
                    }

                    if (loParam.NBCURRENCY_RATE <= 0)
                    {
                        loEx.Add("", @_localizer["_validationBaseCurrencyRate"]);
                    }

                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task JournalForm_RDisplay(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            var data = (GLT00600DTO)eventArgs.Data;
            try
            {
                if (eventArgs.ConductorMode == R_eConductorMode.Normal)
                {
                    DateTime? ParseDate(string dateStr)
                    {
                        if (dateStr != null && DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                            return parsedDate;
                        return null;
                    }
                    _JournalListViewModel.Drefdate = ParseDate(data.CREF_DATE) ?? DateTime.MinValue;
                    _JournalListViewModel.Ddocdate = ParseDate(data.CDOC_DATE) ?? DateTime.MinValue;

                    if (data.CSTATUS == "00" || data.CSTATUS == "10")
                    {
                        _JournalListViewModel.EnableSubmit = true;
                        if (data.CSTATUS == "10")
                        {
                            _JournalListViewModel.SubmitLabel = @_localizer["_btnUndoSubmit"];
                        }
                        else
                        {
                            _JournalListViewModel.SubmitLabel = @_localizer["_btnSubmit"];
                        }
                    }

                    if (data.CSTATUS == "10" && _JournalListViewModel.TransactionCodeCollection.LAPPROVAL_FLAG == true)
                    {
                        _JournalListViewModel.EnableApprove = true;
                    }

                    data.IPERIOD = int.Parse(_JournalListViewModel.CSOFT_PERIOD_YY);
                    if ((data.CSTATUS == "20" || data.CSTATUS == "10" && _JournalListViewModel.TransactionCodeCollection.LAPPROVAL_FLAG == false) || (data.CSTATUS == "80" && _JournalListViewModel.IundoCollection.IOPTION != 1) && data.IPERIOD >= _JournalListViewModel.SystemParamCollection.ISOFT_PERIOD_YY)
                    {
                        _JournalListViewModel.EnableCommit = true;
                    }
                    _JournalListViewModel.CommitLabel = (data.CSTATUS == "80") ? @_localizer["_btnUndoCommit"] : _JournalListViewModel.CommitLabel;
                    _JournalListViewModel.EnableCommit = (data.CSTATUS == "80");

                    if (string.IsNullOrWhiteSpace(_JournalListViewModel.Journal.CREC_ID))
                    {
                        _JournalListViewModel.EnablePrint = _JournalListViewModel.EnableDelete =
                            _JournalListViewModel.EnableEdit = _JournalListViewModel.EnableCopy =
                                _JournalListViewModel.EnableSubmit = _JournalListViewModel.EnableDept = false;
                    }
                    else
                    {
                        _JournalListViewModel.EnableDept = false;
                        _JournalListViewModel.EnableDelete = _JournalListViewModel.EnableCopy =
                            _JournalListViewModel.EnablePrint = _JournalListViewModel.EnableSubmit =
                                _JournalListViewModel.EnableEdit = true;
                    }
                }

                if (eventArgs.ConductorMode != R_eConductorMode.Normal)
                {
                    await _JournalListViewModel.RefreshCurrencyRate();
                    _JournalListViewModel.EnableDept = false;
                    _JournalListViewModel.EnableDelete = _JournalListViewModel.EnableCopy = _JournalListViewModel.EnablePrint = _JournalListViewModel.EnableSubmit = false;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
           
          
        }

        private async Task JournalForm_BeforeCancel(R_BeforeCancelEventArgs eventArgs)
        {
            var res = await R_MessageBox.Show("", @_localizer["_validationYouHaventSave"],
                R_eMessageBoxButtonType.YesNo);
            if (res == R_eMessageBoxResult.No)
            {
                eventArgs.Cancel = true;
            }
            else
            {
                _JournalListViewModel.JournaDetailList.Clear();
                await _JournalListViewModel.GetJournalDetailList();
                _JournalListViewModel.EnableDelete = false;
                await Close(false, false);
            }
        }

        private void JournalForm_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
            //_enableCrudJournalDetail = true;
        }

        private async Task CopyJournalEntryProcess()
        {
            var loEx = new R_Exception();
            try
            {
                var entity = _JournalListViewModel.Journal; ;
                DateTime? ParseDate(string dateStr)
                {
                    if (dateStr != null && DateTime.TryParseExact(dateStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
                        return parsedDate;
                    return null;
                }

                await _conductorRef.Add();
                if (_conductorRef.R_ConductorMode == R_eConductorMode.Add)
                {
                    var Data = (GLT00600DTO)_conductorRef.R_GetCurrentData();

                    Data.CDEPT_CODE = entity.CDEPT_CODE;
                    Data.CDEPT_NAME = entity.CDEPT_NAME;
                    Data.CDOC_NO = entity.CDOC_NO;
                    Data.CREF_DATE = entity.CREF_DATE;
                    Data.CDOC_DATE = entity.CDOC_DATE;
                    Data.CTRANS_DESC = entity.CTRANS_DESC;
                    Data.CSTATUS = entity.CSTATUS;
                    Data.CCURRENCY_CODE = entity.CCURRENCY_CODE;
                    Data.NPRELIST_AMOUNT = entity.NPRELIST_AMOUNT;
                    Data.NDEBIT_AMOUNT = entity.NDEBIT_AMOUNT;
                    Data.NCREDIT_AMOUNT = entity.NCREDIT_AMOUNT;
                    Data.DetailList = _JournalListViewModel.JournaDetailListTemp.ToList();
                    _JournalListViewModel.JournaDetailList = _JournalListViewModel.JournaDetailListTemp;

                    _JournalListViewModel.Drefdate = ParseDate(Data.CREF_DATE) ?? DateTime.MinValue;
                    _JournalListViewModel.Ddocdate = ParseDate(Data.CDOC_DATE) ?? DateTime.MinValue;
                    loCrefNo.FocusAsync();

                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task OnChanged_CurrencyCodeAsync()
        {
            var loEx = new R_Exception();
            try
            {
                await _JournalListViewModel.RefreshCurrencyRate();
                if (_JournalListViewModel.Data.CCURRENCY_CODE != _JournalListViewModel.CompanyCollection.CLOCAL_CURRENCY_CODE && _JournalListViewModel.Data.LFIX_RATE == true)
                {
                    _JournalListViewModel.EnableNLBASE_RATE = true;
                    _JournalListViewModel.EnableNLCURRENCY_RATE = true;
                }
                else
                {
                    _JournalListViewModel.EnableNLBASE_RATE = false;
                    _JournalListViewModel.EnableNLCURRENCY_RATE = false;
                }
                if (_JournalListViewModel.Journal.CCURRENCY_CODE != _JournalListViewModel.CompanyCollection.CBASE_CURRENCY_CODE && _JournalListViewModel.Data.LFIX_RATE == true)
                {
                    _JournalListViewModel.EnableNBBASE_RATE = true;
                    _JournalListViewModel.EnableNBCURRENCY_RATE = true;
                }
                else
                {
                    _JournalListViewModel.EnableNBBASE_RATE = false;
                    _JournalListViewModel.EnableNBCURRENCY_RATE = false;
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region Detail
        private async Task JournalDet_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {

                await _JournalListViewModel.GetJournalDetailList();
                eventArgs.ListEntityResult = _JournalListViewModel.JournaDetailList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private async Task JournalDet_RDisplay(R_DisplayEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var data = (GLT00600JournalGridDetailDTO)eventArgs.Data;
                var loHeaderData = _JournalListViewModel.Journal;
                if (data != null)
                {
                    if (_JournalListViewModel.CenterListData != null)
                    {
                        var firstCenter = _JournalListViewModel.CenterListData.FirstOrDefault();
                        data.CCENTER_CODE = firstCenter.CCENTER_CODE;
                    }
                    data.CDBCR = data.NDEBIT > 0 ? "D" : "C";
                    data.NAMOUNT = data.NDEBIT + data.NCREDIT;

                    data.CDOCUMENT_DATE = _JournalListViewModel.Ddocdate.ToString("yyyyMMdd");
                    data.CDOCUMENT_NO = _JournalListViewModel.Data.CDOC_NO;
                    if (eventArgs.ConductorMode == R_eConductorMode.Normal)
                    {
                        if (_gridDetailRef.DataSource.Count > 0)
                        {
                            loHeaderData.NDEBIT_AMOUNT = _gridDetailRef.DataSource.Sum(x => x.NDEBIT);
                            loHeaderData.NCREDIT_AMOUNT = _gridDetailRef.DataSource.Sum(x => x.NCREDIT);
                        }
                    }
                    loHeaderData.NTRANS_AMOUNT = loHeaderData.NDEBIT_AMOUNT + loHeaderData.NCREDIT_AMOUNT;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void JournalDet_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = eventArgs.Data;
        }
        private void JournalDet_Validation(R_ValidationEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var data = (GLT00600JournalGridDetailDTO)eventArgs.Data;
                if (string.IsNullOrWhiteSpace(data.CGLACCOUNT_NO))
                {
                    loEx.Add("", @_localizer["_validationAccountNoReq"]);
                }

                if (string.IsNullOrWhiteSpace(data.CCENTER_CODE) && (data.CBSIS == 'B' && _JournalListViewModel.CompanyCollection.LENABLE_CENTER_BS == true) || (data.CBSIS == 'I' && _JournalListViewModel.CompanyCollection.LENABLE_CENTER_IS == true))
                {
                    loEx.Add("", $"{@_localizer["_validationCenterCode"]} {data.CGLACCOUNT_NO}!");
                }

                if (data.NDEBIT == 0 && data.NCREDIT == 0)
                {
                    loEx.Add("", @_localizer["_validationJournalAmountCannotZero"]);
                }

                if (data.NDEBIT > 0 && data.NCREDIT > 0)
                {
                    loEx.Add("", @_localizer["_validationJournalAmountDebitCredit"]);
                }
                if (eventArgs.ConductorMode == R_eConductorMode.Add || eventArgs.ConductorMode == R_eConductorMode.Edit)
                {
                    if (_JournalListViewModel.JournaDetailList.Any(item => item.CGLACCOUNT_NO == data.CGLACCOUNT_NO))
                    {
                        loEx.Add("", $"{@_localizer["_validationAccountNo"]} {data.CGLACCOUNT_NO} {@_localizer["_validationAlreadyExist"]}");
                    }
                }
             
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void JournalDet_BeforeAdd(R_BeforeAddEventArgs eventArgs)
        {
            _JournalListViewModel.EnableDept = false;
   
        }

        private void JournalDet_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
            var data = (GLT00600JournalGridDetailDTO)eventArgs.Data;

            if (_JournalListViewModel.CenterListData != null)
            {
                //var firstCenter = _JournalListViewModel.CenterListData.FirstOrDefault();
                //data.CCENTER_CODE = firstCenter.CCENTER_CODE;
            }

            if (_JournalListViewModel.JournaDetailList.Any())
            {
                // Find the maximum INO value in the list and increment it by 1
                int maxINO = _JournalListViewModel.JournaDetailList.Max(item => item.INO);
                data.INO = maxINO + 1;
            }
            else
            {
                // If the list is empty, set INO to 1 (or another initial value)
                data.INO = 1;
            }

            eventArgs.Data = data;
        }
        private void Before_Open_Lookup(R_BeforeOpenGridLookupColumnEventArgs eventArgs)
        {
            var param = new GSL00500ParameterDTO
            {
                CCOMPANY_ID = clientHelper.CompanyId,
                CPROGRAM_CODE = "GLM00100",
                CUSER_ID = clientHelper.UserId,
                CUSER_LANGUAGE = clientHelper.CultureUI.TwoLetterISOLanguageName,
                CBSIS = "",
                CDBCR = "",
                CCENTER_CODE = "",
                CPROPERTY_ID = "",
                LCENTER_RESTR = false,
                LUSER_RESTR = false
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(GSL00500);
        }

        private void After_Open_Lookup(R_AfterOpenGridLookupColumnEventArgs eventArgs)
        {
            var loTempResult = (GSL00500DTO)eventArgs.Result;
            if (loTempResult == null)
                return;
            var loGetData = (GLT00600JournalGridDetailDTO)eventArgs.ColumnData;
            loGetData.CGLACCOUNT_NO = loTempResult.CGLACCOUNT_NO;
            loGetData.CGLACCOUNT_NAME = loTempResult.CGLACCOUNT_NAME;
            loGetData.CBSIS = loTempResult.CBSIS_DESCR.Contains("B") ? 'B' : (loTempResult.CBSIS_DESCR.Contains("I") ? 'I' : default(char));

        }

        private async Task OnLostFocus_LookupDept()
        {
            var loEx = new R_Exception();

            try
            {
                LookupGSL00700ViewModel loLookupViewModel = new LookupGSL00700ViewModel(); //use GSL's model
                var loParam = new GSL00700ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CSEARCH_TEXT = _JournalListViewModel.Parameter.CDEPT_CODE, // property that bindded to search textbox
                };


                var loResult = await loLookupViewModel.GetDepartment(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                            typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                            "_ErrLookup01"));
                    _JournalListViewModel.Parameter.CDEPT_NAME = ""; //kosongin bind textbox name kalo gaada
                                                                     //await GLAccount_TextBox.FocusAsync();
                }
                else
                    _JournalListViewModel.Parameter.CDEPT_NAME = loResult.CDEPT_NAME; //assign bind textbox name kalo ada
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        #endregion

        #region Process
        private async Task ApproveJournalProcess()
        {
            var loEx = new R_Exception();
            try
            {
                if (_JournalListViewModel.Journal.LALLOW_APPROVE == false)
                {
                    R_MessageBox.Show("", @_localizer["_validationAllowApprove"], R_eMessageBoxButtonType.OK);
                    goto EndBlock;
                }

                await _JournalListViewModel.ApproveJournal(_JournalListViewModel.JournalEntity);
                await _JournalListViewModel.GetJournal(new GLT00600DTO() { CREC_ID = _JournalListViewModel.JournalEntity.CREC_ID });
                if (_JournalListViewModel.Journal.CSTATUS == "20")
                {
                    R_MessageBox.Show("", @_localizer["_validationSuccesApprove"], R_eMessageBoxButtonType.OK);
                }
                var param = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(_JournalListViewModel.JournalEntity);
                await _JournalListViewModel.GetJournal(param);
                _JournalListViewModel.EnableDelete = true;
                _JournalListViewModel.SubmitLabel = @_localizer["_btnSubmit"];
            EndBlock:;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task CommitJournalProcess()
        {
            var loEx = new R_Exception();
            try
            {
                GLT00600JournalGridDTO lcdata = new GLT00600JournalGridDTO()
                {
                    CDEPT_CODE = _JournalListViewModel.Journal.CDEPT_CODE,
                    CREF_NO = _JournalListViewModel.Journal.CREF_NO,
                    CREC_ID = _JournalListViewModel.Journal.CREC_ID,
                    CSTATUS = _JournalListViewModel.Journal.CSTATUS,
                    LCOMMIT_APRJRN = _JournalListViewModel.Journal.LCOMMIT_APRJRN,
                    LUNDO_COMMIT = _JournalListViewModel.Journal.LUNDO_COMMIT
                };
                if (_JournalListViewModel.Journal.CSTATUS == "80" && _JournalListViewModel.IundoCollection.IOPTION == 2)
                {
                    var result = await R_MessageBox.Show("", @_localizer["_validationUndoCommit"], R_eMessageBoxButtonType.YesNo);
                    await _JournalListViewModel.UndoReversingJournal(lcdata);
                    if (result == R_eMessageBoxResult.Yes)
                    {
                        goto commit;
                    }
                    goto EndBlock;
                }
                else
                {
                    var result = await R_MessageBox.Show("", @_localizer["_validationCommit"], R_eMessageBoxButtonType.YesNo);
                    if (result == R_eMessageBoxResult.Yes)
                    {
                        goto commit;
                    }
                    else
                    {
                        goto EndBlock;
                    }
                }
            commit:;
                await _JournalListViewModel.CommitJournal(lcdata);
                await _JournalListViewModel.GetJournal(new GLT00600DTO()
                {
                    CREC_ID = _JournalListViewModel.Journal.CREC_ID,
                });
                if (_JournalListViewModel.CSTATUS_TEMP == "80")
                {
                    R_MessageBox.Show("", @_localizer["_validationSuccesUndoCommit"], R_eMessageBoxButtonType.OK);
                    _JournalListViewModel.CommitLabel = @_localizer["_btnCommit"];
                }
                else
                {
                    R_MessageBox.Show("", @_localizer["_validationSuccesCommit"], R_eMessageBoxButtonType.OK);
                    _JournalListViewModel.CommitLabel = @_localizer["_btnUndoCommit"];
                }
                var param = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(_JournalListViewModel.JournalEntity);
                await _JournalListViewModel.GetJournal(param);
            EndBlock:;
                _JournalListViewModel.EnableDelete = true;
                _JournalListViewModel.EnableSubmit = false;
                _JournalListViewModel.SubmitLabel = @_localizer["_btnSubmit"];
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task SubmitJournalProcess()
        {
            var loEx = new R_Exception();
            try
            {
                if (_JournalListViewModel.Journal.CSTATUS == "10")
                {
                    var result = await R_MessageBox.Show("", @_localizer["_validationUndoSubmit"],
                        R_eMessageBoxButtonType.YesNo);
                    if (result == R_eMessageBoxResult.Yes)
                    {
                        _JournalListViewModel.SubmitLabel = @_localizer["_btnSubmit"];
                        goto Submit;
                    }

                }
                var res = await R_MessageBox.Show("", @_localizer["_validationSubmit"],
                    R_eMessageBoxButtonType.YesNo);
                if (res == R_eMessageBoxResult.Yes)
                {
                    _JournalListViewModel.SubmitLabel = @_localizer["_btnUndoSubmit"];
                    goto Submit;

                }

                goto End;
            Submit:;
                await _JournalListViewModel.SubmitJournal(_JournalListViewModel.JournalEntity);
                var param = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(_JournalListViewModel.JournalEntity);
                await _JournalListViewModel.GetJournal(param);
            End:;
                _JournalListViewModel.EnableDelete = true;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task DeleteJournalProcess()
        {
            var loEx = new R_Exception();
            try
            {
                var result = await R_MessageBox.Show("", @_localizer["_validationDelete"],
                    R_eMessageBoxButtonType.YesNo);
                if (result == R_eMessageBoxResult.Yes)
                {
                    goto Delete;
                }

                goto EndBlock;
            Delete:;
                await _JournalListViewModel.DeleteJournal(_JournalListViewModel.JournalEntity);
                var param = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(_JournalListViewModel.JournalEntity);
                await _JournalListViewModel.GetJournal(param);
            EndBlock:;
                _JournalListViewModel.EnableDelete = false;
                _JournalListViewModel.SubmitLabel = @_localizer["_btnSubmit"];
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region Print

        private R_Lookup R_LookupBtnPrint;
        private async Task Before_Open_lookupPrint(R_BeforeOpenLookupEventArgs eventArgs)
        {
            var param = new GLTR00100DTO()
            {
                CREC_ID = _JournalListViewModel.Journal.CREC_ID
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(GLTR00100);
        }

        private void After_Open_lookupPrint(R_AfterOpenLookupEventArgs eventArgs)
        {
            var loTempResult = (GLTR00100)eventArgs.Result;
            if (loTempResult == null)
            {
                return;
            }
        }
        #endregion
        protected override Task<object> R_Set_Result_PredefinedDock()
        {
            var lcResult = _JournalListViewModel.Journal;

            return Task.FromResult<object>(lcResult);
        }
        #region lookupDept

        private R_Lookup R_LookupBtnDept;

        private async Task Before_Open_lookupDept(R_BeforeOpenLookupEventArgs eventArgs)
        {
            var param = new GSL00700ParameterDTO
            {
                CUSER_ID = clientHelper.UserId,
                CCOMPANY_ID = clientHelper.CompanyId
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(GSL00700);
        }

        private void After_Open_lookupDept(R_AfterOpenLookupEventArgs eventArgs)
        {
            var loTempResult = (GSL00700DTO)eventArgs.Result;
            if (loTempResult == null)
            {
                return;
            }

            _JournalListViewModel.Data.CDEPT_CODE = loTempResult.CDEPT_CODE;
            _JournalListViewModel.Data.CDEPT_NAME = loTempResult.CDEPT_NAME;
        }

        #endregion
        //#region Onchange Value

        //private async Task RefreshLastCurrency()
        //{
        //    var loEx = new R_Exception();
        //    try
        //    {
        //        var loData = (GLT00600JournalGridDetailDTO)_conductorRef.R_GetCurrentData();
        //        //var loParam = R_FrontUtility.ConvertObjectToObject<GLT00110LastCurrencyRateDTO>(loData);
        //        var loResult = await _JournalListViewModel.GetLastCurrency(loParam);

        //        if (loResult is null)
        //        {
        //            loData.NLBASE_RATE = 1;
        //            loData.NLCURRENCY_RATE = 1;
        //            loData.NBBASE_RATE = 1;
        //            loData.NBCURRENCY_RATE = 1;
        //        }
        //        else
        //        {
        //            loData.NLBASE_RATE = loResult.NLBASE_RATE_AMOUNT;
        //            loData.NLCURRENCY_RATE = loResult.NLCURRENCY_RATE_AMOUNT;
        //            loData.NBBASE_RATE = loResult.NBBASE_RATE_AMOUNT;
        //            loData.NBCURRENCY_RATE = loResult.NBCURRENCY_RATE_AMOUNT;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }

        //    loEx.ThrowExceptionIfErrors();
        //}

        //private async Task RefDate_OnChange(DateTime poParam)
        //{
        //    var loEx = new R_Exception();
        //    try
        //    {
        //        _JournalEntryViewModel.RefDate = poParam;
        //        if (_JournalEntryViewModel.Data.CCURRENCY_CODE != _JournalEntryViewModel.VAR_GSM_COMPANY.CLOCAL_CURRENCY_CODE
        //            || _JournalEntryViewModel.Data.CCURRENCY_CODE != _JournalEntryViewModel.VAR_GSM_COMPANY.CBASE_CURRENCY_CODE)
        //        {
        //            await RefreshLastCurrency();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }

        //    loEx.ThrowExceptionIfErrors();
        //}

        //private async Task Currency_OnChange(string poParam)
        //{
        //    var loEx = new R_Exception();
        //    try
        //    {
        //        _JournalEntryViewModel.Data.CCURRENCY_CODE = poParam;
        //        await RefreshLastCurrency();
        //    }
        //    catch (Exception ex)
        //    {
        //        loEx.Add(ex);
        //    }

        //    loEx.ThrowExceptionIfErrors();
        //}

        //#endregion
    }
}
