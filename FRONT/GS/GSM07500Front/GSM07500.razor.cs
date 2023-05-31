using R_BlazorFrontEnd.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GSM07500Common.DTOs;
using GSM07500Model.ViewModel;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using R_BlazorFrontEnd.Helpers;

namespace GSM07500Front;

public partial class GSM07500: R_Page
{
    private GSM07510ViewModel _GSM07510ViewModel = new GSM07510ViewModel();
    private R_Conductor _conductorGridPeriodRef;
    private R_Grid<GSM07510DTO> _gridPeriodRef;
    
    private GSM07500ViewModel _GSM07500ViewModel = new GSM07500ViewModel();
    private R_ConductorGrid _conductorGridPeriodDetailRef;
    private R_Grid<GSM07500DTO> _gridPeriodDetailRef;
    
    [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_ContextHeader _contextHeader { get; set; }
    
    
    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {
            await _gridPeriodRef.R_RefreshGrid(null);
            // await _gridPeriodDetailRef.R_RefreshGrid(null);

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    #region Period
    private async Task GridPeriod_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
    {
        var loEx = new R_Exception();

        try
        {
            await _GSM07510ViewModel.GetGridPeriodList();
            arg.ListEntityResult = _GSM07510ViewModel.loGridPeriodList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task ConductorPeriod_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Data);

            await _GSM07510ViewModel.GetPeriodSingle(loParam);
            eventArgs.Result = _GSM07510ViewModel.Period;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private async Task ConductorPeriod_Display(R_DisplayEventArgs eventArgs)
    {
        if (eventArgs.ConductorMode == R_eConductorMode.Normal)
        {
            var loParam = (GSM07510DTO)eventArgs.Data;
            var loHeadPeriod = (GSM07510DTO)_conductorGridPeriodRef.R_GetCurrentData();
            loHeadPeriod.CYEAR = loParam.CYEAR;
            loHeadPeriod.LPERIOD_MODE = loParam.LPERIOD_MODE;
            loHeadPeriod.INO_PERIOD = loParam.INO_PERIOD;

            await _gridPeriodDetailRef.R_RefreshGrid(loParam);
        }
    }
    private void R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
    {
        eventArgs.GridData = R_FrontUtility.ConvertObjectToObject<GSM07500DTO>(eventArgs.Data);
    }
    #endregion
    
    #region PeriodDetail
    private async Task GridPeriodDetail_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loCCYEARParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Parameter);
            _GSM07500ViewModel._cCYear = loCCYEARParam.CYEAR;
            await _GSM07500ViewModel.GetGridPeriodDetailList();
            eventArgs.ListEntityResult = _GSM07500ViewModel.loGridPeriodDetailList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    
    #endregion
    
   
}