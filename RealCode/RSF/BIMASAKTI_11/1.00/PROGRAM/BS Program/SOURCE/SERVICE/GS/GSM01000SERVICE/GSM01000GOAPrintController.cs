using BaseHeaderReportCOMMON;
using GSM001000Back;
using GSM01000Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Cache;
using R_Common;
using R_CommonFrontBackAPI;
using R_ReportFastReportBack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSM01000Back;
using GSM01000Common;
using GSM01000Service.DTOs;
using Microsoft.Extensions.Logging;
using R_CommonFrontBackAPI.Log;


namespace GSM01000Service
{
    public class GSM01000GOAPrintController : R_ReportControllerBase
    {
        private LoggerGSM01000 _Logger;
        private R_ReportFastReportBackClass _ReportCls;
        private GSM01000PrintParamGOADTO _AllGSM01000GOAParameter;
        private readonly ActivitySource _activitySource;


        #region instantiation

        public GSM01000GOAPrintController(ILogger<GSM01000GOAPrintController> Logger)
        {
            LoggerGSM01000.R_InitializeLogger(Logger);
            _Logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_InitializeAndGetActivitySource(nameof(GSM01000GOAPrintController));

            
            _ReportCls = new R_ReportFastReportBackClass();
            _ReportCls.R_InstantiateMainReportWithFileName += _ReportCls_R_InstantiateMainReportWithFileName;
            _ReportCls.R_GetMainDataAndName += _ReportCls_R_GetMainDataAndName;
            _ReportCls.R_SetNumberAndDateFormat += _ReportCls_R_SetNumberAndDateFormat;
        }
        #endregion
        #region Event Handler

        private void _ReportCls_R_InstantiateMainReportWithFileName(ref string pcFileTemplate)
        {
            pcFileTemplate = System.IO.Path.Combine("Reports", "GSM01000GOA.frx");
        }

