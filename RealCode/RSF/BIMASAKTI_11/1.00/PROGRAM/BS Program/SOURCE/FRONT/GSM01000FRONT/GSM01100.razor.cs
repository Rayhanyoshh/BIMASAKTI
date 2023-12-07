
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
    public partial class GSM01100 : R_Page
    {
        private GSM01100ViewModel _GSM01100ViewModel = new GSM01100ViewModel();
        private R_ConductorGrid _conductorGridCoAUserRef;
        private R_Grid<GSM01100DTO> _gridCoAUserRef;
/*        private R_PopupButton R_PopupCheck;
*/        
        [Inject] private  IClientHelper _clientHelper { get; set; }
        [Inject] private R_ContextHeader _contextHeader { get; set; }
        
        protected override async Task R_Init_From_Master(object poParam)
        {
            var loEx = new R_Exception();
            
            try
            {
                await _gridCoAUserRef.R_RefreshGrid(null);
                await _GSM01100ViewModel.GetParameterInfo();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
        private async Task GridCoAUser_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01100ViewModel.GetGridCoAUserList();
                arg.ListEntityResult = _GSM01100ViewModel.loGridCoAUserList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
        private async Task Grid_R_ServiceGetGridCoAUser(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                
                var loParam = R_FrontUtility.ConvertObjectToObject<GSM01100DTO>(eventArgs.Data);
                await _GSM01100ViewModel.GetCoAUserSingle(loParam);

                eventArgs.Result = _GSM01100ViewModel.loEntity;
                
            } 
            catch (Exception ex)
            {
                loEx.Add(ex);
            }   

            loEx.ThrowExceptionIfErrors();
        }
        
        private async Task GridCoAUser_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM01100DTO)eventArgs.Data;
                var loHeadGridCoAUser = (GSM01100DTO)_conductorGridCoAUserRef.R_GetCurrentData();
                loHeadGridCoAUser.CGLACCOUNT_NO = loParam.CGLACCOUNT_NO;
                loHeadGridCoAUser.CGLACCOUNT_NAME = loParam.CGLACCOUNT_NAME;
                
                _GSM01100ViewModel.UserID = loParam.CUSER_ID;
                // if (loParam.LEVERYONE==true)
                // {
                //     R_PopupCheck.Enabled=false;
                // }
                // else
                // {
                //     R_PopupCheck.Enabled = true;
                // }


                await _gridCoAUserRef.R_RefreshGrid(loParam);
            }
        }
        
        private async Task R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs arg)
        {
            arg.GridData = R_FrontUtility.ConvertObjectToObject<GSM01100DTO>(arg.Data);
        }
        
        #region Assign User
        private void R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.Parameter = _GSM01100ViewModel.loParameter.CGLACCOUNT_NO;
            eventArgs.TargetPageType = typeof(GSM01100PopUp);
        }
        private async Task R_After_Open_Popup(R_AfterOpenPopupEventArgs eventArgs)
        {
            var loTempResult = (AssignUserDTO)eventArgs.Result;
            
            var loParam = (GSM01100DTO)_conductorGridCoAUserRef.R_GetCurrentData();
            var loGlAccountTab = _GSM01100ViewModel.loParameter;
            
            if (loTempResult==null)
            {
                return;
            }

            loParam.CUSER_ID = loTempResult.CUSER_ID;
            loParam.CGLACCOUNT_NO = loGlAccountTab.CGLACCOUNT_NO;
            
            await _GSM01100ViewModel.AssignUserToCOA(loParam, 
                eCRUDMode.AddMode);
            await _gridCoAUserRef.R_RefreshGrid(null);
        }
        
        #endregion
        
    }
}
