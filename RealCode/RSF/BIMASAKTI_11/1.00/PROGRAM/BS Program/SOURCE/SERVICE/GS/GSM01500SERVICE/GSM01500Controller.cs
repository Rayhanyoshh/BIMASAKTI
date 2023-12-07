using GSM01500BACK;
using GSM01500COMMON;
using GSM01500COMMON.DTOs;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;

namespace GSM01500SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GSM01500Controller : ControllerBase, IGSM01500
    {
        [HttpPost]
        public IAsyncEnumerable<GSM01500DTO> GetCenterList()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<GSM01500DTO> loRtn = null;
            GetCenterListParameterDTO loParam = new GetCenterListParameterDTO();
            GSM01500Cls loCls = new GSM01500Cls();
            List<GSM01500DTO> loTempRtn = null;

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loTempRtn = loCls.GetCenterList(loParam);

                loRtn = GetCenterStream(loTempRtn);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        private async IAsyncEnumerable<GSM01500DTO> GetCenterStream(List<GSM01500DTO> poParameter)
        {
            foreach (GSM01500DTO item in poParameter)
            {
                yield return item;
            }
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01500DTO> poParameter)
        {
            R_Exception loException = new R_Exception();
            R_ServiceDeleteResultDTO loRtn = new R_ServiceDeleteResultDTO();
            CreateUpdateDeleteParameterDTO loParam = new CreateUpdateDeleteParameterDTO();
            GSM01500Cls loCls = new GSM01500Cls();

            try
            {
                loParam.Data = poParameter.Entity;
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                loCls.R_Delete(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01500DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01500DTO> poParameter)
        {
            var loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01500DTO> loRtn = new R_ServiceGetRecordResultDTO<GSM01500DTO>();
            CreateUpdateDeleteParameterDTO loParam = new CreateUpdateDeleteParameterDTO();
            R_ServiceSaveResultDTO<CreateUpdateDeleteParameterDTO> loTempRtn = new R_ServiceSaveResultDTO<CreateUpdateDeleteParameterDTO>();


            try
            {
                var loCls = new GSM01500Cls();

                loParam.Data = poParameter.Entity;
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loTempRtn.data = loCls.R_GetRecord(loParam);

                loRtn.data = loTempRtn.data.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public CopyFromProcessDTO CopyFromProcess()
        {
            R_Exception loException = new R_Exception();
            CopyFromProcessParameter loParam = new CopyFromProcessParameter();
            CopyFromProcessDTO loRtn = new CopyFromProcessDTO();
            GSM01500Cls loCls = new GSM01500Cls();

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
        public R_ServiceSaveResultDTO<GSM01500DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01500DTO> poParameter)
        {
            R_Exception loException = new R_Exception();
            R_ServiceSaveResultDTO<GSM01500DTO> loRtn = new R_ServiceSaveResultDTO<GSM01500DTO>();
            R_ServiceSaveResultDTO<CreateUpdateDeleteParameterDTO> loTempRtn = new R_ServiceSaveResultDTO<CreateUpdateDeleteParameterDTO>();
            GSM01500Cls loCls = new GSM01500Cls();
            CreateUpdateDeleteParameterDTO loParam = new CreateUpdateDeleteParameterDTO();

            try
            {
                loParam.Data = poParameter.Entity;
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                loTempRtn.data = loCls.R_Save(loParam, poParameter.CRUDMode);
                loRtn.data = loTempRtn.data.Data;
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<CopyFromProcessCompanyDTO> loRtn = null;
            GetCenterListParameterDTO loParam = new GetCenterListParameterDTO();
            GSM01500Cls loCls = new GSM01500Cls();
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
        private async IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyStream(List<CopyFromProcessCompanyDTO> poParameter)
        {
            foreach (CopyFromProcessCompanyDTO item in poParameter)
            {
                yield return item;
            }
        }

        [HttpPost]
        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_CENTERMethod()
        {
            R_Exception loException = new R_Exception();
            ActiveInactiveParameterDTO loParam = new ActiveInactiveParameterDTO();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            GSM01500Cls loCls = new GSM01500Cls();

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CCENTER_CODE = R_Utility.R_GetContext<string>(ContextConstant.ACTIVE_INACTIVE_CENTER_CODE_CONTEXT);
                loParam.LACTIVE = R_Utility.R_GetContext<bool>(ContextConstant.ACTIVE_INACTIVE_LACTIVE_CONTEXT);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls.RSP_GS_ACTIVE_INACTIVE_CENTERMethod(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}
