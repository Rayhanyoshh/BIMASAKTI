using GSM001000Back;
using GSM01000Common.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;

namespace GSM01000Back
{
    public class GSM01310Cls: R_BusinessObject<GSM01310DTO>
    {
        protected override GSM01310DTO R_Display(GSM01310DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM01310DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                
                lcQuery = "RSP_GS_GET_GOA_COA_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE", DbType.String, 50, poEntity.CGOA_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);
                
                
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01310DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        protected override void R_Saving(GSM01310DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loComm;
            DbConnection loConn = null;
            
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loComm = loDb.GetCommand();
                
                lcQuery = "INSERT INTO GSM_GOA_COA " +
                          "VALUES (@CCOMPANY_ID, @CGOA_CODE, @CGLACCOUNT_NO, @CCREATED_BY, GETDATE(), @CUPDATE_BY, GETDATE()) ";
                
                loComm.CommandType = CommandType.Text;
                loComm.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loComm, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loComm, "@CGOA_CODE", DbType.String, 50, poNewEntity.CGOA_CODE);
                loDb.R_AddCommandParameter(loComm, "@CGLACCOUNT_NO", DbType.String, 50, poNewEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loComm, "@CCREATED_BY", DbType.String, 50, poNewEntity.CCREATE_BY);
                loDb.R_AddCommandParameter(loComm, "@CUPDATE_BY", DbType.String, 50, poNewEntity.CUPDATE_BY);
                
                loDb.SqlExecNonQuery(loConn, loComm, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            
            EndBlock:
            loEx.ThrowExceptionIfErrors();
        }

        protected override void R_Deleting(GSM01310DTO poEntity)
        {
            throw new NotImplementedException();
        }
        
        public List<GSM01310DTO> GetGoACoAList (GoAMainDbParameter poParameter)
        {
            var loEx = new R_Exception();
            List<GSM01310DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                lcQuery = "RSP_GS_GET_GOA_COA_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 20, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE", System.Data.DbType.String, 100, poParameter.CGOA_CODE);
                
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM01310DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        
        public List<AssignCoADTO> GetCoAToAssignList(GOAHeadListDbParameter poNewEntity)
        {
            List<AssignCoADTO> loRtn = null;
            R_Exception loEx = new R_Exception();
            R_Db loDB;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDB = new R_Db();
                loConn = loDB.GetConnection();
                loCmd = loDB.GetCommand();

                lcQuery = $"SELECT A.CGLACCOUNT_NO, A.CGLACCOUNT_NAME " +
                          $"FROM GSM_COA A WITH (NOLOCK) " +
                          $"WHERE A.CCOMPANY_ID = @CCOMPANY_ID " +
                          $"AND NOT EXISTS ( SELECT TOP 1 1  " +
                          $"FROM GSM_GOA_COA B WITH (NOLOCK) " +
                          $"WHERE A.CCOMPANY_ID = B.CCOMPANY_ID " +
                          $"AND A.CGLACCOUNT_NO = B.CGLACCOUNT_NO)"; 
                
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;
                
                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<AssignCoADTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
    }
}
