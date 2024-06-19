using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using R_CommonFrontBackAPI;
using R_BackEnd;
using PMR02200Common;
using PMR02200Back;
using PMR02200Common.DTOs;
namespace PMR02200SERVICE;


[ApiController]
[Route("api/[controller]/[action]")]
public class PMR02200Controller: ControllerBase, IPMR02200
{
    private LoggerPMR02200 _logger;
    private readonly ActivitySource _activitySource;
    
    public PMR02200Controller(ILogger<PMR02200Controller> logger)
    {
        //Initial and Get Logger
        LoggerPMR02200.R_InitializeLogger(logger);
        _logger = LoggerPMR02200.R_GetInstanceLogger();
        _activitySource = PMR02200Activity.R_InitializeAndGetActivitySource(nameof(PMR02200Controller));

    }
    
    [HttpPost]
    public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<PropertyListDTO> loRtn = null;
        PMR02200Cls loCls;
        PMR02200DTO loPar;
        List<PropertyListDTO> loRtnTmp;

        try
        {
            loPar = new PMR02200DTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_ID = R_BackGlobalVar.USER_ID;

            loCls = new PMR02200Cls();

            loRtnTmp = loCls.PropertyListDB(loPar);

            loRtn = GetPropertyStream(loRtnTmp);

        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loRtn;
    }
    
    [HttpPost]
    public PMR02200RecordResult<PMR02200PeriodCompanyDTO> GetPeriodYearRange()
    {
        using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        var loEx = new R_Exception();
        PMR02200RecordResult<PMR02200PeriodCompanyDTO> loRtn = new PMR02200RecordResult<PMR02200PeriodCompanyDTO>();
        try
        {
            var loCls = new PMR02200Cls();
            loRtn.Data = loCls.GetYearRange();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
        return loRtn;
    }
    
    #region Helper
    private async IAsyncEnumerable<PropertyListDTO> GetPropertyStream(List<PropertyListDTO> poParameter)
    {
        foreach (PropertyListDTO item in poParameter)
        {
            yield return item;
        }
    }
    #endregion
}