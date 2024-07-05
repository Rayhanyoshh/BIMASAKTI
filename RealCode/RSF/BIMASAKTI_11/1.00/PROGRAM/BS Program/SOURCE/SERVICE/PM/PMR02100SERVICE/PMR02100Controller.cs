using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using R_CommonFrontBackAPI;
using R_BackEnd;
using PMR02100Common;
using PMR02100Back;
using PMR02100Common.DTOs;
namespace PMR02100SERVICE;


[ApiController]
[Route("api/[controller]/[action]")]
public class PMR02100Controller: ControllerBase, IPMR02100
{
    private LoggerPMR02100 _logger;
    private readonly ActivitySource _activitySource;
    
    public PMR02100Controller(ILogger<PMR02100Controller> logger)
    {
        //Initial and Get Logger
        LoggerPMR02100.R_InitializeLogger(logger);
        _logger = LoggerPMR02100.R_GetInstanceLogger();
        _activitySource = PMR02100Activity.R_InitializeAndGetActivitySource(nameof(PMR02100Controller));

    }
    
    [HttpPost]
    public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<PropertyListDTO> loRtn = null;
        PMR02100Cls loCls;
        PMR02100DTO loPar;
        List<PropertyListDTO> loRtnTmp;

        try
        {
            loPar = new PMR02100DTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_ID = R_BackGlobalVar.USER_ID;

            loCls = new PMR02100Cls();

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