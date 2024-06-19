using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R_APIClient;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BusinessObjectFront;
using R_CommonFrontBackAPI;
using PMR02200Common.DTOs;
using PMR02200Common;

namespace PMR02200MODEL
{
    public class PMR02200Model : R_BusinessObjectServiceClientBase<PMR02200DTO>, IPMR02200
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlPM";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/PMR02200";
        private const string DEFAULT_MODULE = "PM";

        public PMR02200Model() :
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
                    nameof(IPMR02200.GetPropertyList),
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

        public async Task<PMR02200PeriodCompanyDTO> GetPeriodYearRangeAsync()
        {
            var loEx = new R_Exception();
            PMR02200PeriodCompanyDTO loResult = null;

            try
            {
                R_HTTPClientWrapper.httpClientName = _HttpClientName;
                var loTempResult = await R_HTTPClientWrapper.R_APIRequestObject<PMR02200RecordResult<PMR02200PeriodCompanyDTO>>(
                    _RequestServiceEndPoint,
                    nameof(IPMR02200.GetPeriodYearRange),
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


        #region Not Implemented
        public PMR02200RecordResult<PMR02200PeriodCompanyDTO> GetPeriodYearRange()
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
