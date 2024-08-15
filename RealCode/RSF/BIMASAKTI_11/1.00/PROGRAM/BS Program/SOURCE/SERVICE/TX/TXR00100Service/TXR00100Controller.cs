
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMR02200Common;
using R_Common;
using R_CommonFrontBackAPI;
using R_BackEnd;
using TXR00100Back;
using TXR00100Common;
using TXR00100Common.DTOs;
using TXR00100Common.PrintDTO;

namespace TXR00100SERVICE;


[ApiController]
[Route("api/[controller]/[action]")]
public class TXR00100Controller: ControllerBase, ITXR00100
{
    private LoggerTXR00100 _logger;
    private readonly ActivitySource _activitySource;
    
    public TXR00100Controller(ILogger<TXR00100Controller> logger)
    {
        //Initial and Get Logger
        LoggerTXR00100.R_InitializeLogger(logger);
        _logger = LoggerTXR00100.R_GetInstanceLogger();
        _activitySource = TXR00100Activity.R_InitializeAndGetActivitySource(nameof(TXR00100Controller));

    }
    
    [HttpPost]
    public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<PropertyListDTO> loRtn = null;
        TXR00100Cls loCls;
        PrintParamTXDTO loPar;
        List<PropertyListDTO> loRtnTmp;

        try
        {
            loPar = new PrintParamTXDTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_LOGIN = R_BackGlobalVar.USER_ID;

            loCls = new TXR00100Cls();

            loRtnTmp = loCls.PropertyListDB(loPar);

            loRtn = HelperStream(loRtnTmp);

        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loRtn;
    }

    [HttpPost]
    public IAsyncEnumerable<TXR00100PeriodDetailDTO> GetPeriodDetailList(string poParameter)
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<TXR00100PeriodDetailDTO> loRtn = null;
        TXR00100Cls loCls;
        PrintParamTXDTO loPar;
        List<TXR00100PeriodDetailDTO> loRtnTmp;

        try
        {
            loPar = new PrintParamTXDTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_LOGIN = R_BackGlobalVar.USER_ID;
            loPar.CTAX_PERIOD_YEAR =  R_Utility.R_GetStreamingContext<string>(ContextConstant.CYEAR);;

            loCls = new TXR00100Cls();

            loRtnTmp = loCls.PeriodDetailListDB(loPar);

            loRtn = HelperStream(loRtnTmp);

        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loRtn;
    }


    #region Helper
    private async IAsyncEnumerable<T> HelperStream<T>(List<T> poParameter)
    {
        foreach (T item in poParameter)
        {
            yield return item;
        }
    }
    #endregion
}