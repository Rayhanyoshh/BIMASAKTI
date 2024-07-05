using BlazorClientHelper;
using GSM01000Common.DTOs;
using GSM01000Model;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Exceptions;

namespace GSM01000Front;

public partial class GSM01300GridMover : R_Page 
{
    private GSM01310ViewModel _GSM01310ViewModel = new GSM01310ViewModel();
    private GSM01300ViewModel _GSM1300ViewModel = new GSM01300ViewModel();
    private R_Grid<GSM01310DTO> _SourceAvailableCOA_gridRef;
    private R_Grid<GSM01310DTO> _SelectedCOA_gridRef;
    private R_ConductorGrid _SourceAvailableCOA_conGrid;
    private R_ConductorGrid _SelectedCOA_conGrid;
    
    private string label1 = "<<";
    private string label2 = "<";
    private string SelectedAcc = "";
    private string SelectedAccName = "";
    [Inject] IClientHelper clientHelper { get; set; }
    
    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {
            string loGOAAcc = poParameter.ToString();
            _GSM1300ViewModel.loGOA = loGOAAcc;

            await _SourceAvailableCOA_gridRef.R_RefreshGrid(poParameter);
            await _SelectedCOA_gridRef.R_RefreshGrid(poParameter);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task AvailableCOA_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            await _GSM01310ViewModel.GetCoAAssignList();
            eventArgs.ListEntityResult = _GSM01310ViewModel.CoAToAssignList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task SelectedCoA_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            //var loCGOAcodeParam = R_FrontUtility.ConvertObjectToObject<GSM01300DTO>(eventArgs.Parameter);
            _GSM01310ViewModel._cGOACode = (string)eventArgs.Parameter;
            await _GSM01310ViewModel.GetGoACoAList();
            eventArgs.ListEntityResult = _GSM01310ViewModel.loGridGoACoAList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    
    private bool HasMove = false;
    private void R_GridRowBeforeDrop(R_GridRowBeforeDropEventArgs eventArgs)
    {
        HasMove = true;
    }

    private void R_GridRowAfterDrop(R_GridRowAfterDropEventArgs eventArgs)
    {
        HasMove = true;
    }

    private void R_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
    {
        eventArgs.Result = eventArgs.Data;
    }
    

    
    #region Save Batch
    private void R_BeforeSaveBatch(R_BeforeSaveBatchEventArgs events)
    {
        var loData = (List<GSM01310DTO>)events.Data;

        events.Cancel = loData.Count == 0;
    }
    private async Task R_ServiceSaveBatch(R_ServiceSaveBatchEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loData = (List<GSM01310DTO>)eventArgs.Data;

            List<string> idList = loData.Select(x => x.CGLACCOUNT_NO).ToList();
            string idString = string.Join(",", idList);

            var loParam = new COAtoAssignParam();
            loParam.CCOA_LIST = idString;

            await _GSM1300ViewModel.SaveCOAAssign(loParam);

            await R_MessageBox.Show("", "Assign COA updated successfully!", R_eMessageBoxButtonType.OK);
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
        _SourceAvailableCOA_gridRef.R_RefreshGrid(null);

    }
    #endregion
    
    private bool ProsessMove = false;
    private async Task BtnProcess()
    {
        var loEx = new R_Exception();

        try
        {
            ProsessMove = true;
            await _SelectedCOA_conGrid.R_SaveBatch();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }

    private async Task BtnClose()
    {
        var loEx = new R_Exception();

        try
        {
            if (HasMove && !ProsessMove)
            {
                var Discard = await R_MessageBox.Show("", "Discard changes? ", R_eMessageBoxButtonType.YesNo);
                if (Discard == R_eMessageBoxResult.Yes)
                {
                    await this.Close(true, null);
                }
            }
            else
            {
                await this.Close(true, null);
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
            
    }
    
    private void R_GridRowBeforeDrop(R_GridDragDropBeforeDropEventArgs<GSM01310DTO> eventArgs)
    {
        //eventArgs.Cancel = true;
    }

    private void R_GridRowAfterDrop(R_GridDragDropAfterDropEventArgs<GSM01310DTO> eventArgs)
    {

    }
    
    private void Allocation_BtnMoveRight()
    {
        HasMove = true;
        _SourceAvailableCOA_gridRef.R_MoveToTargetGrid();
    }

    private void Allocation_BtnAllMoveRight()
    {
        HasMove = true;
        _SourceAvailableCOA_gridRef.R_MoveAllToTargetGrid();
    }

    private void Allocation_BtnAllMoveLeft()
    {
        HasMove = true;
        _SelectedCOA_gridRef.R_MoveAllToTargetGrid();
    }

    private void Allocation_BtnMoveLeft()
    {
        HasMove = true;
        _SelectedCOA_gridRef.R_MoveToTargetGrid();
    }

    
}