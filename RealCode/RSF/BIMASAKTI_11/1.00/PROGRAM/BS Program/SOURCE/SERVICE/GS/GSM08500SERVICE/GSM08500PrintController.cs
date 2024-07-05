using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Cache;
using R_Common;
using R_CommonFrontBackAPI;
using R_ReportFastReportBack;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using BaseHeaderReportCOMMON;
using BaseHeaderReportCOMMON.Models;
using GSM008500Common;
using GSM008500Common.DTOs;
using GSM008500Common.DTOs.PrintDTO;
using GSM08500Back;
using GSM08500Service.DTOs;
using Microsoft.Extensions.Logging;
using R_CommonFrontBackAPI.Log;

namespace GSM08500Service;

public class GSM08500PrintController : R_ReportControllerBase
{
    private LoggerGSM08500Print _logger;
    private R_ReportFastReportBackClass _ReportCls;
    private GSM08500PrintParamStatAccDTO _Parameter;
    private readonly ActivitySource _activitySource;

    
    #region instantiation

    public GSM08500PrintController(ILogger<LoggerGSM08500Print> logger)
    {
        LoggerGSM08500Print.R_InitializeLogger(logger);
        _logger = LoggerGSM08500Print.R_GetInstanceLogger();
        _activitySource = GSM08500Activity.R_InitializeAndGetActivitySource(nameof(GSM08500PrintController));


        _ReportCls = new R_ReportFastReportBackClass();
        _ReportCls.R_InstantiateMainReportWithFileName += _ReportCls_R_InstantiateMainReportWithFileName;
        _ReportCls.R_GetMainDataAndName += _ReportCls_R_GetMainDataAndName;
        _ReportCls.R_SetNumberAndDateFormat += _ReportCls_R_SetNumberAndDateFormat;
    }

    #endregion
    
    #region Event Handler

    private void _ReportCls_R_InstantiateMainReportWithFileName(ref string pcFileTemplate)
    {
        pcFileTemplate = System.IO.Path.Combine("Reports", "GSM08500.frx");
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
    public R_DownloadFileResultDTO StatAccountPost(GSM08500PrintParamStatAccDTO poParameter)
    {
        _logger.LogInfo("Start - Post StatAccount");
        R_Exception loException = new R_Exception();
        GSM08500PrintLogKeyDTO loCache = null;
        R_DownloadFileResultDTO loRtn = null;
        try
        {
            loRtn = new R_DownloadFileResultDTO();
            loCache = new GSM08500PrintLogKeyDTO
            {
                poParam = poParameter,
                poLogKey = (R_NetCoreLogKeyDTO)R_NetCoreLogAsyncStorage.GetData(R_NetCoreLogConstant.LOG_KEY)
            };
            _logger.LogInfo("Set GUID Param - Post StatAccount Status");
            R_DistributedCache.R_Set(loRtn.GuidResult, R_NetCoreUtility.R_SerializeObjectToByte(loCache));
        }
        catch (Exception ex)
        {
            loException.Add(ex);
            _logger.LogError(loException);
        }

        loException.ThrowExceptionIfErrors();
        _logger.LogInfo("End - Post Statistic Account");
        return loRtn;
    }

    [HttpGet, AllowAnonymous]
    public FileStreamResult StatisticAccountReport (string pcGuid)
    {
        _logger.LogInfo("Start - Get Statistic Account");
        R_Exception loException = new R_Exception();
        FileStreamResult loRtn = null;
        GSM08500PrintLogKeyDTO loResultGUID = null;
        try
        {
            // Deserialize the GUID from the cache
            loResultGUID = R_NetCoreUtility.R_DeserializeObjectFromByte<GSM08500PrintLogKeyDTO>(
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
        _logger.LogInfo("End - Get Statistic Account");
        return loRtn;
    }

    #region Helper
    private GSM08500PrintStatAccResultWithBaseHeaderPrintDTO GenerateDataPrint(GSM08500PrintParamStatAccDTO poParam)
    {
        using Activity activity = _activitySource.StartActivity("GenerateDataPrint");
        var loEx = new R_Exception();
        GSM08500PrintStatAccResultWithBaseHeaderPrintDTO loRtn = new GSM08500PrintStatAccResultWithBaseHeaderPrintDTO();
        System.Globalization.CultureInfo loCultureInfo =
            new System.Globalization.CultureInfo(R_BackGlobalVar.REPORT_CULTURE);
        try
        {
            _logger.LogInfo("Generating data for Statistic Account report.");
            //Add Resources
            loRtn.BaseHeaderColumn.Page = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Page", loCultureInfo);
            loRtn.BaseHeaderColumn.Of =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Of", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_Date =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Print_Date", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_By = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Print_By", loCultureInfo);
            
            GSM08500PrintColoumnStatAccDTO loColumnObject = new GSM08500PrintColoumnStatAccDTO();
            var loColumn = AssignValuesWithMessages(typeof(GSM08500BackResources.Resources_Dummy_Class),
                loCultureInfo, loColumnObject);
            var loCls = new GSM08500Cls();

            var loCollection = loCls.GetPrintDataResultStatAcc(poParam);
                
            _logger.LogInfo("Set Base Header Data");
            // Set Base Header Data
            var loParam = new BaseHeaderDTO()
            {
                CCOMPANY_NAME = "PT Realta Chackradarma",
                CPRINT_CODE = poParam.CCOMPANY_ID.ToUpper(),
                CUSER_ID = poParam.CUSER_LOGIN_ID.ToUpper(),
                CPRINT_NAME = "Statistic Account",
                BLOGO_COMPANY = loCls.GetBaseHeaderLogoCompany(poParam.CCOMPANY_ID).CLOGO
            };
            GSM01000PrintStatAccResultDTo loData = new GSM01000PrintStatAccResultDTo()
            {
                Title = "Deposit Type List",
                Header = "Deposit Type List",
                Column = (GSM08500PrintColoumnStatAccDTO)loColumn,
                Data = new List<GSM08500ResultSPPrintStatAccDTO>(),
            };
            
            _logger.LogInfo("Set Parameter");
            loData.Header = $"{poParam.CCOMPANY_ID}";
            
            
            _logger.LogInfo("Get Detail COA Analysis Report");
            loData.Data = loCollection;
            loRtn.BaseHeaderData = loParam;
            loRtn.StatAccountData = loData;
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
    
    private object AssignValuesWithMessages(Type poResourceType, CultureInfo poCultureInfo, object poObject)
    {
        object loObj = Activator.CreateInstance(poObject.GetType());
        var loGetPropertyObject = poObject.GetType().GetProperties();

        foreach (var property in loGetPropertyObject)
        {
            string propertyName = property.Name;
            string message = R_Utility.R_GetMessage(poResourceType, propertyName, poCultureInfo);
            property.SetValue(loObj, message);
        }

        return loObj;
    }
}