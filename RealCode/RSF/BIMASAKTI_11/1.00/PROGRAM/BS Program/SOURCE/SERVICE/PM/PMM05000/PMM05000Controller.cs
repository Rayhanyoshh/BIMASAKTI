using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using R_BackEnd;
using PMM05000Back;
using PMM05000Common;
using PMM05000Common.DTOs;

namespace PMM05000Service
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PMM05000Controller : ControllerBase, IPMM05000
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<PMM05000DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<PMM05000DTO> poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new R_ServiceGetRecordResultDTO<PMM05000DTO>();
            BackParameter loDbPar;
            try
            {
                var loCls = new PMM05000Cls();
                loDbPar = new BackParameter();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                poParameter.Entity.CUNIT_TYPE_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);
                poParameter.Entity.LACTIVE = R_Utility.R_GetStreamingContext<bool>(ContextConstant.LACTIVE_ONLY);
                // poParameter.Entity.CVALID_DATE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CVALID_DATE);
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;

                // poParameter.Entity.CCOMPANY_ID = "RCD";
                // poParameter.Entity.CPROPERTY_ID = "JBMPC";
                // poParameter.Entity.CUNIT_TYPE_ID = "1BRoom";
                // poParameter.Entity.LACTIVE = true;
                // poParameter.Entity.CVALID_DATE = "20230605";

                //poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                // poParameter.Entity.CUSER_ID= "ADMIN";
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
        public R_ServiceSaveResultDTO<PMM05000DTO> R_ServiceSave(R_ServiceSaveParameterDTO<PMM05000DTO> poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new R_ServiceSaveResultDTO<PMM05000DTO>();
            BackParameter loDbPar;

            try
            {
                loDbPar = new BackParameter();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                loDbPar.CUNIT_TYPE_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                //poParameter.Entity.CUSER_ID = "ADMIN";
                var loCls = new PMM05000Cls();

                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<PMM05000DTO> poParameter)
        {
            var loEx = new R_Exception();
            var loRtn = new R_ServiceDeleteResultDTO();

            try
            {
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                //poParameter.Entity.CUSER_ID = "ADMIN";
                poParameter.Entity.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                poParameter.Entity.CUNIT_TYPE_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);

                var loCls = new PMM05000Cls();
                loCls.R_Delete(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;        }

        [HttpPost]
        public IAsyncEnumerable<PropertyListDTO> GetPropertyList()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<PropertyListDTO> loRtn = null;
            PMM05000Cls loCls;
            BackParameter loDbPar;
            List<PropertyListDTO> loRtnTmp;

            try
            {
                loDbPar = new BackParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                // loDbPar.CCOMPANY_ID = "rcd";
                loDbPar.CUSER_ID = "Admin";

                loCls = new PMM05000Cls();

                loRtnTmp = loCls.PropertyListDB(loDbPar);

                loRtn = GetPropertyStream(loRtnTmp);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public PMM05000ListDTO GetUnitTypePriceList()
            {
            var loEx = new R_Exception();
            PMM05000ListDTO loRtn = null;
            BackParameter loDbPar;
            PMM05000Cls loCls;

            try
            {
                loDbPar = new BackParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CPROPERTY_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CPROPERTY_ID);
                loDbPar.CUNIT_TYPE_ID = R_Utility.R_GetStreamingContext<string>(ContextConstant.CUNIT_TYPE_ID);
                loDbPar.LACTIVE = R_Utility.R_GetStreamingContext<bool>(ContextConstant.LACTIVE_ONLY);
                // loDbPar.CCOMPANY_ID = "rcd";
                // loDbPar.CPROPERTY_ID = "JBMPC";
                // loDbPar.CUNIT_TYPE_ID = "1BRoom";
                // loDbPar.LACTIVE = true;
                loDbPar.CUSER_ID = "ADMIN";
                loCls = new PMM05000Cls();
                loRtn = new PMM05000ListDTO();
                loRtn.Data = loCls.GetTypePriceListDb(loDbPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        
        [HttpPost]
        public ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_Method()
        {
            R_Exception loException = new R_Exception();
            ActiveInactiveParameterDTO loParam = new ActiveInactiveParameterDTO();
            ActiveInactiveDTO loRtn = new ActiveInactiveDTO();
            PMM05000Cls loCls = new PMM05000Cls();

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CPROPERTY_ID = R_Utility.R_GetContext<string>(ContextConstant.CPROPERTY_ID);
                loParam.CUNIT_TYPE_ID = R_Utility.R_GetContext<string>(ContextConstant.CUNIT_TYPE_ID);
                loParam.CVALID_INTERNAL_ID = R_Utility.R_GetContext<string>(ContextConstant.CVALID_INTERNAL_ID);
                loParam.LACTIVE = R_Utility.R_GetContext<bool>(ContextConstant.LACTIVE_ONLY);
                loParam.CUSER_ID = R_BackGlobalVar.USER_ID;

                loCls.RSP_GS_ACTIVE_INACTIVE_PRICE_Method(loParam);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        #region Helper
        private async IAsyncEnumerable<PropertyListDTO> GetPropertyStream(List<PropertyListDTO> poParameter)
        {
            foreach (PropertyListDTO item in poParameter)
            {
                yield return item;
            }
        }

        #endregion
    }
}

