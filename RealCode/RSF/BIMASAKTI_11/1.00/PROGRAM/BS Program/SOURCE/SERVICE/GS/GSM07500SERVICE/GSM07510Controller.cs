using System.Diagnostics;
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
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM07510Controller  : ControllerBase, IGSM07510
    {
        private LoggerGSM07500 _logger;
        private readonly ActivitySource _activitySource;

        
        public GSM07510Controller(ILogger<GSM07510Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM07500.R_InitializeLogger(logger);
            _logger = LoggerGSM07500.R_GetInstanceLogger();
            _activitySource = GSM07500Activity.R_InitializeAndGetActivitySource(nameof(GSM07500Controller));

        }   
        
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM07510DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM07510DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceGetRecord");

            _logger.LogInfo("Start - R_ServiceGetRecord");

            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM07510DTO> loRtn = null;
            GSM07510Cls loCls;
            LastYearDTO loYear;

            try
            {
                loCls = new GSM07510Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM07510DTO>();

                _logger.LogInfo("Set Parameters: CCOMPANY_ID = {CCOMPANY_ID}, CUSER_ID = {CUSER_ID}", 
                    poParameter.Entity.CCOMPANY_ID, poParameter.Entity.CUSER_ID);

                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;


                _logger.LogInfo("Get Record");
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in R_ServiceGetRecord");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - R_ServiceGetRecord");

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM07510DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM07510DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceSave");

            _logger.LogInfo("Start - R_ServiceSave");

            var loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM07510DTO> loRtn = new R_ServiceSaveResultDTO<GSM07510DTO>();

            try
            {
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                _logger.LogInfo("Set Parameters: CCOMPANY_ID = {CCOMPANY_ID}, CUSER_ID = {CUSER_ID}", 
                    poParameter.Entity.CCOMPANY_ID, poParameter.Entity.CUSER_ID);

                // Add any additional log messages as needed for context

                var loCls = new GSM07510Cls();
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in R_ServiceSave");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - R_ServiceSave");

            return loRtn;
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM07510DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceDelete");

            _logger.LogInfo("Start - R_ServiceDelete");

            R_Exception loEx = new R_Exception();
            R_ServiceDeleteResultDTO loRtn = new R_ServiceDeleteResultDTO();
            GSM07510Cls loCls;
            try
            {
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                _logger.LogInfo("Set Parameters: CCOMPANY_ID = {CCOMPANY_ID}, CUSER_ID = {CUSER_ID}", 
                    poParameter.Entity.CCOMPANY_ID, poParameter.Entity.CUSER_ID);

                // Add any additional log messages as needed for context

                loCls = new GSM07510Cls();
                loCls.R_Delete(poParameter.Entity);
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in R_ServiceDelete");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - R_ServiceDelete");

            return loRtn;
        }

        [HttpPost]
        public GSM07510ListDTO PeriodList()
        {
            using Activity activity = _activitySource.StartActivity("PeriodList");

            _logger.LogInfo("Start - PeriodList");

            R_Exception loEx = new R_Exception();
            GSM07510ListDTO loRtn = null;
            List<GSM07510DTO> loResult;
            GSM07510DTO loDbPar;
            GSM07510Cls loCls;

            try
            {
                loDbPar = new GSM07510DTO();
                // Add log message to indicate setting the company ID
                _logger.LogInfo("Setting Company ID: CCOMPANY_ID = {CCOMPANY_ID}", loDbPar.CCOMPANY_ID);
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;



                loCls = new GSM07510Cls();
                loResult = loCls.GetPeriodDbList(loDbPar);
                foreach (var item in loResult)
                {
                    if (item.LPERIOD_MODE)
                    {
                        item.DescPeriodMode = "Normal Calendar";
                    }
                    else
                    {
                        item.DescPeriodMode = "Custom Period";
                    }
                }
                loRtn = new GSM07510ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in PeriodList");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - PeriodList");

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM07510DTO> PeriodListStream()
        {
            using Activity activity = _activitySource.StartActivity("PeriodListStream");

            _logger.LogInfo("Start - PeriodListStream");

            R_Exception loException = new R_Exception();
            GSM07510DTO loDbPar;
            List<GSM07510DTO> loRtnTmp;
            GSM07510Cls loCls;
            IAsyncEnumerable<GSM07510DTO> loRtn = null;

            try
            {
                loDbPar = new GSM07510DTO();
                _logger.LogInfo("Set Parameters");
                // loDbPar.CCOMPANY_ID = "RCD";
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                loCls = new GSM07510Cls();
                loRtnTmp = loCls.GetPeriodDbList(loDbPar);

                loRtn = GetPeriodHelper(loRtnTmp);
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in PeriodListStream");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - PeriodListStream");

            return loRtn;
        }
        
        [HttpPost]
        public LastYearDTO GetLastYear()
        {
            using Activity activity = _activitySource.StartActivity("GetLastYear");

            _logger.LogInfo("Start - GetLastYear");

            R_Exception loEx = new R_Exception();
            LastYearDTO loRtn = null;
            GSM07510DTO loDbPar;
            GSM07510Cls loCls;
            LastYearDTO loResult = null;

            try
            {
                loDbPar = new GSM07510DTO();
                _logger.LogInfo("Set Parameters");

                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                loCls = new GSM07510Cls();
                loResult = loCls.GetLastYear(loDbPar);

            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in GetLastYear");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - GetLastYear");

            return loResult;
        }

        private async IAsyncEnumerable<GSM07510DTO> GetPeriodHelper (List<GSM07510DTO> poParameter)
        {
            foreach (GSM07510DTO item in poParameter)
            {
                yield return item;
            }
        }
    }
}
