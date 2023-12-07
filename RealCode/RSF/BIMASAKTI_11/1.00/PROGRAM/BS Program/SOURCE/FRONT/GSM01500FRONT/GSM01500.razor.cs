using GSM01500COMMON.DTOs;
using GSM01500MODEL.ViewModel;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;

namespace GSM01500FRONT
{
    public partial class GSM01500 : R_Page
    {
        private GSM01500ViewModel CenterViewModel = new();

        private GSM01510ViewModel DeptViewModel = new();

        private R_ConductorGrid _conGridCenterRef;

        private R_Grid<GSM01500DTO> _gridRef;

        private string loLabel = "";

        protected override async Task R_Init_From_Master(object poParameter)
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

        private async Task Grid_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM01500DTO)eventArgs.Data;
                DeptViewModel.SelectedCenterCode = loParam.CCENTER_CODE;
                CenterViewModel.SelectedActiveInactiveCenterCode = loParam.CCENTER_CODE;
                CenterViewModel.SelectedActiveInactiveLACTIVE = loParam.LACTIVE;
                if (loParam.LACTIVE)
                {
                    loLabel = "Inactive";
                    CenterViewModel.SelectedActiveInactiveLACTIVE = false;
                }
                else
                {
                    loLabel = "Activate";
                    CenterViewModel.SelectedActiveInactiveLACTIVE = true;
                }
            }
        }

        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await CenterViewModel.GetCenterListStreamAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();

            try
            {
                GSM01500DTO loParam = (GSM01500DTO)eventArgs.Data;
                await CenterViewModel.GetCenterAsync(loParam);

                eventArgs.Result = CenterViewModel.loCenter;
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }

        private void Grid_BeforeEdit(R_BeforeEditEventArgs eventArgs)
        {
        }

        private void Grid_BeforeCancel(R_BeforeCancelEventArgs eventArgs)
        {
        }

        private void Grid_BeforeAdd(R_BeforeAddEventArgs eventArgs)
        {
        }

        private void Grid_AfterAdd(R_AfterAddEventArgs eventArgs)
        {
        }

        private void Grid_Validation(R_ValidationEventArgs eventArgs)
        {

        }

        private void Grid_Saving(R_SavingEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Add)
            {
                var loData = (GSM01500DTO)eventArgs.Data;
            }
        }

        private async Task Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await CenterViewModel.SaveCenterAsync(
                    (GSM01500DTO)eventArgs.Data,
                    (eCRUDMode)eventArgs.ConductorMode);

                eventArgs.Result = CenterViewModel.loCenter;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_AfterSave(R_AfterSaveEventArgs eventArgs)
        {

        }

        private void Grid_BeforeDelete(R_BeforeDeleteEventArgs eventArgs)
        {
        }

        private async Task Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                GSM01500DTO loData = (GSM01500DTO)eventArgs.Data;
                await CenterViewModel.DeleteCenterAsync(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

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
                    await CenterViewModel.ActiveInactiveProcessAsync();
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            await _gridRef.R_RefreshGrid(null);
        }

        private async Task Grid_AfterDelete()
        {

        }

    }
}
