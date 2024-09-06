using BlazorClientHelper;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;
using TXR00100Common.PrintDTO;
using TXR00100FrontResources;
using TXR00100MODEL.ViewModel;

namespace TXR00100Front;

public partial class TXR00100 : R_Page
{
    private TXR00100ViewModel _TXR00100ViewModel = new();
    private R_Conductor _ConductorRef;
    
    [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_IReport _reportService { get; set; }
    [Inject] private R_ILocalizer<ResourcesDummyTXR00100> _localizer { get; set; }

    
    #region init

    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {
            await _TXR00100ViewModel.InitProcess(_localizer);

            await _TXR00100ViewModel.GetPropertyList();
            await _TXR00100ViewModel.GetPeriodDetailList();
            _TXR00100ViewModel.WHTaxRadioSelected = _TXR00100ViewModel.WHTaxRadio.FirstOrDefault().CWH_TAX_TYPE;
            _TXR00100ViewModel.SortByRadioSelected = _TXR00100ViewModel.SortByRadio.FirstOrDefault().CSORT_BY;
         
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    
    private async Task Button_OnClickProcessAsync()
    {
        var loEx = new R_Exception();
        PrintParamTXDTO loParam;
        
        try
        {
            loParam = new PrintParamTXDTO()
            {
                CCOMPANY_ID = _clientHelper.CompanyId,
                CPROPERTY_ID = _TXR00100ViewModel.PropertyDefault,
                CTAX_PERIOD_YEAR = _TXR00100ViewModel.PeriodYear.ToString(),
                CTAX_PERIOD_MONTH = _TXR00100ViewModel.PeriodMonthDefault,
                CWH_TAX_TYPE = _TXR00100ViewModel.WHTaxRadioSelected,
                CSORT_BY = _TXR00100ViewModel.SortByRadioSelected,
                CUSER_LOGIN = _clientHelper.UserId
            };
                
            await _reportService.GetReport(
                "R_DefaultServiceUrlTX",
                "TX",
                "rpt/TXR00100Print/DownloadResultPrintPost",
                "rpt/TXR00100Print/ReportListGet",
                loParam);

                
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        
        loEx.ThrowExceptionIfErrors();
    }
}