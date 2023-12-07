using GSM01500COMMON.DTOs;
using R_BackEnd;
using R_Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSM01500BACK
{
    public class GSM01510Cls 
    {
        public List<GSM01510DepartmentDTO> GetCenterDepartmentList(GetCenterDeptListParameter poEntity)
        {
            R_Exception loException = new R_Exception();
            List<GSM01510DepartmentDTO> loResult = null;

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_GET_CENTER_DEPT_LIST '{poEntity.CCOMPANY_ID}', '{poEntity.CCENTER_CODE}', '{poEntity.CUSER_LOGIN_ID}'";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<GSM01510DepartmentDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
    }
}
