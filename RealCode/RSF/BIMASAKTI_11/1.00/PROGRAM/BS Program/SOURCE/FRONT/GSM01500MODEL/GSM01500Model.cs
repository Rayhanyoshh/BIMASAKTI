using GSM01500COMMON;
using GSM01500COMMON.DTOs;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GSM01500MODEL
{
    public class GSM01500Model : R_BusinessObjectServiceClientBase<GSM01500DTO>, IGSM01500
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01500";

        public GSM01500Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, plSendWithContext, plSendWithToken)
        {
        }


        public CopyFromProcessDTO CopyFromProcess()
        {
            throw new System.NotImplementedException();
        }

        public async Task CopyFromProcessAsync()
        {
            var loEx = new R_Exception();
            CopyFromProcessDTO loRtn = new CopyFromProcessDTO();
            //R_ContextHeader loContextHeader = new R_ContextHeader();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<CopyFromProcessDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01500.CopyFromProcess),
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


        public IAsyncEnumerable<GSM01500DTO> GetCenterList()
        {
            throw new System.NotImplementedException();
        }
        public async Task<GSM01500CenterListDTO> GetCenterListStreamAsync()
        {
            var loEx = new R_Exception();
            List<GSM01500DTO> loResult = null;
            GSM01500CenterListDTO loRtn = new GSM01500CenterListDTO();
            //R_ContextHeader loContextHeader = new R_ContextHeader();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<GSM01500DTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01500.GetCenterList),
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

        public IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList()
        {
            throw new NotImplementedException();
        }

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
                    nameof(IGSM01500.GetCompanyList),
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

        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_CENTERMethod()
        {
            throw new NotImplementedException();
        }
        public async Task RSP_GS_ACTIVE_INACTIVE_CENTERMethodAsync()
        {
            var loEx = new R_Exception();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            //R_ContextHeader loContextHeader = new R_ContextHeader();

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<ActiveInactiveDTO>(
                    _RequestServiceEndPoint,
                    nameof(IGSM01500.RSP_GS_ACTIVE_INACTIVE_CENTERMethod),
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
    }
}
