using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using R_CommonFrontBackAPI;
using PMM05000Back;
using PMM05000Common;
using PMM05000Common.DTOs;
using R_BackEnd;

namespace PMM05000Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PMM05010Controller : ControllerBase, IPMM05010
    {
     private LoggerPMM05000 _logger;

        private readonly ActivitySource _activitySource;

        public PMM05010Controller(ILogger<LoggerPMM05000> logger)
        {
            LoggerPMM05000.R_InitializeLogger(logger);
            _logger = LoggerPMM05000.R_GetInstanceLogger();
            _activitySource = PMM05000Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        [HttpPost]
        public IAsyncEnumerable<PricingRateDTO> GetPricingRateDateList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingRateDTO> loRtnTemp = null;
            PMM05001Cls loCls;
            try
            {
                loCls = new PMM05001Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetPricingRateDateList(new PricingRateSaveParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID),
                    CPRICE_TYPE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPRICE_TYPE),
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                ShowLogError(loException);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return StreamListHelper(loRtnTemp);
        }

        [HttpPost]
        public IAsyncEnumerable<PricingRateDTO> GetPricingRateList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingRateDTO> loRtnTemp = null;
            PMM05001Cls loCls;
            try
            {
                loCls = new PMM05001Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetPricingRateList(new PricingRateSaveParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID),
                    CUNIT_TYPE_CATEGORY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_CATEGORY_ID),
                    CPRICE_TYPE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPRICE_TYPE),
                    CRATE_DATE= R_Utility.R_GetStreamingContext<string>(ContextConstant.CRATE_DATE),
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                });
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                ShowLogError(loException);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return StreamListHelper(loRtnTemp);
        }

        [HttpPost]
        public PricingDumpResultDTO SavePricingRate(PricingRateSaveParamDTO poParam)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            PricingDumpResultDTO loRtn = new();
            R_Exception loException = new R_Exception();
            PMM05001Cls loCls;
            try
            {
                loCls = new PMM05001Cls();
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                ShowLogExecute();
                loCls.SavePricingRate(poParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                ShowLogError(loException);
            }
        EndBlock:
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        private async IAsyncEnumerable<T> StreamListHelper<T>(List<T> poList)
        {
            foreach (T loEntity in poList)
            {
                yield return loEntity;
            }
        }   
        
        #region logger

        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);

        #endregion
    }
}

