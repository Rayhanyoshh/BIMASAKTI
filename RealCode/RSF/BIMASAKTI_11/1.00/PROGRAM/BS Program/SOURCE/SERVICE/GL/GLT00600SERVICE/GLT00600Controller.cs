using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GLT00600Back;
using GLT00600Common;
using GLT00600Common.DTOs;
using R_BackEnd;

namespace GLT00600Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GLT00600Controller : ControllerBase, IGLT00600
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GLT00600DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GLT00600DTO> poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new R_ServiceGetRecordResultDTO<GLT00600DTO>();
            GLT00600ParameterDTO loDbPar;
            try
            {
                var loCls = new GLT00600Cls();
                loDbPar = new GLT00600ParameterDTO();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;        
        }
        
        [HttpPost]
        public R_ServiceSaveResultDTO<GLT00600DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GLT00600DTO> poParameter)
        {
            R_ServiceSaveResultDTO<GLT00600DTO> loRtn = null;
            R_Exception loException = new R_Exception();
            GLT00600Cls loCls;
            try
            {
                loCls = new GLT00600Cls();
                loRtn = new R_ServiceSaveResultDTO<GLT00600DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);//call clsMethod to save
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
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GLT00600DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IAsyncEnumerable<GLT00600JournalGridDTO> GetJournalList()
        {
            R_Exception loEx = new R_Exception();
            IAsyncEnumerable<GLT00600JournalGridDTO> loRtn = null;
            GLT00600ParameterDTO loDbPar;
            GLT00600Cls loCls;
            List<GLT00600JournalGridDTO> loRtnTemp;
            try
            {
                loDbPar = new GLT00600ParameterDTO();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                loDbPar.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loDbPar.CTRANS_CODE = "000088";
                loDbPar.CPERIOD = R_Utility.R_GetContext<string>(ContextConstant.CPERIOD);
                loDbPar.CSEARCH_TEXT = R_Utility.R_GetContext<string>(ContextConstant.CSEARCH_TEXT);
                loDbPar.CDEPT_CODE = R_Utility.R_GetContext<string>(ContextConstant.CDEPT_CODE);
                loDbPar.CSTATUS = R_Utility.R_GetContext<string>(ContextConstant.CSTATUS);
                loCls = new GLT00600Cls();
                loRtnTemp = loCls.GetJournalList(loDbPar);
                loRtn = GetJournalGridStream(loRtnTemp);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GLT00600JournalGridDetailDTO> GetAllJournalDetailList()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<GLT00600JournalGridDetailDTO> loRtn = null;
            GLT00600ParameterDTO loDbPar;
            GLT00600Cls loCls;
            List<GLT00600JournalGridDetailDTO> loRtnTemp;
            try
            {
                loCls = new GLT00600Cls();
                loDbPar = new GLT00600ParameterDTO();
                loDbPar.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loDbPar.CREC_ID = R_Utility.R_GetContext<string>(ContextConstant.CREC_ID);
                loRtnTemp = loCls.GetJournalDetailList(loDbPar);
                loRtn = GetJournalGridDetailStream(loRtnTemp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            return loRtn;
        }
        
        [HttpPost]
        public GLT00600JournalGridDTO ProcessReversingJournal(GLT00600JournalGridDTO poData)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public GLT00600JournalGridDTO UndoReversingJournal(GLT00600JournalGridDTO poData)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public GLT00600JournalGridDTO RefreshCurrencyRate(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            //_loggerGLT00400.LogInfo("Start RefreshCurrencyRate GLT00400");
            GLT00600ParameterDTO loParam = new GLT00600ParameterDTO();
            GLT00600JournalGridDTO loRtn = new GLT00600JournalGridDTO();
            GLT00600Cls loCls = new GLT00600Cls();

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;
                //_loggerGLT00600.LogDebug("Go To GLT00400Cls.RefreshCurrencyRate");
                loRtn = loCls.RefreshCurrencyRate(loParam, poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            if (loEx.Haserror)
            {
                //_loggerGLT00600.LogError(loEx);
            }
            loEx.ThrowExceptionIfErrors();

            //_loggerGLT00400.LogInfo("End RefreshCurrencyRate GLT00400");
            return loRtn;
        }

        [HttpPost]
        public GLT00600JournalGridDTO JournalProcesStatus(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            GLT00600ParameterDTO loParam = new GLT00600ParameterDTO();
            GLT00600JournalGridDTO loRtn = new GLT00600JournalGridDTO();
            GLT00600Cls loCls = new GLT00600Cls();

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls.ProcessJournalStatus(loParam, poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public VAR_GSM_COMPANYDTO GetCompanyDTO()
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbParameter = new();
            VAR_GSM_COMPANYDTO loReturn = null;
            try
            {
                var loCls = new GLT00600Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loReturn = loCls.GetJournalCompany(loDbParameter);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        
        [HttpPost]
        public VAR_GL_SYSTEM_PARAMDTO GetSystemParam()
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbPar = new();
            VAR_GL_SYSTEM_PARAMDTO loReturn = null;
            GLT00600Cls loCls;
            try
            {
                loCls = new GLT00600Cls();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loReturn = loCls.GetSystemParam(loDbPar);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        
        [HttpPost]
        public VAR_USER_DEPARTMENT_LISTDTO GetDeptList()
        {
            var loEx = new R_Exception();
            VAR_USER_DEPARTMENT_LISTDTO loRtn = null;
            GLT00600ParameterDTO loDbPar;
            GLT00600Cls loCls;
            try
            {
                loDbPar = new GLT00600ParameterDTO();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                loCls = new GLT00600Cls();

                loRtn = new VAR_USER_DEPARTMENT_LISTDTO();
                loRtn.Data = loCls.GetDeptList(loDbPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;        }

        [HttpPost]
        public VAR_CCURRENT_PERIOD_START_DATEDTO GetCurrentPeriodStartDate(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbParameter = new();
            VAR_CCURRENT_PERIOD_START_DATEDTO loReturn = null;
            try
            {
                var loCls = new GLT00600Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loReturn = loCls.GetCurrentPeriodStartDate(loDbParameter, poData);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }

        [HttpPost]
        public VAR_CSOFT_PERIOD_START_DATEDTO GetSoftPeriodStartDate(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbParameter = new();
            VAR_CSOFT_PERIOD_START_DATEDTO loReturn = null;
            try
            {
                var loCls = new GLT00600Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loReturn = loCls.GetSoftPeriodStartDate(loDbParameter, poData);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;        }

        [HttpPost]
        public VAR_IUNDO_COMMIT_JRNDTO GetIOption()
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbParameter = new();
            VAR_IUNDO_COMMIT_JRNDTO loReturn = null;
            try
            {
                var loCls = new GLT00600Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loReturn = loCls.GetIOption(loDbParameter);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }

        [HttpPost]
        public VAR_GSM_TRANSACTION_CODEDTO GetLincementLapproval()
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbParameter = new();
            VAR_GSM_TRANSACTION_CODEDTO loReturn = null;
            try
            {
                var loCls = new GLT00600Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loReturn = loCls.GetLincementLapproval(loDbParameter);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }

        [HttpPost]
        public VAR_GSM_PERIODDTO GetPeriod()
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbParameter = new();
            VAR_GSM_PERIODDTO loReturn = null;
            try
            {
                var loCls = new GLT00600Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loReturn = loCls.GetPeriod(loDbParameter);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }

        [HttpPost]
        public StatusListDTO GetStatusList()
        {
            R_Exception loEx = new R_Exception();
            StatusListDTO loRtn = null;
            List<StatusDTO> loResult;
            GLT00600ParameterDTO loDbPar;
            GLT00600Cls loCls;

            try
            {
                loDbPar = new GLT00600ParameterDTO();
                
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CLANGUAGE_ID = R_BackGlobalVar.CULTURE;
                loCls = new GLT00600Cls();
                loResult = loCls.GetStatus(loDbPar);
                loRtn = new StatusListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public CurrencyCodeListDTO GetCurrencyCodeList()
        {
          
            R_Exception loEx = new R_Exception();
            CurrencyCodeListDTO loRtn = null;
            List<CurrencyCodeDTO> loResult;
            GLT00600ParameterDTO loDbPar;
            GLT00600Cls loCls;

            try
            {
                loDbPar = new GLT00600ParameterDTO();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                loCls = new GLT00600Cls();
                loResult = loCls.GetCurrency(loDbPar);
                loRtn = new CurrencyCodeListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public GetCenterListDTO GetCenterList()
        {
            R_Exception loEx = new R_Exception();
            GetCenterListDTO loRtn = null;
            List<GetCenterDTO> loResult;
            GLT00600ParameterDTO loDbPar;
            GLT00600Cls loCls;

            try
            {
                loDbPar = new GLT00600ParameterDTO();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                loCls = new GLT00600Cls();
                loResult = loCls.GetCenterList(loDbPar);
                loRtn = new GetCenterListDTO() { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public GSM_TRANSACTION_APPROVALDTO GetTransactionApproval()
        {
            R_Exception loException = new R_Exception();
            GLT00600ParameterDTO loDbParameter = new();
            GSM_TRANSACTION_APPROVALDTO loReturn = null;
            try
            {
                var loCls = new GLT00600Cls();
                loDbParameter.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loReturn = loCls.GetTransactionApproval(loDbParameter);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loReturn;
        }
        
        #region Helper
        private async IAsyncEnumerable<GLT00600JournalGridDetailDTO> GetJournalGridDetailStream(List<GLT00600JournalGridDetailDTO> poParameter)
        {
            foreach (GLT00600JournalGridDetailDTO item in poParameter)
            {
                yield return item;
            }
        }
        private async IAsyncEnumerable<GLT00600JournalGridDTO> GetJournalGridStream(List<GLT00600JournalGridDTO> poParameter)
        {
            foreach (GLT00600JournalGridDTO item in poParameter)
            {
                yield return item;
            }
        }

        #endregion
    }
}

