using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PMR02200Common;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;
using TXR00100Common.DTOs;
using TXR00100Common.PrintDTO;

namespace TXR00100MODEL
{
    public class TXR00100Model : R_BusinessObjectServiceClientBase<TXR00100DataResultDTO>, ITXR00100
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlTX";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/TXR00100";
        private const string DEFAULT_MODULE = "TX";

        public TXR00100Model() :
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
                    nameof(ITXR00100.GetPropertyList),
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
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<TXR00100PeriodDetailDTO>(
                    _RequestServiceEndPoint,
                    nameof(ITXR00100.GetPeriodDetailList),
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

        public IAsyncEnumerable<TXR00100PeriodDetailDTO> GetPeriodDetailList(string poYear)
        {
            throw new NotImplementedException();
        }
        
        #endregion
    }
}
