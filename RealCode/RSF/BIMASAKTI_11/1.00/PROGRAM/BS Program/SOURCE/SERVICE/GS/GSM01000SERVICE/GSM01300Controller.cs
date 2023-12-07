using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using Microsoft.Extensions.Logging;
using R_BackEnd;

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01300Controller : ControllerBase, IGSM01300
    {
        private LoggerGSM01000 _logger;
        
        public GSM01300Controller(ILogger<GSM01300Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM01000.R_InitializeLogger(logger);
            _logger = LoggerGSM01000.R_GetInstanceLogger();
        }
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01300DTO> R_ServiceGetRecord(
            R_ServiceGetRecordParameterDTO<GSM01300DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01300DTO> loRtn = null;
            GSM01300Cls loCls;

            try
            {
                _logger.LogInfo("Start - R_ServiceGetRecord");

                loCls = new GSM01300Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01300DTO>();
                _logger.LogInfo("Set Parameter - R_ServiceGetRecord");
                
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                // poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CGOA_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGOA_CODE);

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
        public R_ServiceSaveResultDTO<GSM01300DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01300DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01300DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public GSM01300ListDTO GetAllGOA()
        {
            R_Exception loEx = new R_Exception();
            GSM01300ListDTO loRtn = null;
            List<GSM01300DTO> loResult;
            GOAHeadListDbParameter loDbPar;
            GSM01300Cls loCls;

            try
            {
                _logger.LogInfo("Start - GetAllGOA");

                _logger.LogInfo("Set Parameter GSM01300Cls instance");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CCOMPANY_ID = "RCD";

                _logger.LogInfo("Creating GSM01300Cls instance");
                loCls = new GSM01300Cls();

                _logger.LogInfo("Fetching GOA data from the database");
                loResult = loCls.GetGoAListDb(loDbPar);

                loRtn = new GSM01300ListDTO { Data = loResult };

                _logger.LogInfo("End - GetAllGOA");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetAllGOA: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM01300DTO> GetAllGOAStream()
        {
            R_Exception loException = new R_Exception();
            GOAHeadListDbParameter loDbPar;
            List<GSM01300DTO> loRtnTmp;
            GSM01300Cls loCls;
            IAsyncEnumerable<GSM01300DTO> loRtn = null;

            try
            {
                _logger.LogInfo("Start - GetAllGOAStream");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = "RCD";

                _logger.LogInfo("Creating GSM01300Cls instance");
                loCls = new GSM01300Cls();

                _logger.LogInfo("Fetching GOA data from the database");
                loRtnTmp = loCls.GetGoAListDb(loDbPar);

                _logger.LogInfo("Converting GOA data to IAsyncEnumerable");
                loRtn = GetGOAStream(loRtnTmp);

                _logger.LogInfo("End - GetAllGOAStream");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetAllGOAStream: {ex.Message}");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public AssignCOAResultDTO AssignCOAAction(COAtoAssignParam poParam)
        {
            var loEx = new R_Exception();
            AssignCOAResultDTO loRtn = new AssignCOAResultDTO();
            COAtoAssignParam loparam;

            try
            {
                _logger.LogInfo("Start - AssignCOAAction");

                _logger.LogInfo("Creating GSM01300Cls instance");
                GSM01300Cls loCls = new GSM01300Cls();

                loparam = new COAtoAssignParam()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CGOA_CODE = poParam.CGOA_CODE,
                    CCOA_LIST = poParam.CCOA_LIST,
                };

                _logger.LogInfo("Saving COA assignment to the database");
                loCls.SaveAssignCOAToDb(loparam);

                _logger.LogInfo("End - AssignCOAAction");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in AssignCOAAction: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }


        private async IAsyncEnumerable<GSM01300DTO> GetGOAStream(List<GSM01300DTO> poParameter)
        {
            foreach (GSM01300DTO item in poParameter)
            {
                yield return item;
            }
        }
        
       
    }
}