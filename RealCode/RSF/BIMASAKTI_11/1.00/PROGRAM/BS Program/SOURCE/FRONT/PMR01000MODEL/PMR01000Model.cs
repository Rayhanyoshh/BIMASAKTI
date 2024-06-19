using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;
using PMR01000Common;
using PMR01000Common.DTO_s;

namespace PMR01000MODEL
{
    public class PMR01000Model: R_BusinessObjectServiceClientBase<PMR01000DTO>, IPMR01000
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlPM";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/PMR01000";
        private const string DEFAULT_MODULE = "PM";
        
        public PMR01000Model() :
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
                    nameof(IPMR01000.GetPropertyList),
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
        
        public async Task<PMR01000BuildingDataDTO> GetBuildingListAsync()
        {
            var loEx = new R_Exception();
            PMR01000BuildingDataDTO loResult = new PMR01000BuildingDataDTO();
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;

                var loTempResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PMR01000BuildingListDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMR01000.GetBuildinglList),
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
        
        
        public async Task<PMR01000CBSystemParamDTO> GetCBSystemParamAsync()
        {
            var loEx = new R_Exception();
            PMR01000CBSystemParamDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<PMR01000RecordResult<PMR01000CBSystemParamDTO>>(
                    _RequestServiceEndPoint,
                    nameof(IPMR01000.GetCBSystemParam),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loResult = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        
        public async Task<PMR01000PeriodCompanyDTO> GetPeriodYearRangeAsync()
        {
            var loEx = new R_Exception();
            PMR01000PeriodCompanyDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<PMR01000RecordResult<PMR01000PeriodCompanyDTO>>(
                    _RequestServiceEndPoint,
                    nameof(IPMR01000.GetPeriodYearRange),
                    DEFAULT_MODULE,
                    _SendWithContext,
                    _SendWithToken);

                loResult = loTempResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        
        public async Task<List<PMR01000PeriodDTDTO>> GetPeriodDetailAsync()
        {
            var loEx = new R_Exception();
            List<PMR01000PeriodDTDTO> loResult = null;
            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                loResult = await R_HTTPClientWrapper.R_APIRequestStreamingObject<PMR01000PeriodDTDTO>(
                    _RequestServiceEndPoint,
                    nameof(IPMR01000.GetPeriodDetailList),
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
        
        #region Not Implemented
        public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
        {
            throw new NotImplementedException();
        }

        public PMR01000RecordResult<PMR01000CBSystemParamDTO> GetCBSystemParam()
        {
            throw new NotImplementedException();
        }

        public PMR01000RecordResult<PMR01000PeriodCompanyDTO> GetPeriodYearRange()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PMR01000PeriodDTDTO> GetPeriodDetailList()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PMR01000BuildingListDTO> GetBuildinglList()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}