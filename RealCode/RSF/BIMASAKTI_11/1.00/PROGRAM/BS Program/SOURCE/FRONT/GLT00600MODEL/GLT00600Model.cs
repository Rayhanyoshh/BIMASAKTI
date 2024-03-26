using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using GLT00600Common.DTOs;
using GLT00600Common;
using R_BlazorFrontEnd;

namespace GLT00600Model
{
    public class GLT00600Model : R_BusinessObjectServiceClientBase<GLT00600DTO>, IGLT00600
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlGL";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GLT00600";
        private const string DEFAULT_MODULE = "GL";

        public GLT00600Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
        string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
        string pcModuleName = DEFAULT_MODULE,
        bool plSendWithContext = true,
        bool plSendWithToken = true) : 
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext,
        plSendWithToken)
        {
        }

        public async Task<GLT00600JournalGridListDTO> GetJournalListAsync(string lcPeriod, string lcTransCode, string lcSearch, string lcDeptCode, string lcStatus)
        {
            R_Exception loEx = new R_Exception();
            GLT00600JournalGridListDTO loResult = new GLT00600JournalGridListDTO();
            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CPERIOD, lcPeriod);
                R_FrontContext.R_SetContext(ContextConstant.CSEARCH_TEXT, lcSearch);
                R_FrontContext.R_SetContext(ContextConstant.CDEPT_CODE, lcDeptCode);
                R_FrontContext.R_SetContext(ContextConstant.CSTATUS, lcStatus);
                R_FrontContext.R_SetContext(ContextConstant.CTRANS_CODE, lcTransCode);
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                var loResTemp = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GLT00600JournalGridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetJournalList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                loResult.Data = loResTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        public async Task<GLT00600JournalGridDetailListDTO> GetAllJournalDetailListAsync(string lcCrecId)
        {
            R_Exception loEx = new R_Exception();
            GLT00600JournalGridDetailListDTO loResult = new GLT00600JournalGridDetailListDTO();
            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CREC_ID, lcCrecId);
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                var loResTemp = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GLT00600JournalGridDetailDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetAllJournalDetailList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                loResult.Data = loResTemp;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        

        public async Task ProcessReversingJournalAsync(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            GLT00600JournalGridListDTO loResult = new GLT00600JournalGridListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GLT00600JournalGridListDTO, GLT00600JournalGridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.ProcessReversingJournal),
                    poData,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }

        public async Task<GLT00600JournalGridListDTO> RefreshCurrencyRateAsync(GLT00600JournalGridDTO poData)
        {
            var loEx = new R_Exception();
            GLT00600JournalGridListDTO loResult = new GLT00600JournalGridListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GLT00600JournalGridListDTO, GLT00600JournalGridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.RefreshCurrencyRate),
                    poData,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task JournalProcessAsync(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            GLT00600JournalGridListDTO loResult = new GLT00600JournalGridListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GLT00600JournalGridListDTO, GLT00600JournalGridDTO>(
                _RequestServiceEndPoint,
                nameof(IGLT00600.JournalProcesStatus),
                poData,
                DEFAULT_MODULE,
                _SendWithContext,
                _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task UndoReversingJournalProcessAsync(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            GLT00600JournalGridListDTO loResult = new GLT00600JournalGridListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GLT00600JournalGridListDTO, GLT00600JournalGridDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.UndoReversingJournal),
                    poData,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }

        #region InitialProcess
        public async Task<VAR_GSM_COMPANYDTO> GetCompanyDTOAsync()
        {
            var loEx = new R_Exception();
            VAR_GSM_COMPANYDTO loResult = new VAR_GSM_COMPANYDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GSM_COMPANYDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetCompanyDTO),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<StatusListDTO> GetStatusListAsync()
        {
            var loEx = new R_Exception();
            StatusListDTO loResult = new StatusListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<StatusListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetStatusList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<CurrencyCodeListDTO> GetCurrencyCodeListAsync()
        {
            var loEx = new R_Exception();
            CurrencyCodeListDTO loResult = new CurrencyCodeListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<CurrencyCodeListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetCurrencyCodeList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<GetCenterListDTO> GetCenterListAsync()
        {
            var loEx = new R_Exception();
            GetCenterListDTO loResult = new GetCenterListDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GetCenterListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetCenterList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        
        public async Task<VAR_GL_SYSTEM_PARAMDTO> GetSystemParamAsync()
        {
            var loEx = new R_Exception();
            VAR_GL_SYSTEM_PARAMDTO loResult = new VAR_GL_SYSTEM_PARAMDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GL_SYSTEM_PARAMDTO>(
                   _RequestServiceEndPoint,
                   nameof(IGLT00600.GetSystemParam),
                   DEFAULT_MODULE,
                   _SendWithContext,
                   _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<VAR_USER_DEPARTMENT_LISTDTO> GetDeptListAsync()
        {
            var loEx = new R_Exception();
            VAR_USER_DEPARTMENT_LISTDTO loResult = new VAR_USER_DEPARTMENT_LISTDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_USER_DEPARTMENT_LISTDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetDeptList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<VAR_CCURRENT_PERIOD_START_DATEDTO> GetCurrentPeriodStartDateAsync(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            var loEx = new R_Exception();
            VAR_CCURRENT_PERIOD_START_DATEDTO loResult = new VAR_CCURRENT_PERIOD_START_DATEDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_CCURRENT_PERIOD_START_DATEDTO, VAR_GL_SYSTEM_PARAMDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetCurrentPeriodStartDate),
                    poData,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<VAR_CSOFT_PERIOD_START_DATEDTO> GetSoftPeriodStartDateAsync(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            var loEx = new R_Exception();
            VAR_CSOFT_PERIOD_START_DATEDTO loResult = new VAR_CSOFT_PERIOD_START_DATEDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_CSOFT_PERIOD_START_DATEDTO, VAR_GL_SYSTEM_PARAMDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetSoftPeriodStartDate),
                    poData,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<VAR_IUNDO_COMMIT_JRNDTO> GetIOptionAsync()
        {
            var loEx = new R_Exception();
            VAR_IUNDO_COMMIT_JRNDTO loResult = new VAR_IUNDO_COMMIT_JRNDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_IUNDO_COMMIT_JRNDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetIOption),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<VAR_GSM_TRANSACTION_CODEDTO> GetLincementLapprovalAsync()
        {
            var loEx = new R_Exception();
            VAR_GSM_TRANSACTION_CODEDTO loResult = new VAR_GSM_TRANSACTION_CODEDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GSM_TRANSACTION_CODEDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetLincementLapproval),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<VAR_GSM_PERIODDTO> GetPeriodAsync()
        {
            var loEx = new R_Exception();
            VAR_GSM_PERIODDTO loResult = new VAR_GSM_PERIODDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<VAR_GSM_PERIODDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetPeriod),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        public async Task<GSM_TRANSACTION_APPROVALDTO> GetTransactionApprovalAsync()
        {
            var loEx = new R_Exception();
            GSM_TRANSACTION_APPROVALDTO loResult = new GSM_TRANSACTION_APPROVALDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM_TRANSACTION_APPROVALDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGLT00600.GetTransactionApproval),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }
        #endregion
        #region not Implemented
        public IAsyncEnumerable<GLT00600JournalGridDTO> GetJournalList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GLT00600JournalGridDetailDTO> GetAllJournalDetailList()
        {
            throw new NotImplementedException();
        }

        public GLT00600JournalGridDTO ProcessReversingJournal(GLT00600JournalGridDTO poData)
        {
            throw new NotImplementedException();
        }

        public GLT00600JournalGridDTO UndoReversingJournal(GLT00600JournalGridDTO poData)
        {
            throw new NotImplementedException();
        }

        public GLT00600JournalGridDTO JournalProcesStatus(GLT00600JournalGridDTO poData)
        {
            throw new NotImplementedException();
        }

        public VAR_GSM_COMPANYDTO GetCompanyDTO()
        {
            throw new NotImplementedException();
        }

        public VAR_GL_SYSTEM_PARAMDTO GetSystemParam()
        {
            throw new NotImplementedException();
        }

        public VAR_USER_DEPARTMENT_LISTDTO GetDeptList()
        {
            throw new NotImplementedException();
        }

        public VAR_CCURRENT_PERIOD_START_DATEDTO GetCurrentPeriodStartDate(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            throw new NotImplementedException();
        }

        public VAR_CSOFT_PERIOD_START_DATEDTO GetSoftPeriodStartDate(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            throw new NotImplementedException();
        }

        public VAR_IUNDO_COMMIT_JRNDTO GetIOption()
        {
            throw new NotImplementedException();
        }

        public VAR_GSM_TRANSACTION_CODEDTO GetLincementLapproval()
        {
            throw new NotImplementedException();
        }

        public VAR_GSM_PERIODDTO GetPeriod()
        {
            throw new NotImplementedException();
        }

        public StatusListDTO GetStatusList()
        {
            throw new NotImplementedException();
        }

        public CurrencyCodeListDTO GetCurrencyCodeList()
        {
            throw new NotImplementedException();
        }

        public GetCenterListDTO GetCenterList()
        {
            throw new NotImplementedException();
        }

        public GSM_TRANSACTION_APPROVALDTO GetTransactionApproval()
        {
            throw new NotImplementedException();
        }

        public GLT00600JournalGridDTO RefreshCurrencyRate(GLT00600JournalGridDTO poData)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}