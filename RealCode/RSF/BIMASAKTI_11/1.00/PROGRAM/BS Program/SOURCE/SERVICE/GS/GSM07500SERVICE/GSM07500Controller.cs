using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM07500Back;
using GSM07500Common;
using GSM07500Common.DTOs;
using GSM07510Common;
using Microsoft.Extensions.Logging;
using R_BackEnd;

namespace GSM07500Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GSM07500Controller : ControllerBase, IGSM07500
    {
        private LoggerGSM07500 _logger;
        
        public GSM07500Controller(ILogger<GSM07500Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM07500.R_InitializeLogger(logger);
            _logger = LoggerGSM07500.R_GetInstanceLogger();
        }   

        [HttpPost]
        public GSM07500ListDTO PeriodDetailList()
        {
            _logger.LogInfo("Start - PeriodDetailList");

            R_Exception loEx = new R_Exception();
            GSM07500DTO loDbPar;
            GSM07500ListDTO loRtn = null;
            List<GSM07500DTO> loResult;
            GSM07500Cls loCls;

            try
            {
                loDbPar = new GSM07500DTO();
                _logger.LogInfo("Set Parameter");
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CCYEAR = R_Utility.R_GetContext<string>(ContextConstant.CCYEAR);

                loCls = new GSM07500Cls();
                loResult = loCls.GetPeriodDetailDbList(loDbPar);
                loResult = loResult.OrderBy(item => int.Parse(item.CPERIOD_NO)).ToList();

                loRtn = new GSM07500ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in PeriodDetailList");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            _logger.LogInfo("End - PeriodDetailList");
            return loRtn;
        }
        
        
        [HttpPost]
        public IAsyncEnumerable<GSM07500DTO> PeriodDetailListStream()
        {
            _logger.LogInfo("Start - PeriodDetailListStream");

            R_Exception loException = new R_Exception();
            GSM07500DTO loDbPar;
            List<GSM07500DTO> loRtnTmp;
            GSM07500Cls loCls;
            IAsyncEnumerable<GSM07500DTO> loRtn = null;

            try
            {
                loDbPar = new GSM07500DTO();
                // loDbPar.CCOMPANY_ID = "RCD";
                _logger.LogInfo("Set Parameter");
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                loCls = new GSM07500Cls();
                loRtnTmp = loCls.GetPeriodDetailDbList(loDbPar);

                loRtn = GetPeriodDetailHelper(loRtnTmp);
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in PeriodDetailListStream");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            _logger.LogInfo("End - PeriodDetailListStream");

            return loRtn;
        }
        
        [HttpPost]
        public GSM07500ListDTO RftPeriodGenerator()
        {
            _logger.LogInfo("Start - RftPeriodGenerator");

            R_Exception loEx = new R_Exception();
            GSM07500ListDTO loRtn = null;
            List<GSM07500DTO> loResult;
            GeneratePeriodParameter loDbPar;
            GSM07500Cls loCls;

            try
            {
                loDbPar = new GeneratePeriodParameter();
                _logger.LogInfo("Set Parameter");

                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CYEAR = R_Utility.R_GetStreamingContext<string>(ContextConstant.CYEAR);
                loDbPar.LPERIOD_MODE = R_Utility.R_GetStreamingContext<bool>(ContextConstant.LPERIOD_MODE);
                loDbPar.INO_PERIOD = R_Utility.R_GetStreamingContext<int>(ContextConstant.INO_PERIOD);
        
                loCls = new GSM07500Cls();
                loResult = loCls.RftGenerateGSMPeriod(loDbPar);
                loRtn = new GSM07500ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in RftPeriodGenerator");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            _logger.LogInfo("End - RftPeriodGenerator");

            return loRtn;
        }

        
        private async IAsyncEnumerable<GSM07500DTO> GetPeriodDetailHelper (List<GSM07500DTO> poParameter)
        {
            foreach (GSM07500DTO item in poParameter)
            {
                yield return item;
            }
        }

        [HttpPost]
        public R_ServiceGetRecordResultDTO<SaveBatchGSM07500DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<SaveBatchGSM07500DTO> poParameter)
        {
            _logger.LogInfo("Start - R_ServiceGetRecord");
            var loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<SaveBatchGSM07500DTO> loRtn = new R_ServiceGetRecordResultDTO<SaveBatchGSM07500DTO>();

            try
            {
                _logger.LogInfo("Set Parameter");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                var loCls = new GSM07500Cls();
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in R_ServiceGetRecord");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<SaveBatchGSM07500DTO> R_ServiceSave(R_ServiceSaveParameterDTO<SaveBatchGSM07500DTO> poParameter)
        {
            _logger.LogInfo("Start - R_ServiceSave");
            var loEx = new R_Exception();
            R_ServiceSaveResultDTO<SaveBatchGSM07500DTO> loRtn = new R_ServiceSaveResultDTO<SaveBatchGSM07500DTO>();

            try
            {
                _logger.LogInfo("Set Parameter");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                var loCls = new GSM07500Cls();

                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in R_ServiceSave");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - R_ServiceSave");
            return loRtn;
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<SaveBatchGSM07500DTO> poParameter)
        {
            throw new NotImplementedException();
        }
    }
}