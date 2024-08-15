using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PMR02200Common;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;
using TXR00200Common.DTOs;
using TXR00200Common.PrintDTO;

namespace TXR00200MODEL
{
    public class TXR00200Model : R_BusinessObjectServiceClientBase<TXR00200DataResultDTO>, ITXR00200
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlTX";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/TXR00200";
        private const string DEFAULT_MODULE = "TX";

        public TXR00200Model() :
            base(DEFAULT_HTTP_NAME, DEFAULT_SERVICEPOINT_NAME, DEFAULT_MODULE, true, true)
        {
        }

        public async Task<PropertyListDataDTO> GetProperyListAsync()
        {
            var loEx = new R_Exception();
            PropertyListDataDTO loResult = new PropertyListDataDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                var loTempResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PropertyListDTO>(
                    _RequestServiceEndPoint,
                    nameof(ITXR00200.GetPropertyList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                loResult.Data = loTempResult;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        
        public async Task<TXR00200DataDTO> GetEFakturListAsync()
        {
            var loEx = new R_Exception();
            TXR00200DataDTO loResult = new TXR00200DataDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                var loTempResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<TXR00200DTO>(
                    _RequestServiceEndPoint,
                    nameof(ITXR00200.GerEFakturList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                loResult.Data = loTempResult;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        public async Task<PeriodDetailListDataDTO> GetPerioDetailModel(string poYear)
        {
            var loEx = new R_Exception();
            PeriodDetailListDataDTO loResult = new PeriodDetailListDataDTO();   
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CYEAR, poYear);

                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<TXR00200PeriodDetailDTO>(
                    _RequestServiceEndPoint,
                    nameof(ITXR00200.GetPeriodDetailList),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);
                loResult.Data = loTempResult;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }


        #region Not Implemented

        public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<TXR00200DTO> GerEFakturList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<TXR00200PeriodDetailDTO> GetPeriodDetailList(string poYear)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
