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
    private readonly ActivitySource _activitySource;


    #region instantiation

    public GSM01000PrintController(ILogger<LoggerGSM01000Print> logger)
    {
        LoggerGSM01000Print.R_InitializeLogger(logger);
        _logger = LoggerGSM01000Print.R_GetInstanceLogger();
        _activitySource = GSM01000Activity.R_InitializeAndGetActivitySource(nameof(GSM01000PrintController));


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
        using Activity activity = _activitySource.StartActivity("AllCOAPost");

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
        using Activity activity = _activitySource.StartActivity("ChartOfAccountReport");

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
        using Activity activity = _activitySource.StartActivity("GenerateDataPrint");
        var loEx = new R_Exception();
     
        GSM01000PrintCOAResultWithBaseHeaderPrintDTO loRtn = new GSM01000PrintCOAResultWithBaseHeaderPrintDTO();
   

        System.Globalization.CultureInfo loCultureInfo =
            new System.Globalization.CultureInfo(R_BackGlobalVar.REPORT_CULTURE);
        
        try
        {
            _logger.LogInfo("Generating data for Chart Of Account report.");
            //Add Resources
            loRtn.BaseHeaderColumn.Page = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Page", loCultureInfo);
            loRtn.BaseHeaderColumn.Of =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Of", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_Date =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Print_Date", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_By = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Print_By", loCultureInfo);
            
            GSM01000PrintColoumnDTO loColumnObject = new GSM01000PrintColoumnDTO();
            var loColumn = AssignValuesWithMessages(typeof(GSM01000BackResources.Resources_Dummy_Class),
                loCultureInfo, loColumnObject);
            var loCls = new GSM01000Cls();

            var loCollection = loCls.GetPrintDataResult(poParam);
                
            _logger.LogInfo("Set Base Header Data");
            // Set Base Header Data
            var loParam = new BaseHeaderDTO()
            {
                CCOMPANY_NAME = poParam.CCOMPANY_ID.ToUpper(),
                CPRINT_CODE = "GSM01000",
                CPRINT_NAME = "Chart Of Account",
                CUSER_ID = poParam.CUSER_LOGIN_ID.ToUpper(),
                BLOGO_COMPANY = loCls.GetBaseHeaderLogoCompany(poParam.CCOMPANY_ID).CLOGO

            };
            GSM01000PrintCOAResultDTo loData = new GSM01000PrintCOAResultDTo()
            {
                Title = "Chart Of Accounts",
                Column = (GSM01000PrintColoumnDTO)loColumn,
                Data = new List<GSM01000ResultSPPrintCOADTO>()
            };
          
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

