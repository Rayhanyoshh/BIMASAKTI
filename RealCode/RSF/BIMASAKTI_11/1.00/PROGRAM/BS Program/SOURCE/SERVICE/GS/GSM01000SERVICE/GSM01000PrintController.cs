using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Cache;
using R_Common;
using R_CommonFrontBackAPI;
using R_ReportFastReportBack;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using GSM01000Common;
using GSM01000Back;
using BaseHeaderReportCOMMON;
using BaseHeaderReportCOMMON.Models;
using GSM001000Back;
using GSM01000Common.DTOs;
using GSM01000Service.DTOs;
using Microsoft.Extensions.Logging;
using R_CommonFrontBackAPI.Log;

namespace GSM01000Service;

public class GSM01000PrintController : R_ReportControllerBase
{
    private LoggerGSM01000Print _logger;
    private R_ReportFastReportBackClass _ReportCls;
    private GSM01000PrintParamCOADTO _Parameter;

    #region instantiation

    public GSM01000PrintController(ILogger<LoggerGSM01000Print> logger)
    {
        LoggerGSM01000Print.R_InitializeLogger(logger);
        _logger = LoggerGSM01000Print.R_GetInstanceLogger();

        _ReportCls = new R_ReportFastReportBackClass();
        _ReportCls.R_InstantiateMainReportWithFileName += _ReportCls_R_InstantiateMainReportWithFileName;
        _ReportCls.R_GetMainDataAndName += _ReportCls_R_GetMainDataAndName;
        _ReportCls.R_SetNumberAndDateFormat += _ReportCls_R_SetNumberAndDateFormat;
    }

    #endregion

    #region Event Handler

    private void _ReportCls_R_InstantiateMainReportWithFileName(ref string pcFileTemplate)
    {
        pcFileTemplate = System.IO.Path.Combine("Reports", "GSM01000.frx");
    }

    private void _ReportCls_R_GetMainDataAndName(ref ArrayList poData, ref string pcDataSourceName)
    {
        poData.Add(GenerateDataPrint(_Parameter));
        pcDataSourceName = "ResponseDataModel";
    }

    private void _ReportCls_R_SetNumberAndDateFormat(ref R_ReportFormatDTO poReportFormat)
    {
        poReportFormat.DecimalSeparator = R_BackGlobalVar.REPORT_FORMAT_DECIMAL_SEPARATOR;
        poReportFormat.GroupSeparator = R_BackGlobalVar.REPORT_FORMAT_GROUP_SEPARATOR;
        poReportFormat.DecimalPlaces = R_BackGlobalVar.REPORT_FORMAT_DECIMAL_PLACES;
        poReportFormat.ShortDate = R_BackGlobalVar.REPORT_FORMAT_SHORT_DATE;
        poReportFormat.ShortTime = R_BackGlobalVar.REPORT_FORMAT_SHORT_TIME;
    }

    #endregion

    [HttpPost]
    public R_DownloadFileResultDTO AllCOAPost(GSM01000PrintParamCOADTO poParameter)
    {
        _logger.LogInfo("Start - Post COA Status");
        R_Exception loException = new R_Exception();
        GSM01000PrintLogKeyDTO loCache = null;
        R_DownloadFileResultDTO loRtn = null;
        try
        {
            loRtn = new R_DownloadFileResultDTO();
            loCache = new GSM01000PrintLogKeyDTO
            {
                poParam = poParameter,
                poLogKey = (R_NetCoreLogKeyDTO)R_NetCoreLogAsyncStorage.GetData(R_NetCoreLogConstant.LOG_KEY)
            };
            _logger.LogInfo("Set GUID Param - Post COA Status");
            R_DistributedCache.R_Set(loRtn.GuidResult, R_NetCoreUtility.R_SerializeObjectToByte(loCache));
        }
        catch (Exception ex)
        {
            loException.Add(ex);
            _logger.LogError(loException);
        }

        loException.ThrowExceptionIfErrors();
        _logger.LogInfo("End - Post COA Status");
        return loRtn;
    }

    [HttpGet, AllowAnonymous]
    public FileStreamResult ChartOfAccountReport(string pcGuid)
    {
        _logger.LogInfo("Start - Get COA Status");
        R_Exception loException = new R_Exception();
        FileStreamResult loRtn = null;
        GSM01000PrintLogKeyDTO loResultGUID = null;
        try
        {
            // Deserialize the GUID from the cache
            loResultGUID = R_NetCoreUtility.R_DeserializeObjectFromByte<GSM01000PrintLogKeyDTO>(
                R_DistributedCache.Cache.Get(pcGuid));

            // Get Parameter
            R_NetCoreLogUtility.R_SetNetCoreLogKey(loResultGUID.poLogKey);

            _Parameter = loResultGUID.poParam;

            _logger.LogDebug("Deserialized GUID: {pcGuid}", pcGuid);
            _logger.LogDebug("Deserialized Parameters: {@Parameters}", _Parameter);

            loRtn = new FileStreamResult(_ReportCls.R_GetStreamReport(), R_ReportUtility.GetMimeType(R_FileType.PDF));
        
            _logger.LogInfo("Report generated successfully.");
        }
        catch (Exception ex)
        {
            loException.Add(ex);
            _logger.LogError(loException);
        }

        loException.ThrowExceptionIfErrors();
        _logger.LogInfo("End - Get COA Status");
        return loRtn;
    }

    #region Helper
    private GSM01000PrintCOAResultWithBaseHeaderPrintDTO GenerateDataPrint(GSM01000PrintParamCOADTO poParam)
    {
        var loEx = new R_Exception();
        GSM01000PrintCOAResultWithBaseHeaderPrintDTO loRtn = new GSM01000PrintCOAResultWithBaseHeaderPrintDTO();

        try
        {
            var loCls = new GSM01000Cls();

            var loCollection = loCls.GetPrintDataResult(poParam);
                
            _logger.LogInfo("Set Base Header Data");
            // Set Base Header Data
            var loParam = new BaseHeaderDTO()
            {

                
                CCOMPANY_NAME = "PT Realta Chackradarma",
                CPRINT_CODE = poParam.CCOMPANY_ID.ToUpper(),
                CPRINT_NAME = "Chart Of Account",
                CUSER_ID = poParam.CUSER_LOGIN_ID.ToUpper(),
            };
            
            var loData = new GSM01000PrintCOAResultDTo();
            _logger.LogInfo("Set Parameter");
            loData.Header = $"{poParam.CCOMPANY_ID}";
            
            
            _logger.LogInfo("Get Detail COA Report");
            loData.Data = loCollection;
            loData.ParamDTO = poParam;
            loRtn.BaseHeaderData = loParam;
            loRtn.CenterData = loData;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
            _logger.LogError(loEx);

        }

        loEx.ThrowExceptionIfErrors();

        return loRtn;
    }
    #endregion
}

