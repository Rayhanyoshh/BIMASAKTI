using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using LMM05000Back;
using LMM05000Common;
using LMM05000Common.DTOs;
using R_BackEnd;

namespace LMM05000Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LMM05010Controller : ControllerBase, ILMM05010
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<LMM05010DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<LMM05010DTO> poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new R_ServiceGetRecordResultDTO<LMM05010DTO>();
            BackParameter loDbPar;
            try
            {
                var loCls = new LMM05010Cls();
                loDbPar = new BackParameter();
                // poParameter.Entity.CCOMPANY_ID = "rcd";
                // poParameter.Entity.CPROPERTY_ID = "JBMPC";
                // poParameter.Entity.CUNIT_TYPE_ID = "1BRoom";
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                poParameter.Entity.CUNIT_TYPE_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<LMM05010DTO> R_ServiceSave(R_ServiceSaveParameterDTO<LMM05010DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<LMM05010DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public UnitTypeDataDTO GetUnitTypeList()
        {
            R_Exception loEx = new R_Exception();
            UnitTypeDataDTO loRtn = null;
            List<LMM05010DTO> loResult;
            BackParameter loDbPar;
            LMM05010Cls loCls;

            try
            {
                loDbPar = new BackParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                loDbPar.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CPROPERTY_ID = "JBMPC";
                loDbPar.CUNIT_TYPE_CATEGORY_ID = "";
                // loDbPar.CUSER_ID = "ADMIN";
                loCls = new LMM05010Cls();
                loResult = loCls.UnitTypeListDB(loDbPar);
                loRtn = new UnitTypeDataDTO() { Data = loResult };

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        [HttpPost]
        public IAsyncEnumerable<LMM05010DTO> GetUnitTypeListAsyc()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<LMM05010DTO> loRtn = null;
            LMM05010Cls loCls;
            BackParameter loDbPar;
            List<LMM05010DTO> loRtnTmp;

            try
            {
                loDbPar = new BackParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                loDbPar.CUNIT_TYPE_CATEGORY_ID = "";
                // loDbPar.CCOMPANY_ID = "rcd";
                loDbPar.CUSER_ID = "Admin";

                loCls = new LMM05010Cls();

                loRtnTmp = loCls.UnitTypeListDB(loDbPar);

                loRtn = GetUnitTypeStream(loRtnTmp);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        #region Helper
        private async IAsyncEnumerable<LMM05010DTO> GetUnitTypeStream(List<LMM05010DTO> poParameter)
        {
            foreach (LMM05010DTO item in poParameter)
            {
                yield return item;
            }
        }

        #endregion
    }
}

