using GSM01000Common.DTOs;
using GSM01000Model.ViewModel;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace GSM01000Front;

public partial class GSM01300PopUp : R_Page
{
    private GSM01310ViewModel _GSM1310ViewModel = new GSM01310ViewModel();
    private R_Grid<AssignCoADTO> _gridCoAListToAssignRef;
    
    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {
            await _gridCoAListToAssignRef.R_RefreshGrid(null);
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
    
    public async Task Button_OnClickOkAsync()
    {
        var loData = _gridCoAListToAssignRef.GetCurrentData();
        await this.Close(true, loData);
    }
    public async Task Button_OnClickCloseAsync()
    {
        await this.Close(true, null);
    }
}