using System.Diagnostics;
using System.Reflection;
using GSM008500Common;
using GSM008500Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM08500Common;
using GSM08500Common.DTOs;
using GSM08500Back;
using Microsoft.Extensions.Logging;
using R_BackEnd;

namespace GSM08500Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    
    public class GSM08500Controller : ControllerBase, IGSM08500
    {
        private LoggerGSM08500 _logger;
        private readonly ActivitySource _activitySource;
        
        public GSM08500Controller(ILogger<GSM08500Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM08500.R_InitializeLogger(logger);
            _logger = LoggerGSM08500.R_GetInstanceLogger();
            _activitySource = GSM08500Activity.R_InitializeAndGetActivitySource(nameof(GSM08500Controller));

        }  
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM08500DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM08500DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceGetRecord");

            _logger.LogInfo("Start - R_ServiceGetRecord");

            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM08500DTO> loRtn = null;
            GSM08500Cls loCls;

            try
            {
                _logger.LogInfo("Set parameters");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);

                loCls = new GSM08500Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM08500DTO>();

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
        public R_ServiceSaveResultDTO<GSM08500DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM08500DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceSave");
            _logger.LogInfo("Start - R_ServiceSave");

            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM08500DTO> loRtn = null;
            GSM08500Cls loCls;

            try {
                _logger.LogInfo("Set parameters");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                // poParameter.Entity.CCOMPANY_ID = "RCD";
                // poParameter.Entity.CUSER_ID = "Admin";

                loCls = new GSM08500Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM08500DTO>();

                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                // Log the exception using _logger
                _logger.LogError(ex, "An error occurred in R_ServiceSave");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - R_ServiceSave");

            return loRtn;  
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM08500DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceDelete");
            _logger.LogInfo("Start - R_ServiceDelete");

            R_Exception loEx = new R_Exception();
            R_ServiceDeleteResultDTO loRtn = new R_ServiceDeleteResultDTO();
            GSM08500Cls loCls;

            try
            {
                _logger.LogInfo("Set parameters");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls = new GSM08500Cls();
                loCls.R_Delete(poParameter.Entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in R_ServiceDelete");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - R_ServiceDelete");

            return loRtn;
            
        }
        
        [HttpPost]
        public GSM08500ListDTO GetStatisticAccDbList()
        {
            using Activity activity = _activitySource.StartActivity("GetStatisticAccDbList");

            _logger.LogInfo("Start - GetStatisticAccDbList");

            R_Exception loEx = new R_Exception();
            GSM08500ListDTO loRtn = null;
            List<GSM08500DTO> loResult;
            GSM08500DTO loDbPar;
            GSM08500Cls loCls;

            try
            {
                _logger.LogInfo("Set parameters");
                loDbPar = new GSM08500DTO();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls = new GSM08500Cls();
                _logger.LogInfo("Exec GetStatisticAccDbList");
                loResult = loCls.GetStatAccListDb(loDbPar);
                loRtn = new GSM08500ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetStatisticAccDbList");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - GetStatisticAccDbList");

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM08500DTO> GetStatisticAccDbListStream()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList()
        {
            using Activity activity = _activitySource.StartActivity("GetCompanyList");
            _logger.LogInfo("Start - GetCompanyList");

            R_Exception loException = new R_Exception();
            IAsyncEnumerable<CopyFromProcessCompanyDTO> loRtn = null;
            GetCompanyDTO loParam = new GetCompanyDTO();
            GSM08500Cls loCls = new GSM08500Cls();
            List<CopyFromProcessCompanyDTO> loTempRtn = null;

            try
            {
                _logger.LogInfo("Set parameters");
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                _logger.LogInfo("Exec GetCompanyList");

                loTempRtn = loCls.GetCompanyList();
                loRtn = GetCompanyStream(loTempRtn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in GetCompanyList");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - GetCompanyList");

            return loRtn;
        }

        [HttpPost]
        public CopyFromProcess CopyFromProcess()
        {
            using Activity activity = _activitySource.StartActivity("CopyFromProcess");
            _logger.LogInfo("Start - CopyFromProcess");

            R_Exception loException = new R_Exception();
            CopyFromProcessParameter loParam = new CopyFromProcessParameter();
            CopyFromProcess loRtn = new CopyFromProcess();
            GSM08500Cls loCls = new GSM08500Cls();

            try
            {
                _logger.LogInfo("Set parameters");
                loParam.CLOGIN_COMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CCOPY_FROM_COMPANY_ID = R_Utility.R_GetContext<string>(ContextConstant.COPY_FROM_COMPANY_ID_CONTEXT);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                _logger.LogInfo("Exec CopyFromProcess");

                loCls.CopyFromProcessGSM08500Cls(loParam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CopyFromProcess");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - CopyFromProcess");

            return loRtn; 
        }

        [HttpPost]
        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_StatAccMethod()
        {
            using Activity activity = _activitySource.StartActivity("RSP_GS_ACTIVE_INACTIVE_COAMethod");
            _logger.LogInfo("Start - RSP_GS_ACTIVE_INACTIVE_StatAccMethod");

            R_Exception loException = new R_Exception();
            ActiveInactiveParameterDTO loParam = new ActiveInactiveParameterDTO();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            GSM08500Cls loCls = new GSM08500Cls();

            try
            {
                _logger.LogInfo("Set parameters");
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CGLACCOUNT_NO = R_Utility.R_GetContext<string>(ContextConstant.ACTIVE_INACTIVE_CGL_NO_CONTEXT);
                loParam.LACTIVE = R_Utility.R_GetContext<bool>(ContextConstant.ACTIVE_INACTIVE_LACTIVE_CONTEXT);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                _logger.LogInfo("Exec Active/InActive");

                loCls.RSP_GS_ACTIVE_INACTIVE_StatAcc_Method(loParam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in RSP_GS_ACTIVE_INACTIVE_StatAccMethod");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - RSP_GS_ACTIVE_INACTIVE_StatAccMethod");

            return loRtn;
        }

        [HttpPost]
        public GSM08500CoAExcelDTO CoAExcelTemplate()
        {
            using Activity activity = _activitySource.StartActivity("CoAExcelTemplate");
            _logger.LogInfo("Start - CoAExcelTemplate");

            var loEx = new R_Exception();
            var loRtn = new GSM08500CoAExcelDTO();

            try
            {
                _logger.LogInfo("Loading Excel template");
                Assembly loAsm = Assembly.Load("BIMASAKTI_GS_API");
                var lcResourceFile = "BIMASAKTI_GS_API.Template.Statistic Account.xlsx";

                using (Stream resFilestream = loAsm.GetManifestResourceStream(lcResourceFile))
                {
                    var ms = new MemoryStream();
                    resFilestream.CopyTo(ms);
                    var bytes = ms.ToArray();

                    loRtn.FileBytes = bytes;
                }
                _logger.LogInfo("Excel template loaded");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CoAExcelTemplate");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - CoAExcelTemplate");

            return loRtn;          
        }

        [HttpPost]
        public GSM08500UploadHeaderDTO CompanyDetail()
        {
            using Activity activity = _activitySource.StartActivity("CompanyDetail");
            _logger.LogInfo("Start - CompanyDetail");

            R_Exception loException = new R_Exception();
            GSM08500DTO loParam = new GSM08500DTO();
            GSM08500UploadHeaderDTO loRtn = new GSM08500UploadHeaderDTO();
            GSM08500Cls loCls = new GSM08500Cls(); 
            try
            {
                _logger.LogInfo("Set parameters");
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                _logger.LogInfo("Get Company Detail");
                loRtn = loCls.CompanyDetailCls(loParam);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred in CompanyDetail");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - CompanyDetail");

            return loRtn; 
        }

        [HttpPost]
        public PrimaryAccountDTO PrimaryAccountCheck()
        {
            using Activity activity = _activitySource.StartActivity("PrimaryAccountCheck");
            _logger.LogInfo("Start - Primary account check");

            R_Exception loException = new R_Exception();
            GSM08500DTO loParam = new GSM08500DTO();
            PrimaryAccountDTO loRtn = new PrimaryAccountDTO();
            GSM08500Cls loCls = new GSM08500Cls(); 
            try
            {
                // loParam.CCOMPANY_ID = "rcd";
                _logger.LogInfo("Set Parameter");
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                _logger.LogInfo("Get - Primary account check");
                loRtn = loCls.PrimaryAccountCheckCls(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - Primary account check");
            return loRtn; 
        }

        private async IAsyncEnumerable<GSM08500DTO> GetStatAccStream(List<GSM08500DTO> poParameter)
        {
            foreach (GSM08500DTO item in poParameter)
            {
                yield return item;
            }
        }
        
        private async IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyStream(List<CopyFromProcessCompanyDTO> poParameter)
        {
            foreach (CopyFromProcessCompanyDTO item in poParameter)
            {
                yield return item;
            }
        }
    }
}
