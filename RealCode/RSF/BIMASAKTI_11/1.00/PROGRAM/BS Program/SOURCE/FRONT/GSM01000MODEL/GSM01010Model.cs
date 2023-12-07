using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;

namespace GSM01000Model
{
    public class GSM01010Model: R_BusinessObjectServiceClientBase<GSM01010DTO>, IGSM01010
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01010";
        private const string DEFAULT_MODULE = "GS";

        public GSM01010Model(
           string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)        {
        }

        public GSM01010ListDTO GetGoA()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GSM01010DTO> GetGOAListByGLAccountStream()
        {
            throw new NotImplementedException();
        }


        #region GetAllGoAAsync

        public async Task<GSM01010ListDTO> GetAllGoAAsync()
        {
            var loEx = new R_Exception();
            GSM01010ListDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01010ListDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01010.GetGoA),
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
    }
}