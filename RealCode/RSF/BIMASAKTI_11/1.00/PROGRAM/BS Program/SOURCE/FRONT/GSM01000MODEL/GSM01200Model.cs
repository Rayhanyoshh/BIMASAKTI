using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_APIClient;
using R_BusinessObjectFront;

namespace GSM01000Model
{
    public class GSM01200Model: R_BusinessObjectServiceClientBase<GSM01200DTO>, IGSM01200
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01200";
        private const string DEFAULT_MODULE = "GS";

        public GSM01200Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)        {
        }

        public async Task<List<AssignCenterDTO>> GetCenterToAssignListAsync()
        {
            var loEx = new R_Exception();
            List<AssignCenterDTO> loResult = null;

            try
            {
                // R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, pcCGLACCOUNT_NO);
                
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<AssignCenterDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01200.GetCenterToAssignList),
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

        #region GetAllCoACenterAsync

        public async Task<GSM01200CenterListDTO> GetAllCoACenterAsync()
        {
            var loEx = new R_Exception();
            GSM01200CenterListDTO loResult = null;

            try
            {
                // R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, pcCGLACCOUNT_NO);
                
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01200CenterListDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01200.GetCoACenterList),
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
        
        public async Task<GSM1200CenterCoAParameterDTO> GetParameterInfoAsync ()
        {
            var loEx = new R_Exception();
            GSM1200CenterCoAParameterDTO loResult = new GSM1200CenterCoAParameterDTO();
            // pcCGLACCOUNT_NO = "11.10.1000";

            try
            {
                // R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, pcCGLACCOUNT_NO);
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM1200CenterCoAParameterDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01200.GetParameterInfo),
                    DEFAULT_MODULE,
                    _SendWithContext, 
                    _SendWithToken
                );
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
            
        }
        #endregion
        
        public GSM01200CenterListDTO GetCoACenterList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GSM01200DTO> GetCoACenterListStream()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<AssignCenterDTO> GetCenterToAssignList()
        {
            throw new NotImplementedException();
        }

        public GSM01200DTO GetParameterInfo()
        {
            throw new NotImplementedException();
        }

        public AssignCenterResultDTO AssignCenterAction(CentertoAssignParam poParam)
        {
            throw new NotImplementedException();
        }

        public async Task AssignCenterActionAsync(CentertoAssignParam poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new AssignCenterResultDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<AssignCenterResultDTO, CentertoAssignParam>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01200.AssignCenterAction),
                    poParameter,
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
    }
}