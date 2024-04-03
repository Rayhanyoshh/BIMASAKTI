using CBT01200Common.DTOs;
using CBT01200Common;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CBT01200Common;

namespace CBT01200MODEL
{
    public class CBT01200Model : R_BusinessObjectServiceClientBase<CBT01200DTO>, ICBT01200
    {
        private const string DEFAULT_HTTP = "R_DefaultServiceUrlCB";
        private const string DEFAULT_ENDPOINT = "api/CBT01100";
        private const string DEFAULT_MODULE = "CB";

        public CBT01200Model(string pcHttpClientName = DEFAULT_HTTP,
                             string pcRequestServiceEndPoint = DEFAULT_ENDPOINT,
                             bool plSendWithContext = true,
                             bool plSendWithToken = true) : base(
                                 pcHttpClientName,
                                 pcRequestServiceEndPoint,
        DEFAULT_MODULE,
                                 plSendWithContext,
                                 plSendWithToken)
        { }

        public async Task<List<CBT01200DTO>> GetJournalListAsync(CBT01200ParamDTO poEntity)
        {
            var loEx = new R_Exception();
            List<CBT01200DTO> loResult = null;

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CDEPT_CODE, poEntity.CDEPT_CODE);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CPERIOD, poEntity.CPERIOD);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CSTATUS, string.IsNullOrWhiteSpace(poEntity.CSTATUS) ? "" : poEntity.CSTATUS);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CSEARCH_TEXT, string.IsNullOrWhiteSpace(poEntity.CSEARCH_TEXT) ? "" : poEntity.CSEARCH_TEXT);


                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<CBT01200DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01200.GetJournalList),
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

        public async Task<List<CBT01201DTO>> GetJournalDetailListAsync(CBT01201DTO poEntity)
        {
            var loEx = new R_Exception();
            List<CBT01201DTO> loResult = null;

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CREC_ID, poEntity.CREC_ID);


                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<CBT01201DTO>(
                    _RequestServiceEndPoint,
                    nameof(ICBT01200.GetJournalDetailList),
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

        public async Task UpdateJournalStatusAsync(CBT01200UpdateStatusDTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<CBT01200RecordResult<CBT01200UpdateStatusDTO>, CBT01200UpdateStatusDTO>(
                _RequestServiceEndPoint,
                nameof(ICBT01200.UpdateJournalStatus),
                poEntity,
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

        #region Not Implement

        public CBT01200RecordResult<CBT01200UpdateStatusDTO> UpdateJournalStatus(CBT01200UpdateStatusDTO poEntity)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<CBT01201DTO> GetJournalDetailList()
        {
            throw new NotImplementedException();
        }
        public IAsyncEnumerable<CBT01200DTO> GetJournalList()
        {
            throw new NotImplementedException();
        }

        public CBT01200RecordResult<CBT01200RapidApprovalValidationDTO> ValidationRapidApproval(CBT01200RapidApprovalValidationDTO poEntity)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
