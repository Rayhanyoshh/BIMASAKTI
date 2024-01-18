using System.Diagnostics;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Authorization;
using R_BackEnd;

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"),AllowAnonymous]
    public class GSM01000Controller : ControllerBase, IGSM01000
    {
        private readonly ActivitySource _activitySource;
        private LoggerGSM01000 _logger;
        
        public GSM01000Controller(ILogger<GSM01000Controller> logger)
        {
            //Initial and Get Logger
            LoggerGSM01000.R_InitializeLogger(logger);
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_InitializeAndGetActivitySource(nameof(GSM01000Controller));

        }

        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01000DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01000DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceGetRecord");

            _logger.LogInfo("Start - Get Account Record");

            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01000DTO> loRtn = null;
            GSM01000Cls loCls;

            try
            {
                loCls = new GSM01000Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01000DTO>();

                _logger.LogInfo("Set Parameter");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                _logger.LogInfo("Get Account Record");
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - Get Account Record");
            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM01000DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01000DTO> poParameter)
         {
            using Activity activity = _activitySource.StartActivity("R_ServiceSave");
            _logger.LogInfo("Start - Save Account Record");
            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM01000DTO> loRtn = null;
            GSM01000Cls loCls;

            try
            {
                loCls = new GSM01000Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM01000DTO>();
                _logger.LogInfo("Set Parameter");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                // Cek dan isi dengan string kosong jika null
                if (poParameter.Entity.CCASH_FLOW_CODE == null)
                {
                    poParameter.Entity.CCASH_FLOW_CODE = "";
                }

                if (poParameter.Entity.CCASH_FLOW_GROUP_CODE == null)
                {
                    poParameter.Entity.CCASH_FLOW_GROUP_CODE = "";
                }
                
            
                _logger.LogInfo("Save Account Entity");
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - Save Account Entity");
            return loRtn;
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01000DTO> poParameter)
        {
            using Activity activity = _activitySource.StartActivity("R_ServiceDelete");

            _logger.LogInfo("Start - Delete Tax Entity");
            R_Exception loEx = new R_Exception();
            R_ServiceDeleteResultDTO loRtn = new R_ServiceDeleteResultDTO();
            GSM01000Cls loCls;

            try
            {
                _logger.LogInfo("Set Parameter");
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                // poParameter.Entity.CCOMPANY_ID = "RCD";
                // poParameter.Entity.CUSER_ID = "Admin";
                loCls = new GSM01000Cls();
                _logger.LogInfo("Delete Account Entity");
                loCls.R_Delete(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End - Delete Account Entity");
            return loRtn;
        }

        [HttpPost]
        public GSM01000ListDTO GetAllCOA()
        {
            using Activity activity = _activitySource.StartActivity("GetAllCOA");

            _logger.LogInfo("Start - Get COA List");
            R_Exception loEx = new R_Exception();
            GSM01000ListDTO loRtn = null;
            List<GSM01000DTO> loResult;
            COAListDbParameter loDbPar;
            GSM01000Cls loCls;

            try
            {
                loDbPar = new COAListDbParameter();
                _logger.LogInfo("Set Parameter");
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;

                // loDbPar.CUSER_ID = "Admin";
                // loDbPar.CCOMPANY_ID = "RCD";
                loCls = new GSM01000Cls();
                _logger.LogInfo("Get COA List");
                loResult = loCls.GetCoaListDb(loDbPar);
                loRtn = new GSM01000ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
    
            loEx.ThrowExceptionIfErrors();
            _logger.LogInfo("End- Get COA List");
            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM01000DTO> GetAllCOAStream()
        {
            using Activity activity = _activitySource.StartActivity("GetAllCOAStream");

            _logger.LogInfo("Start - Get COA List Stream");
            R_Exception loException = new R_Exception();
            COAListDbParameter loDbPar;
            List<GSM01000DTO> loRtnTmp;
            GSM01000Cls loCls;
            IAsyncEnumerable<GSM01000DTO> loRtn = null;

            try
            {
                loDbPar = new COAListDbParameter();
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CUSER_ID = "Admin";

                loCls = new GSM01000Cls();
                loRtnTmp = loCls.GetCoaListDb(loDbPar);

                loRtn = GetCOAStream(loRtnTmp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList()
        {
            using Activity activity = _activitySource.StartActivity("GetCompanyList");

            _logger.LogInfo("Start - Get Company List");
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<CopyFromProcessCompanyDTO> loRtn = null;
            COAListDbParameter loParam = new COAListDbParameter();
            GSM01000Cls loCls = new GSM01000Cls();
            List<CopyFromProcessCompanyDTO> loTempRtn = null;

            try
            {
                _logger.LogInfo("Set Parameter");
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loTempRtn = loCls.GetCompanyList(loParam);
                _logger.LogInfo("Get Company List");
                loRtn = GetCompanyStream(loTempRtn);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - Get Company List");
            return loRtn;
        }

        [HttpPost]
        public CopyFromProcess CopyFromProcess()
        {
            using Activity activity = _activitySource.StartActivity("CopyFromProcess");

            _logger.LogInfo("Start - Copy From Process");
            R_Exception loException = new R_Exception();
            CopyFromProcessParameter loParam = new CopyFromProcessParameter();
            CopyFromProcess loRtn = new CopyFromProcess();
            GSM01000Cls loCls = new GSM01000Cls(); 
            try
            {
                _logger.LogInfo("Set Parameter");
                loParam.CLOGIN_COMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CCOPY_FROM_COMPANY_ID = R_Utility.R_GetContext<string>(ContextConstant.COPY_FROM_COMPANY_ID_CONTEXT);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                _logger.LogInfo("Get Copy From Process");
                loCls.CopyFromProcessGSM01000Cls(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - Copy From Process");
            return loRtn;        
        }

        [HttpPost]
        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_COAMethod()
        {
            using Activity activity = _activitySource.StartActivity("RSP_GS_ACTIVE_INACTIVE_COAMethod");

            _logger.LogInfo("Start - Set Active/Inactive");
            R_Exception loException = new R_Exception();
            ActiveInactiveParameterDTO loParam = new ActiveInactiveParameterDTO();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            GSM01000Cls loCls = new GSM01000Cls();

            try
            {
                _logger.LogInfo("Set Parameter");
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CGLACCOUNT_NO = R_Utility.R_GetContext<string>(ContextConstant.ACTIVE_INACTIVE_CGL_NO_CONTEXT);
                loParam.LACTIVE = R_Utility.R_GetContext<bool>(ContextConstant.ACTIVE_INACTIVE_LACTIVE_CONTEXT);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                _logger.LogInfo("Set Active/Inactive");

                loCls.RSP_GS_ACTIVE_INACTIVE_COA_Method(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - Set Active/Inactive");
            return loRtn;
        }
        
        [HttpPost]
        public PrimaryAccountDTO PrimaryAccountCheck()
        {
            using Activity activity = _activitySource.StartActivity("PrimaryAccountCheck");

            _logger.LogInfo("Start - Primary account check");

            R_Exception loException = new R_Exception();
            GSM01000DTO loParam = new GSM01000DTO();
            PrimaryAccountDTO loRtn = new PrimaryAccountDTO();
            GSM01000Cls loCls = new GSM01000Cls(); 
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

        [HttpPost]
        public GSM01000CoAExcelDTO CoAExcelTemplate()
        {
            using Activity activity = _activitySource.StartActivity("CoAExcelTemplate");

            _logger.LogInfo("Start - Template Download Process");

            var loEx = new R_Exception();
            var loRtn = new GSM01000CoAExcelDTO();

            try
            {
                _logger.LogInfo("Taking Resource file From API");
                Assembly loAsm = Assembly.Load("BIMASAKTI_GS_API");
                var lcResourceFile = "BIMASAKTI_GS_API.Template.Chart of Account.xlsx";

                using (Stream resFilestream = loAsm.GetManifestResourceStream(lcResourceFile))
                {
                    var ms = new MemoryStream();
                    resFilestream.CopyTo(ms);
                    var bytes = ms.ToArray();

                    loRtn.FileBytes = bytes;
                }

                _logger.LogInfo("Resource file successfully loaded from API");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            _logger.LogInfo("End - Template Download Process");

            return loRtn;
        }

        [HttpPost]
        public GSM01000UploadHeaderDTO CompanyDetail()
        {
            using Activity activity = _activitySource.StartActivity("CompanyDetail");

            R_Exception loException = new R_Exception();
            COAListDbParameter loParam = new COAListDbParameter();
            GSM01000UploadHeaderDTO loRtn = new GSM01000UploadHeaderDTO();
            GSM01000Cls loCls = new GSM01000Cls(); 

            try
            {
                _logger.LogInfo("Getting Company Details");

                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loRtn = loCls.CompanyDetailCls(loParam);

                _logger.LogInfo("Company Details successfully retrieved");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        private async IAsyncEnumerable<GSM01000DTO> GetCOAStream(List<GSM01000DTO> poParameter)
        {
            foreach (GSM01000DTO item in poParameter)
            {
                _logger.LogInfo($"Yielding item with ID: {item}");
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