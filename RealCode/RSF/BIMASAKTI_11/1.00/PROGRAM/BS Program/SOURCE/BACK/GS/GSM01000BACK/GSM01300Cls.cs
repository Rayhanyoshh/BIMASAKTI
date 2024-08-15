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
using System.Windows.Input;

namespace GSM01000Back
{
    public class GSM01300Cls : R_BusinessObject<GSM01300DTO>
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;


        public GSM01300Cls()
        {
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_GetInstanceActivitySource();

        }
        protected override GSM01300DTO R_Display(GSM01300DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("R_Display");

            R_Exception loEx = new R_Exception();
            GSM01300DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_GOA_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE", System.Data.DbType.String, 50, poEntity.CGOA_CODE);

                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CGOA_CODE = {CGOA_CODE}", poEntity.CGOA_CODE);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01300DTO>(loDataTable).FirstOrDefault();
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

        protected override void R_Saving(GSM01300DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GSM01300DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public List<GSM01300DTO> GetGoAListDb(GOAHeadListDbParameter poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetGoAListDb");

            R_Exception loException = new R_Exception();
            List<GSM01300DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_GOA_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);

                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01300DTO>(loDataTable).ToList();
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
        
        public void SaveAssignCOAToDb(COAtoAssignParam poEntity)
        {
            using Activity activity = _activitySource.StartActivity("SaveAssignCOAToDb");

            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = loDb.GetConnection();

            try
            {
                string lcQuery = $"EXEC RSP_GS_ASSIGN_GOA_COA " +
                                 $"'{poEntity.CCOMPANY_ID}', " +
                                 $"'{poEntity.CGOA_CODE}', " +
                                 $"'{poEntity.CCOA_LIST}', " +
                                 $"'{poEntity.CUSER_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                R_ExternalException.R_SP_Init_Exception(loConn);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                }
                catch (Exception ex)
                {
                    // Log the exception using R_LogError
                    _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                    loException.Add(ex);
                }

                loException.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred while executing the query.");
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
