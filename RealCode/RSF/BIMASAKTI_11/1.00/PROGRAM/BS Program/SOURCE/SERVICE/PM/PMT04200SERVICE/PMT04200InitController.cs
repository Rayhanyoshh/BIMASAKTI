using PMT04200Back;
using PMT04200Common.Loggers;
using PMT04200Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using PMT04200Common.DTOs;
using R_BackEnd;

namespace PMT04200SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PMT04200InitController : ControllerBase, IPMT04200Init
    {
        private LoggerInitPMT04200 _logger;
        private readonly ActivitySource _activitySource;

        public PMT04200InitController(ILogger<LoggerInitPMT04200> logger)
        {
            //Initial and Get Logger
            LoggerInitPMT04200.R_InitializeLogger(logger);
            _logger = LoggerInitPMT04200.R_GetInstanceLogger();
            _activitySource = PMT04200Activity.R_InitializeAndGetActivitySource(GetType().Name);
        }

        #region Stream List Data
        private async IAsyncEnumerable<T> StreamListData<T>(List<T> poParameter)
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }
        #endregion

        [HttpPost]

        public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<PropertyListDTO> loRtn = null;
            PMT04200InitCls loCls;
            PMT04200ParamDTO loPar;
            List<PropertyListDTO> loRtnTmp;

            try
            {
                loPar = new PMT04200ParamDTO();
                loPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loPar.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls = new PMT04200InitCls();

                loRtnTmp = loCls.PropertyListDB(loPar);

                loRtn = StreamListData<PropertyListDTO>(loRtnTmp);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<PMT04200GSCurrencyDTO> GetCurrencyList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<PMT04200GSCurrencyDTO> loRtn = null;

            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                var loTempRtn = loCls.GetCurrencyList();

                loRtn = StreamListData<PMT04200GSCurrencyDTO>(loTempRtn);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200GLSystemParamDTO> GetGLSystemParam()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200GLSystemParamDTO> loRtn = new PMT04200RecordResult<PMT04200GLSystemParamDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetGLSystemParamRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200CBSystemParamDTO> GetCBSystemParam()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200CBSystemParamDTO> loRtn = new PMT04200RecordResult<PMT04200CBSystemParamDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetCBSystemParamRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }
        
        [HttpPost]
        public PMT04200RecordResult<PMT04200PMSystemParamDTO> GetPMSystemParam()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200PMSystemParamDTO> loRtn = new PMT04200RecordResult<PMT04200PMSystemParamDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetPMSystemParamRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<PMT04200GSGSBCodeDTO> GetGSBCodeList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<PMT04200GSGSBCodeDTO> loRtn = null;

            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                var loTempRtn = loCls.GetGSBCodeList();

                loRtn = StreamListData<PMT04200GSGSBCodeDTO>(loTempRtn);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        public PMT04200RecordResult<PMT04200LastCurrencyRateDTO> GetLastCurrencyRate(PMT04200LastCurrencyRateDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetLastCurrencyRate");
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200LastCurrencyRateDTO> loRtn = new PMT04200RecordResult<PMT04200LastCurrencyRateDTO>();
            _logger.LogInfo("Start GetLastCurrencyRate");

            try
            {
                var loCls = new PMT04200InitCls();

                _logger.LogInfo("Call Back Method GetLastCurrency");
                loRtn.Data = loCls.GetLastCurrency(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End GetLastCurrencyRate");

            return loRtn;
        }

        public PMT04200RecordResult<PMT04200InitDTO> GetTabTransactionListInitVar()
        {
            using Activity activity = _activitySource.StartActivity("GetTabJournalListInitialProcess");
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200InitDTO> loRtn = new PMT04200RecordResult<PMT04200InitDTO>();
            _logger.LogInfo("Start GetTabJournalListInitialProcess");

            try
            {
                var loCls = new PMT04200InitCls();
                _logger.LogInfo("Call All Back Method GetTabJournalListInitialProcess");
                var loTempResult = new PMT04200InitDTO
                {
                    VAR_PM_SYSTEM_PARAM = loCls.GetPMSystemParamRecord(),
                    VAR_GL_SYSTEM_PARAM = loCls.GetGLSystemParamRecord(),
                };

                loRtn.Data = loTempResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End GetTabJournalListInitialProcess");

            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200GSCompanyInfoDTO> GetGSCompanyInfo()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200GSCompanyInfoDTO> loRtn = new PMT04200RecordResult<PMT04200GSCompanyInfoDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetCompanyInfoRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200GSPeriodDTInfoDTO> GetGSPeriodDTInfo(PMT04200ParamGSPeriodDTInfoDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200GSPeriodDTInfoDTO> loRtn = new PMT04200RecordResult<PMT04200GSPeriodDTInfoDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetPeriodDTInfoRecord(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200GSPeriodYearRangeDTO> GetGSPeriodYearRange()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200GSPeriodYearRangeDTO> loRtn = new PMT04200RecordResult<PMT04200GSPeriodYearRangeDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetPeriodYearRangeRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200GSTransInfoDTO> GetGSTransCodeInfo()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200GSTransInfoDTO> loRtn = new PMT04200RecordResult<PMT04200GSTransInfoDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetTransCodeInfoRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200InitDTO> GetTabJournalListInitVar()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200InitDTO> loRtn = new PMT04200RecordResult<PMT04200InitDTO>();

            try
            {
                var loCls = new PMT04200InitCls();
                ShowLogExecute();

                loRtn.Data = new()
                {
                    VAR_GL_SYSTEM_PARAM = loCls.GetGLSystemParamRecord(),
                    VAR_GSM_TRANSACTION_CODE = loCls.GetTransCodeInfoRecord(),
                    VAR_GSM_PERIOD = loCls.GetPeriodYearRangeRecord(),
                    VAR_TODAY = loCls.GetTodayDateRecord(),
                    VAR_GSM_COMPANY = loCls.GetCompanyInfoRecord(),
                    VAR_CB_SYSTEM_PARAM = loCls.GetCBSystemParamRecord(),
                    VAR_PM_SYSTEM_PARAM = loCls.GetPMSystemParamRecord(),
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
            ShowLogEnd();
            return loRtn;
        }

        [HttpPost]
        public PMT04200RecordResult<PMT04200TodayDateDTO> GetTodayDate()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            PMT04200RecordResult<PMT04200TodayDateDTO> loRtn = new PMT04200RecordResult<PMT04200TodayDateDTO>();
            try
            {
                var loCls = new PMT04200InitCls();

                ShowLogExecute();
                loRtn.Data = loCls.GetTodayDateRecord();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);

            }

            loEx.ThrowExceptionIfErrors();
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

