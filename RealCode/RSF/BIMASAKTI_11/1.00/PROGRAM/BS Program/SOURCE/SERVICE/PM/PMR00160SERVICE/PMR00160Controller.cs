using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PMR00160BACK;
using PMR00160COMMON;
using PMR00160COMMON.Utility_Report;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Diagnostics;

namespace PMR00160SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PMR00160Controller : ControllerBase, IPMR00160
    {
        private LoggerPMR00160 _logger;
        private readonly ActivitySource _activitySource;
        public PMR00160Controller(ILogger<PMR00160Controller> logger)
        {
            LoggerPMR00160.R_InitializeLogger(logger);
            _logger= LoggerPMR00160.R_GetInstanceLogger();
            _activitySource = PMR00160Activity.R_InitializeAndGetActivitySource(nameof(PMR00160Controller));
        }
        [HttpPost]
        public IAsyncEnumerable<PMR00160PropertyDTO> GetPropertyListStream()
        {
            string lcMethodName = nameof(GetPropertyListStream);
            using Activity activity = _activitySource.StartActivity(lcMethodName)!;
            _logger.LogInfo(string.Format("START process method {0} on Controller", lcMethodName));

            var loEx = new R_Exception();
            IAsyncEnumerable<PMR00160PropertyDTO> loRtn = null;
            List<PMR00160PropertyDTO> loRtnTemp;
            try
            {
                var loDbParameter = new PMR00160DBParamDTO();
                var loCls = new PMR00160Cls();

                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbParameter.CUSER_ID = R_BackGlobalVar.USER_ID;
                _logger.LogDebug("DbParameter {@Parameter} ", loDbParameter);

                _logger.LogInfo("Call method GetAllPropertyList");
                loRtnTemp = loCls.GetPropertyList(loDbParameter);
                _logger.LogInfo("Call method to streaming data");
                loRtn = HelperStream(loRtnTemp);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo(string.Format("END process method {0} on Controller", lcMethodName));
            return loRtn;
        }
        [HttpPost]
        public PMR00160InitialProcess GetInitialProcess()
        {
            string lcMethodName = nameof(GetInitialProcess);
            using Activity activity = _activitySource.StartActivity(lcMethodName)!;
            _logger.LogInfo(string.Format("START process method {0} on Controller", lcMethodName));

            R_Exception loException = new R_Exception();
            PMR00160DBParamDTO loDbParameter = new();
            PMR00160InitialProcess loReturn = null;
            try
            {
                var loCls = new PMR00160Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                _logger.LogInfo("Call method InitialProcess on Controller");

                loReturn = loCls.GetInitialProcess(loDbParameter);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                _logger.LogError(loException);
            }
            loException.ThrowExceptionIfErrors();
            _logger.LogInfo(string.Format("END process method {0} on Controller", lcMethodName));
            return loReturn!;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        private async IAsyncEnumerable<T> HelperStream<T>(List<T> poParameter)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach (var item in poParameter)
            {
                yield return item;
            }
        }

    }
}
