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

namespace GSM01000Back
{

    public class GSM01100Cls : R_BusinessObject<GSM01100DTO>
    {
        protected override GSM01100DTO R_Display(GSM01100DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM01100DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                
                lcQuery = "RSP_GS_GET_COA_USER_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01100DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        
        public List<GSM01100DTO> GetCoAUserListDb(GOAHeadListDbParameter poEntity) // nunggu revisian
        {
            R_Exception loException = new R_Exception();
            List<GSM01100DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                
                
                lcQuery = "RSP_GS_GET_COA_USER_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", System.Data.DbType.String, 50, poEntity.CGLACCOUNT_NO);
                
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01100DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public List<AssignUserDTO> GetUserToAssignList(GOAHeadListDbParameter poNewEntity)
        {
            List<AssignUserDTO> loRtn = null;
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

                lcQuery = $"SELECT A.CUSER_ID, A.CUSER_NAME " +
                          $"FROM SAM_USER A WITH (NOLOCK) " +
                          $"WHERE NOT EXISTS " +
                          $"( SELECT TOP 1 1 " + 
                            $"FROM GSM_COA_USER B WITH (NOLOCK) " +
                            $"WHERE B.CCOMPANY_ID = @CCOMPANY_ID " +
                            $"AND A.CUSER_ID = B.CUSER_ID)";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;
                
                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<AssignUserDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        
         protected override void R_Saving(GSM01100DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loComm;
            string lcQuery = null;
            string lcAction = "";

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loComm = loDb.GetCommand();
                
                lcQuery = "INSERT INTO GSM_COA_USER " +
                          "VALUES (@CCOMPANY_ID, @CGLACCOUNT_NO, @CUSER_ID, @CCREATE_BY, GETDATE(), @CUPDATE_BY, GETDATE())";
                loComm.CommandType = CommandType.Text;
                loComm.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loComm, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loComm, "@CGLACCOUNT_NO", DbType.String, 50, poNewEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loComm, "@CUSER_ID", DbType.String, 50, poNewEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loComm, "@CCREATE_BY", DbType.String, 50, poNewEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loComm, "@CUPDATE_BY", DbType.String, 50, poNewEntity.CUSER_ID);

                loDb.SqlExecNonQuery(loConn, loComm, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            EndBlock:
            loEx.ThrowExceptionIfErrors();
        }

        protected override void R_Deleting(GSM01100DTO poEntity)
        {
            throw new NotImplementedException();
        }
        
        public GSM01100DTO GSM01100GetParameterDb(ParameterHeadGLDbParameter poParameter)
        {
            R_Exception loException = new R_Exception();
            GSM01100DTO loRtn = null;
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

                loRtn = R_Utility.R_ConvertTo<GSM01100DTO>(loDataTable).FirstOrDefault();

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}
