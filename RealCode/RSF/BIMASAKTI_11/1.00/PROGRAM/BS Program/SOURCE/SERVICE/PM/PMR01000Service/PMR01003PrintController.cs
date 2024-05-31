using System.Collections;
using System.Diagnostics;
using System.Globalization;
using BaseHeaderReportCOMMON;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMR01000Back;
using PMR01000Common;
using PMR01000Common.DTO_s.PrintDTO;
using R_BackEnd;
using R_Cache;
using R_Common;
using R_CommonFrontBackAPI;
using R_CommonFrontBackAPI.Log;
using R_ReportFastReportBack;

namespace PMR01000Service;

public class PMR01003PrintController  : R_ReportControllerBase
{
    private LoggerPMR01000Print _logger;
    private R_ReportFastReportBackClass _ReportCls;
    private PMR01000PrintParamDTO _Parameter;
    private readonly ActivitySource _activitySource;
    
    #region instantiation

    public PMR01003PrintController(ILogger<LoggerPMR01000Print> logger)
    {
        LoggerPMR01000Print.R_InitializeLogger(logger);
        _logger = LoggerPMR01000Print.R_GetInstanceLogger();
        _activitySource = PMR01000Activity.R_InitializeAndGetActivitySource(nameof(PMR01003PrintController));


        _ReportCls = new R_ReportFastReportBackClass();
        _ReportCls.R_InstantiateMainReportWithFileName += _ReportCls_R_InstantiateMainReportWithFileName;
        _ReportCls.R_GetMainDataAndName += _ReportCls_R_GetMainDataAndName;
        _ReportCls.R_SetNumberAndDateFormat += _ReportCls_R_SetNumberAndDateFormat;
    }
    #endregion
    
    #region Event Handler

