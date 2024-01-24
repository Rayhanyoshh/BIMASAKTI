using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using GSM01200Back;
using Microsoft.Extensions.Logging;
using R_BackEnd;

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01200Controller: ControllerBase, IGSM01200
    {
        private readonly ActivitySource _activitySource;
        private LoggerGSM01000 _logger;
        
        public GSM01200Controller(ILogger<GSM01200Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM01000.R_InitializeLogger(logger);
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_InitializeAndGetActivitySource(nameof(GSM01000Controller));

        }
        
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01200DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01200DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceGetRecord");

            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01200DTO> loRtn = null;
            GSM01200Cls loCls;
    
            try
            {
                _logger.LogInfo("Start - R_ServiceGetRecord");

                loCls = new GSM01200Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01200DTO>();
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

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM01200DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01200DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceSave");

            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM01200DTO> loRtn = null;
            GSM01200Cls loCls;
    
            try
            {
                _logger.LogInfo("Start - R_ServiceSave");

                loCls = new GSM01200Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM01200DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CCREATE_BY = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CUPDATE_BY = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
        
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);

                _logger.LogInfo("End - R_ServiceSave");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in R_ServiceSave: {ex.Message}");
                loEx.Add(ex);
            }
            EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        
        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01200DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        
       [HttpPost]
        public IAsyncEnumerable<GSM01200DTO> GetCoACenterListStream()
        {
            using Activity activity = _activitySource.StartActivity("GetCoACenterListStream");

            R_Exception loException = new R_Exception();
            GOAHeadListDbParameter loDbPar;
            List<GSM01200DTO> loRtnTmp;
            GSM01200Cls loCls;
            IAsyncEnumerable<GSM01200DTO> loRtn = null;

            try
            {
                _logger.LogInfo("Start - GetCoACenterListStream");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CGLACCOUNT_NO = "11.10.1000";

                loCls = new GSM01200Cls();
                loRtnTmp = loCls.GetCoACenterListDb(loDbPar);
                loRtn = GetCOACenterStream(loRtnTmp);

                _logger.LogInfo("End - GetCoACenterListStream");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetCoACenterListStream: {ex.Message}");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public GSM01200CenterListDTO GetCoACenterList()
        {
            using Activity activity = _activitySource.StartActivity("GetCoACenterList");

            R_Exception loEx = new R_Exception();
            GSM01200CenterListDTO loRtn = null;
            GOAHeadListDbParameter loDbPar;
            List<GSM01200DTO> loResult;
            GSM01200Cls loCls;

            try
            {
                _logger.LogInfo("Start - GetCoACenterList");

                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CGLACCOUNT_NO = "11.10.1000";   

                loCls = new GSM01200Cls();
                loResult = loCls.GetCoACenterListDb(loDbPar);
                loRtn = new GSM01200CenterListDTO { Data = loResult };

                _logger.LogInfo("End - GetCoACenterList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetCoACenterList: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        return loRtn;
    
    }


    private async IAsyncEnumerable<GSM01200DTO> GetCOACenterStream(List<GSM01200DTO> poParameter)
    {
        foreach (GSM01200DTO item in poParameter)
        {
            yield return item;
        }
    }
    
        [HttpPost]
        public IAsyncEnumerable<AssignCenterDTO> GetCenterToAssignList()
        {
            using Activity activity = _activitySource.StartActivity("GetCenterToAssignList");

            CenterAssignParameter loDbPar;
            List<AssignCenterDTO> loRtnTemp = null;
            R_Exception loEx = new R_Exception();
            GSM01200Cls loCls;
            try
            {
                _logger.LogInfo("Start - GetCenterToAssignList");

                loDbPar = new CenterAssignParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CPROGRAM_ID = "GSM01000";
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                // loDbPar.CCOMPANY_ID = "RCD";
                loCls = new GSM01200Cls();
                loRtnTemp = loCls.GetCenterToAssignList(loDbPar);

                _logger.LogInfo("End - GetCenterToAssignList");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in GetCenterToAssignList: {ex.Message}");
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return GetCenterListHelper(loRtnTemp);
        }

        [HttpPost]
        public GSM01200DTO GetParameterInfo()
        {
            using Activity activity = _activitySource.StartActivity("GetParameterInfo");

            R_Exception loException = new R_Exception();
            ParameterHeadGLDbParameter loDbPar;
            GSM01200Cls loCls;
            GSM01200DTO loReturn = null;

            try
            {
                _logger.LogInfo("Start - GetParameterInfo");

                loDbPar = new ParameterHeadGLDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                if (loDbPar.CCOMPANY_ID != null)
                {
                    loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                    loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                }
                loCls = new GSM01200Cls();
                loReturn = loCls.GSM01200GetParameterDb(loDbPar);

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

        [HttpPost]
        public AssignCenterResultDTO AssignCenterAction(CentertoAssignParam poParam)
        {
            using Activity activity = _activitySource.StartActivity("AssignCenterAction");

            var loEx = new R_Exception();
            AssignCenterResultDTO loRtn = new AssignCenterResultDTO();
            CentertoAssignParam loparam;

            try
            {
                _logger.LogInfo("Start - AssignCenterAction");

                GSM01200Cls loCls = new GSM01200Cls();

                loparam = new CentertoAssignParam()
                {
                    CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID,
                    CUSER_ID = R_BackGlobalVar.USER_ID,
                    CGLACCOUNT_NO = poParam.CGLACCOUNT_NO,
                    CCENTER_LIST = poParam.CCENTER_LIST
                };
                
                loCls.SaveAssignCenterToDb(loparam);

                _logger.LogInfo("End - AssignCenterAction");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in AssignCenterAction: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }



        private async IAsyncEnumerable<AssignCenterDTO> GetCenterListHelper(List<AssignCenterDTO> loRtnTemp)
        {
            foreach (AssignCenterDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }
    }
}
