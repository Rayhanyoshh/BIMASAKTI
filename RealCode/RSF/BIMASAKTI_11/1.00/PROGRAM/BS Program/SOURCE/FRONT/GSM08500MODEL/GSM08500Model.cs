using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSM008500Common;
using GSM008500Common.DTOs;
using GSM08500Common;
using GSM08500Common.DTOs;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;

namespace GSM08500Model
{
    public class GSM08500Model : R_BusinessObjectServiceClientBase<GSM08500DTO>, IGSM08500
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM08500";
        private const string DEFAULT_MODULE = "GS";
        
        public GSM08500Model(
            string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) : 
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)
        {
        }
        
        public async Task<GSM08500ListDTO> GetAllStatAccAsync()
        {
            var loEx = new R_Exception();
            GSM08500ListDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM08500ListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM08500.GetStatisticAccDbList), DEFAULT_MODULE,
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
        
        #region GetAllStreamAsync

        public async Task<List<GSM08500DTO>> GetAllStreamAsync()
        {
            var loEx = new R_Exception();
            List<GSM08500DTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM08500DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM08500.GetStatisticAccDbListStream),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken,
                    null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        #endregion

        #region Active/Inactive
        public async Task RSP_GS_ACTIVE_INACTIVE_StatMethodAsync()
        {
            var loEx = new R_Exception();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            //R_ContextHeader loContextHeader = new R_ContextHeader();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<ActiveInactiveDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM08500.RSP_GS_ACTIVE_INACTIVE_StatAccMethod),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
        #endregion
        
        #region CopyFrom
        public async Task<CopyFromProcessCompanyListDTO> GetCompanyListStreamAsync()
        {
            var loEx = new R_Exception();
            List<CopyFromProcessCompanyDTO> loResult = null;
            CopyFromProcessCompanyListDTO loRtn = new CopyFromProcessCompanyListDTO();
            //R_ContextHeader loContextHeader = new R_ContextHeader();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<CopyFromProcessCompanyDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM08500.GetCompanyList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn.Data = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public async Task CopyFromProcessAsync()
        {
            var loEx = new R_Exception();
            CopyFromProcess loRtn = new CopyFromProcess();
            //R_ContextHeader loContextHeader = new R_ContextHeader();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<CopyFromProcess>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM08500.CopyFromProcess),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
        }
        #endregion
        
        public async Task<GSM08500UploadHeaderDTO> CompanyDetailModel()
        {
            var loEx = new R_Exception();
            GSM08500UploadHeaderDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM08500UploadHeaderDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM08500.CompanyDetail), DEFAULT_MODULE,
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
        
        public GSM08500ListDTO GetStatisticAccDbList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GSM08500DTO> GetStatisticAccDbListStream()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList()
        {
            throw new NotImplementedException();
        }

        public CopyFromProcess CopyFromProcess()
        {
            throw new NotImplementedException();
        }

        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_StatAccMethod()
        {
            throw new NotImplementedException();
        }

        public GSM08500CoAExcelDTO CoAExcelTemplate()
        {
            throw new NotImplementedException();
        }

        public GSM08500UploadHeaderDTO CompanyDetail()
        {
            throw new NotImplementedException();
        }

        public async Task<GSM08500CoAExcelDTO> GSM01000DownloadTemplateFileModel()
        {
            var loEx = new R_Exception();
            GSM08500CoAExcelDTO loResult = new GSM08500CoAExcelDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM08500CoAExcelDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM08500.CoAExcelTemplate),
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
    }
}