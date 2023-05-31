using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using R_Common;
using R_CommonFrontBackAPI;
using GSM001000Back;
using GSM01000Back;
using GSM01000Common;
using GSM01000Common.DTOs;
using GSM01200Back;
using R_BackEnd;

namespace GSM01000Service
{
    [ApiController]
    [Route("api/[controller]/[action]"), AllowAnonymous]
    public class GSM01200Controller: ControllerBase, IGSM01200
    {
        [HttpPost]
        public R_ServiceGetRecordResultDTO<GSM01200DTO> R_ServiceGetRecord(R_ServiceGetRecordParameterDTO<GSM01200DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceGetRecordResultDTO<GSM01200DTO> loRtn = null;
            GSM01200Cls loCls;
            
            try
            {
                loCls = new GSM01200Cls();
                loRtn = new R_ServiceGetRecordResultDTO<GSM01200DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                // poParameter.Entity.CCOMPANY_ID = "RCD";
                // poParameter.Entity.CGLACCOUNT_NO = "11.10.1000";
                // poParameter.Entity.CUSER_ID = "RYC";
            
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
        public R_ServiceSaveResultDTO<GSM01200DTO> R_ServiceSave(R_ServiceSaveParameterDTO<GSM01200DTO> poParameter)
        {
            R_Exception loEx = new R_Exception();
            R_ServiceSaveResultDTO<GSM01200DTO> loRtn = null;
            GSM01200Cls loCls;
            
            try
            {
                loCls = new GSM01200Cls();
                loRtn = new R_ServiceSaveResultDTO<GSM01200DTO>();
                poParameter.Entity.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                poParameter.Entity.CCREATE_BY = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CUPDATE_BY = R_BackGlobalVar.USER_ID;
                poParameter.Entity.CUSER_ID = R_BackGlobalVar.USER_ID;
                
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
        public R_ServiceDeleteResultDTO R_ServiceDelete(R_ServiceDeleteParameterDTO<GSM01200DTO> poParameter)
        {
            throw new NotImplementedException();
        }
        
        [HttpPost]
        public IAsyncEnumerable<GSM01200DTO> GetCoACenterListStream()
        {
            R_Exception loException = new R_Exception();
            GOAHeadListDbParameter loDbPar;
            List<GSM01200DTO> loRtnTmp;
            GSM01200Cls loCls;
            IAsyncEnumerable<GSM01200DTO> loRtn = null;
            
            try
            {
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                loDbPar.CCOMPANY_ID = "RCD";
                loDbPar.CGLACCOUNT_NO = "11.10.1000";

                loCls = new GSM01200Cls();
                loRtnTmp = loCls.GetCoACenterListDb(loDbPar);
                loRtn = GetCOACenterStream(loRtnTmp);
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
        public GSM01200CenterListDTO GetCoACenterList()
        {
            R_Exception loEx = new R_Exception();
            GSM01200CenterListDTO loRtn = null;
            GOAHeadListDbParameter loDbPar;
            List<GSM01200DTO> loResult;
            GSM01200Cls loCls;

            try
            {
                loDbPar = new GOAHeadListDbParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                loDbPar.CCOMPANY_ID = "RCD";
                loDbPar.CGLACCOUNT_NO = "11.10.1000";   

                loCls = new GSM01200Cls();
                loResult = loCls.GetCoACenterListDb(loDbPar);
                loRtn = new GSM01200CenterListDTO { Data = loResult };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        private async IAsyncEnumerable<GSM01200DTO> GetCOACenterStream(List<GSM01200DTO> poParameter)
        {
            foreach (GSM01200DTO item in poParameter)
            {
                yield return item;
            }
        }
        
        [HttpPost]
        public IAsyncEnumerable<AssignCenterDTO> GetCenterToAssignList()
        {
            CenterAssignParameter loDbPar;
            List<AssignCenterDTO> loRtnTemp = null;
            R_Exception loEx = new R_Exception();
            GSM01200Cls loCls;
            try
            {
                loDbPar = new CenterAssignParameter();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loDbPar.CCOMPANY_ID = "RCD";
                loCls = new GSM01200Cls();
                loRtnTemp = loCls.GetCenterToAssignList(loDbPar);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return GetCenterListHelper(loRtnTemp);
        }
        
        [HttpPost]
        public GSM01200DTO GetParameterInfo()
        {
            R_Exception loException = new R_Exception();
            ParameterHeadGLDbParameter loDbPar;
            GSM01200Cls loCls;
            GSM01200DTO loReturn = null;

            try
            {
                loDbPar = new();
                loDbPar.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;

                if (loDbPar.CCOMPANY_ID != null)
                {
                    loDbPar.CGLACCOUNT_NO = R_Utility.R_GetStreamingContext<string>(ContextConstant.CGLACCOUNT_NO);
                    loDbPar.CUSER_ID = R_BackGlobalVar.USER_ID;
                }
                else
                {
                    loDbPar.CCOMPANY_ID = "rcd";
                    loDbPar.CGLACCOUNT_NO = "11.10.1000";
                    loDbPar.CUSER_ID= "Admin";
                }

                //Use Context!

                loCls = new ();

                loReturn = loCls.GSM01200GetParameterDb(loDbPar);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loReturn;
        }

        private async IAsyncEnumerable<AssignCenterDTO> GetCenterListHelper(List<AssignCenterDTO> loRtnTemp)
        {
            foreach (AssignCenterDTO loEntity in loRtnTemp)
            {
                yield return loEntity;
            }
        }
    }
}
