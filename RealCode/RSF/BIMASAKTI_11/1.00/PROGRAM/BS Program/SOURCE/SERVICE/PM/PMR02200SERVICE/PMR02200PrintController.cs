using System.Collections;
using System.Diagnostics;
using System.Globalization;
using BaseHeaderReportCOMMON;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMR02200Back;
using PMR02200Common;
using PMR02200Common.DTOs;
using PMR02200Common.DTOs.PrintDTO;
using R_BackEnd;
using R_Cache;
using R_Common;
using R_CommonFrontBackAPI;
using R_CommonFrontBackAPI.Log;
using R_ReportFastReportBack;

namespace PMR02200SERVICE;

public class PMR02200PrintController: R_ReportControllerBase
{
    private LoggerPMR02200Print _logger;
    private R_ReportFastReportBackClass _ReportCls;
    private PMR02200PrintParamDTO _Parameter;
    private readonly ActivitySource _activitySource;
    
    #region instantiation

    public PMR02200PrintController(ILogger<LoggerPMR02200Print> logger)
    {
        LoggerPMR02200Print.R_InitializeLogger(logger);
        _logger = LoggerPMR02200Print.R_GetInstanceLogger();
        _activitySource = PMR02200Activity.R_InitializeAndGetActivitySource(nameof(PMR02200PrintController));


        _ReportCls = new R_ReportFastReportBackClass();
        _ReportCls.R_InstantiateMainReportWithFileName += _ReportCls_R_InstantiateMainReportWithFileName;
        _ReportCls.R_GetMainDataAndName += _ReportCls_R_GetMainDataAndName;
        _ReportCls.R_SetNumberAndDateFormat += _ReportCls_R_SetNumberAndDateFormat;
    }
    #endregion
    
    #region Event Handler

