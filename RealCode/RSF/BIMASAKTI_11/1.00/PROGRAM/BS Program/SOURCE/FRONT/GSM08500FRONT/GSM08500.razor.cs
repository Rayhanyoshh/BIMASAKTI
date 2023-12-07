using R_BlazorFrontEnd.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GSM008500Common.DTOs;
using GSM08500Common;
using GSM08500Common.DTOs;
using GSM08500Model.ViewModel;
using Lookup_GSCOMMON.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid.Columns;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Controls.MessageBox;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls.Popup;
using GFF00900COMMON.DTOs;

namespace GSM08500Front;

public partial class GSM08500 : R_Page
{
    private GSM08500ViewModel _GSM08500ViewModel = new GSM08500ViewModel();
    private R_ConductorGrid _conductorGridRef;
    private R_Grid<GSM08500DTO> _gridRef;
    
    private string loLabelButton = "";

    // [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_ContextHeader _contextHeader { get; set; }
    [Inject] private IJSRuntime JS { get; set; }
    [Inject] private R_PopupService PopupService { get; set; }


    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {
            _GSM08500ViewModel.BSIS_Option = new List<DropDownDTO>
            {
                new DropDownDTO { Id = "B", Text = "Balance Sheet"},
                new DropDownDTO { Id = "I", Text = "Income Statement" }
            };

            _GSM08500ViewModel.CDBCR_Option = new List<DropDownDTO>
            {
                new DropDownDTO { Id = "D", Text = "Debit" },
                new DropDownDTO { Id = "C", Text = "Credit" }
            };
            await _GSM08500ViewModel.GetGridList();
            await _gridRef.R_RefreshGrid(null);
                
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
    {
        var loEx = new R_Exception();

        try
        {
            await _GSM08500ViewModel.GetGridList();
            arg.ListEntityResult = _GSM08500ViewModel.loGridList;
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
            var loParam = (GSM08500DTO)eventArgs.Data;

            _GSM08500ViewModel.SelectedActiveInactiveCenterCode = loParam.CGLACCOUNT_NO;
            _GSM08500ViewModel.SelectedActiveInactiveLACTIVE = loParam.LACTIVE;

            if (loParam.LACTIVE)
            {
                loLabelButton = "Inactive";
                _GSM08500ViewModel.SelectedActiveInactiveLACTIVE = false;
            } 
            else 
            {
                loLabelButton = "Activate";
                _GSM08500ViewModel.SelectedActiveInactiveLACTIVE = true;
            }
            // await _gridRef.R_RefreshGrid(loParam);
        }
    }
    
    private async Task Grid_R_ServiceGetGridCoA(R_ServiceGetRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var lcParam = ((GSM08500DTO)eventArgs.Data).CGLACCOUNT_NO;
            _contextHeader.R_Context.R_SetStreamingContext(GSM08500Common.DTOs.ContextConstant.CGLACCOUNT_NO, lcParam);

            var loParam = R_FrontUtility.ConvertObjectToObject<GSM08500DTO>(eventArgs.Data);
            await _GSM08500ViewModel.GetStatAccSingle(loParam);

            eventArgs.Result = _GSM08500ViewModel.loEntity;


        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    
    private async Task Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            await _GSM08500ViewModel.SaveEntity((GSM08500DTO)eventArgs.Data, (eCRUDMode)eventArgs.ConductorMode);
            eventArgs.Result = _GSM08500ViewModel.loEntity;
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
            var loData = (GSM08500DTO)eventArgs.Data;
            await _GSM08500ViewModel.DeleteEntity(loData);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    
    private async Task R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs arg)
    {
        arg.GridData = R_FrontUtility.ConvertObjectToObject<GSM08500DTO>(arg.Data);
    }

    #region Active/Inactive
    private async Task R_Before_Open_Popup_ActivateInactive(R_BeforeOpenPopupEventArgs eventArgs)
    {
        R_Exception loException = new R_Exception();
        try
        {
            var loValidateViewModel = new GFF00900Model.ViewModel.GFF00900ViewModel();
            loValidateViewModel.ACTIVATE_INACTIVE_ACTIVITY_CODE =
                "GSM08501"; //Uabh Approval Code sesuai Spec masing masing
            await loValidateViewModel.RSP_ACTIVITY_VALIDITYMethodAsync(); //Jika IAPPROVAL_CODE == 3, maka akan keluar RSP_ERROR disini

            //Jika Approval User ALL dan Approval Code 1, maka akan langsung menjalankan ActiveInactive
            if (loValidateViewModel.loRspActivityValidityList.FirstOrDefault().CAPPROVAL_USER == "ALL" &&
                loValidateViewModel.loRspActivityValidityResult.Data.FirstOrDefault().IAPPROVAL_MODE == 1)
            {
                await _GSM08500ViewModel
                    .ActiveInactiveProcessAsync(); //Ganti jadi method ActiveInactive masing masing
                await _gridRef.R_RefreshGrid(null);
                return;
            }
            else //Disini Approval Code yang didapat adalah 2, yang berarti Active Inactive akan dijalankan jika User yang diinput ada di RSP_ACTIVITY_VALIDITY
            {
                eventArgs.Parameter = new GFF00900ParameterDTO()
                {
                    Data = loValidateViewModel.loRspActivityValidityList,
                    IAPPROVAL_CODE = "GSM08501" //Uabh Approval Code sesuai Spec masing masing
                };
                eventArgs.TargetPageType = typeof(GFF00900FRONT.GFF00900);
            }
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }

        loException.ThrowExceptionIfErrors();
    }

    private async Task R_After_Open_Popup_ActivateInactive(R_AfterOpenPopupEventArgs eventArgs)
    {
        R_Exception loException = new R_Exception();
        try
        {
            bool result = (bool)eventArgs.Result;
            if (result == true)
            {
                await _GSM08500ViewModel.ActiveInactiveProcessAsync();
            }
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();       
        await _gridRef.R_RefreshGrid(null);
    }
    #endregion
    
    #region CopyFrom
    private void R_Before_Open_Popup_CopyFrom(R_BeforeOpenPopupEventArgs eventArgs)
    {
        eventArgs.TargetPageType = typeof(CopyFromModal);
    }

    private async Task R_After_Open_Popup_CopyFrom(R_AfterOpenPopupEventArgs eventArgs)
    {
        await _gridRef.R_RefreshGrid(null);

    }
    #endregion

    #region Template
    private async Task DownloadTemplate()
    {
        var loEx = new R_Exception();

        try
        {
            var loValidate = await R_MessageBox.Show("", "Are you sure download this template?", R_eMessageBoxButtonType.YesNo);

            if (loValidate == R_eMessageBoxResult.Yes)
            {
                var loByteFile = await _GSM08500ViewModel.DownloadTemplate();

                var saveFileName = $"STATISTIC_ACCOUNT.xlsx";

                await JS.downloadFileFromStreamHandler(saveFileName, loByteFile.FileBytes);
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    #endregion
    
    #region Upload
    private void R_Before_Open_Popup_Upload(R_BeforeOpenPopupEventArgs eventArgs)
    {

       eventArgs.TargetPageType = typeof(UploadPopUpStatisticAcc); 
    }

    private async Task R_After_Open_Popup_Upload(R_AfterOpenPopupEventArgs eventArgs)
    {
        await _gridRef.R_RefreshGrid(null);
        await this.Close(true, false);

    }
    #endregion

    #region BeforeAdd
    private void R_After_Add_COA(R_AfterAddEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            // var loTempParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Data);
            var loData = (GSM08500DTO)eventArgs.Data;
            loData.LACTIVE = true;

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();

    }


    private async Task GridAddCOA_Validation(R_ValidationEventArgs eventArgs)
    {
        R_Exception loException = new R_Exception();
        R_PopupResult loResult = null;
        GFF00900ParameterDTO loParam = null;
        GSM08500DTO loData = null;
        try
        {
            loData = (GSM08500DTO)eventArgs.Data;
            if (loData.LACTIVE == true && _conductorGridRef.R_ConductorMode == R_eConductorMode.Add)
            {
                var loValidateViewModel = new GFF00900Model.ViewModel.GFF00900ViewModel();
                loValidateViewModel.ACTIVATE_INACTIVE_ACTIVITY_CODE = "GSM08501";
                await loValidateViewModel.RSP_ACTIVITY_VALIDITYMethodAsync();

                if (loValidateViewModel.loRspActivityValidityList.FirstOrDefault().CAPPROVAL_USER == "ALL" && loValidateViewModel.loRspActivityValidityResult.Data.FirstOrDefault().IAPPROVAL_MODE == 1)
                {
                    eventArgs.Cancel = false;
                }
                else
                {
                    loParam = new GFF00900ParameterDTO()
                    {
                        Data = loValidateViewModel.loRspActivityValidityList,
                        IAPPROVAL_CODE = "GSM08501"
                    };
                    loResult = await PopupService.Show(typeof(GFF00900FRONT.GFF00900), loParam);
                    if (loResult.Success == false || (bool)loResult.Result == false)
                    {
                        eventArgs.Cancel = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();
    }
    #endregion

    #region Print

    private void R_Before_Open_Popup_Print(R_BeforeOpenPopupEventArgs eventArgs)
    {

        eventArgs.TargetPageType = typeof(PrintPopupStatAcc);
    }

    private async void R_After_Open_Popup_Print(R_AfterOpenPopupEventArgs eventArgs)
    {
        R_Exception loException = new R_Exception();
        try
        {
            await _gridRef.R_RefreshGrid(null);

        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }

        loException.ThrowExceptionIfErrors();
    }

    #endregion
}