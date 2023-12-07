using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSM07500Common.DTOs;
using GSM07510Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;

namespace GSM07500Model
{
    public class GSM07510Model : R_BusinessObjectServiceClientBase<GSM07510DTO>, IGSM07510
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM07510";
        private const string DEFAULT_MODULE = "GS";


        public GSM07510Model(
            string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)
        {
        }

        public LastYearDTO GetLastYear()
        {
            throw new NotImplementedException();
        }
        
        public async Task<LastYearDTO> GetLastYearAsync()
        {
            var loEx = new R_Exception();
            LastYearDTO loRtn = new LastYearDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<LastYearDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM07510.GetLastYear),
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
            return loRtn;
        } 
        
        #region PeriodList

        public async Task<GSM07510ListDTO> GetPeriodListAsync()
        {
            var loEx = new R_Exception();
            GSM07510ListDTO loResult = null;

            try
            {
                loResult = new GSM07510ListDTO();
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM07510ListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM07510.PeriodList),DEFAULT_MODULE,
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

        public async Task<List<GSM07510DTO>>  GetPeriodListStream()
            {
                var loEx = new R_Exception();
                List<GSM07510DTO> loResult = null;

                try
                {
                    R_HTTPClientWrapper.httpClientName = _HttpClientName;
                    loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM07510DTO>(
                        _RequestServiceEndPoint,
                        nameof(IGSM07510.PeriodListStream),  DEFAULT_MODULE,
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

            public GSM07510ListDTO PeriodList()
            {
                throw new NotImplementedException();
            }

            public IAsyncEnumerable<GSM07510DTO> PeriodListStream()
            {
                throw new NotImplementedException();
            }
            
           
    }
}
