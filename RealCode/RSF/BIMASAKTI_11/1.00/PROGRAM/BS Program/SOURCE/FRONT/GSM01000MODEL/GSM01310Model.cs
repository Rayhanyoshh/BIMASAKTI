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
    public class GSM01310Model : R_BusinessObjectServiceClientBase<GSM01310DTO>, IGSM01310
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01310";
        private const string DEFAULT_MODULE = "GS";

        public GSM01310Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)        {
        }

        public GSM01310ListDTO GetGoACoA()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<GSM01310DTO> GetGoACoAListStream()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<AssignCoADTO> GetCoAToAssignList()
        {
            throw new NotImplementedException();
        }

        #region GetAllGoAAsync

        public async Task<GSM01310ListDTO> GetAllGoACoAAsync()
        {
            var loEx = new R_Exception();
            GSM01310ListDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM01310ListDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01310.GetGoACoA),
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
        
        public async Task<List<GSM01310DTO>> GetCoAToAssignListAsync()
        {
            var loEx = new R_Exception();
            List<GSM01310DTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM01310DTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM01310.GetCoAToAssignList),
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

        IAsyncEnumerable<GSM01310DTO> IGSM01310.GetCoAToAssignList()
        {
            throw new NotImplementedException();
        }
    }
}