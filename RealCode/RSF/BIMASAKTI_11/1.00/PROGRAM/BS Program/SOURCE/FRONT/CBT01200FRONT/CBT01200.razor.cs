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
        private CBT01200ViewModel _JournalEntryViewModel = new();
        private R_Conductor _conductorRef;
        private R_Grid<CBT01200DTO> _gridRef;
        private R_Grid<CBT01201DTO> _gridDetailRef;

        [Inject] IClientHelper clientHelper { get; set; }

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                await _JournalEntryViewModel.GetAllUniversalData();
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
                if (string.IsNullOrEmpty(_JournalEntryViewModel.JournalParam.CDEPT_CODE))
                {
                    loEx.Add(new Exception("Please input keyword to search!"));
                    goto EndBlock;
                }
                if (string.IsNullOrEmpty(_JournalEntryViewModel.JournalParam.CSEARCH_TEXT))
                {
                    loEx.Add(new Exception("Please input keyword to search!"));
                    goto EndBlock;
                }
                if (!string.IsNullOrEmpty(_JournalEntryViewModel.JournalParam.CSEARCH_TEXT)
                    && _JournalEntryViewModel.JournalParam.CSEARCH_TEXT.Length < 3)
                {
                    loEx.Add(new Exception("Minimum search keyword is 3 characters!"));
                    goto EndBlock;
                }
                _JournalEntryViewModel.JournalDetailGrid.Clear();
                await _gridRef.R_RefreshGrid(null);
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
                _JournalEntryViewModel.JournalDetailGrid.Clear();
                await _gridRef.R_RefreshGrid(null);
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
                await _JournalEntryViewModel.GetJournalList();
                eventArgs.ListEntityResult = _JournalEntryViewModel.JournalGrid;
                if (_JournalEntryViewModel.JournalGrid.Count <= 0)
                {
                    loEx.Add("", "Data Not Found!");
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            R_DisplayException(loEx);
        }
        private void JournalGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            eventArgs.Result = eventArgs.Data;
        }
        private async Task JournalGrid_Display(R_DisplayEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loData = (CBT01200DTO)eventArgs.Data;
                if (eventArgs.ConductorMode == R_eConductorMode.Normal)
                {
                    if (!string.IsNullOrWhiteSpace(loData.CREC_ID))
                    {
                        await _gridDetailRef.R_RefreshGrid(eventArgs.Data);
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
                loParam.LAUTO_COMMIT = _JournalEntryViewModel.VAR_GL_SYSTEM_PARAM.LCOMMIT_APVJRN;
                loParam.LUNDO_COMMIT = false;
                loParam.CNEW_STATUS = "20";

                await _JournalEntryViewModel.UpdateJournalStatus(loParam);
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

                if (loData.CSTATUS == "80")
                {
                    if (_JournalEntryViewModel.VAR_IUNDO_COMMIT_JRN.IOPTION == 3)
                    {
                        loValidate = await R_MessageBox.Show("", "Are you sure want to undo committed this journal? ", R_eMessageBoxButtonType.YesNo);
                        if (loValidate == R_eMessageBoxResult.No)
                            goto EndBlock;
                    }
                }
                else
                {
                    loValidate = await R_MessageBox.Show("", "Are you sure want to commit this journal? ", R_eMessageBoxButtonType.YesNo);
                    if (loValidate == R_eMessageBoxResult.No)
                        goto EndBlock;
                }

                var loParam = R_FrontUtility.ConvertObjectToObject<CBT01200UpdateStatusDTO>(loData);
                loParam.LAUTO_COMMIT = _JournalEntryViewModel.VAR_GL_SYSTEM_PARAM.LCOMMIT_APVJRN;
                loParam.LUNDO_COMMIT = loData.CSTATUS == "80" ? true : false;
                loParam.CNEW_STATUS = loData.CSTATUS == "80" ? "20" : "80";

                await _JournalEntryViewModel.UpdateJournalStatus(loParam);
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
                var loParam = R_FrontUtility.ConvertObjectToObject<CBT01201DTO>(eventArgs.Parameter);
                await _JournalEntryViewModel.GetJournalDetailList(loParam);
                eventArgs.ListEntityResult = _JournalEntryViewModel.JournalDetailGrid;
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

            _JournalEntryViewModel.JournalParam.CDEPT_CODE = loTempResult.CDEPT_CODE;
            _JournalEntryViewModel.JournalParam.CDEPT_NAME = loTempResult.CDEPT_NAME;
        }
        private async Task OnLostFocus_LookupDept()
        {
            var loEx = new R_Exception();

            try
            {
                LookupGSL00700ViewModel loLookupViewModel = new LookupGSL00700ViewModel(); //use GSL's model
                var loParam = new GSL00700ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CSEARCH_TEXT = _JournalEntryViewModel.JournalParam.CDEPT_CODE, // property that bindded to search textbox
                };


                var loResult = await loLookupViewModel.GetDepartment(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                            typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                            "_ErrLookup01"));
                    _JournalEntryViewModel.JournalParam.CDEPT_NAME = ""; //kosongin bind textbox name kalo gaada
                    //await GLAccount_TextBox.FocusAsync();
                }
                else
                    _JournalEntryViewModel.JournalParam.CDEPT_NAME = loResult.CDEPT_NAME; //assign bind textbox name kalo ada
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
            var loData = _conductorRef.R_GetCurrentData();
            //eventArgs.TargetPageType = typeof(GLT00110);
            eventArgs.Parameter = loData;
        }
        private void AfterPredef_JournalEntry(R_AfterOpenPredefinedDockEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                //await _gridRef.R_RefreshGrid(null);
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