    private void _ReportCls_R_InstantiateMainReportWithFileName(ref string pcFileTemplate)
    {
        pcFileTemplate = System.IO.Path.Combine("Reports", "PMR01003.frx");
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
    public R_DownloadFileResultDTO DownloadResultPrintPost (PMR01000PrintParamDTO poParameter)
    {
        using Activity activity = _activitySource.StartActivity("DownloadResultPrintPost");

        _logger.LogInfo("Start - Post DownloadResultPrintPost Status");
        R_Exception loException = new R_Exception();
        PMR01000PrintLogKeyDTO loCache = null;
        R_DownloadFileResultDTO loRtn = null;
        try
        {
            loRtn = new R_DownloadFileResultDTO();
            loCache = new PMR01000PrintLogKeyDTO
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
    public FileStreamResult DepositReportActivityGet (string pcGuid)
    {
        using Activity activity = _activitySource.StartActivity("DepositReport");

        _logger.LogInfo("Start - Retrieving data for Deposit Report");        R_Exception loException = new R_Exception();
        FileStreamResult loRtn = null;
        PMR01000PrintLogKeyDTO loResultGUID = null;
        try
        {
            // Deserialize the GUID from the cache
            loResultGUID = R_NetCoreUtility.R_DeserializeObjectFromByte<PMR01000PrintLogKeyDTO>(
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
    private PMR01003PrintResultWithBaseHeaderPrintDTO GenerateDataPrint(PMR01000PrintParamDTO poParam)
    {
         using Activity activity = _activitySource.StartActivity("GenerateDataPrint");

            var loEx = new R_Exception();
            PMR01003PrintResultWithBaseHeaderPrintDTO loRtn = new PMR01003PrintResultWithBaseHeaderPrintDTO();
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
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Print_Date", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_By = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Print_By", loCultureInfo);
            
            PMR01001PrintColoumnDTO loColumnObject = new PMR01001PrintColoumnDTO();
            var loColumn = AssignValuesWithMessages(typeof(PMR01000BackResources.Resources_Dummy_Class),
                loCultureInfo, loColumnObject);
            
            // Set base header data
            _logger.LogDebug("Deserialized Print Parameters: {@PrintParameters}");

            loParam.CCOMPANY_NAME = R_BackGlobalVar.COMPANY_ID.ToUpper();
            loParam.CPRINT_CODE = "001";
            loParam.CPRINT_NAME = "Deposit Activity Report";
            loParam.CUSER_ID = R_BackGlobalVar.USER_ID.ToUpper();
            
            // Create an instance of PMR01000PrintGOAResultDTo
            PMR01003PrintResultDTO loData = new PMR01003PrintResultDTO()
            {
                Title = "Deposit Type Activity",
                Header = "Deposit Type Activity",
                Column = new PMR01003PrintColoumnDTO(),
                Data = new List<PMR01003DataResultDTO>(),
                HeaderParam = poParam
            };

            // Create an instance of PMR01000Cls
            var loCls = new PMR01000Cls();

            // Get print data for Group Of Account report
            var loCollection = loCls.GetPrintDataResult(poParam);
            _logger.LogInfo("Data generation successful. Processing data for printing.");


            // Process the data and create a formatted list
             var loTempData = loCollection
                .GroupBy(data1a => new
                {
                    data1a.CDEPOSIT_ID,
                    data1a.CDEPOSIT_NAME,
                }).Select(data1b => new PMR01003DataResultDTO()
                {
                    CDEPOSIT_ID = data1b.Key.CDEPOSIT_ID,
                    CDEPOSIT_NAME = data1b.Key.CDEPOSIT_NAME,
                    Detail1 = data1b.GroupBy(data2a => new
                    {
                        data2a.CBUILDING_ID,
                        data2a.CDEPOSIT_TYPE,

                    }).Select(data2b => new PMR01003DataResultChild1DTO()
                    {
                        CBUILDING_ID = data2b.Key.CBUILDING_ID,
                        CDEPOSIT_TYPE = data2b.Key.CDEPOSIT_TYPE,
                        Detail2 = data2b.GroupBy(data3a => new
                        {
                            data3a.CCUSTOMER_NAME,
                            data3a.CTRANSACTION_TYPE,
                            data3a.CREFERENCE_NO,
                            data3a.CSTATUS,
                            data3a.CTYPES,
                            data3a.CTRANSACTION_NO,
                            data3a.CUNIT_DESCRIPTION,
                            data3a.CINVOICE_NO,
                            data3a.CDEPOSIT_DATE,
                            data3a.CPAYMENT_STATUS,
                            data3a.CCURRENCY_CODE,
                            data3a.NAMOUNT,
                            data3a.NCREDIT_NOTE,
                            data3a.NADJUSTMENT,
                            data3a.NREFUND,
                            data3a.NDEPOSIT_AMOUNT,
                            data3a.NDEPOSIT_BALANCE,
                        }).Select(data3b => new PMR01003DataResultChild2DTO()
                        {
                            CCUSTOMER_NAME = data3b.Key.CCUSTOMER_NAME,
                            CTRANSACTION_TYPE = data3b.Key.CTRANSACTION_TYPE,
                            CREFERENCE_NO = data3b.Key.CREFERENCE_NO,
                            CSTATUS = data3b.Key.CSTATUS,
                            CTYPES = data3b.Key.CTYPES,
                            CTRANSACTION_NO = data3b.Key.CTRANSACTION_NO,
                            CUNIT_DESCRIPTION = data3b.Key.CUNIT_DESCRIPTION,
                            CINVOICE_NO = data3b.Key.CINVOICE_NO,
                            CDEPOSIT_DATE = data3b.Key.CDEPOSIT_DATE,
                            CPAYMENT_STATUS = data3b.Key.CPAYMENT_STATUS,
                            CCURRENCY_CODE = data3b.Key.CCURRENCY_CODE,
                            NAMOUNT = data3b.Key.NAMOUNT,
                            NCREDIT_NOTE = data3b.Key.NCREDIT_NOTE,
                            NADJUSTMENT = data3b.Key.NADJUSTMENT,
                            NREFUND = data3b.Key.NREFUND,
                            NDEPOSIT_AMOUNT = data3b.Key.NDEPOSIT_AMOUNT,
                            NDEPOSIT_BALANCE = data3b.Key.NDEPOSIT_BALANCE,
                            Detail3 = data3b.GroupBy(data4a => new
                            {
                                data4a.CDETAIL_DEPOSIT_TYPE,
                                data4a.CDETAIL_TRANSACTION_NO,
                                data4a.CDETAIL_DEPOSIT_DATE,
                                data4a.CDETAIL_PAYMENT_STATUS,
                                data4a.CDETAIL_CURRENCY_CODE,
                                data4a.NDETAIL_AMOUNT,
                                data4a.NDETAIL_DEPOSIT_BALANCE,
                            }).Select(data4b => new PMR01003DataResultChild3DTO()
                            {
                                CDETAIL_DEPOSIT_TYPE = data4b.Key.CDETAIL_DEPOSIT_TYPE,
                                CDETAIL_TRANSACTION_NO = data4b.Key.CDETAIL_TRANSACTION_NO,
                                CDETAIL_DEPOSIT_DATE = data4b.Key.CDETAIL_DEPOSIT_DATE,
                                CDETAIL_PAYMENT_STATUS = data4b.Key.CDETAIL_PAYMENT_STATUS,
                                CDETAIL_CURRENCY_CODE = data4b.Key.CDETAIL_CURRENCY_CODE,
                                NDETAIL_AMOUNT = data4b.Key.NDETAIL_AMOUNT,
                                NDETAIL_DEPOSIT_BALANCE = data4b.Key.NDETAIL_DEPOSIT_BALANCE,
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();



            // Set the generated data in loRtn
            _logger.LogInfo("Data processed successfully. Generating print output.");
            loRtn.Data3= loData;
            loRtn.BaseHeaderData = loParam;
            loData.Data = loTempData;
            
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