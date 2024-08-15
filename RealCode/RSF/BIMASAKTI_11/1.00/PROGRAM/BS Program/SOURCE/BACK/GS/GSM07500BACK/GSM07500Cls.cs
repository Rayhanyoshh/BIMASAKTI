using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM07500Common.DTOs;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Transactions;
using RSP_GS_MAINTAIN_PERIODResources;
using GSM07500Common;

namespace GSM07500Back
{
    public class GSM07500Cls: R_BusinessObject<SaveBatchGSM07500DTO>
    {
        private LoggerGSM07500 _logger;
        private readonly ActivitySource _activitySource;
        private RSP_GS_MAINTAIN_PERIODResources.Resources_Dummy_Class _RSP_PERIOD = new RSP_GS_MAINTAIN_PERIODResources.Resources_Dummy_Class();

        public GSM07500Cls()
        {
            _logger = LoggerGSM07500.R_GetInstanceLogger();
            _activitySource = GSM07500Activity.R_GetInstanceActivitySource();

        }
    
        public List<GSM07500DTO> GetPeriodDetailDbList (GSM07500DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetPeriodDetailDbList");
            R_Exception loException = new R_Exception();
            List<GSM07500DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
        
                lcQuery = $"SELECT * FROM GSM_PERIOD_DT A (NOLOCK) " +
                          $"WHERE A.CCOMPANY_ID = '{poEntity.CCOMPANY_ID}' " +
                          $"AND A.CCYEAR = '{poEntity.CCYEAR}' ";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                // Log the SQL query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                // No need to log parameters since they are directly used in the query

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM07500DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public List<GSM07500DTO> RftGenerateGSMPeriod (GeneratePeriodParameter poEntity)
        {
            using Activity activity = _activitySource.StartActivity("RftGenerateGSMPeriod");

            R_Exception loException = new R_Exception();
            List<GSM07500DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
        
                lcQuery = $"SELECT CPERIOD_NO, CSTART_DATE, CEND_DATE " +
                          $"FROM dbo.RFT_GENERATE_GSM_PERIODS('{poEntity.@CCOMPANY_ID}', {poEntity.CYEAR}, 1, 12)";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;
        
                // Log the SQL query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CYEAR = {CYEAR}", poEntity.CYEAR);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM07500DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }

        protected override SaveBatchGSM07500DTO R_Display(SaveBatchGSM07500DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("R_Display");

            _logger.LogInfo("Start - R_Display");
            SaveBatchGSM07500DTO loRtn = null;
            R_Exception loException = new R_Exception();
            try
            {
                loRtn = poEntity;
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred during R_Display.");
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
            _logger.LogInfo("End - R_Display");

            return loRtn;
        }

        protected override void R_Saving(SaveBatchGSM07500DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            using Activity activity = _activitySource.StartActivity("R_Saving");

            var loEx = new R_Exception();
            string lcQuery = "";
            var loDb = new R_Db();
            DbConnection loConn = null;
            var loCmd = loDb.GetCommand();

            // Log the start of the R_Saving method
            _logger.LogInfo("Starting R_Saving for poNewEntity: {@poNewEntity}, poCRUDMode: {poCRUDMode}", poNewEntity, poCRUDMode);

            try
            {
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    poNewEntity.CACTION = "ADD";
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    poNewEntity.CACTION = "EDIT";
                }

                using (var TransScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    loConn = loDb.GetConnection();

                    lcQuery = "DECLARE @CPERIOD_LIST AS RDT_COMMON_OBJECT ";

                    if (poNewEntity.CPERIOD_LIST != null && poNewEntity.CPERIOD_LIST.Count > 0)
                    {
                        lcQuery += "INSERT INTO @CPERIOD_LIST  " +
                            "(COBJECT_ID, COBJECT_DESC, CATTRIBUTE01 ) " +
                            "VALUES ";
                        foreach (var loRate in poNewEntity.CPERIOD_LIST)
                        {
                            lcQuery += $"('{loRate.CPERIOD_NO}', '{loRate.CSTART_DATE}', '{loRate.CEND_DATE}' ) ,";
                        }
                        lcQuery = lcQuery.Substring(0, lcQuery.Length - 1) + " ";
                    }

                    lcQuery += "EXEC RSP_GS_MAINTAIN_PERIOD " +
                        $"@CCOMPANY_ID = '{poNewEntity.CCOMPANY_ID}' " +
                        $",@CYEAR = '{poNewEntity.CYEAR}' " +
                        $",@LPERIOD_MODE = {poNewEntity.LPERIOD_MODE} " +
                        $",@INO_PERIOD = {poNewEntity.INO_PERIOD} " +
                        $",@CACTION = '{poNewEntity.CACTION}' " +
                        $",@CUSER_ID = '{poNewEntity.CUSER_ID}' " +
                        ",@CPERIOD_LIST = @CPERIOD_LIST ";

                    // Log the SQL query using LogDebug
                    _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                    R_ExternalException.R_SP_Init_Exception(loConn);

                    try
                    {
                        loDb.SqlExecQuery(lcQuery, loConn, false);
                    }
                    catch (Exception ex)
                    {
                        // Log the exception using R_LogError
                        _logger.LogError(ex, "An error occurred while executing the SQL query.");
                        loEx.Add(ex);
                    }

                    loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));

                    TransScope.Complete();
                };
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred during R_Saving.");
                loEx.Add(ex);
            }
            finally
            {
                // Log the end of the R_Saving method
                _logger.LogInfo("R_Saving completed.");

                if (loConn != null)
                {
                    if (loConn.State != System.Data.ConnectionState.Closed)
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }

            loEx.ThrowExceptionIfErrors();
        }

        protected override void R_Deleting(SaveBatchGSM07500DTO poEntity)
        {
            throw new NotImplementedException();
        }
    }
}
