using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_BackEnd;
using GSM001000Back;
using GSM01000Back;
using Microsoft.Extensions.Logging;

namespace GSM01010Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01010Controller : ControllerBase, IGSM01010
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;
        
        public GSM01010Controller(ILogger<GSM01010Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM01000.R_InitializeLogger(logger);
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_InitializeAndGetActivitySource(nameof(GSM01010Controller));

        }
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01010DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01010DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceGetRecord");

            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01010DTO> loRtn = null;
            GSM01010Cls loCls;

            try
            {
                _logger.LogInfo("Start - R_ServiceGetRecord");

                loCls = new GSM01010Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01010DTO>();

                // poParameter.Entity.CCOMPANY_ID = "RCD";
                // poParameter.Entity.CGLACCOUNT_NO = "12.40.0000";

                loRtn.data = loCls.R_GetRecord(poParameter.Entity);

                _logger.LogInfo("End - R_ServiceGetRecord");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in R_ServiceGetRecord: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM01010DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01010DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01010DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public GSM01010ListDTO GetGoA()
        {
            R_Exception loEx = new R_Exception();
            GSM01010ListDTO loRtn = null;
            GOAHeadListDbParameter loDbPar;
            GSM01010Cls loCls;

            try
            {
                using Activity activity = _activitySource.StartActivity("GetGoA");

                _logger.LogInfo("Start - GetGoA");

                loRtn = new GSM01010ListDTO();
                _logger.LogInfo("Set Parameter");
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                loDbPar.CUSER_LOGIN_ID = R_BackGlobalVar.USER_ID;
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CGLACCOUNT_NO = "12.40.0000";

                loCls = new GSM01010Cls();
                loRtn.Data = loCls.GetGoAListByGlAccount(loDbPar);

                _logger.LogInfo("End - GetGoA");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetGoA: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        

        [HttpPost]
        public IAsyncEnumerable<GSM01010DTO> GetGOAListByGLAccountStream()
        {
            using Activity activity = _activitySource.StartActivity("GetGOAListByGLAccountStream");

            R_Exception loException = new R_Exception();
            GOAHeadListDbParameter loDbPar;
            List<GSM01010DTO> loRtnTmp;
            GSM01010Cls loCls;
            IAsyncEnumerable<GSM01010DTO> loRtn = null;

            try
            {
                _logger.LogInfo("Start - GetGOAListByGLAccountStream");

                var liCompanyId = R_BackGlobalVar.COMPANY_ID;
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = liCompanyId;

                loCls = new GSM01010Cls();
                loRtnTmp = loCls.GetGoAListByGlAccount(loDbPar);

                loRtn = GetGOAListStream(loRtnTmp);

                _logger.LogInfo("End - GetGOAListByGLAccountStream");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetGOAListByGLAccountStream: {ex.Message}");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        private async IAsyncEnumerable<GSM01010DTO> GetGOAListStream(List<GSM01010DTO> poParameter)
        {
            foreach (GSM01010DTO item in poParameter)
            {
                yield return item;
            }
        }
        
    }
    
}
