
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
    public partial class GSM01200 : R_Page
    {
        private GSM01200ViewModel _GSM01200ViewModel = new GSM01200ViewModel();
        private R_ConductorGrid _conductorGridCoACenterRef;
        private R_Grid<GSM01200DTO> _gridCoACenterRef;
/*        private R_PopupButton R_PopupCheck;
*/
        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] private R_ContextHeader _contextHeader { get; set; }

        protected override async Task R_Init_From_Master(object poParam)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01200ViewModel.GetParameterInfo();
                await _gridCoACenterRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task GridCoACenter_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01200ViewModel.GetGridCoACenterList();
                arg.ListEntityResult = _GSM01200ViewModel.loGridCoACenterList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
        private async Task Grid_R_ServiceGetGridCoACenter(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                
                var loParam = R_FrontUtility.ConvertObjectToObject<GSM01200DTO>(eventArgs.Data);
                await _GSM01200ViewModel.GetCoACenterSingle(loParam);

                eventArgs.Result = _GSM01200ViewModel.loEntity;
                
            } 
            catch (Exception ex)
            {
                loEx.Add(ex);
            }   

            loEx.ThrowExceptionIfErrors();
        }
        
        private async Task GridCoACenter_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM01200DTO)eventArgs.Data;
                var loHeadGridCoACenter = (GSM01200DTO)_conductorGridCoACenterRef.R_GetCurrentData();
                loHeadGridCoACenter.CGLACCOUNT_NO = loParam.CGLACCOUNT_NO;
                loHeadGridCoACenter.CGLACCOUNT_NAME = loParam.CGLACCOUNT_NAME;

                _GSM01200ViewModel.UserId = loParam.CUSER_ID;
                await _gridCoACenterRef.R_RefreshGrid(loParam);
            }
        }

        private async Task R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs arg)
        {
            arg.GridData = R_FrontUtility.ConvertObjectToObject<GSM01200DTO>(arg.Data);
        }
        
        #region Assign Center
        private void R_Before_Open_Popup(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GSM01200PopUp);
        }
        private async Task R_After_Open_Popup(R_AfterOpenPopupEventArgs eventArgs)
        {
            var loTempResult = (AssignCenterDTO)eventArgs.Result;
            var loParam = (GSM01200DTO)_conductorGridCoACenterRef.R_GetCurrentData();
            var loGlAccountTab = _GSM01200ViewModel.loParameter;
            
            if (loTempResult==null)
            {
                return;
            }

            loParam.CCENTER_CODE = loTempResult.CCENTER_CODE;
            loParam.CGLACCOUNT_NO = loGlAccountTab.CGLACCOUNT_NO;

            await _GSM01200ViewModel.AssignCenterToCOA(loParam, 
                eCRUDMode.AddMode);
            await _gridCoACenterRef.R_RefreshGrid(null);
        }
        
        #endregion
    }
}
