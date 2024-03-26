using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GSM01000Common;
using GSM01000Common.DTOs;
using GSM01000Model;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Forms;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using R_BlazorFrontEnd.Helpers;

namespace GSM01000Front
{
    public partial class GSM01300
    {
        private GSM01300ViewModel _GSM01300ViewModel = new GSM01300ViewModel();
        private R_ConductorGrid _conductorGridGoARef;
        private R_Grid<GSM01300DTO> _gridGoARef;
        
        private GSM01310ViewModel _GSM01310ViewModel = new GSM01310ViewModel();
        private R_ConductorGrid _conductorGoACoARef;
        private R_Grid<GSM01310DTO> _gridGoACoARef;
/*        private R_PopupButton R_PopupCheck;
*/        
        [Inject] private  IClientHelper _clientHelper { get; set; }
        [Inject] private R_ContextHeader _contextHeader { get; set; }

        
        protected override async Task R_Init_From_Master(object poParam)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridGoARef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        #region GOA
        private async Task GridGoA1300_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01300ViewModel.GetGridGoAList();
                arg.ListEntityResult = _GSM01300ViewModel.loGridGoAList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
        private async Task GridGoA_Display(R_DisplayEventArgs eventArgs)
        {

            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM01300DTO)eventArgs.Data;
                _GSM01300ViewModel.GOAcode = loParam.CGOA_CODE;
                _GSM01300ViewModel.GOAname  = loParam.CGOA_NAME;

                await _gridGoACoARef.R_RefreshGrid(loParam.CGOA_CODE);
            }
        }
        
        private async Task Grid_R_ServiceGetGridGoA(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                // var lcParam = ((GSM01000DTO)eventArgs.Data).CGLACCOUNT_NO;
                // _contextHeader.R_Context.R_SetStreamingContext(ContextConstant.CGL_ACCOUNT, lcParam);
                
                var loParam = R_FrontUtility.ConvertObjectToObject<GSM01300DTO>(eventArgs.Data);
                await _GSM01300ViewModel.GetGoASingle(loParam);

                eventArgs.Result = _GSM01300ViewModel.loEntity;
                
                
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
        
        #region GOACOA
        
        private async Task GridGoACoA_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                //var loCGOAcodeParam = R_FrontUtility.ConvertObjectToObject<GSM01300DTO>(eventArgs.Parameter);
                _GSM01310ViewModel._cGOACode = (string)eventArgs.Parameter;
                await _GSM01310ViewModel.GetGoACoAList();
                eventArgs.ListEntityResult = _GSM01310ViewModel.loGridGoACoAList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        #endregion
        
        #region Assign COA
        private void R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            //var loCGOAcodeParam = R_FrontUtility.ConvertObjectToObject<GSM01300DTO>(eventArgs.Parameter);

            //_GSM01310ViewModel._cGOACode = loCGOAcodeParam.CGOA_CODE;
            eventArgs.Parameter = _GSM01310ViewModel._cGOACode;
            eventArgs.TargetPageType = typeof(GSM01300GridMover);
            //eventArgs.TargetPageType = typeof(GSM01300PopUp);

        }
        private async Task R_After_Open_Popup(R_AfterOpenPopupEventArgs eventArgs)
        {
            //var loTempResult = (AssignCoADTO)eventArgs.Result
            //var loCoa = (GSM01310DTO)_conductorGoACoARef.R_GetCurrentData();
            //if (loTempResult == null)
            //{
            //    return;
            //}
            var loEx = new R_Exception();
            try
            {
                var lcGOA = _GSM01310ViewModel._cGOACode;
                await _gridGoACoARef.R_RefreshGrid(lcGOA);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();


        }

        #endregion
        #region Print

        private void R_Before_Open_PopupPrint(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.Parameter = _GSM01300ViewModel.loGridGoAList.FirstOrDefault();   
            
            eventArgs.TargetPageType = typeof(PrintPopupGOA);
        }

        private async void R_After_Open_PopupPrint(R_AfterOpenPopupEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();
            try
            {
                await _gridGoARef.R_RefreshGrid(null);
                await Close(true, true);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }

        #endregion
    }
}
