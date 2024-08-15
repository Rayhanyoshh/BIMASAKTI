using GSM001000Back;
using GSM01000Common.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using GSM01000Common;

namespace GSM01000Back
{
    public class GSM01310Cls: R_BusinessObject<GSM01310DTO>
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;


        public GSM01310Cls()
        {
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_GetInstanceActivitySource();

        }
        protected override GSM01310DTO R_Display(GSM01310DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("R_Display");

            R_Exception loEx = new R_Exception();
            GSM01310DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
    
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
        
                lcQuery = "RSP_GS_GET_GOA_COA_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
        
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE", DbType.String, 50, poEntity.CGOA_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 50, poEntity.CGLACCOUNT_NO);
        
                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CGOA_CODE = {CGOA_CODE}", poEntity.CGOA_CODE);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poEntity.CGLACCOUNT_NO);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01310DTO>(loDataTable).FirstOrDefault();
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

        protected override void R_Saving(GSM01310DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            using Activity activity = _activitySource.StartActivity("R_Saving");

            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loComm;
            DbConnection loConn = null;
    
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
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
        
                // Log the SQL query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poNewEntity.CCOMPANY_ID);
                _logger.LogDebug("CGOA_CODE = {CGOA_CODE}", poNewEntity.CGOA_CODE);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poNewEntity.CGLACCOUNT_NO);
                _logger.LogDebug("CCREATED_BY = {CCREATED_BY}", poNewEntity.CCREATE_BY);
                _logger.LogDebug("CUPDATE_BY = {CUPDATE_BY}", poNewEntity.CUPDATE_BY);

                loDb.SqlExecNonQuery(loConn, loComm, true);
            }
            catch (Exception ex)
            {
                // Log the exception using R_LogError
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        protected override void R_Deleting(GSM01310DTO poEntity)
        {
            throw new NotImplementedException();
        }
        
        public List<GSM01310DTO> GetGoACoAList (GoAMainDbParameter poParameter)
        {
            using Activity activity = _activitySource.StartActivity("GetGoACoAList");

            R_Exception loEx = new R_Exception();
            List<GSM01310DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
                lcQuery = "RSP_GS_GET_GOA_COA_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 20, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE", System.Data.DbType.String, 100, poParameter.CGOA_CODE);
        
                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poParameter.CCOMPANY_ID);
                _logger.LogDebug("CGOA_CODE = {CGOA_CODE}", poParameter.CGOA_CODE);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM01310DTO>(loDataTable).ToList();
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
        
        public List<GSM01310DTO> GetCoAToAssignList(GOAHeadListDbParameter poNewEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetCoAToAssignList");

            List<GSM01310DTO> loRtn = null;
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

                lcQuery = $"RSP_GS_GET_LOOKUP_COA_LIST"; 
        
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
        
                loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, poNewEntity.CCOMPANY_ID);
                loDB.R_AddCommandParameter(loCmd, "@CPROGRAM_ID", DbType.String, 50, "GSM01000");

                // Log the stored procedure name using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                // Log each parameter using LogDebug
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poNewEntity.CCOMPANY_ID);
                _logger.LogDebug("CPROGRAM_ID = {CPROGRAM_ID}", "GSM01000");

                var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
                loRtn = R_Utility.R_ConvertTo<GSM01310DTO>(loRtnTemp).ToList();
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
    }
}
