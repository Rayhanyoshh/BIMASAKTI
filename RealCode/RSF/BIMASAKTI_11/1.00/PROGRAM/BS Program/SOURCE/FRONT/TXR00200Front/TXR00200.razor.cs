using BlazorClientHelper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;
using TXR00200Common.PrintDTO;
using TXR00200MODEL.ViewModel;

namespace TXR00200Front;

public partial class TXR00200 : R_Page
{
    private TXR00200ViewModel _TXR00200ViewModel = new();
    private R_Conductor _ConductorRef;
    
    [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_IReport _reportService { get; set; }
    [Inject] R_IExcel ExcelInject { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    
    #region init

    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {
            await _TXR00200ViewModel.GetPropertyList();
            await _TXR00200ViewModel.GetPeriodDetailList();
            _TXR00200ViewModel.TaxTypeRadioSelected = _TXR00200ViewModel.TaxTypeRadio.FirstOrDefault().CTAX_TYPE;
            if(_TXR00200ViewModel.TaxTypeRadioSelected == "O")
            {
                _TXR00200ViewModel.TransCodeComboBoxList = _TXR00200ViewModel.TransCodeComboBox;
            }
            else
            {
                _TXR00200ViewModel.TransCodeComboBoxList = _TXR00200ViewModel.TransCodeComboBox1;
            }
            _TXR00200ViewModel.TransCodeSelected = _TXR00200ViewModel.TransCodeComboBoxList.FirstOrDefault().CTRANS_CODE;
         
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
                CPROPERTY_ID = _TXR00200ViewModel.PropertyDefault,
                CTAX_PERIOD_YEAR = _TXR00200ViewModel.PeriodYear.ToString(),
                CTAX_PERIOD_MONTH = _TXR00200ViewModel.PeriodMonthDefault,
                CTAX_TYPE = _TXR00200ViewModel.TaxTypeRadioSelected,
                CTRANS_CODE = _TXR00200ViewModel.TransCodeSelected,
                CUSER_LOGIN = _clientHelper.UserId
            };
                
            await _TXR00200ViewModel.GetEfakturList(loParam);
            var loByte = ExcelInject.R_WriteToExcel(_TXR00200ViewModel.ExcelDataSet);
            var lcName = $"EFaktur" + ".csv";

            await JSRuntime.downloadFileFromStreamHandler(lcName, loByte);
            

                
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        
        loEx.ThrowExceptionIfErrors();
    }
    
    private void OnChangeTaxType(object poParam)
    {
        var loEx = new R_Exception();
        try
        {
            if(poParam == "O")
            {
                _TXR00200ViewModel.TransCodeComboBoxList = _TXR00200ViewModel.TransCodeComboBox;
                _TXR00200ViewModel.TransCodeSelected = _TXR00200ViewModel.TransCodeComboBoxList.FirstOrDefault().CTRANS_CODE;
            }
            else
            {
                _TXR00200ViewModel.TransCodeComboBoxList = _TXR00200ViewModel.TransCodeComboBox1;
                _TXR00200ViewModel.TransCodeSelected = _TXR00200ViewModel.TransCodeComboBoxList.FirstOrDefault().CTRANS_CODE;
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    

}