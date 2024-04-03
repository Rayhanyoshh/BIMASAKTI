using CBT01200BACK;
using CBT01200Common;
using CBT01200Common.Loggers;
using CBT01200Back;
using CBT01200Common.Loggers;
using CBT01200Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using R_Common;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using CBT01200Common;
using CBT01200Back;

namespace CBT01100SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CBT01100Controller : ControllerBase, ICBT01200
    {
        private LoggerCBT01200 _logger;

        private readonly ActivitySource _activitySource;

        public CBT01100Controller(ILogger<LoggerCBT01200> logger)
        {
            //Initial and Get Logger
            LoggerCBT01200.R_InitializeLogger(logger);
            _logger = LoggerCBT01200.R_GetInstanceLogger();
            _activitySource = CBT01200Activity.R_InitializeAndGetActivitySource(GetType().Name);

        }

        [HttpPost]
        public IAsyncEnumerable<CBT01201DTO> GetJournalDetailList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<CBT01201DTO> loRtn = null;

            try
            {
                var loRecId = R_Utility.R_GetStreamingContext<string>(ContextConstant.CREC_ID);
                var loCls = new CBT01200Cls();
                var loTempRtn = loCls.GetJournalDetailList(loRecId);
                ShowLogExecute();
                loRtn = StreamListData<CBT01201DTO>(loTempRtn);
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
        public IAsyncEnumerable<CBT01200DTO> GetJournalList()
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            IAsyncEnumerable<CBT01200DTO> loRtn = null;

            try
            {
                var loParam = new CBT01200ParamDTO();
                loParam.CDEPT_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CDEPT_CODE);
                loParam.CPERIOD = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPERIOD);
                loParam.CSTATUS = R_Utility.R_GetStreamingContext<string>(ContextConstant.CSTATUS);
                loParam.CSEARCH_TEXT = R_Utility.R_GetStreamingContext<string>(ContextConstant.CSEARCH_TEXT);

                var loCls = new CBT01200Cls();

                ShowLogExecute();
                var loTempRtn = loCls.GetJournalList(loParam);

                loRtn = StreamListData<CBT01200DTO>(loTempRtn);
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
        public CBT01200RecordResult<CBT01200UpdateStatusDTO> UpdateJournalStatus(CBT01200UpdateStatusDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity($"{GetType().Name}.{MethodBase.GetCurrentMethod().Name}");
            ShowLogStart();
            var loEx = new R_Exception();
            CBT01200RecordResult<CBT01200UpdateStatusDTO> loRtn = new CBT01200RecordResult<CBT01200UpdateStatusDTO>();
            try
            {
                var loCls = new CBT01200Cls();

                ShowLogExecute();
                loCls.UpdateJournalStatus(poEntity);
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

        #region Stream List Data

        private async IAsyncEnumerable<T> StreamListData<T>(List<T> poParameter)
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }

        #endregion

        #region logger

        private void ShowLogStart([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Starting {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogExecute([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"Executing cls method in {GetType().Name}.{pcMethodCallerName}");

        private void ShowLogEnd([CallerMemberName] string pcMethodCallerName = "") => _logger.LogInfo($"End {pcMethodCallerName} in {GetType().Name}");

        private void ShowLogError(Exception exception, [CallerMemberName] string pcMethodCallerName = "") => _logger.LogError(exception);

        #endregion
    }
}
