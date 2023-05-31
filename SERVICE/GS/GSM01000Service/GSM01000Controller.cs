using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_BackEnd;
using GSM001000Back;

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01000Controller : ControllerBase, IGSM01000
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01000DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01000DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01000DTO> loRtn = null;
            GSM01000Cls loCls;

            try
            {
                loCls = new GSM01000Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01000DTO>();

                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CUSER_ID = "Admin";
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM01000DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01000DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM01000DTO> loRtn = null;
            GSM01000Cls loCls;

            try
            {
                loCls = new GSM01000Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM01000DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CUSER_ID = "Admin";

                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01000DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceDeleteResultDTO loRtn = new R_ServiceDeleteResultDTO();
            GSM01000Cls loCls;

            try
            {
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CUSER_ID = "Admin";
                loCls = new GSM01000Cls();
                loCls.R_Delete(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public GSM01000ListDTO GetAllCOA()
        {
            R_Exception loEx = new R_Exception();
            GSM01000ListDTO loRtn = null;
            List<GSM01000DTO> loResult;
            COAListDbParameter loDbPar;
            GSM01000Cls loCls;

            try
            {
                loDbPar = new COAListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;

                loDbPar.CUSER_ID = "Admin";
                loDbPar.CCOMPANY_ID = "RCD";

                loCls = new GSM01000Cls();
                loResult = loCls.GetCoaListDb(loDbPar);
                loRtn = new GSM01000ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM01000DTO> GetAllCOAStream()
        {
            R_Exception loException = new R_Exception();
            COAListDbParameter loDbPar;
            List<GSM01000DTO> loRtnTmp;
            GSM01000Cls loCls;
            IAsyncEnumerable<GSM01000DTO> loRtn = null;

            try
            {
                loDbPar = new COAListDbParameter();
                loDbPar.CCOMPANY_ID = "RCD";
                loDbPar.CUSER_ID = "Admin";

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
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<CopyFromProcessCompanyDTO> loRtn = null;
            GetCompanyDTO loParam = new GetCompanyDTO();
            GSM01000Cls loCls = new GSM01000Cls();
            List<CopyFromProcessCompanyDTO> loTempRtn = null;

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loTempRtn = loCls.GetCompanyList();

                loRtn = GetCompanyStream(loTempRtn);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public CopyFromProcessDTO CopyFromProcess()
        {
            R_Exception loException = new R_Exception();
            CopyFromProcessParameter loParam = new CopyFromProcessParameter();
            CopyFromProcessDTO loRtn = new CopyFromProcessDTO();
            GSM01000Cls loCls = new GSM01000Cls();

            try
            {
                loParam.CLOGIN_COMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CCOPY_FROM_COMPANY_ID = R_Utility.R_GetContext<string>(ContextConstant.COPY_FROM_COMPANY_ID_CONTEXT);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls.CopyFromProcess(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;        
        }

        [HttpPost]
        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_COAMethod()
        {
            R_Exception loException = new R_Exception();
            ActiveInactiveParameterDTO loParam = new ActiveInactiveParameterDTO();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            GSM01000Cls loCls = new GSM01000Cls();

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CGLACCOUNT_NO = R_Utility.R_GetContext<string>(ContextConstant.ACTIVE_INACTIVE_CGL_NO_CONTEXT);
                loParam.LACTIVE = R_Utility.R_GetContext<bool>(ContextConstant.ACTIVE_INACTIVE_LACTIVE_CONTEXT);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls.RSP_GS_ACTIVE_INACTIVE_COA_Method(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        

        private async IAsyncEnumerable<GSM01000DTO> GetCOAStream(List<GSM01000DTO> poParameter)
        {
            foreach (GSM01000DTO item in poParameter)
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