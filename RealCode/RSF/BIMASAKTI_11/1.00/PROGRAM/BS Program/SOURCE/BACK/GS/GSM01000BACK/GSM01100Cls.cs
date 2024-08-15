using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM01000Common;
using GSM01000Common.DTOs;
using System.Data;
using System.Net.Http.Headers;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection.Metadata;
using GSM001000Back;

namespace GSM01000Back
{

    public class GSM01100Cls : R_BusinessObject<GSM01100DTO>
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;


        public GSM01100Cls()
        {
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_GetInstanceActivitySource();

        }
        protected override GSM01100DTO R_Display(GSM01100DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("R_Display");

            R_Exception loEx = new R_Exception();
            GSM01100DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_COA_USER_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);

                // Log the query using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);
        
                // Log each parameter
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CUSER_ID = {CUSER_ID}", poEntity.CUSER_ID);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poEntity.CGLACCOUNT_NO);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01100DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        
        public List<GSM01100DTO> GetCoAUserListDb(GOAHeadListDbParameter poEntity) // nunggu revisian
        {
            using Activity activity = _activitySource.StartActivity("GetCoAUserListDb");

            R_Exception loException = new R_Exception();
            List<GSM01100DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;

            try
            {
                loDb =  new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
        
        
                lcQuery = "RSP_GS_GET_COA_USER_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
        
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", System.Data.DbType.String, 50, poEntity.CGLACCOUNT_NO);
        
                // Log the query using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);
        
                // Log each parameter
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poEntity.CGLACCOUNT_NO);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01100DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public List<AssignUserDTO> GetUserToAssignList(GOAHeadListDbParameter poNewEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetUserToAssignList");

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

                lcQuery = "RSP_GS_GET_LOOKUP_USER_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROGRAM_ID", DbType.String, 50, poNewEntity.CPROGRAM_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPARAMETER_ID", DbType.String, 50, poNewEntity.CGLACCOUNT_NO);

                // Log the query using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poNewEntity.CCOMPANY_ID);
                _logger.LogDebug("CPROGRAM_ID = {CPROGRAM_ID}", poNewEntity.CPROGRAM_ID);
                _logger.LogDebug("CPARAMETER_ID = {CPARAMETER_ID}", poNewEntity.CGLACCOUNT_NO);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<AssignUserDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        
         protected override void R_Saving(GSM01100DTO poEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GSM01100DTO poEntity)
        {
            throw new NotImplementedException();
        }
        
       public GSM01100DTO GSM01100GetParameterDb(UsertoAssignParam poParameter)
        {
            using Activity activity = _activitySource.StartActivity("GSM01100GetParameterDb");

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

                int lnCUSER_LISTLength = poParameter.CUSER_LIST.Length + 1;

                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CGL_ACCOUNT", DbType.String, 50, poParameter.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_LIST", DbType.String, lnCUSER_LISTLength, poParameter.CUSER_ID);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);

                // Log the query using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poParameter.CCOMPANY_ID);
                _logger.LogDebug("CGL_ACCOUNT = {CGL_ACCOUNT}", poParameter.CGLACCOUNT_NO);
                _logger.LogDebug("CUSER_LIST = {CUSER_LIST}", poParameter.CUSER_ID);
                _logger.LogDebug("CUSER_ID = {CUSER_ID}", poParameter.CUSER_ID);

                DataTable loDataTable = loDb.SqlExecQuery(loConn, loCommand, true);

                loRtn = R_Utility.R_ConvertTo<GSM01100DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        public void SaveAssignUserToDb(GSM01100DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("SaveAssignUserToDb");
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = loDb.GetConnection();

            try
            {
                string lcQuery = $"EXEC RSP_GS_ASSIGN_COA_USER " +
                                 $"'{poEntity.CCOMPANY_ID}', " +
                                 $"'{poEntity.CGLACCOUNT_NO}', " +
                                 $"'{poEntity.CUSER_LIST}', " +
                                 $"'{poEntity.CUSER_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                R_ExternalException.R_SP_Init_Exception(loConn);

                // Log the query using LogDebug
                _logger.LogDebug("Executing query: {lcQuery}", lcQuery);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred while executing the query.");
                    loException.Add(ex);
                }

                loException.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred.");
                loException.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != System.Data.ConnectionState.Closed)
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }
            loException.ThrowExceptionIfErrors();
        }
    }
}
