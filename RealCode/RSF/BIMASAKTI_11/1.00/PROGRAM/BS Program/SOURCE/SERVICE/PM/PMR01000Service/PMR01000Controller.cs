using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMR01000Back;
using PMR01000Common;
using PMR01000Common.DTO_s;
using R_Common;
using R_CommonFrontBackAPI;
using R_BackEnd;

namespace PMR01000Service;

[ApiController]
[Route("api/[controller]/[action]")]
public class PMR01000Controller : ControllerBase, IPMR01000
{
    private LoggerPMR01000 _logger;
    private readonly ActivitySource _activitySource;
    
    public PMR01000Controller(ILogger<PMR01000Controller> logger)
    {
        //Initial and Get Logger
        LoggerPMR01000.R_InitializeLogger(logger);
        _logger = LoggerPMR01000.R_GetInstanceLogger();
        _activitySource = PMR01000Activity.R_InitializeAndGetActivitySource(nameof(PMR01000Controller));

    }
    
    [HttpPost]
    public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<PropertyListDTO> loRtn = null;
        PMR01000Cls loCls;
        PMR01000DTO loPar;
        List<PropertyListDTO> loRtnTmp;

        try
        {
            loPar = new PMR01000DTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_ID = R_BackGlobalVar.USER_ID;

            loCls = new PMR01000Cls();

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
    public PMR01000RecordResult<PMR01000CBSystemParamDTO> GetCBSystemParam()
    {
        using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        var loEx = new R_Exception();
        PMR01000RecordResult<PMR01000CBSystemParamDTO> loRtn = new PMR01000RecordResult<PMR01000CBSystemParamDTO>();
        try
        {
            var loCls = new PMR01000Cls();
            loRtn.Data = loCls.GetCBSystemParamRecord();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
        return loRtn;
    }
    
    [HttpPost]
    public PMR01000RecordResult<PMR01000PeriodCompanyDTO> GetPeriodYearRange()
    {
        using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
        var loEx = new R_Exception();
        PMR01000RecordResult<PMR01000PeriodCompanyDTO> loRtn = new PMR01000RecordResult<PMR01000PeriodCompanyDTO>();
        try
        {
            var loCls = new PMR01000Cls();
            loRtn.Data = loCls.GetYearRange();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
        return loRtn;
    }
    
    [HttpPost]
    public IAsyncEnumerable<PMR01000PeriodDTDTO> GetPeriodDetailList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<PMR01000PeriodDTDTO> loRtn = null;
        PMR01000Cls loCls;
        PMR01000DTO loPar;
        List<PMR01000PeriodDTDTO> loRtnTmp;

        try
        {
            loPar = new PMR01000DTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_ID = R_BackGlobalVar.USER_ID;

            loCls = new PMR01000Cls();

            loRtnTmp = loCls.PeriodDetailCls(loPar);

            loRtn = GetPeriodDetailStream(loRtnTmp);

        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loRtn;
    }
    
    [HttpPost]
    public IAsyncEnumerable<PMR01000BuildingListDTO> GetBuildinglList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<PMR01000BuildingListDTO> loRtn = null;
        PMR01000Cls loCls;
        PMR01000DTO loPar;
        List<PMR01000BuildingListDTO> loRtnTmp;

        try
        {
            loPar = new PMR01000DTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CPROPERTY_ID = R_Utility.R_GetContext<string>(ContextConstant.CPROPERTY_ID);
            loPar.CUSER_ID = R_BackGlobalVar.USER_ID;

            loCls = new PMR01000Cls();

            loRtnTmp = loCls.BuildingListCLs(loPar);

            loRtn = GetBuildingListStream(loRtnTmp);

        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

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
    
    private async IAsyncEnumerable<PMR01000BuildingListDTO> GetBuildingListStream(List<PMR01000BuildingListDTO> poParameter)
    {
        foreach (PMR01000BuildingListDTO item in poParameter)
        {
            yield return item;
        }
    }
    
    private async IAsyncEnumerable<PMR01000PeriodDTDTO> GetPeriodDetailStream(List<PMR01000PeriodDTDTO> poParameter)
    {
        foreach (PMR01000PeriodDTDTO item in poParameter)
        {
            yield return item;
        }
    }

    #endregion
}