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

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01310Controller : ControllerBase, IGSM01310
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;

        
        public GSM01310Controller(ILogger<GSM01310Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM01000.R_InitializeLogger(logger);
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_InitializeAndGetActivitySource(nameof(GSM01000Controller));

        }
        
       [HttpPost]
       public R_ServiceGetRecordResultDTO<GSM01310DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01310DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceGetRecord");

            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01310DTO> loRtn = null;
            GSM01310Cls loCls;

            try
            {
                _logger.LogInfo("Start - R_ServiceGetRecord");

                loCls = new GSM01310Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01310DTO>();

                _logger.LogInfo("Setting parameters");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                _logger.LogInfo("Fetching record data");
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in R_ServiceGetRecord: {ex.Message}");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            _logger.LogInfo("End - R_ServiceGetRecord");

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM01310DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01310DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceSave");

            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM01310DTO> loRtn = null;
            GSM01310Cls loCls;

            try
            {
                _logger.LogInfo("Start - R_ServiceSave");

                loCls = new GSM01310Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM01310DTO>();

                _logger.LogInfo("Setting parameters");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CCREATE_BY = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CUPDATE_BY = R_BackGlobalVar.USER_ID;

                _logger.LogInfo("Saving data");
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in R_ServiceSave: {ex.Message}");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            _logger.LogInfo("End - R_ServiceSave");

            return loRtn;
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01310DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public GSM01310ListDTO GetGoACoA()
        {
            using Activity activity = _activitySource.StartActivity("GetGoACoA");

            R_Exception loEx = new R_Exception();
            GSM01310ListDTO loRtn = null;
            GoAMainDbParameter loDbPar;
            GSM01310Cls loCls;

            try
            {
                _logger.LogInfo("Start - GetGoACoA");

                loRtn = new GSM01310ListDTO();
                loDbPar = new GoAMainDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGOA_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGOA_CODE);

                _logger.LogInfo("Fetching data from the database");
                loCls = new GSM01310Cls();
                loRtn.Data = loCls.GetGoACoAList(loDbPar);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetGoACoA: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            _logger.LogInfo("End - GetGoACoA");

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM01310DTO> GetGoACoAListStream()
        {
            using Activity activity = _activitySource.StartActivity("GetGoACoAListStream");

            R_Exception loException = new R_Exception();
            GoAMainDbParameter loDbPar;
            List<GSM01310DTO> loRtnTmp;
            GSM01310Cls loCls;
            IAsyncEnumerable<GSM01310DTO> loRtn = null;

            try
            {
                _logger.LogInfo("Start - GetGoACoAListStream");

                var liCompanyId = R_BackGlobalVar.COMPANY_ID;
                loDbPar = new GoAMainDbParameter();

                _logger.LogInfo("Creating GSM01310Cls instance");
                loCls = new GSM01310Cls();

                _logger.LogInfo("Fetching data from the database");
                loRtnTmp = loCls.GetGoACoAList(loDbPar);

                _logger.LogInfo("Converting data to IAsyncEnumerable");
                loRtn = GetGOACOAListStream(loRtnTmp);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetGoACoAListStream: {ex.Message}");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            _logger.LogInfo("End - GetGoACoAListStream");

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM01310DTO> GetCoAToAssignList()
        {
            using Activity activity = _activitySource.StartActivity("GetCoAToAssignList");

            GOAHeadListDbParameter loDbPar;
            List<GSM01310DTO> loRtnTemp = null;
            R_Exception loEx = new R_Exception();
            GSM01310Cls loCls;
            try
            {
                _logger.LogInfo("Start - GetCoAToAssignList");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                _logger.LogInfo("Creating GSM01310Cls instance");
                loCls = new GSM01310Cls();

                _logger.LogInfo("Fetching data for CoA assignment");
                loRtnTemp = loCls.GetCoAToAssignList(loDbPar);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetCoAToAssignList: {ex.Message}");
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - GetCoAToAssignList");

            return GetCoAListHelper(loRtnTemp);
        }

        
        private async IAsyncEnumerable<GSM01310DTO> GetCoAListHelper(List<GSM01310DTO> loRtnTemp)
        {
            foreach (GSM01310DTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        private async IAsyncEnumerable<GSM01310DTO> GetGOACOAListStream(List<GSM01310DTO> poParameter)
        {
            foreach (GSM01310DTO item in poParameter)
            {
                yield return item;
            }
        }
    }
}
