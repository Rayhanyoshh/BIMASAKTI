using CBT01200Common;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CBT01200Common.DTOs;

namespace CBT01200MODEL
{
    public class CBT01210Model : R_BusinessObjectServiceClientBase<CBT01210DTO>, ICBT01210
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrlCB";
        private const string DEFAULT_ENDPOINT = "api/CBT01210";
        private const string DEFAULT_MODULE = "CB";

        public CBT01210Model
            (
                string pcHttpClientName = DEFAULT_HTTP,
                string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
                bool plSendWithContext = true,
                bool plSendWithToken = true
            )
        : base
            (
                pcHttpClientName,
                pcRequestServiceEndPoint,
                DEFAULT_MODULE,
                plSendWithContext,
                plSendWithToken
            )
        { }

        public async Task<CBT01210DTO> GetJournalRecordAsync(CBT01210DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01210DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01200RecordResult<CBT01210DTO>, CBT01210DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01210.GetJournalRecord),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01211DTO> GetJournalDetailRecordAsync(CBT01211DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01211DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01200RecordResult<CBT01211DTO>, CBT01211DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01210.GetJournalDetailRecord),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01210LastCurrencyRateDTO> GetLastCurrencyAsync(CBT01210LastCurrencyRateDTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01210LastCurrencyRateDTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01200RecordResult<CBT01210LastCurrencyRateDTO>, CBT01210LastCurrencyRateDTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01210.GetLastCurrency),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01210DTO> SaveJournalAsync(CBT01210DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01210DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01200RecordResult<CBT01210DTO>, CBT01210DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01210.SaveJournal),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01210DTO> SaveJournalDetailAsync(CBT01211DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01210DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01200RecordResult<CBT01210DTO>, CBT01211DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01210.SaveJournalDetail),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        public async Task<CBT01210DTO> DeleteJournalDetailAsync(CBT01211DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01210DTO loRtn = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01200RecordResult<CBT01210DTO>, CBT01211DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01210.DeleteJournalDetail),
                    poEntity,
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loRtn = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        } 

        #region Not Implement

        public CBT01200RecordResult<CBT01210LastCurrencyRateDTO> GetLastCurrency(CBT01210LastCurrencyRateDTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01200RecordResult<CBT01210DTO> GetJournalRecord(CBT01210DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01200RecordResult<CBT01210DTO> SaveJournal(CBT01210DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01200RecordResult<CBT01210DTO> SaveJournalDetail(CBT01211DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01200RecordResult<CBT01210DTO> DeleteJournalDetail(CBT01211DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public CBT01200RecordResult<CBT01211DTO> GetJournalDetailRecord(CBT01211DTO poEntity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
