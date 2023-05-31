using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GSM01000Common;
using GSM01000Common.DTOs;
using GSM01000Model.ViewModel;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using R_BlazorFrontEnd.Helpers;


namespace GSM01000Front
{
    public partial class GSM01000 : R_Page
    {
        private GSM01000ViewModel _GSM01000ViewModel = new GSM01000ViewModel();
        private R_ConductorGrid _conductorGridRef;
        private R_Grid<GSM01000DTO> _gridRef;

        private GSM01010ViewModel _GSM01010ViewModel = new GSM01010ViewModel();
        private R_ConductorGrid _conductorGoARef;
        private R_Grid<GSM01010DTO> _gridGoARef;

        private string loLabel = "";

        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] private R_ContextHeader _contextHeader { get; set; }


        protected override async Task R_Init_From_Master(object poParam)
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

            R_DisplayException(loEx);
        }

        #region COA
        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01000ViewModel.GetGridList();
                arg.ListEntityResult = _GSM01000ViewModel.loGridList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task Grid_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM01000DTO)eventArgs.Data;
                var loHeadGridGOA = (GSM01010DTO)_conductorGoARef.R_GetCurrentData();
                loHeadGridGOA.CGLACCOUNT_NO = loParam.CGLACCOUNT_NO;
                loHeadGridGOA.CGLACCOUNT_NAME = loParam.CGLACCOUNT_NAME;

                _GSM01000ViewModel.SelectedActiveInactiveCenterCode = loParam.CGLACCOUNT_NO;
                _GSM01000ViewModel.SelectedActiveInactiveLACTIVE = loParam.LACTIVE;
                if (loParam.LACTIVE)
                {
                    loLabel = "Inactive";
                    _GSM01000ViewModel.SelectedActiveInactiveLACTIVE = false;
                }
                else
                {
                    loLabel = "Activate";
                    _GSM01000ViewModel.SelectedActiveInactiveLACTIVE = true;
                }

                await _gridGoARef.R_RefreshGrid(loParam);
            }
        }

        private async Task Grid_R_ServiceGetGridCoA(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                // var lcParam = ((GSM01000DTO)eventArgs.Data).CGLACCOUNT_NO;
                // _contextHeader.R_Context.R_SetStreamingContext(ContextConstant.CGL_ACCOUNT, lcParam);

                var loParam = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(eventArgs.Data);
                await _GSM01000ViewModel.GetCoASingle(loParam);

                eventArgs.Result = _GSM01000ViewModel.loEntity;


            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Conductor_Validation(R_ValidationEventArgs arg)
        {
            var loEx = new R_Exception();
            try
            {
                // nanti kerjakan resources nya
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            arg.Cancel = loEx.HasError;
            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01000ViewModel.SaveEntity((GSM01000DTO)eventArgs.Data, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _GSM01000ViewModel.loEntity;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (GSM01000DTO)eventArgs.Data;
                await _GSM01000ViewModel.DeleteEntity(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs arg)
        {
            arg.GridData = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(arg.Data);
        }
        #endregion


        #region GOA
        private async Task GridGoA_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loCGLParam = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(eventArgs.Parameter);
                _GSM01010ViewModel._cglAccountNo = loCGLParam.CGLACCOUNT_NO;
                await _GSM01010ViewModel.GetGOAListbyGL();
                eventArgs.ListEntityResult = _GSM01010ViewModel.loGridGoAList;

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        #endregion

        private void R_Before_Open_Popup_CopyFrom(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(CopyFromModal);
        }

        private async Task R_After_Open_Popup_CopyFrom(R_AfterOpenPopupEventArgs eventArgs)
        {
            await _gridRef.R_RefreshGrid(null);

        }
        private void R_Before_Open_Popup_ActivateInactive(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.Parameter = "GSM01501";
            eventArgs.TargetPageType = typeof(GFF00900FRONT.GFF00900);
        }

        private async Task R_After_Open_Popup_ActivateInactive(R_AfterOpenPopupEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();
            try
            {
                bool result = (bool)eventArgs.Result;
                if (result == true)
                {
                    await _GSM01000ViewModel.ActiveInactiveProcessAsync();
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();       
            await _gridRef.R_RefreshGrid(null);
        }
    }
}
