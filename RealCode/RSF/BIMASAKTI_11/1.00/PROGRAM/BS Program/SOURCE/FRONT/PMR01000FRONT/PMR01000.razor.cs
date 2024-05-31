using System.Globalization;
using BlazorClientHelper;
using Microsoft.AspNetCore.Components;
using PMR01000Common.DTO_s;
using PMR01000Common.DTO_s.PrintDTO;
using PMR01000MODEL;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using R_CommonFrontBackAPI;
using R_LockingFront;

namespace PMR01000FRONT;

public partial class PMR01000 : R_Page
{
    private PMR01000ViewModel _PMR01000ViewModel = new();
    private R_Conductor _ConductorRef;
    
    [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_IReport _reportService { get; set; }
    #region LockUnlock
    private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlPM";
    private const string DEFAULT_MODULE_NAME = "PM";
    protected async override Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        var llRtn = false;
        R_LockingFrontResult loLockResult = null;

        try
        {
            var loData = R_FrontUtility.ConvertObjectToObject<PMR01000DTO>(eventArgs.Data);

            var loCls = new R_LockingServiceClient(pcModuleName: DEFAULT_MODULE_NAME,
                plSendWithContext: true,
                plSendWithToken: true,
                pcHttpClientName: DEFAULT_HTTP_NAME);

            if (eventArgs.Mode == R_eLockUnlock.Lock)
            {
                var loLockPar = new R_ServiceLockingLockParameterDTO
                {
                    Company_Id = _clientHelper.CompanyId,
                    User_Id = _clientHelper.UserId,
                    Program_Id = "PMR01000",
                    Table_Name = "PMT_AGREEMENT_DEPOSIT",
                    Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CDEPOSIT_ID)
                };

                loLockResult = await loCls.R_Lock(loLockPar);
            }
            else
            {
                var loUnlockPar = new R_ServiceLockingUnLockParameterDTO
                {
                    Company_Id = _clientHelper.CompanyId,
                    User_Id = _clientHelper.UserId,
                    Program_Id = "PMR01000",
                    Table_Name = "PMT_AGREEMENT_DEPOSIT",
                    Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CDEPOSIT_ID)
                };

                loLockResult = await loCls.R_UnLock(loUnlockPar);
            }

            llRtn = loLockResult.IsSuccess;
            if (!loLockResult.IsSuccess && loLockResult.Exception != null)
                throw loLockResult.Exception;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();

        return llRtn;
    }
    #endregion
    
    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {
            await _PMR01000ViewModel.GetAllUniversalData();
            // Set FromPeriodMonth and ToPeriodMonth to the name of the current month
            DateTime today = DateTime.Today;
              List<string> monthNames = CultureInfo.CurrentCulture.DateTimeFormat
                    .MonthNames
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList();
              _PMR01000ViewModel.FromPeriodMonth = _PMR01000ViewModel.PeriodDetailList.FirstOrDefault().CPERIOD_NO;
            _PMR01000ViewModel. ToPeriodMonth = _PMR01000ViewModel.PeriodDetailList.LastOrDefault().CPERIOD_NO;
            _PMR01000ViewModel.DepositTypeSelected = _PMR01000ViewModel.DepositType.FirstOrDefault().CDEPOSIT_TYPE;
            _PMR01000ViewModel.ToTypeSelected = _PMR01000ViewModel.ToType.FirstOrDefault().Id;
            _PMR01000ViewModel.FromTypeSelected = _PMR01000ViewModel.FromType.FirstOrDefault().Id;
            _PMR01000ViewModel.ToTransTypeSelected = _PMR01000ViewModel.ToTransTypeList.FirstOrDefault().Id;
            _PMR01000ViewModel.FromTransTypeSelected = _PMR01000ViewModel.FromTransTypeList.FirstOrDefault().Id;
            await _PMR01000ViewModel.GetPropertyList();
            _PMR01000ViewModel.loProperty = _PMR01000ViewModel.PropertyDefault;
            await _PMR01000ViewModel.GetBuildingList();

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task OnChangedProperty(object poParam)
    {
        var loEx = new R_Exception();
        try
        {
            _PMR01000ViewModel.loProperty = _PMR01000ViewModel.PropertyDefault;
            await _PMR01000ViewModel.GetBuildingList();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private bool EnablePeriod;
    private bool EnableCutOffDate;
    private bool EnableBuilding;
    private bool EnableType;
    private bool EnableTransType;

    private async Task Button_OnClickProcessAsync()
    {
        var loEx = new R_Exception();
        PMR01000PrintParamDTO loParam;

        try
        {
            loParam = new PMR01000PrintParamDTO()
            {
                CCOMPANY_ID = _clientHelper.CompanyId,
                CPROPERTY_ID = _PMR01000ViewModel.PropertyDefault,
                CDEPOSIT_TYPE = _PMR01000ViewModel.DepositTypeSelected,
                CFROM_PERIOD = _PMR01000ViewModel.FromPeriodYear.ToString() + _PMR01000ViewModel.FromPeriodMonth,
                CTO_PERIOD = _PMR01000ViewModel.ToPeriodYear.ToString() + _PMR01000ViewModel.ToPeriodMonth,
                CFROM_BUILDING = _PMR01000ViewModel.FromBuildingDefault,
                CTO_BUILDING = _PMR01000ViewModel.ToBuildingDefault,
                CCUT_OFF_DATE = _PMR01000ViewModel.CutOffDate.ToString("yyyyMMdd"),
                CTO_TYPE = _PMR01000ViewModel.ToTypeSelected,
                CFROM_TYPE = _PMR01000ViewModel.FromTypeSelected,
                CTO_TRANS_TYPE = _PMR01000ViewModel.ToTransTypeSelected,
                CFROM_TRANS_TYPE = _PMR01000ViewModel.FromTransTypeSelected
            };

            if (loParam.CDEPOSIT_TYPE == "1")
            {
                loParam.CCUT_OFF_DATE = "";
                loParam.CTO_TYPE = "";
                loParam.CFROM_TYPE = "";
                loParam.CTO_TRANS_TYPE  = "";
                loParam.CFROM_TRANS_TYPE = "";
                
                await _reportService.GetReport(
                    "R_DefaultServiceUrlPM",
                    "PM",
                    "rpt/PMR01001Print/DownloadResultPrintPost",
                    "rpt/PMR01001Print/DepositReportListGet",
                    loParam);
            }
            else if (loParam.CDEPOSIT_TYPE == "2")
            {
                loParam.CFROM_PERIOD = "";
                loParam.CTO_PERIOD = "";
                loParam.CFROM_BUILDING = "";
                loParam.CTO_BUILDING = "";
                loParam.CTO_TYPE = "";
                loParam.CFROM_TYPE = "";
                loParam.CTO_TRANS_TYPE  = "";
                loParam.CFROM_TRANS_TYPE = "";
                
                await _reportService.GetReport(
                    "R_DefaultServiceUrlPM",
                    "PM",
                    "rpt/PMR01002Print/DownloadResultPrintPost",
                    "rpt/PMR01002Print/DepositReportOutStandingGet",
                    loParam);
            }
            else
            {
                loParam.CCUT_OFF_DATE = "";
                await _reportService.GetReport(
                    "R_DefaultServiceUrlPM",
                    "PM",
                    "rpt/PMR01003Print/DownloadResultPrintPost",
                    "rpt/PMR01003Print/DepositReportActivityGet",
                    loParam);
            }

     
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
}