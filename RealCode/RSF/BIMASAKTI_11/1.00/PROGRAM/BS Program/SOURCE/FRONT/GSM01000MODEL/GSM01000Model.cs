using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using GSM01000Common;
using GSM01000Common.DTOs;

namespace GSM01000Model
{
    public class GSM01000Model : R_BusinessObjectServiceClientBase<GSM01000DTO>, IGSM01000
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01000";
        private const string DEFAULT_MODULE = "GS";

        public GSM01000Model(
            string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)        
        {
        }

        #region Active/Inactive
        public async Task RSP_GS_ACTIVE_INACTIVE_COAMethodAsync()
        {
            var loEx = new R_Exception();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            //R_ContextHeader loContextHeader = new R_ContextHeader();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<ActiveInactiveDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01000.RSP_GS_ACTIVE_INACTIVE_COAMethod),
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
                    nameof(IGSM01000.GetCompanyList),
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
                    nameof(IGSM01000.CopyFromProcess),
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
        
        #region GetAllAsync
    
        public async Task<GSM01000ListDTO> GetAllAsync()
        {
            var loEx = new R_Exception();
            GSM01000ListDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01000ListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01000.GetAllCOA), DEFAULT_MODULE,
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

        #region GetAllStreamAsync

        public async Task<List<GSM01000DTO>> GetAllStreamAsync()
        {
            var loEx = new R_Exception();
            List<GSM01000DTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM01000DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01000.GetAllCOAStream),
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
        
        
        public async Task<PrimaryAccountDTO> PrimaryAccModel()
        {
            var loEx = new R_Exception();
            PrimaryAccountDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<PrimaryAccountDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01000.PrimaryAccountCheck), DEFAULT_MODULE,
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
        
        public GSM01000ListDTO GetAllCOA()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GSM01000DTO> GetAllCOAStream()
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

        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_COAMethod()
        {
            throw new NotImplementedException();
        }

        public PrimaryAccountDTO PrimaryAccountCheck()
        {
            throw new NotImplementedException();
        }

        public GSM01000CoAExcelDTO CoAExcelTemplate()
        {
            throw new NotImplementedException();
        }

        public GSM01000UploadHeaderDTO CompanyDetail()
        {
            throw new NotImplementedException();
        }
        
        public async Task<GSM01000UploadHeaderDTO> CompanyDetailModel()
        {
            var loEx = new R_Exception();
            GSM01000UploadHeaderDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01000UploadHeaderDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01000.CompanyDetail), DEFAULT_MODULE,
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

        public async Task<GSM01000CoAExcelDTO> GSM01000DownloadTemplateFileModel()
        {
            var loEx = new R_Exception();
            GSM01000CoAExcelDTO loResult = new GSM01000CoAExcelDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01000CoAExcelDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01000.CoAExcelTemplate),
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
