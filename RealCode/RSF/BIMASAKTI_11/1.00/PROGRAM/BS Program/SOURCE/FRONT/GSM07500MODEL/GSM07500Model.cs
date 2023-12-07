using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GSM07500Common;
using GSM07500Common.DTOs;
using GSM07510Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;

namespace GSM07500Model
{
    public class GSM07500Model : R_BusinessObjectServiceClientBase<SaveBatchGSM07500DTO>, IGSM07500
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM07500";
        private const string DEFAULT_MODULE = "GS";


        public GSM07500Model(
            string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)        {
        }
        
        public async Task<GSM07500ListDTO> GetPeriodDetailListAsync()
        {
            var loEx = new R_Exception();
            GSM07500ListDTO loResult = null;

            try
            {
                loResult = new GSM07500ListDTO();
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM07500ListDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM07500.PeriodDetailList)
                    ,DEFAULT_MODULE,
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
        
        public async Task<List<GSM07500DTO>>  GetPeriodDetailListStream()
        {
            var loEx = new R_Exception();
            List<GSM07500DTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM07500DTO>(
                    _RequestServiceEndPoint, nameof(IGSM07500.PeriodDetailListStream),
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

        public async Task<GSM07500ListDTO> GetGenerateGSMPeriodStream()
        {
            var loEx = new R_Exception();
            GSM07500ListDTO loResult = null;

            try
            {
                loResult = new GSM07500ListDTO();
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<GSM07500ListDTO>(
                    _RequestServiceEndPoint, 
                    nameof(IGSM07500.RftPeriodGenerator), 
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

        public IAsyncEnumerable<GSM07500DTO> PeriodDetailListStream()
        {
            throw new NotImplementedException();
        }

        public GSM07500ListDTO RftPeriodGenerator()
        {
            throw new NotImplementedException();
        }

        public GSM07500ListDTO PeriodDetailList()
        {
            throw new NotImplementedException();
        }
    }
}