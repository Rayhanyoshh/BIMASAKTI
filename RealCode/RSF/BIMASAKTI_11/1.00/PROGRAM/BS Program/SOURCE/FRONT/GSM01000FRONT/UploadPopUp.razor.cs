using BlazorClientHelper;
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
using GSM01000Common.DTOs;
using GSM01000Model;
using R_APICommonDTO;

namespace GSM01000Front;

public partial class UploadPopUp : R_Page
{
    [Inject] IClientHelper clientHelper { get; set; }
    [Inject] R_IExcel ExcelInject { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }

    private R_eFileSelectAccept[] accepts = { R_eFileSelectAccept.Excel };

    private GSM01001ViewModel _GSM01001ViewModel = new GSM01001ViewModel();
    private R_Grid<GSM01001ErrorValidateDTO> _COAUploadGridRef;

    private bool FileHasData = false;

    // Create Method Action For Download Excel if Has Error
    private async Task ActionFuncDataSetExcel()
    {
        var loByte = ExcelInject.R_WriteToExcel(_GSM01001ViewModel.ExcelDataSet);
        var lcName = $"COA {_GSM01001ViewModel.CompanyValue}" + ".xlsx";

        await JSRuntime.downloadFileFromStreamHandler(lcName, loByte);
    }
    // Create Method Action StateHasChange
    #region Invoke
    private void StateChangeInvoke()
    {
        StateHasChanged();
    }
    private async Task ShowSuccessInvoke()
    {
        var loValidate = await R_MessageBox.Show("", "Upload Process Successfully!", R_eMessageBoxButtonType.OK);
        if (loValidate == R_eMessageBoxResult.OK)
        {
            await this.Close(true, true);
        }
    }
    #endregion


    private void ShowErrorInvoke(R_APIException poEx)
    {
        var loEx = new R_Exception(poEx.ErrorList.Select(x => new R_BlazorFrontEnd.Exceptions.R_Error(x.ErrNo, x.ErrDescp)).ToList());
        this.R_DisplayException(loEx);
    }

    protected override async Task<Task> R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();
        var Param = (GSM01001ErrorValidateDTO)poParameter;

        try
        {
            //Asign Company and User id
            _GSM01001ViewModel.CompanyID = clientHelper.CompanyId;
            _GSM01001ViewModel.UserId = clientHelper.UserId;
            _GSM01001ViewModel.GetCompanyDetailAsync();

            //Assign Action
            _GSM01001ViewModel.StateChangeAction = StateChangeInvoke;
            _GSM01001ViewModel.ShowErrorAction = ShowErrorInvoke;
            _GSM01001ViewModel.ShowSuccessAction = ShowSuccessInvoke;
            _GSM01001ViewModel.ActionDataSetExcel = ActionFuncDataSetExcel;

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
            await _GSM01001ViewModel.GetCompanyDetailAsync();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }


    private async Task _COA_SourceUpload_OnChange(InputFileChangeEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            _GSM01001ViewModel.SourceFileName = eventArgs.File.Name;
            //import from excel
            var loMS = new MemoryStream();
            await eventArgs.File.OpenReadStream().CopyToAsync(loMS);
            var loFileByte = loMS.ToArray();

            //add filebyte to DTO
            var loExcel = ExcelInject;

            var loDataSet = loExcel.R_ReadFromExcel(loFileByte, new string[] { "COA" });

            var loResult = R_FrontUtility.R_ConvertTo<GSM01001DTO>(loDataSet.Tables[0]);
           
        
            FileHasData = loResult.Count > 0 ? true : false;

            await _COAUploadGridRef.R_RefreshGrid(loResult);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }

    private async Task COA_Upload_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loData = (List<GSM01001DTO>)eventArgs.Parameter;

            await _GSM01001ViewModel.ConvertGrid(loData);
            eventArgs.ListEntityResult = _GSM01001ViewModel.COAValidateUploadError;
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
                await _GSM01001ViewModel.SaveBulkFile();
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
      
            await ActionFuncDataSetExcel();
       
    }

}