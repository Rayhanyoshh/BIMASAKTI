using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Common;
using GSM01000Back;
using GSM01000Common.DTOs;
using Microsoft.Extensions.Logging;
using R_BackEnd;

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01100Controller : ControllerBase, IGSM01100
    {
        private LoggerGSM01000 _logger;
        
        public GSM01100Controller(ILogger<GSM01100Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM01000.R_InitializeLogger(logger);
            _logger = LoggerGSM01000.R_GetInstanceLogger();
        }
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01100DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01100DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01100DTO> loRtn = null;
            GSM01100Cls loCls;

            try
            {
                _logger.LogInfo("Start - R_ServiceGetRecord");

                loCls = new GSM01100Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01100DTO>();

                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
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
        public R_ServiceSaveResultDTO<GSM01100DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01100DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM01100DTO> loRtn = null;
            GSM01100Cls loCls;

            try
            {
                _logger.LogInfo("Start - R_ServiceSave");

                loCls = new GSM01100Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM01100DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CCREATE_BY = R_BackGlobalVar.USER_ID;

                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);

                _logger.LogInfo("End - R_ServiceSave");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in R_ServiceSave Assign User : {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01100DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public GSM01100UserListDTO GetCoAUserList()
        {
            R_Exception loEx = new R_Exception();
            GSM01100UserListDTO loRtn = null;
            List<GSM01100DTO> loResult;
            GOAHeadListDbParameter loDbPar;
            GSM01100Cls loCls;

            try
            {
                _logger.LogInfo("Start - GetCoAUserList");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);

                loCls = new GSM01100Cls();
                loResult = loCls.GetCoAUserListDb(loDbPar);
                loRtn = new GSM01100UserListDTO { Data = loResult };

                _logger.LogInfo("End - GetCoAUserList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetCoAUserList: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM01100DTO> GetCoAUserListStream()
        {
            R_Exception loException = new R_Exception();
            GOAHeadListDbParameter loDbPar;
            List<GSM01100DTO> loRtnTmp;
            GSM01100Cls loCls;
            IAsyncEnumerable<GSM01100DTO> loRtn = null;

            try
            {
                _logger.LogInfo("Start - GetCoAUserListStream");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);

                loCls = new GSM01100Cls();
                loRtnTmp = loCls.GetCoAUserListDb(loDbPar);

                loRtn = GetCOAUserStream(loRtnTmp);

                _logger.LogInfo("End - GetCoAUserListStream");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetCoAUserListStream: {ex.Message}");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

// ... (The rest of the actions with similar log statements)


        private async IAsyncEnumerable<GSM01100DTO> GetCOAUserStream(List<GSM01100DTO> poParameter)
        {
            foreach (GSM01100DTO item in poParameter)
            {
                yield return item;
            }
        }
        
      [HttpPost]
        public IAsyncEnumerable<AssignUserDTO> GetUserToAssignList()
        {
            GOAHeadListDbParameter loDbPar;
            List<AssignUserDTO> loRtnTemp = null;
            R_Exception loEx = new R_Exception();
            GSM01100Cls loCls;
            try
            {
                _logger.LogInfo("Start - GetUserToAssignList");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CPROGRAM_ID = "GSM01000";
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                loCls = new GSM01100Cls();
                loRtnTemp = loCls.GetUserToAssignList(loDbPar);

                _logger.LogInfo("End - GetUserToAssignList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetUserToAssignList: {ex.Message}");
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return GetUserListHelper(loRtnTemp);
        }

        [HttpPost]
        public AssignUserResultDTO AssignUserAction(UsertoAssignParam poParam)
        {
            var loEx = new R_Exception();
            AssignUserResultDTO loRtn = new AssignUserResultDTO();
            GSM01100DTO loparam;

            try
            {
                _logger.LogInfo("Start - AssignUserAction");

                GSM01100Cls loCls = new GSM01100Cls();

                loparam = new GSM01100DTO()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CGLACCOUNT_NO = poParam.CGLACCOUNT_NO,
                    CUSER_LIST = poParam.CUSER_LIST
                };
                
                loCls.SaveAssignUserToDb(loparam);

                _logger.LogInfo("End - AssignUserAction");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in AssignUserAction: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public GSM01100DTO GetParameterInfo()
        {
            R_Exception loException = new R_Exception();
            UsertoAssignParam loDbPar;
            GSM01100Cls loCls;
            GSM01100DTO loReturn = null;

            try
            {
                _logger.LogInfo("Start - GetParameterInfo");

                loDbPar = new();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls = new GSM01100Cls();

                loReturn = loCls.GSM01100GetParameterDb(loDbPar);

                _logger.LogInfo("End - GetParameterInfo");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetParameterInfo: {ex.Message}");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loReturn;
        }


        private async IAsyncEnumerable<AssignUserDTO> GetUserListHelper(List<AssignUserDTO> loRtnTemp)
        {
            foreach (AssignUserDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }
    }
}

