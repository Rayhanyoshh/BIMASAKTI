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
using GSM01000Back;

namespace GSM01200Back
{
    public class GSM01200Cls: R_BusinessObject<GSM01200DTO>
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;


        public GSM01200Cls()
        {
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_GetInstanceActivitySource();
    
        }
        protected override GSM01200DTO R_Display(GSM01200DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("R_Display");
            R_Exception loEx = new R_Exception();
            GSM01200DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_COA_CENTER_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CCENTER_CODE", DbType.String, 50, poEntity.CCENTER_CODE);

                // Log the query using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poEntity.CGLACCOUNT_NO);
                _logger.LogDebug("CCENTER_CODE = {CCENTER_CODE}", poEntity.CCENTER_CODE);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01200DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, $"An error {ex} occurred while executing the stored procedure.");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        protected override void R_Saving(GSM01200DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            using Activity activity = _activitySource.StartActivity("R_Saving");

            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loComm;
            DbConnection loConn = null;
            string lcAction = "";

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
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

                // Log the query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                // Log each parameter
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poNewEntity.CCOMPANY_ID);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poNewEntity.CGLACCOUNT_NO);
                _logger.LogDebug("CCENTER_C0DE = {CCENTER_C0DE}", poNewEntity.CCENTER_CODE);
                _logger.LogDebug("CCREATED_BY = {CCREATED_BY}", poNewEntity.CCREATE_BY);
                _logger.LogDebug("CUPDATE_BY = {CUPDATE_BY}", poNewEntity.CUPDATE_BY);

                loDb.SqlExecNonQuery(loConn, loComm, true);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
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
            using Activity activity = _activitySource.StartActivity("GetCoACenterListDb");

            R_Exception loException = new R_Exception();
            List<GSM01200DTO> loRtn = null;
            string lcCmd;
            R_Db loDb;
            DbCommand loDbCommand;

            try
            {
                loDb = new R_Db();
                loDbCommand = loDb.GetCommand();
                lcCmd = "RSP_GS_GET_COA_CENTER_LIST @CCOMPANY_ID, @CGLACCOUNT_NO";

                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcCmd}", lcCmd);

                loDbCommand.CommandText = lcCmd;

                loDb.R_AddCommandParameter(loDbCommand, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loDbCommand, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poEntity.CGLACCOUNT_NO);

                var loDataTable = loDb.SqlExecQuery(loDb.GetConnection(), loDbCommand, true);

                loRtn = R_Utility.R_ConvertTo<GSM01200DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError("An error {ex} occurred while executing the stored procedure.",ex);
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public List<AssignCenterDTO> GetCenterToAssignList(CenterAssignParameter poNewEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetCenterToAssignList");

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

                lcQuery = "RSP_GS_GET_LOOKUP_CENTER_LIST";

                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROGRAM_ID", DbType.String, 50, poNewEntity.CPROGRAM_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPARAMETER_ID", DbType.String, 50, poNewEntity.CGLACCOUNT_NO);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poNewEntity.CCOMPANY_ID);
                _logger.LogDebug("CPROGRAM_ID = {CPROGRAM_ID}", poNewEntity.CPROGRAM_ID);
                _logger.LogDebug("CPARAMETER_ID = {CPARAMETER_ID}", poNewEntity.CGLACCOUNT_NO);

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<AssignCenterDTO>(loRtnTemp).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        
        public GSM01200DTO GSM01200GetParameterDb(ParameterHeadGLDbParameter poParameter)
        {
            using Activity activity = _activitySource.StartActivity("GSM01200GetParameterDb");

            R_Exception loException = new R_Exception();
            GSM01200DTO loRtn = null;
            string lcQuery;
            DbConnection loConn = null;
            DbCommand loCommand;
            R_Db loDb;

            try
            {
                loDb = new R_Db();
                loCommand = loDb.GetCommand();
                loConn = loDb.GetConnection();
                lcQuery = "RSP_GS_GET_COA_DETAIL";
                loCommand.CommandType = CommandType.StoredProcedure;
                loCommand.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CGLACCOUNT_NO", DbType.String, 50, poParameter.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);

                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poParameter.CCOMPANY_ID);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poParameter.CGLACCOUNT_NO);
                _logger.LogDebug("CUSER_ID = {CUSER_ID}", poParameter.CUSER_ID);

                DataTable loDataTable = loDb.SqlExecQuery(loConn, loCommand, true);

                loRtn = R_Utility.R_ConvertTo<GSM01200DTO>(loDataTable).FirstOrDefault();

            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        public void SaveAssignCenterToDb(CentertoAssignParam poEntity)
        {
            using Activity activity = _activitySource.StartActivity("SaveAssignCenterToDb");

            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = loDb.GetConnection();

            try
            {
                string lcQuery = $"EXEC RSP_GS_ASSIGN_COA_CENTER " +
                                 $"'{poEntity.CCOMPANY_ID}', " +
                                 $"'{poEntity.CGLACCOUNT_NO}', " +
                                 $"'{poEntity.CCENTER_LIST}', " +
                                 $"'{poEntity.CUSER_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                // Log the query using LogDebug
                _logger.LogDebug("Executing query: {lcQuery}", lcQuery);

                R_ExternalException.R_SP_Init_Exception(loConn);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                }
                catch (Exception ex)
                {
                    // Log the exception using R_LogError
                    _logger.LogError(ex, "An error occurred while executing the query.");
                    loException.Add(ex);
                }

                loException.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
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
