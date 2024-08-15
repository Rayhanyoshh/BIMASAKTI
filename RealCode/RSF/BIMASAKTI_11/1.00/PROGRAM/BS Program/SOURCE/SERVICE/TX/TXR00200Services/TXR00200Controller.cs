
using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMR02200Common;
using R_Common;
using R_CommonFrontBackAPI;
using R_BackEnd;
using TXR00200Back;
using TXR00200Common;
using TXR00200Common.DTOs;
using TXR00200Common.PrintDTO;

namespace TXR00200SERVICE;


[ApiController]
[Route("api/[controller]/[action]")]
public class TXR00200Controller: ControllerBase, ITXR00200
{
    private LoggerTXR00200 _logger;
    private readonly ActivitySource _activitySource;
    
    public TXR00200Controller(ILogger<TXR00200Controller> logger)
    {
        //Initial and Get Logger
        LoggerTXR00200.R_InitializeLogger(logger);
        _logger = LoggerTXR00200.R_GetInstanceLogger();
        _activitySource = TXR00200Activity.R_InitializeAndGetActivitySource(nameof(TXR00200Controller));

    }
    
    [HttpPost]
    public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<PropertyListDTO> loRtn = null;
        TXR00200Cls loCls;
        PrintParamTXDTO loPar;
        List<PropertyListDTO> loRtnTmp;

        try
        {
            loPar = new PrintParamTXDTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_LOGIN = R_BackGlobalVar.USER_ID;

            loCls = new TXR00200Cls();

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
    public IAsyncEnumerable<TXR00200DTO> GerEFakturList()
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<TXR00200DTO> loRtn = null;
        TXR00200Cls loCls;
        PrintParamTXDTO loPar;
        List<TXR00200DTO> loRtnTmp;

        try
        {
            loPar = new PrintParamTXDTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
            loPar.CTAX_PERIOD_YEAR = R_Utility.R_GetStreamingContext<string>(ContextConstant.CTAX_PERIOD_YEAR);
            loPar.CTAX_PERIOD_MONTH = R_Utility.R_GetStreamingContext<string>(ContextConstant.CTAX_PERIOD_MONTH);
            loPar.CTAX_TYPE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CTAX_TYPE);
            loPar.CTRANS_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CTRANS_CODE);
            loPar.CUSER_LOGIN = R_BackGlobalVar.USER_ID;

            loCls = new TXR00200Cls();

            loRtnTmp = loCls.GetPrintDataResult(loPar);

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
    public IAsyncEnumerable<TXR00200PeriodDetailDTO> GetPeriodDetailList(string poParameter)
    {
        R_Exception loException = new R_Exception();
        IAsyncEnumerable<TXR00200PeriodDetailDTO> loRtn = null;
        TXR00200Cls loCls;
        PrintParamTXDTO loPar;
        List<TXR00200PeriodDetailDTO> loRtnTmp;

        try
        {
            loPar = new PrintParamTXDTO();
            loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
            loPar.CUSER_LOGIN = R_BackGlobalVar.USER_ID;
            loPar.CTAX_PERIOD_YEAR =  R_Utility.R_GetStreamingContext<string>(ContextConstant.CYEAR);;

            loCls = new TXR00200Cls();

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