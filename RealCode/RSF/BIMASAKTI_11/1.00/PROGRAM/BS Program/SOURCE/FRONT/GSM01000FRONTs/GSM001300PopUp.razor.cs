using GSM01000Common.DTOs;
using GSM01000Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace GSM01000Front;

public partial class GSM01300PopUp : R_Page
{
    private R_ConductorGrid _ConAssignCOARef;

    private GSM01300ViewModel _GSM1300ViewModel = new GSM01300ViewModel();
    private GSM01310ViewModel _GSM1310ViewModel = new GSM01310ViewModel();
    private R_Grid<AssignCoADTO> _gridCoAListToAssignRef;
    
    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {
            string loGOAAcc = poParameter.ToString();
            _GSM1300ViewModel.loGOA = loGOAAcc;
            await _gridCoAListToAssignRef.R_RefreshGrid(poParameter);
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
            await _GSM1310ViewModel.GetCoAAssignList();
            eventArgs.ListEntityResult = _GSM1310ViewModel.CoAToAssignList;
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
        var loData = (List<AssignCoADTO>)events.Data;

        events.Cancel = loData.Count == 0;
    }
    private async Task R_ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            //_GSM1300ViewModel.loGetCOAAssignList = (List<AssignCoADTO>)eventArgs.Data;//take list from selected
            //string lcCombinedCenterWithCommaSeparator = string.Join(",", _GSM1300ViewModel.loGetCOAAssignList.Where(dto => dto.LSELECTED).Select(dto => dto.CGLACCOUNT_NO));
            //await _GSM1300ViewModel.SaveCOAAssign(lcCombinedCenterWithCommaSeparator);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    private void R_AfterSaveBatch(R_AfterSaveBatchEventArgs eventArgs)
    {
        //string loCGLGOA = _GSM1300ViewModel.loGOA;
        _gridCoAListToAssignRef.R_RefreshGrid(null);

    }
    #endregion

    public async Task Button_OnClickOkAsync()
    {
        await _gridCoAListToAssignRef.R_SaveBatch();
        await this.Close(true, true);
    }
    public async Task Button_OnClickCloseAsync()
    {
        await this.Close(true, null);
    }
}