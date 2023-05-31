using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_BackEnd;
using GSM001000Back;
using GSM01000Back;

namespace GSM01010Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01010Controller : ControllerBase, IGSM01010
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01010DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01010DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01010DTO> loRtn = null;        
            GSM01010Cls loCls;
            
            try
            {
                loCls = new GSM01010Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01010DTO>();
                
                // poParameter.Entity.CCOMPANY_ID = "RCD";
                // poParameter.Entity.CGLACCOUNT_NO = "12.40.0000";
            
                loRtn.data = loCls.R_GetRecord(poParameter.Entity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        
            EndBlock:
            loEx.ThrowExceptionIfErrors();
        
            return loRtn;
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM01010DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01010DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01010DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public GSM01010ListDTO GetGoA()
        {
            R_Exception loEx = new R_Exception();
            GSM01010ListDTO loRtn = null;
            GOAHeadListDbParameter loDbPar;
            GSM01010Cls loCls;

            try
            {
                loRtn = new GSM01010ListDTO();
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CGLACCOUNT_NO = "12.40.0000";
                
                loCls = new GSM01010Cls();
                loRtn.Data = loCls.GetGoAListByGlAccount(loDbPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        

        [HttpPost]
        public IAsyncEnumerable<GSM01010DTO> GetGOAListByGLAccountStream()
        {
            R_Exception loException = new R_Exception();
            GOAHeadListDbParameter loDbPar;
            List<GSM01010DTO> loRtnTmp;
            GSM01010Cls loCls;
            IAsyncEnumerable<GSM01010DTO> loRtn = null;

            try
            {
                var liCompanyId = R_BackGlobalVar.COMPANY_ID;
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = "RCD";
                loDbPar.CGLACCOUNT_NO = "12.40.0000";

                loCls = new GSM01010Cls();
                loRtnTmp = loCls.GetGoAListByGlAccount(loDbPar);

                loRtn = GetGOAListStream(loRtnTmp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        private async IAsyncEnumerable<GSM01010DTO> GetGOAListStream(List<GSM01010DTO> poParameter)
        {
            foreach (GSM01010DTO item in poParameter)
            {
                yield return item;
            }
        }
        
    }
    
}
