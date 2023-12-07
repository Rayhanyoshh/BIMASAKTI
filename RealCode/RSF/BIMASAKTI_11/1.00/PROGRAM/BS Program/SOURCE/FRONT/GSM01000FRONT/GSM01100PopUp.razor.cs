using System.Collections.ObjectModel;
using GSM01000Common.DTOs;
using GSM01000Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace GSM01000Front;

public partial class GSM01100PopUp : R_Page
{
    private R_ConductorGrid _ConAssignUserRef;
   
    private R_Grid<AssignUserDTO> _gridUserListToAssignRef;
    private GSM01100ViewModel _GSM1100ViewModel = new GSM01100ViewModel();
    public ObservableCollection<AssignUserDTO> loGridUserAssignList { get; set; } = new();


    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {
            string lcGlAccount = poParameter.ToString();
            _GSM1100ViewModel.cglAccount = lcGlAccount;
            await _gridUserListToAssignRef.R_RefreshGrid(poParameter);
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
            await _GSM1100ViewModel.GetUserToAssignList();
            eventArgs.ListEntityResult = _GSM1100ViewModel.UsersToAssignList;
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
        var loData = (List<AssignUserDTO>)events.Data;

        events.Cancel = loData.Count == 0;
    }
    private async Task R_ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            _GSM1100ViewModel.loGetUserAssignList = (List<AssignUserDTO>)eventArgs.Data;//take list from selected
            string lcCombinedUserIdWithCommaSeparator = string.Join(",", _GSM1100ViewModel.loGetUserAssignList.Where(dto => dto.LSELECTED).Select(dto => dto.CUSER_ID));
            await _GSM1100ViewModel.SaveUserAssign(lcCombinedUserIdWithCommaSeparator);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    private void R_AfterSaveBatch(R_AfterSaveBatchEventArgs eventArgs)
    {
        var loCGL = _GSM1100ViewModel.cglAccount;
        _gridUserListToAssignRef.R_RefreshGrid(loCGL);

    }
    #endregion



    public async Task Button_OnClickOkAsync()
    {
        var loEx = new R_Exception();

        try
        {
            await _ConAssignUserRef.R_SaveBatch();
            await Close(true, true);
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

    
}