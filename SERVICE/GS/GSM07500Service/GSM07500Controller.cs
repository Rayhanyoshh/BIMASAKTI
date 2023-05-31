using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM07500Back;
using GSM07500Common;
using GSM07500Common.DTOs;
using GSM07510Common;
using R_BackEnd;

namespace GSM07500Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM07500Controller : ControllerBase, IGSM07500
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM07500DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM07500DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceSaveResultDTO<GSM07500DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM07500DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM07500DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public GSM07500ListDTO PeriodDetailList()
        {
            R_Exception loEx = new R_Exception();
            GSM07500ListDTO loRtn = null;
            List<GSM07500DTO> loResult;
            GSM07500DTO loDbPar;
            GSM07500Cls loCls;

            try
            {
                loDbPar = new GSM07500DTO();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CCYEAR = R_Utility.R_GetStreamingContext<string>(ContextConstant.CCYEAR);
                // loDbPar.CCOMPANY_ID = "rcd";
                // loDbPar.CCYEAR = "2023";
                loCls = new GSM07500Cls();
                loResult = loCls.GetPeriodDetailDbList(loDbPar);
                loRtn = new GSM07500ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        
        [HttpPost]
        public IAsyncEnumerable<GSM07500DTO> PeriodDetailListStream()
        {
            R_Exception loException = new R_Exception();
            GSM07500DTO loDbPar;
            List<GSM07500DTO> loRtnTmp;
            GSM07500Cls loCls;
            IAsyncEnumerable<GSM07500DTO> loRtn = null;

            try
            {
                loDbPar = new GSM07500DTO();
                loDbPar.CCOMPANY_ID = "RCD";
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                
                loCls = new GSM07500Cls();
                loRtnTmp = loCls.GetPeriodDetailDbList(loDbPar);

                loRtn = GetPeriodDetailHelper(loRtnTmp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        private async IAsyncEnumerable<GSM07500DTO> GetPeriodDetailHelper (List<GSM07500DTO> poParameter)
        {
            foreach (GSM07500DTO item in poParameter)
            {
                yield return item;
            }
        }
    }
}