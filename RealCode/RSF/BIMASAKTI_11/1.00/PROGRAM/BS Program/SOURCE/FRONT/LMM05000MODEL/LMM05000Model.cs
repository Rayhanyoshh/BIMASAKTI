using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LMM05000Common;
using LMM05000Common.DTOs;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;

namespace LMM05000Model
{
    public class LMM05000Model : R_BusinessObjectServiceClientBase<LMM05000DTO>, ILMM05000
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlLM";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/LMM05000";
        private const string DEFAULT_MODULE = "LM";

        public LMM05000Model() :
            base(DEFAULT_HTTP_NAME, DEFAULT_SERVICEPOINT_NAME, DEFAULT_MODULE, true, true)
        {
        }

        #region Property
        public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
        {
            throw new NotImplementedException();
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
                    nameof(ILMM05000.GetPropertyList),
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
        #endregion


        #region Unit Type Price List
        public LMM05000ListDTO GetUnitTypePriceList()
        {
            throw new NotImplementedException();
        }
        
        public async Task<LMM05000ListDTO> GetUnitPriceListAsync()
        {
            var loEx = new R_Exception();
            LMM05000ListDTO loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<LMM05000ListDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM05000.GetUnitTypePriceList),
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

        #region MyRegion
        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_Method()
        {
            throw new NotImplementedException();
        }
        
        public async Task RSP_GS_ACTIVE_INACTIVE_MethodAsync()
        {
            var loEx = new R_Exception();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                loRtn = await R_HTTPClientWrapper.R_APIRequestObject<ActiveInactiveDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM05000.RSP_GS_ACTIVE_INACTIVE_Method),
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
        #endregion
       
    }
}

