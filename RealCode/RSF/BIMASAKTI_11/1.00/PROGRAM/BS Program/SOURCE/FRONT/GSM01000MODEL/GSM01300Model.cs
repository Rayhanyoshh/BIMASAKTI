using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;

namespace GSM01000Model
{
    public class GSM01300Model : R_BusinessObjectServiceClientBase<GSM01300DTO>, IGSM01300
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01300";
        private const string DEFAULT_MODULE = "GS";

        public GSM01300Model(
            string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)        {
        }
        
        #region GetAllGOAAsync

        public async Task<GSM01300ListDTO> GetAllGOAAsync()
        {
            var loEx = new R_Exception();
            GSM01300ListDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01300ListDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01300.GetAllGOA),
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
        
        
        #region GetAllStreamAsync
        public async Task<List<GSM01300DTO>> GetAllGOAStreamAsync()
        {
            var loEx = new R_Exception();
            List<GSM01300DTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM01300DTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01300.GetAllGOAStream),
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
        
        public GSM01300ListDTO GetAllGOA()
        {
            throw new System.NotImplementedException();
        }

        public IAsyncEnumerable<GSM01300DTO> GetAllGOAStream()
        {
            throw new System.NotImplementedException();
        }

        public IAsyncEnumerable<AssignCoADTO> GetCoAToAssignList()
        {
            throw new NotImplementedException();
        }

        public async Task AssignCenterActionAsync(COAtoAssignParam poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new AssignCOAResultDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<AssignCOAResultDTO, COAtoAssignParam>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01300.AssignCOAAction),
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

        public AssignCOAResultDTO AssignCOAAction(COAtoAssignParam poParam)
        {
            throw new NotImplementedException();
        }
    }
}