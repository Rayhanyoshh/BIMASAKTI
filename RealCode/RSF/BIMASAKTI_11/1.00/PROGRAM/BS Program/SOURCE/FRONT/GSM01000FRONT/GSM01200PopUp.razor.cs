using GSM01000Common.DTOs;
using GSM01000Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using System.Collections.ObjectModel;

namespace GSM01000Front;

public partial class GSM01200PopUp : R_Page
{
    private R_ConductorGrid _ConAssignCenterRef;

    private R_Grid<AssignCenterDTO> _gridCenterListToAssignRef;
    private GSM01200ViewModel _GSM1200ViewModel = new GSM01200ViewModel();
    public ObservableCollection<AssignCenterDTO> loGridCenterAssignList { get; set; } = new();

    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {
            string lcGlAccount = poParameter.ToString();
            _GSM1200ViewModel.cglAccount = lcGlAccount;
            await _gridCenterListToAssignRef.R_RefreshGrid(poParameter);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task R_ServiceGetListRecordAsync(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            await _GSM1200ViewModel.GetCenterToAssignList();
            eventArgs.ListEntityResult = _GSM1200ViewModel.CenterToAssignList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }

    #region Save Batch
    private void R_BeforeSaveBatch(R_BeforeSaveBatchEventArgs events)
    {
        var loData = (List<AssignCenterDTO>)events.Data;

        events.Cancel = loData.Count == 0;
    }
    private async Task R_ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            _GSM1200ViewModel.loGetCenterAssignList = (List<AssignCenterDTO>)eventArgs.Data;//take list from selected
            string lcCombinedCenterWithCommaSeparator = string.Join(",", _GSM1200ViewModel.loGetCenterAssignList.Where(dto => dto.LSELECTED).Select(dto => dto.CCENTER_CODE));
            await _GSM1200ViewModel.SaveCenterAssign(lcCombinedCenterWithCommaSeparator);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    private void R_AfterSaveBatch(R_AfterSaveBatchEventArgs eventArgs)
    {
        var loCGL = _GSM1200ViewModel.cglAccount;
        _gridCenterListToAssignRef.R_RefreshGrid(loCGL);    

    }
    #endregion

    public async Task Button_OnClickOkAsync()
    {
        await _ConAssignCenterRef.R_SaveBatch();
        await this.Close(true, true);
    }
    public async Task Button_OnClickCloseAsync()
    {
        await this.Close(true, null);
    }
}