    private void _ReportCls_R_InstantiateMainReportWithFileName(ref string pcFileTemplate)
    {
        pcFileTemplate = System.IO.Path.Combine("Reports", "PMR02200.frx");
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
    public R_DownloadFileResultDTO DownloadResultPrintPost (PMR02200PrintParamDTO poParameter)
    {
        using Activity activity = _activitySource.StartActivity("DownloadResultPrintPost");

        _logger.LogInfo("Start - Post DownloadResultPrintPost Status");
        R_Exception loException = new R_Exception();
        PMR02200PrintLogKeyDTO loCache = null;
        R_DownloadFileResultDTO loRtn = null;
        try
        {
            loRtn = new R_DownloadFileResultDTO();
            loCache = new PMR02200PrintLogKeyDTO
            {
                poParam = poParameter,
                poLogKey = (R_NetCoreLogKeyDTO)R_NetCoreLogAsyncStorage.GetData(R_NetCoreLogConstant.LOG_KEY)
            };
            _logger.LogInfo("Set GUID Param - Post DownloadResultPrintPost Status");
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
    public FileStreamResult StatementAccountReportListGet(string pcGuid)
    {
        using Activity activity = _activitySource.StartActivity("DepositReport");
        R_Exception loException = new R_Exception();
        FileStreamResult loRtn = null;
        PMR02200PrintLogKeyDTO loResultGUID = null;
        try
        {
            // Deserialize the GUID from the cache
            loResultGUID = R_NetCoreUtility.R_DeserializeObjectFromByte<PMR02200PrintLogKeyDTO>(
                R_DistributedCache.Cache.Get(pcGuid));
            _logger.LogDebug("Deserialized GUID: {pcGuid}", pcGuid);

            // Get Parameter
            R_NetCoreLogUtility.R_SetNetCoreLogKey(loResultGUID.poLogKey);

            _Parameter = loResultGUID.poParam;

            _logger.LogDebug("Deserialized GUID: {pcGuid}", pcGuid);
            _logger.LogDebug("Deserialized Parameters: {@Parameters}", _Parameter);

            loRtn = new FileStreamResult(_ReportCls.R_GetStreamReport(), R_ReportUtility.GetMimeType(R_FileType.PDF));
            _logger.LogInfo("Data retrieval successful. Generating report.");

            _logger.LogInfo("Report generated successfully.");
        }
        catch (Exception ex)
        {
            loException.Add(ex);
            _logger.LogError(loException);
        }

        loException.ThrowExceptionIfErrors();
        _logger.LogInfo("End - Deposit Report Generation");
        return loRtn;
    }
    
    #region Helper

    private PMR02200PrintResultWithBaseHeaderPrintDTO GenerateDataPrint(PMR02200PrintParamDTO poParam)
    {
        using Activity activity = _activitySource.StartActivity("GenerateDataPrint");
        
        var loEx = new R_Exception();
        PMR02200PrintResultWithBaseHeaderPrintDTO loRtn = new PMR02200PrintResultWithBaseHeaderPrintDTO();
        var loParam = new BaseHeaderDTO();

        System.Globalization.CultureInfo loCultureInfo =
            new System.Globalization.CultureInfo(R_BackGlobalVar.REPORT_CULTURE);

        try
        {
            _logger.LogInfo("_logger.LogInfo(\"Start - Generating data for Print\");\n data for Deposit report.");

            //Add Resources
            loRtn.BaseHeaderColumn.Page = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Page", loCultureInfo);
            loRtn.BaseHeaderColumn.Of =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Of", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_Date =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Print_Date",
                    loCultureInfo);
            loRtn.BaseHeaderColumn.Print_By = R_Utility.R_GetMessage(
                typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Print_By", loCultureInfo);

            PMR02200PrintColumnDTO loColumnObject = new PMR02200PrintColumnDTO();
            var loColumn = AssignValuesWithMessages(typeof(PMR02200BackResources.Resources_Dummy_Class),
                loCultureInfo, loColumnObject);

            // Set base header data
            _logger.LogDebug("Deserialized Print Parameters: {@PrintParameters}");
            var loCls = new PMR02200Cls();
            poParam.CLANG_ID = R_BackGlobalVar.CULTURE;

            loParam.CCOMPANY_NAME = R_BackGlobalVar.COMPANY_ID.ToUpper();
            loParam.CPRINT_CODE = "001";
            loParam.CPRINT_NAME = "Deposit List Report";
            loParam.CUSER_ID = R_BackGlobalVar.USER_ID.ToUpper();
            loParam.BLOGO_COMPANY = loCls.GetBaseHeaderLogoCompany(poParam.CCOMPANY_ID).CLOGO;

            // Create an instance of PMR01000PrintGOAResultDTo
            poParam.DSTATEMENT_DATE = DateTime.ParseExact(poParam.CSTATEMENT_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
            poParam.DCUT_OFF_DATE = DateTime.ParseExact(poParam.CCUT_OFF_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);

            PMR02200PrintResultDTO loData = new PMR02200PrintResultDTO()
            {
                Title = "Deposit Type List",
                Header = "Deposit Type List",
                Column = (PMR02200PrintColumnDTO)loColumn,
                DataResult = new List<PMR02200DTO>(),
                HeaderParam = poParam
            };
            
            

            // Create an instance of PMR01000Cls
            // Get print data for Group Of Account report
            var loCollection = loCls.GetPrintDataResult(poParam);
            _logger.LogInfo("Data generation successful. Processing data for printing.");
            
            // Set the generated data in loRtn
            _logger.LogInfo("Data processed successfully. Generating print output.");
            loData.DataResult = loCollection;
            loData.Param = poParam;
            loRtn.Data1 = loData;
            loRtn.BaseHeaderData = loParam;
            
            _logger.LogInfo("Print output generated successfully. Saving print file.");
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
            _logger.LogError(loEx);
        }

        loEx.ThrowExceptionIfErrors();
        return loRtn;
        _logger.LogInfo("End - Data Generation for Print");
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