using R_BlazorFrontEnd.Controls;
using BlazorClientHelper;
using GSM07500Common.DTOs;
using GSM07500Model.ViewModel;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System.Diagnostics.Tracing;
using System.Xml.Linq;

namespace GSM07500Front;

public partial class GSM07500 : R_Page
{
    private GSM07510ViewModel _GSM07510ViewModel = new GSM07510ViewModel();
    private R_Conductor _conductorGridPeriodRef;
    private R_Grid<GSM07510DTO> _gridPeriodRef;

    private GSM07500ViewModel _GSM07500ViewModel = new GSM07500ViewModel();
    private R_ConductorGrid _conductorGridPeriodDetailRef;
    private R_Grid<GSM07500DTO> _gridPeriodDetailRef;


    public string lcCompany = "";

    [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_ContextHeader _contextHeader { get; set; }


    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {
            await _gridPeriodRef.R_RefreshGrid(lcCompany);
            //await _gridPeriodDetailRef.R_RefreshGrid(null);

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
            _GSM07500ViewModel._CCYEAR = loParam.CYEAR;
            //var loParam2 = R_FrontUtility.ConvertObjectToObject<GSM07500DTO>(eventArgs.Data);
            //_GSM07510ViewModel.Cyear = loParam2.CCYEAR;


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
        var loParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Data);

        if (eventArgs.ConductorMode == R_eConductorMode.Normal)
        {
            var loPar = (GSM07510DTO)eventArgs.Data;

            await _gridPeriodDetailRef.R_RefreshGrid(loParam);
        }
    }

    private async Task Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loData = (GSM07510DTO)eventArgs.Data;
            await _GSM07510ViewModel.DeleteEntity(loData);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private void R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs eventArgs)
    {
        eventArgs.GridData = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Data);
        var YearParam = eventArgs.GridData;
    }

    #endregion

    #region PeriodDetail

    private async Task GridPeriodDetail_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loTempParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Parameter);

            var loPar = new GSM07500DTO()
            {
                CCYEAR = loTempParam.CYEAR,

            };
            _GSM07500ViewModel._CCYEAR = loPar.CCYEAR;
            await _GSM07500ViewModel.GetGridPeriodDetailList(loPar);
            loPar.CCYEAR = _GSM07500ViewModel._CCYEAR;
            eventArgs.ListEntityResult = _GSM07500ViewModel.loGridPeriodDetailList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }


    #endregion

    private async Task GetOnClickAfterAddButton(R_AfterAddEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            // var loTempParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Data);
            var loData = (GSM07510DTO)eventArgs.Data;
            loData.CYEAR = await _GSM07510ViewModel.GetLastYearSingle(loData);
            loData.INO_PERIOD = 12;
            loData.LPERIOD_MODE = true;
        
            await ShowGenerateList();


        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private bool R_SetGridDetailEnabled = false;

    private void GridPeriod_SetAdd(R_SetEventArgs args)
    {
        R_SetGridDetailEnabled = !args.Enable;
    }


    private async Task ShowGenerateList()
    {
        var loParam = new GSM07510DTO();
        loParam.LPERIOD_MODE = true;
        loParam.INO_PERIOD = 12;

        var loYearParam = _GSM07510ViewModel.LastYear.NEXT_YEAR;

        await _GSM07500ViewModel.GetGenaratorListPeriod(loParam, loYearParam);
    }

   
    private async Task OnClose()
    {
        await this.Close(true, false);
    }

    private async void BeforeDelete(R_BeforeDeleteEventArgs eventArgs)
    {
        var loParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Data);
        var loEx = new R_Exception();

        try
        {
            string targetYear = loParam.CYEAR;

            // Mengecek apakah targetYear ada dalam _GSM07510ViewModel.loGridPeriodList
            bool isValidYear = _GSM07510ViewModel.loGridPeriodList.Any(item => item.CYEAR == targetYear);

            if (!isValidYear)
            {
                loEx.Add("001", "Invalid Year: Year not found in the list");
            }
            else if (targetYear != _GSM07510ViewModel.loGridPeriodList.Max(item => item.CYEAR))
            {
                loEx.Add("002", "Can delete Last Period only");
                eventArgs.Cancel = true;
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }



    private async Task Grid_ServiceSave(R_ServiceSaveEventArgs poEntity)
    {
        R_Exception loException = new R_Exception();
        try
        {
            var loList = _GSM07500ViewModel.loGridPeriodDetailList;
            var loData = (GSM07510DTO)poEntity.Data;

            loData.CPERIOD_LIST = new List<GSM07500DTO>(loList);

            await _GSM07510ViewModel.SavePeriodDetailGenerator(loData, (eCRUDMode)poEntity.ConductorMode);
            //var loData = _GSM07510ViewModel.Data;

            //loData.CPERIOD_LIST = new List<GSM07500DTO>((List<GSM07500DTO>)eventArgs.Data);
     
            poEntity.Result = _GSM07510ViewModel.Period;
            //await _gridPeriodRef.R_RefreshGrid(null);
        }
        catch (Exception ex)
        {

            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();
    }

    private async Task Grid_AfterSave(R_AfterSaveEventArgs poEntity)
    {
        //var loData = (GSM07510DTO)poEntity.Data;

        ////GSM07500DTO param = new GSM07500DTO()
        ////{
        ////    CCYEAR = loData.CCYEAR,
        ////    CCOMPANY_ID = loData.CCOMPANY_ID
        ////};
        //string loComp = loData.CCOMPANY_ID;
        //await _gridPeriodRef.R_RefreshGrid(loComp);
    }

    private async Task Grid_ValidationPeriodGenerate(R_ValidationEventArgs eventArgs)
    {
        await _conductorGridPeriodDetailRef.R_SaveBatch();
    }
}