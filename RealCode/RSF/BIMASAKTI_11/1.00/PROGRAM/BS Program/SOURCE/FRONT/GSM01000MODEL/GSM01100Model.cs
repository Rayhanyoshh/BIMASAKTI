using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;

namespace GSM01000Model
{
    public class GSM01100Model: R_BusinessObjectServiceClientBase<GSM01100DTO>, IGSM01100
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01100";
        private const string DEFAULT_MODULE = "GS";

        public GSM01100Model(
            string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)        {
        }

        public GSM01100UserListDTO GetCoAUserList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GSM01100DTO> GetCoAUserListStream()
        {
            throw new NotImplementedException();
        }



        #region GetAllCoAUserAsync

        public async Task<GSM01100UserListDTO> GetAllCoAUserAsync()
        {
            var loEx = new R_Exception();
            GSM01100UserListDTO loResult = null;

            try
            {
                // R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, pcCGLACCOUNT_NO);
                
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01100UserListDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01100.GetCoAUserList),
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
        
        #region GetUserListToAssign
        public IAsyncEnumerable<AssignUserDTO> GetUserToAssignList()
        {
            throw new NotImplementedException();
        }

        public GSM01100DTO GetParameterInfo()
        {
            throw new NotImplementedException();
        }

        public async Task<List<AssignUserDTO>> GetUserToAssignListAsync()
        {
            var loEx = new R_Exception();
            List<AssignUserDTO> loResult = null;

            try
            {
                // R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, pcCGLACCOUNT_NO);

                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<AssignUserDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01100.GetUserToAssignList),
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
        
        public async Task<GSM01100UserCOAParameterDTO> GetParameterInfoAsync (string pcCGLACCOUNT_NO)
        {
            var loEx = new R_Exception();
            GSM01100UserCOAParameterDTO loResult = new GSM01100UserCOAParameterDTO();
            pcCGLACCOUNT_NO = "11.10.1000";

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, pcCGLACCOUNT_NO);
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01100UserCOAParameterDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01100.GetParameterInfo),
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

        public AssignUserResultDTO AssignUserAction(UsertoAssignParam poParam)
        {
            throw new NotImplementedException();
        }

        public async Task AssignUserActionAsync(UsertoAssignParam poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new AssignUserResultDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<AssignUserResultDTO, UsertoAssignParam>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01100.AssignUserAction),
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

        #endregion

    }
}
