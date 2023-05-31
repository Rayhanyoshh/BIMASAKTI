using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM01000Common;
using GSM01000Common.DTOs;
using System.Data;
using System.Net.Http.Headers;
using System.Data.Common;
using System.Reflection.Metadata;
using GSM001000Back;
using GSM01000Back;

namespace GSM01200Back
{
    public class GSM01200Cls: R_BusinessObject<GSM01200DTO>
    {
        protected override GSM01200DTO R_Display(GSM01200DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM01200DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                
                lcQuery = "RSP_GS_GET_COA_CENTER_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CCENTER_CODE", DbType.String, 50, poEntity.CCENTER_CODE);
                
                
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01200DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        protected override void R_Saving(GSM01200DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loComm;
            DbConnection loConn = null;
            string lcAction = "";

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loComm = loDb.GetCommand();
                
                lcQuery = "INSERT INTO GSM_COA_CENTER " +
                          "VALUES (@CCOMPANY_ID, @CGLACCOUNT_NO, @CCENTER_C0DE , @CCREATED_BY, GETDATE(), @CUPDATE_BY , GETDATE()) ";
                
                loComm.CommandType = CommandType.Text;
                loComm.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loComm, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loComm, "@CGLACCOUNT_NO", DbType.String, 50, poNewEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loComm, "@CCENTER_C0DE", DbType.String, 50, poNewEntity.CCENTER_CODE);
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

        protected override void R_Deleting(GSM01200DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public List<GSM01200DTO> GetCoACenterListDb(GOAHeadListDbParameter poEntity)
        {
            R_Exception loException = new R_Exception();
            List<GSM01200DTO> loRtn = null;
            string lcCmd;
            R_Db loDb;
            DbCommand loDbCommand;

            try
            {
                loDb = new();
                loDbCommand = loDb.GetCommand();
                lcCmd = $"RSP_GS_GET_COA_CENTER_LIST @CCOMPANY_ID, @CGLACCOUNT_NO";
                
                loDbCommand.CommandText = lcCmd;

                loDb.R_AddCommandParameter(loDbCommand, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loDbCommand, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);
                
                var loDataTable = loDb.SqlExecQuery(loDb.GetConnection("R_DefaultConnectionString"), loDbCommand, true);

                loRtn = R_Utility.R_ConvertTo<GSM01200DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public List<AssignCenterDTO> GetCenterToAssignList(CenterAssignParameter poNewEntity)
        {
            List<AssignCenterDTO> loRtn = null;
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

                lcQuery = $"SELECT CCENTER_CODE, CCENTER_NAME FROM GSM_CENTER A (NOLOCK) " +
                          "WHERE A.CCOMPANY_ID = @CCOMPANY_ID " +
                          "AND NOT EXISTS(SELECT TOP 1 1 FROM " +
                          "GSM_COA_CENTER B (NOLOCK) " +
                          "WHERE A.CCOMPANY_ID = B.CCOMPANY_ID " +
                          "AND A.CCENTER_CODE = B.CCENTER_CODE)"; 
                
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;
                
                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<AssignCenterDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        
        public GSM01200DTO GSM01200GetParameterDb(ParameterHeadGLDbParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            GSM01200DTO loRtn = null;
            string lcQuery;
            DbConnection loConn = null;
            DbCommand loCommand;
            R_Db loDb;

            try
            {
                loDb = new();
                loCommand = loDb.GetCommand();
                loConn = loDb.GetConnection();
                lcQuery = "RSP_GS_GET_COA_DETAIL";
                loCommand.CommandType = CommandType.StoredProcedure;
                loCommand.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCommand,"@CCOMPANY_ID",DbType.String,50,poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand,"@CGLACCOUNT_NO", DbType.String,50,poParameter.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCommand,"@CUSER_ID", DbType.String,50,poParameter.CUSER_ID);

                DataTable loDataTable = loDb.SqlExecQuery(loConn, loCommand, true);

                loRtn = R_Utility.R_ConvertTo<GSM01200DTO>(loDataTable).FirstOrDefault();

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }    }
}
