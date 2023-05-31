using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_BackEnd;

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01300Controller : ControllerBase, IGSM01300
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01300DTO> R_ServiceGetRecord(
            R_ServiceGetRecordParameterDTO<GSM01300DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01300DTO> loRtn = null;
            GSM01300Cls loCls;

            try
            {
                loCls = new GSM01300Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01300DTO>();

                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CCOMPANY_ID = "RCD";
                poParameter.Entity.CGOA_CODE = "CASH";
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
        public R_ServiceSaveResultDTO<GSM01300DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01300DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01300DTO> poParameter)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public GSM01300ListDTO GetAllGOA()
        {
            R_Exception loEx = new R_Exception();
            GSM01300ListDTO loRtn = null;
            List<GSM01300DTO> loResult;
            GOAHeadListDbParameter loDbPar;
            GSM01300Cls loCls;

            try
            {
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CCOMPANY_ID = "RCD";

                loCls = new GSM01300Cls();
                loResult = loCls.GetGoAListDb(loDbPar);
                loRtn = new GSM01300ListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<GSM01300DTO> GetAllGOAStream()
        {
            R_Exception loException = new R_Exception();
            GOAHeadListDbParameter loDbPar;
            List<GSM01300DTO> loRtnTmp;
            GSM01300Cls loCls;
            IAsyncEnumerable<GSM01300DTO> loRtn = null;

            try
            {
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = "RCD";


                loCls = new GSM01300Cls();
                loRtnTmp = loCls.GetGoAListDb(loDbPar);

                loRtn = GetGOAStream(loRtnTmp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        private async IAsyncEnumerable<GSM01300DTO> GetGOAStream(List<GSM01300DTO> poParameter)
        {
            foreach (GSM01300DTO item in poParameter)
            {
                yield return item;
            }
        }
        
       
    }
}