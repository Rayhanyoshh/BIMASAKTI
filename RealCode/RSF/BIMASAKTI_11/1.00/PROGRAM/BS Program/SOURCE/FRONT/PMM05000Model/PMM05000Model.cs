using PMM05000Common;
using PMM05000Common.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;

namespace PMM05000Model
{
    public class PMM05000Model : R_BusinessObjectServiceClientBase<PricingSaveParamDTO>, IPMM05000
    {
             private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlPM";
        private const string DEFAULT_CHECKPOINT_NAME = "api/PMM05000";
        private const string DEFAULT_MODULE = "PM";
        public PMM05000Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_CHECKPOINT_NAME,
            bool plSendWithContext = true,
            bool plSendWithToken = true
            ) : base(
                pcHttpClientName,
                pcRequestServiceEndPoint,
                DEFAULT_MODULE,
                plSendWithContext,
                plSendWithToken)
        {
        }

        public async Task<List<PropertyDTO>> GetPropertyListAsync()
        {
            var loEx = new R_Exception();
            List<PropertyDTO> loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PropertyDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMM05000.GetPropertyList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;

        }

        public async Task<List<PricingDTO>> GetPricingDateListAsync()
        {
            var loEx = new R_Exception();
            List<PricingDTO> loResult = new List<PricingDTO>();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PricingDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMM05000.GetPricingDateList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task<List<UnitTypeCategoryDTO>> GetUnitTypeCategoryListAsync()
        {
            var loEx = new R_Exception();
            List<UnitTypeCategoryDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<UnitTypeCategoryDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMM05000.GetUnitTypeCategoryList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task<List<PricingDTO>> GetPricingListAsync()
        {
            var loEx = new R_Exception();
            List<PricingDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PricingDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMM05000.GetPricingList),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task<List<TypeDTO>> GetPriceChargesTypeAsync()
        {
            var loEx = new R_Exception();
            List<TypeDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<TypeDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMM05000.GetPriceChargesType),
                    DEFAULT_MODULE, _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loResult;
        }

        public async Task SavePricingAsync(PricingSaveParamDTO poParam)
        {
            var loEx = new R_Exception();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                await R_HTTPClientWrapper.R_APIRequestObject<PricingDumpResultDTO, PricingSaveParamDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMM05000.SavePricing),
                    poParam,
                    DEFAULT_MODULE
                    , _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task ActiveInactivePricingAsync(PricingParamDTO poParam)
        {
            var loEx = new R_Exception();
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                await R_HTTPClientWrapper.R_APIRequestObject<PricingDumpResultDTO, PricingParamDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMM05000.ActiveInactivePricing),
                    poParam,
                    DEFAULT_MODULE
                    , _SendWithContext,
                    _SendWithToken);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        //NotImplementedException (Method)
        public IAsyncEnumerable<PropertyDTO> GetPropertyList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<UnitTypeCategoryDTO> GetUnitTypeCategoryList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PricingDTO> GetPricingDateList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PricingDTO> GetPricingList()
        {
            throw new NotImplementedException();
        }

        public PricingDumpResultDTO SavePricing(PricingSaveParamDTO poParam)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<TypeDTO> GetPriceChargesType()
        {
            throw new NotImplementedException();
        }

        public PricingDumpResultDTO ActiveInactivePricing(PricingParamDTO poParam)
        {
            throw new NotImplementedException();
        }
    }
}

