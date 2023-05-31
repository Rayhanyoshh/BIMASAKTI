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

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01310Controller : ControllerBase, IGSM01310
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01310DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01310DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01310DTO> loRtn = null;        
            GSM01310Cls loCls;
            
            try
            {
                loCls = new GSM01310Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01310DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                // poParameter.Entity.CCOMPANY_ID = "RCD";
                // poParameter.Entity.CGOA_CODE = "ARDEP";
            
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
        public R_ServiceSaveResultDTO<GSM01310DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01310DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM01310DTO> loRtn = null;
            GSM01310Cls loCls;
            
            try
            {
                loCls = new GSM01310Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM01310DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CCREATE_BY = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CUPDATE_BY = R_BackGlobalVar.USER_ID;
                
                loRtn.data = loCls.R_Save(poParameter.Entity, poParameter.CRUDMode);
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
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01310DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public GSM01310ListDTO GetGoACoA()
        {
            R_Exception loEx = new R_Exception();
            GSM01310ListDTO loRtn = null;
            GoAMainDbParameter loDbPar;
            GSM01310Cls loCls;

            try
            {
                loRtn = new GSM01310ListDTO();
                loDbPar = new GoAMainDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGOA_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGOA_CODE);
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CGOA_CODE = "ARDEP";
                
                loCls = new GSM01310Cls();
                loRtn.Data = loCls.GetGoACoAList(loDbPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        [HttpPost]
        public IAsyncEnumerable<GSM01310DTO> GetGoACoAListStream()
        {
            R_Exception loException = new R_Exception();
            GoAMainDbParameter loDbPar;
            List<GSM01310DTO> loRtnTmp;
            GSM01310Cls loCls;
            IAsyncEnumerable<GSM01310DTO> loRtn = null;

            try
            {
                var liCompanyId = R_BackGlobalVar.COMPANY_ID;
                loDbPar = new GoAMainDbParameter();
                // loDbPar.CCOMPANY_ID = "RCD";
                // loDbPar.CGOA_CODE = "ARDEP";

                loCls = new GSM01310Cls();
                loRtnTmp = loCls.GetGoACoAList(loDbPar);

                loRtn = GetGOACOAListStream(loRtnTmp);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        [HttpPost]
        public IAsyncEnumerable<AssignCoADTO> GetCoAToAssignList()
        {
            GOAHeadListDbParameter loDbPar;
            List<AssignCoADTO> loRtnTemp = null;
            R_Exception loEx = new R_Exception();
            GSM01310Cls loCls;
            try
            {
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loCls = new GSM01310Cls();
                loRtnTemp = loCls.GetCoAToAssignList(loDbPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return GetCoAListHelper(loRtnTemp);

        }
        
        private async IAsyncEnumerable<AssignCoADTO> GetCoAListHelper(List<AssignCoADTO> loRtnTemp)
        {
            foreach (AssignCoADTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }

        private async IAsyncEnumerable<GSM01310DTO> GetGOACOAListStream(List<GSM01310DTO> poParameter)
        {
            foreach (GSM01310DTO item in poParameter)
            {
                yield return item;
            }
        }
    }
}
