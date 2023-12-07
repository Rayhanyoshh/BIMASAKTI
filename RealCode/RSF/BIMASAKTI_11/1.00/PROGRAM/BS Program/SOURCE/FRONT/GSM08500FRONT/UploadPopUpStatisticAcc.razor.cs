using BlazorClientHelper;
using GSM008500Common.DTOs;
using R_BlazorFrontEnd.Controls;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using Microsoft.JSInterop;
using GSM08500Common.DTOs;
using GSM08500Model;
using R_APICommonDTO;

namespace GSM08500Front;

public partial class UploadPopUpStatisticAcc : R_Page
{
        [Inject] IClientHelper clientHelper { get; set; }
        [Inject] R_IExcel ExcelInject { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }
    
        private R_eFileSelectAccept[] accepts = { R_eFileSelectAccept.Excel };
    
        private GSM08501ViewModel _GSM08501ViewModel = new GSM08501ViewModel();
        private R_Grid<GSM08501ErrorValidateDTO> _STATISTIC_ACCOUNTUploadGridRef;
    
        private bool FileHasData = false;
        
        
    // Create Method Action For Download Excel if Has Error
    private async Task ActionFuncDataSetExcel()
    {
        var loByte = ExcelInject.R_WriteToExcel(_GSM08501ViewModel.ExcelDataSet);
        var lcName = $"STATISTIC ACCOUNT {_GSM08501ViewModel.CompanyValue}" + ".xlsx";

        await JSRuntime.downloadFileFromStreamHandler(lcName, loByte);
    }
    // Create Method Action StateHasChange
    private void StateChangeInvoke()
    {
        StateHasChanged();
    }

    private void ShowErrorInvoke(R_APIException poEx)
    {
        var loEx = new R_Exception(poEx.ErrorList.Select(x => new R_BlazorFrontEnd.Exceptions.R_Error(x.ErrNo, x.ErrDescp)).ToList());
        this.R_DisplayException(loEx);
    }

    protected override Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();
        var Param = (GSM08501ErrorValidateDTO)poParameter;

        try
        {
            //Asign Company and User id
            _GSM08501ViewModel.CompanyID = clientHelper.CompanyId;
            _GSM08501ViewModel.UserId = clientHelper.UserId;
             _GSM08501ViewModel.GetCompanyDetailAsync();

            //Assign Action
            _GSM08501ViewModel.StateChangeAction = StateChangeInvoke;
            _GSM08501ViewModel.ShowErrorAction = ShowErrorInvoke;
            _GSM08501ViewModel.ActionDataSetExcel = ActionFuncDataSetExcel;

            

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
        return Task.CompletedTask;

    }
    public async Task GetCompanyDetail(string pcCompID)
    {
        var loEx = new R_Exception();
        try
        {
             await _GSM08501ViewModel.GetCompanyDetailAsync();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }


    private async Task _STATISTIC_ACCOUNT_SourceUpload_OnChange(InputFileChangeEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            _GSM08501ViewModel.SourceFileName = eventArgs.File.Name;
            //import from excel
            var loMS = new MemoryStream();
            await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
            var loFileByte = loMS.ToArray();

            //add filebyte to DTO
            var loExcel = ExcelInject;

            var loDataSet = loExcel.R_ReadFromExcel(loFileByte, new string[] { "StatisticAccount" });

            var loResult = R_FrontUtility.R_ConvertTo<GSM08501DTO>(loDataSet.Tables[0]);

            FileHasData = loResult.Count > 0 ? true : false;

            await _STATISTIC_ACCOUNTUploadGridRef.R_RefreshGrid(loResult);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }

    private async Task STATISTIC_ACCOUNT_Upload_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loData = (List<GSM08501DTO>)eventArgs.Parameter;

            await _GSM08501ViewModel.ConvertGrid(loData);
            eventArgs.ListEntityResult = _GSM08501ViewModel.STATISTIC_ACCOUNTValidateUploadError;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        //loEx.ThrowExceptionIfErrors();

        R_DisplayException(loEx);

    }

    public async Task Button_OnClickOkAsync()
    {
        var loEx = new R_Exception();

        try
        {
            var loValidate = await R_MessageBox.Show("", "Are you sure want to import data?", R_eMessageBoxButtonType.YesNo);

            if (loValidate == R_eMessageBoxResult.Yes)
            {
                await _GSM08501ViewModel.SaveBulkFile();

                if (_GSM08501ViewModel.VisibleError)
                {
                    await R_MessageBox.Show("", "Statistic Account uploaded successfully!", R_eMessageBoxButtonType.OK);
                    await this.Close(true, false);
                }
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    public async Task Button_OnClickCloseAsync()
    {
        await this.Close(true, false);
    }

    public async Task Button_OnClickSaveExcelAsync()
    {
        var loValidate = await R_MessageBox.Show("", "Are you sure want to save to excel again?", R_eMessageBoxButtonType.YesNo);

        if (loValidate == R_eMessageBoxResult.Yes)
        {
            await ActionFuncDataSetExcel();
        }
    }
    
}