        private void _ReportCls_R_GetMainDataAndName(ref ArrayList poData, ref string pcDataSourceName)
        {
            poData.Add(GenerateDataPrint(_AllGSM01000GOAParameter));
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
        public R_DownloadFileResultDTO AllGOAPost(GSM01000PrintParamGOADTO poParameter)
        {
            using Activity activity = _activitySource.StartActivity("AllGOAPost");

            _Logger.LogInfo("Start - Post GOA");
            R_Exception loException = new R_Exception();
            GSM01000GOAPrintLogKeyDTO loCache = null;
            R_DownloadFileResultDTO loRtn = null;
            try
            {
                loRtn = new R_DownloadFileResultDTO();
                loCache = new GSM01000GOAPrintLogKeyDTO
                {
                    poParam = poParameter,
                    poLogKey = (R_NetCoreLogKeyDTO)R_NetCoreLogAsyncStorage.GetData(R_NetCoreLogConstant.LOG_KEY)
                };
                _Logger.LogInfo("Set GUID Param - Post GOA Status");
        
                // Log the parameter details
                _Logger.LogInfo("Parameter Details - poParam: {poParam}, poLogKey: {poLogKey}", 
                    poParameter, loCache.poLogKey);
        
                R_DistributedCache.R_Set(loRtn.GuidResult, 
                    R_NetCoreUtility.R_SerializeObjectToByte<GSM01000PrintParamGOADTO>(poParameter));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                _Logger.LogError(loException);
            }

            loException.ThrowExceptionIfErrors();
            _Logger.LogInfo("End - Post GOA Status");
            return loRtn;
        }

        [HttpGet, AllowAnonymous]
        public FileStreamResult GroupOfAccountReport(string pcGuid)
        {
            using Activity activity = _activitySource.StartActivity("GroupOfAccountReport");

            _Logger.LogInfo("Start - Get GOA Status");
            R_Exception loException = new R_Exception();
            FileStreamResult loRtn = null;
            GSM01000GOAPrintLogKeyDTO loResultGUID = null;
            try
            {
                // Deserialize the GUID from the cache
                loResultGUID = R_NetCoreUtility.R_DeserializeObjectFromByte<GSM01000GOAPrintLogKeyDTO>(
                    R_DistributedCache.Cache.Get(pcGuid));

                _Logger.LogDebug("Deserialized GUID: {pcGuid}", pcGuid);

                _Logger.LogDebug("Deserialized GUID: {pcGuid}", pcGuid);
                _Logger.LogDebug("Deserialized Parameters: {@Parameters}", _AllGSM01000GOAParameter);

                loRtn = new FileStreamResult(_ReportCls.R_GetStreamReport(), R_ReportUtility.GetMimeType(R_FileType.PDF));
        
                _Logger.LogInfo("Report generated successfully.");
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                _Logger.LogError(loException);
            }

            loException.ThrowExceptionIfErrors();
            _Logger.LogInfo("End - Get GOA Status");
            return loRtn;
        }


        #region Helper
        private GSM01000PrintGOAResultWithBaseHeaderPrintDTO GenerateDataPrint(GSM01000PrintParamGOADTO poParam)
        {
            using Activity activity = _activitySource.StartActivity("GenerateDataPrint");

            var loEx = new R_Exception();
            GSM01000PrintGOAResultWithBaseHeaderPrintDTO loRtn = new GSM01000PrintGOAResultWithBaseHeaderPrintDTO();
            var loParam = new BaseHeaderDTO();

            System.Globalization.CultureInfo loCultureInfo =
                new System.Globalization.CultureInfo(R_BackGlobalVar.REPORT_CULTURE);
            try
            {
            _Logger.LogInfo("Generating data for Group Of Account report.");

               
            //Add Resources
            loRtn.BaseHeaderColumn.Page = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Page", loCultureInfo);
            loRtn.BaseHeaderColumn.Of =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Of", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_Date =
                R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class), "Print_Date", loCultureInfo);
            loRtn.BaseHeaderColumn.Print_By = R_Utility.R_GetMessage(typeof(BaseHeaderResources.Resources_Dummy_Class),
                "Print_By", loCultureInfo);
            
            GSM01000PrintColoumnGOADTO loColumnObject = new GSM01000PrintColoumnGOADTO();
            var loColumn = AssignValuesWithMessages(typeof(GSM01000BackResources.Resources_Dummy_Class),
                loCultureInfo, loColumnObject);
            
            
            // Set base header data
            loParam.CCOMPANY_NAME = "PT Realta Chackradarma";
            loParam.CPRINT_CODE = poParam.CCOMPANY_ID.ToUpper();
            loParam.CPRINT_NAME = "Group Of Account";
            loParam.CUSER_ID = poParam.CUSER_LOGIN_ID.ToUpper();
            
            // Create an instance of GSM01000PrintGOAResultDTo
            GSM01000PrintGOAResultDTo loData = new GSM01000PrintGOAResultDTo()
            {
                Title = "Group Of Accounts",
                Header = "001",
                Column = new GSM01000PrintColoumnGOADTO(),
                Data = new List<GSM01000DataResultGOADTO>()
            };

            // Create an instance of GSM01000Cls
            var loCls = new GSM01000Cls();

            // Get print data for Group Of Account report
            var loCollection = loCls.GetPrintDataResultGOA(poParam);

            // Process the data and create a formatted list
            var loTempData = loCollection
                .GroupBy(data1a => new
                {
                    data1a.CGOA_CODE,
                    data1a.CGOA_NAME,
                    data1a.CGOA_BSIS,
                    data1a.CGOA_DBCR,
                }).Select(data1b => new GSM01000DataResultGOADTO()
                {
                    CGOA_CODE = data1b.Key.CGOA_CODE,
                    CGOA_NAME = data1b.Key.CGOA_NAME,
                    CGOA_BSIS = data1b.Key.CGOA_BSIS,
                    CGOA_DBCR = data1b.Key.CGOA_DBCR,
                    GSM01001DataResultGOADTO = data1b.GroupBy(data2a => new
                    {
                        data2a.CGLACCOUNT_NO,
                        data2a.CGLACCOUNT_NAME,
                        data2a.CBSIS,
                        data2a.CDBCR,
                        data2a.CCASH_FLOW_CODE,
                        data2a.CCASH_FLOW_NAME,
                        data2a.LACTIVE,
                    }).Select(data2b => new GSM01001DataResultGOADTO()
                    {
                        CGLACCOUNT_NO = data2b.Key.CGLACCOUNT_NO,
                        CGLACCOUNT_NAME = data2b.Key.CGLACCOUNT_NAME,
                        CBSIS = data2b.Key.CBSIS,
                        CDBCR = data2b.Key.CDBCR,
                        CCASH_FLOW_CODE = data2b.Key.CCASH_FLOW_CODE,
                        CCASH_FLOW_NAME = data2b.Key.CCASH_FLOW_NAME,
                        LACTIVE = data2b.Key.LACTIVE,
                    }).ToList()
                }).ToList();

            // Set the generated data in loRtn
            _Logger.LogInfo("Get Detail GOA Analysis Report");
            loRtn.GOAData = loData;
            loRtn.BaseHeaderData = loParam;
            loData.Data = loTempData;
            
            _Logger.LogInfo("Data generation for Group Of Account report completed.");
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
            _Logger.LogError(loEx);
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
    
}
