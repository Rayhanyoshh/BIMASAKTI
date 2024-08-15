using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using R_CommonFrontBackAPI;
using R_BackEnd;
using PMM05000Back;
using PMM05000Common;
using PMM05000Common.DTOs;

namespace PMM05000SERVICE
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PMM05000Controller : ControllerBase, IPMM05000
    {
        private LoggerPMM05000 _logger;

        private readonly ActivitySource _activitySource;
        public PMM05000Controller(ILogger<PMM05000Controller> logger)
        {
            //initiate
            LoggerPMM05000.R_InitializeLogger(logger);
            _logger = LoggerPMM05000.R_GetInstanceLogger();
            _activitySource = PMM05000Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        [HttpPost]
        public IAsyncEnumerable<PropertyDTO> GetPropertyList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PropertyDTO> loRtnTemp = null;
            PMM05000InitCls loCls;
            try
            {
                loCls = new PMM05000InitCls();
                ShowLogExecute();
                loRtnTemp = loCls.GetPropertyList(new PropertyDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
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
        public IAsyncEnumerable<UnitTypeCategoryDTO> GetUnitTypeCategoryList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<UnitTypeCategoryDTO> loRtnTemp = null;
            PMM05000Cls loCls;
            try
            {
                loCls = new PMM05000Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetUnitTypeCategoryList(new UnitTypeCategoryParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID)
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
        public IAsyncEnumerable<PricingDTO> GetPricingDateList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingDTO> loRtnTemp = null;
            PMM05000Cls loCls;
            try
            {
                loCls = new PMM05000Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetPricingDateList(new PricingParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID),
                    CUNIT_TYPE_CATEGORY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_CATEGORY_ID),
                    CPRICE_TYPE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPRICE_TYPE),
                    CTYPE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CTYPE),
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
        public IAsyncEnumerable<PricingDTO> GetPricingList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<PricingDTO> loRtnTemp = null;
            PMM05000Cls loCls;
            try
            {
                loCls = new PMM05000Cls();
                ShowLogExecute();
                loRtnTemp = loCls.GetPricingList(new PricingParamDTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID),
                    CUNIT_TYPE_CATEGORY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_CATEGORY_ID),
                    CPRICE_TYPE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPRICE_TYPE),
                    LACTIVE = R_Utility.R_GetStreamingContext<bool>(ContextConstant.LACTIVE),
                    CTYPE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CTYPE),
                    CVALID_DATE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CVALID_DATE),
                    CVALID_INTERNAL_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CVALID_ID),
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

        private async IAsyncEnumerable<T> StreamListHelper<T>(List<T> poList)
        {
            foreach (T loEntity in poList)
            {
                yield return loEntity;
            }
        }

        [HttpPost]
        public PricingDumpResultDTO SavePricing(PricingSaveParamDTO poParam)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            PricingDumpResultDTO loRtn = new();
            R_Exception loException = new R_Exception();
            PMM05000Cls loCls;
            try
            {
                loCls = new PMM05000Cls();
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                ShowLogExecute();
                loCls.SavePricing(poParam);
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

        [HttpPost]
        public IAsyncEnumerable<TypeDTO> GetPriceChargesType()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            List<TypeDTO> loRtnTemp = null;
            PMM05000InitCls loCls;
            try
            {
                loCls = new PMM05000InitCls();
                ShowLogExecute();
                loRtnTemp = loCls.GetChargesPriceTypeList(new TypeParam()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CLASS_APPLICATION = R_Utility.R_GetStreamingContext<string>(ContextConstant.CLASS_APPLICATION),
                    CLASS_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CLASS_ID),
                    REC_ID_LIST = R_Utility.R_GetStreamingContext<string>(ContextConstant.REC_ID_LIST),
                    LANG_ID = R_BackGlobalVar.CULTURE,
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
        public PricingDumpResultDTO ActiveInactivePricing(PricingParamDTO poParam)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            R_Exception loException = new R_Exception();
            PricingDumpResultDTO loRtn = null;
            PMM05000Cls loCls;
            try
            {
                loRtn = new();
                loCls = new PMM05000Cls();
                ShowLogExecute();
                poParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                loCls.ActiveInactivePricing(poParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                ShowLogError(loException);
            }
            loException.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }
        
        #region logger

        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);


        #endregion


    }
}
