using GSM01500BACK;
using GSM01500COMMON;
using GSM01500COMMON.DTOs;
using Microsoft.AspNetCore.Mvc;
using R_BackEnd;
using R_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM01500SERVICE
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class GSM01510Controller : ControllerBase, IGSM01510
    {

        [HttpPost]
        public IAsyncEnumerable<GSM01510DepartmentDTO> GetCenterDepartmentList()
        {
            R_Exception loException = new R_Exception();
            IAsyncEnumerable<GSM01510DepartmentDTO> loRtn = null;
            GetCenterDeptListParameter loParam = new GetCenterDeptListParameter();
            GSM01510Cls loCls = new GSM01510Cls();
            List<GSM01510DepartmentDTO> loTempRtn = null;

            try
            {
                loParam.CCOMPANY_ID = R_BackGlobalVar.COMPANY_ID;
                loParam.CCENTER_CODE = R_Utility.R_GetStreamingContext<string>(ContextConstant.CENTER_CODE_STREAM_CONTEXT);
                loParam.CUSER_LOGIN_ID = R_BackGlobalVar.USER_ID;

                loTempRtn = loCls.GetCenterDepartmentList(loParam);

                loRtn = GetCenterDepartmentStream(loTempRtn);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        private async IAsyncEnumerable<GSM01510DepartmentDTO> GetCenterDepartmentStream(List<GSM01510DepartmentDTO> poParameter)
        {
            foreach (GSM01510DepartmentDTO item in poParameter)
            {
                yield return item;
            }
        }
        
    }
}
