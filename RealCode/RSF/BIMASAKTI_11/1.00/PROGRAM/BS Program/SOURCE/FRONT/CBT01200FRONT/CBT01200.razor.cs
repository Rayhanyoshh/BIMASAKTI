﻿using BlazorClientHelper;
using CBT01200Common;
using CBT01200Common.DTOs;
using CBT01200MODEL;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Lookup_GSModel.ViewModel;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBT01200FRONT
{
    public partial class CBT01200 : R_Page
    {
        private CBT01200ViewModel _TransactionListViewModel = new();
        private CBT01210ViewModel _TransactionEntryViewModel = new();
        private R_Conductor _conductorRef;
        private R_Grid<CBT01200DTO> _gridRef;
        private R_Grid<CBT01201DTO> _gridDetailRef;

        [Inject] IClientHelper clientHelper { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _TransactionListViewModel.GetAllUniversalData();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        #region Search
        public async Task OnclickSearch()
        {
            var loEx = new R_Exception();
            try
            {
                if (string.IsNullOrEmpty(_TransactionListViewModel.JournalParam.CDEPT_CODE))
                {
                    loEx.Add(new Exception("Please input keyword to search!"));
                    goto EndBlock;
                }
                if (string.IsNullOrEmpty(_TransactionListViewModel.JournalParam.CSEARCH_TEXT))
                {
                    loEx.Add(new Exception("Please input keyword to search!"));
                    goto EndBlock;
                }
                if (!string.IsNullOrEmpty(_TransactionListViewModel.JournalParam.CSEARCH_TEXT)
                    && _TransactionListViewModel.JournalParam.CSEARCH_TEXT.Length < 3)
                {
                    loEx.Add(new Exception("Minimum search keyword is 3 characters!"));
                    goto EndBlock;
                }
                _TransactionEntryViewModel.JournalDetailGrid.Clear();
                await _gridRef.R_RefreshGrid(null);
                if (_TransactionListViewModel.JournalGrid.Count <= 0)
                {
                    loEx.Add("", "Data Not Found!");
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

        EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
        public async Task OnClickShowAll()
        {
            var loEx = new R_Exception();
            try
            {
                //reset detail
                _TransactionListViewModel.JournalParam.CSEARCH_TEXT = "";
                await _gridRef.R_RefreshGrid(null);
                if (_TransactionListViewModel.JournalGrid.Count <= 0)
                {
                    _TransactionListViewModel.JournalDetailGrid.Clear();
                    loEx.Add("", "Data Not Found!");
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        #endregion

        #region JournalGrid
        private async Task JournalGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _TransactionListViewModel.GetJournalList();
                eventArgs.ListEntityResult = _TransactionListViewModel.JournalGrid;
      
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private async Task JournalGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();

            try
            {
                eventArgs.Result = (CBT01200DTO)eventArgs.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }

        private bool EnableCommit = false;
        private bool EnableApprove = false;


        private async Task JournalGrid_Display(R_DisplayEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                if (eventArgs.ConductorMode == R_eConductorMode.Normal)
                {
                    var loData = (CBT01200DTO)eventArgs.Data;
                    if (!string.IsNullOrWhiteSpace(loData.CREC_ID))
                    {
                        _TransactionEntryViewModel._CREC_ID=loData.CREC_ID;
                        await _gridDetailRef.R_RefreshGrid(null);
                    }

                    EnableCommit = (loData.CSTATUS == "20" || loData.CSTATUS == "80")
                        && int.Parse(loData.CREF_PRD) >= int.Parse(_TransactionListViewModel.VAR_CB_SYSTEM_PARAM.CSOFT_PERIOD);
                    _TransactionListViewModel.CommitLabel = (loData.CSTATUS == "80") ? @_localizer["_btnUndoCommit"] : @_localizer["_btnCommit"];
                    EnableApprove = loData.CSTATUS == "10" && loData.LALLOW_APPROVE == true;
                    if ((loData.CSTATUS == "10") && loData.LALLOW_APPROVE)
                    {
                        EnableApprove = true;
                    }
                    else
                    {
                        EnableApprove = false;
                    }
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        
      
        private async Task ApproveJournalProcess()
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (CBT01200DTO)_conductorRef.R_GetCurrentData();
                if (!loData.LALLOW_APPROVE)
                {
                    loEx.Add("", "You don’t have right to approve this journal!");
                    goto EndBlock;
                }

                var loParam = R_FrontUtility.ConvertObjectToObject<CBT01200UpdateStatusDTO>(loData);
                loParam.LAUTO_COMMIT = _TransactionEntryViewModel.VAR_GL_SYSTEM_PARAM.LCOMMIT_APVJRN;
                loParam.LUNDO_COMMIT = false;
                loParam.CNEW_STATUS = "20";

                await _TransactionEntryViewModel.UpdateJournalStatus(loParam);
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
        private async Task CommitJournalProcess()
        {
            var loEx = new R_Exception();
            R_eMessageBoxResult loValidate;

            try
            {
                var loData = (CBT01200DTO)_conductorRef.R_GetCurrentData();

                if (loData.CSTATUS == "80" )
                {
                    _TransactionListViewModel.CommitLabel = @_localizer["_btnCommit"];
                    if (_TransactionListViewModel.VAR_GSM_TRANSACTION_CODE.LAPPROVAL_FLAG == true)
                    {
                        loData.CSTATUS = "10";
                    }
                    else
                    {
                        loData.CSTATUS = "00";
                    }

                    loValidate = await R_MessageBox.Show("", _localizer["Q01"], R_eMessageBoxButtonType.YesNo);
                    if (loValidate == R_eMessageBoxResult.No)
                        goto EndBlock;
                }
                else
                {
                    loData.CSTATUS = "80";
                    _TransactionListViewModel.CommitLabel = @_localizer["_btnUndoCommit"];
                    loValidate = await R_MessageBox.Show("", "Are you sure want to commit this journal? ", R_eMessageBoxButtonType.YesNo);
                    if (loValidate == R_eMessageBoxResult.No)
                        goto EndBlock;
                }

                var loParam = R_FrontUtility.ConvertObjectToObject<CBT01200UpdateStatusDTO>(loData);
                loParam.LAUTO_COMMIT = _TransactionEntryViewModel.VAR_GL_SYSTEM_PARAM.LCOMMIT_APVJRN;
                loParam.CNEW_STATUS = loData.CSTATUS;

                await _TransactionEntryViewModel.UpdateJournalStatus(loParam);
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region JournalGridDetail
        private async Task JournalGridDetail_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                // var loParam = R_FrontUtility.ConvertObjectToObject<CBT01201DTO>(eventArgs.Parameter);
                await _TransactionEntryViewModel.GetJournalDetailList();
                eventArgs.ListEntityResult = _TransactionEntryViewModel.JournalDetailGrid;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        #endregion

        #region lookupDept
        private async void BeforeOpen_lookupDept(R_BeforeOpenLookupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var param = new GSL00700ParameterDTO
                {
                    CUSER_ID = clientHelper.UserId,
                    CCOMPANY_ID = clientHelper.CompanyId
                };
                eventArgs.Parameter = param;
                eventArgs.TargetPageType = typeof(GSL00700);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private void AfterOpen_lookupDept(R_AfterOpenLookupEventArgs eventArgs)
        {
            var loTempResult = (GSL00700DTO)eventArgs.Result;
            if (loTempResult == null)
            {
                return;
            }

            _TransactionListViewModel.JournalParam.CDEPT_CODE = loTempResult.CDEPT_CODE;
            _TransactionListViewModel.JournalParam.CDEPT_NAME = loTempResult.CDEPT_NAME;
        }
        private async Task OnLostFocus_LookupDept()
        {
            var loEx = new R_Exception();

            try
            {
                LookupGSL00700ViewModel loLookupViewModel = new LookupGSL00700ViewModel(); //use GSL's model
                var loParam = new GSL00700ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CSEARCH_TEXT = _TransactionListViewModel.JournalParam.CDEPT_CODE, // property that bindded to search textbox
                };


                var loResult = await loLookupViewModel.GetDepartment(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                            typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                            "_ErrLookup01"));
                    _TransactionListViewModel.JournalParam.CDEPT_NAME = ""; //kosongin bind textbox name kalo gaada
                    //await GLAccount_TextBox.FocusAsync();
                }
                else
                    _TransactionListViewModel.JournalParam.CDEPT_NAME = loResult.CDEPT_NAME; //assign bind textbox name kalo ada
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        #endregion

        #region Predefine Journal Entry
        private void Predef_JournalEntry(R_InstantiateDockEventArgs eventArgs)
        {
            var loParam = (CBT01200DTO)_conductorRef.R_GetCurrentData();
            eventArgs.TargetPageType = typeof(CBT01210);
            eventArgs.Parameter = loParam;
        }
        private async Task AfterPredef_JournalEntry(R_AfterOpenPredefinedDockEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion
    }
}

