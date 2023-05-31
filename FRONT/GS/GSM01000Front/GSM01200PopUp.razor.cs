using GSM01000Common.DTOs;
using GSM01000Model.ViewModel;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace GSM01000Front;

public partial class GSM01200PopUp : R_Page
{
    private GSM01200ViewModel _GSM1200ViewModel = new GSM01200ViewModel();
    private R_Grid<AssignCenterDTO> _gridCenterListToAssignRef;
    
    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {
            await _gridCenterListToAssignRef.R_RefreshGrid(null);
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
    
      public async Task Button_OnClickOkAsync()
    {
        var loData = _gridCenterListToAssignRef.GetCurrentData();
        await this.Close(true, loData);
    }
    public async Task Button_OnClickCloseAsync()
    {
        await this.Close(true, null);
    }
}