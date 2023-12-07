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
    public class LMM05010Model: R_BusinessObjectServiceClientBase<LMM05010DTO>, ILMM05010
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlLM";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/LMM05010";
        private const string DEFAULT_MODULE = "LM";
        
        public LMM05010Model() :
            base(DEFAULT_HTTP_NAME, DEFAULT_SERVICEPOINT_NAME, DEFAULT_MODULE, true, true)
        {
        }
        
        public UnitTypeDataDTO GetUnitTypeList()
        {
            throw new NotImplementedException();
        }
        
        public async Task<UnitTypeDataDTO> GetAllUnitTypeAsync()
        {
            var loEx = new R_Exception();
            UnitTypeDataDTO loResult = new UnitTypeDataDTO();

            try
            {
                R_HTTPClientWrapper.httpClientName = DEFAULT_HTTP_NAME;
                loResult = await R_HTTPClientWrapper.R_APIRequestObject<UnitTypeDataDTO>(
                    _RequestServiceEndPoint,
                    nameof(ILMM05010.GetUnitTypeList), 
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
    }
}
