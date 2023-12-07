using GSM008500Common.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;
using GSM001000Back;
using GSM008500Common;
using GSM008500Common.DTOs.PrintDTO;
using GSM08500Common.DTOs;

namespace GSM08500Back
{
    public class GSM08500Cls: R_BusinessObject<GSM08500DTO>
    {
        private LoggerGSM08500 _logger;

        public GSM08500Cls()
        {
            _logger = LoggerGSM08500.R_GetInstanceLogger();
        }
        protected override GSM08500DTO R_Display(GSM08500DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM08500DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_STATISTIC_ACCOUNT_DT";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", System.Data.DbType.String, 50, poEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", System.Data.DbType.String, 50, poEntity.CUSER_ID);

                var loDbParam = loCmd.Parameters.Cast<DbParameter>()
                    .Where(x =>
                        x.ParameterName is
                            "@CCOMPANY_ID" or
                            "@CGLACCOUNT_NO" or
                            "@CUSER_ID"
                    )
                    .Select(x => x.Value);

                // Log the query and parameter values
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);
                _logger.LogDebug("Parameters: {@loDbParam}", loDbParam);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM08500DTO>(loDataTable).FirstOrDefault();
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

        protected override void R_Saving(GSM08500DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn = null;
            string lcAction = "";

            try
            {
                _logger.LogDebug("R_Saving Start");

                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);

                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    lcAction = "ADD";
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    lcAction = "EDIT";
                }

                lcQuery = "RSP_GS_MAINTAIN_STATISTIC_ACCOUNT";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                _logger.LogDebug("Operation: {lcAction}, Query: {lcQuery}", lcAction, lcQuery);


                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 20, poNewEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NAME", DbType.String, 60, poNewEntity.CGLACCOUNT_NAME);
                loDb.R_AddCommandParameter(loCmd, "@CBSIS", DbType.String, 1, poNewEntity.CBSIS);
                loDb.R_AddCommandParameter(loCmd, "@CDBCR", DbType.String, 1, poNewEntity.CDBCR);
                loDb.R_AddCommandParameter(loCmd, "@LACTIVE", DbType.Boolean, 2, poNewEntity.LACTIVE);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 10, lcAction);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, poNewEntity.CUSER_ID);

                // Log the query and parameter values
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);
                foreach (DbParameter parameter in loCmd.Parameters)
                {
                    _logger.LogDebug("Parameter: {ParameterName} = {ParameterValue}", parameter.ParameterName, parameter.Value);
                }

                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                    _logger.LogDebug("R_Saving Executed Successfully");
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                    loEx.Add(ex);
                }

                loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred.");
                loEx.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != ConnectionState.Closed)
                    {
                        loConn.Close();
                        _logger.LogDebug("Connection Closed");
                    }
                    loConn.Dispose();
                    _logger.LogDebug("Connection Disposed");
                }
            }

            _logger.LogDebug("R_Saving End");

            EndBlock:
            loEx.ThrowExceptionIfErrors();
        }

        protected override void R_Deleting(GSM08500DTO poNewEntity)
        { 
            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loComm;
            DbConnection loConn = null;
            string lcAction = "DELETE";

            try
            {
                // Start logging
                _logger.LogDebug("Start R_Deleting: Entity = {Entity}, Action = {Action}", poNewEntity, lcAction);

                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loComm = loDb.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);

                lcQuery = "RSP_GS_MAINTAIN_STATISTIC_ACCOUNT";
                loComm.CommandType = CommandType.StoredProcedure;
                loComm.CommandText = lcQuery;

                // Add parameters with logging
                loDb.R_AddCommandParameter(loComm, "@CCOMPANY_ID", DbType.String, 8, poNewEntity.CCOMPANY_ID);
                _logger.LogDebug("Parameter added: @CCOMPANY_ID = {CCOMPANY_ID}", poNewEntity.CCOMPANY_ID);

                loDb.R_AddCommandParameter(loComm, "@CGLACCOUNT_NO", DbType.String, 20, poNewEntity.CGLACCOUNT_NO);
                _logger.LogDebug("Parameter added: @CGLACCOUNT_NO = {CGLACCOUNT_NO}", poNewEntity.CGLACCOUNT_NO);

                loDb.R_AddCommandParameter(loComm, "@CGLACCOUNT_NAME", DbType.String, 60, poNewEntity.CGLACCOUNT_NAME);
                _logger.LogDebug("Parameter added: @CGLACCOUNT_NAME = {CGLACCOUNT_NAME}", poNewEntity.CGLACCOUNT_NAME);

                loDb.R_AddCommandParameter(loComm, "@CBSIS", DbType.String, 1, poNewEntity.CBSIS);
                _logger.LogDebug("Parameter added: @CBSIS = {CBSIS}", poNewEntity.CBSIS);

                loDb.R_AddCommandParameter(loComm, "@CDBCR", DbType.String, 1, poNewEntity.CDBCR);
                _logger.LogDebug("Parameter added: @CDBCR = {CDBCR}", poNewEntity.CDBCR);

                loDb.R_AddCommandParameter(loComm, "@LACTIVE", DbType.Boolean, 2, poNewEntity.LACTIVE);
                _logger.LogDebug("Parameter added: @LACTIVE = {LACTIVE}", poNewEntity.LACTIVE);

                loDb.R_AddCommandParameter(loComm, "@CACTION", DbType.String, 10, lcAction);
                _logger.LogDebug("Parameter added: @CACTION = {CACTION}", lcAction);

                loDb.R_AddCommandParameter(loComm, "@CUSER_ID", DbType.String, 8, poNewEntity.CUSER_ID);
                _logger.LogDebug("Parameter added: @CUSER_ID = {CUSER_ID}", poNewEntity.CUSER_ID);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loComm, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred during DELETE operation.");
                    loEx.Add(ex);
                }

                loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred.");
                loEx.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != ConnectionState.Closed)
                    {
                        loConn.Close();
                    }
                    loConn.Dispose();
                }

                // End logging
                _logger.LogDebug("End R_Deleting");
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public List<GSM08500DTO> GetStatAccListDb (GSM08500DTO poEntity)
        {
            R_Exception loException = new R_Exception();
            List<GSM08500DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = "RSP_GS_GET_STATISTIC_ACCOUNT_LIST";

            try
            {
                // Start logging
                _logger.LogDebug("Start GetStatAccListDb: Entity = {Entity}", poEntity);

                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                // Add parameters with logging
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                _logger.LogDebug("Parameter added: @CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);

                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", System.Data.DbType.String, 50, poEntity.CUSER_ID);
                _logger.LogDebug("Parameter added: @CUSER_ID = {CUSER_ID}", poEntity.CUSER_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM08500DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during GetStatAccListDb.");
                loException.Add(ex);
            }

            // End logging
            _logger.LogDebug("End GetStatAccListDb");

            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public void CopyFromProcessGSM08500Cls(CopyFromProcessParameter poEntity)
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                // Create the SQL query with placeholders for parameters
                string lcQuery = "EXEC RSP_GS_COPY_STATISTIC_ACCOUNT " +
                                 "@CLOGIN_COMPANY_ID, @CCOPY_FROM_COMPANY_ID, @CUSER_ID";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                // Add parameters
                loDb.R_AddCommandParameter(loCmd, "@CLOGIN_COMPANY_ID", DbType.String, 50, poEntity.CLOGIN_COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCOPY_FROM_COMPANY_ID", DbType.String, 50, poEntity.CCOPY_FROM_COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, poEntity.CUSER_ID);

                // Log the query and parameters
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);
                foreach (DbParameter parameter in loCmd.Parameters)
                {
                    _logger.LogDebug("Parameter: {ParameterName} = {ParameterValue}", parameter.ParameterName, parameter.Value);
                }

                loDb.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();

        }
        
        public List<CopyFromProcessCompanyDTO> GetCompanyList()
        {
            R_Exception loException = new R_Exception();
            List<CopyFromProcessCompanyDTO> loResult = null;

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                // Create the SQL query
                string lcQuery = @"SELECT A.CCOMPANY_ID, B.CCOMPANY_NAME FROM GSM_COMPANY A (NOLOCK)
                           INNER JOIN SAM_COMPANIES B (NOLOCK) ON A.CCOMPANY_ID = B.CCOMPANY_ID
                           WHERE A.LPRIMARY_ACCOUNT = 1 --[TRUE]";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                // Log the query
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<CopyFromProcessCompanyDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }
        
        public void ActiveInactiveProcess()
        {
            R_Exception loException = new R_Exception();

            try
            {
                if (loException.Haserror == false)
                {

                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();
        }
        
        public void RSP_GS_ACTIVE_INACTIVE_StatAcc_Method(ActiveInactiveParameterDTO poEntity)
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                // Create the SQL query
                string lcQuery = $"EXEC RSP_GS_ACTIVE_INACTIVE_STATISTIC_ACCOUNT " +
                                 $"'{poEntity.CCOMPANY_ID}', " +
                                 $"'{poEntity.CGLACCOUNT_NO}', " +
                                 $"'{poEntity.LACTIVE}', " +
                                 $"'{poEntity.CUSER_ID}'";

                // Log the query
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                loDb.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();
        }
        
        public GSM08500UploadHeaderDTO CompanyDetailCls(GSM08500DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM08500UploadHeaderDTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "SELECT * FROM SAM_COMPANIES WHERE CCOMPANY_ID = @CCOMPANY_ID";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);

                // Log the query
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM08500UploadHeaderDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }
          public List<GSM08500ResultSPPrintStatAccDTO> GetPrintDataResultStatAcc (GSM08500PrintParamStatAccDTO poEntity)
       {
           R_Exception loEx = new R_Exception();
           List<GSM08500ResultSPPrintStatAccDTO> loResult = null;

           try
           {
               var loDb = new R_Db();
               var loConn = loDb.GetConnection("R_ReportConnectionString");
               var loCmd = loDb.GetCommand();

               var lcQuery = "RSP_GS_PRINT_STATISTIC_ACCOUNT";
               loCmd.CommandText = lcQuery;
               loCmd.CommandType = CommandType.StoredProcedure;

               loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poEntity.CCOMPANY_ID);
               loDb.R_AddCommandParameter(loCmd, "@CGL_ACCOUNT_FROM", DbType.String, 50, poEntity.CGL_ACCOUNT_FROM);
               loDb.R_AddCommandParameter(loCmd, "@CGL_ACCOUNT_TO", DbType.String, 50, poEntity.CGL_ACCOUNT_TO);
               loDb.R_AddCommandParameter(loCmd, "@LBALANCE_SHEET", DbType.Boolean, 2, poEntity.LBALANCE_SHEET);
               loDb.R_AddCommandParameter(loCmd, "@LINCOME_STATEMENT", DbType.Boolean, 2, poEntity.LINCOME_STATEMENT);
               loDb.R_AddCommandParameter(loCmd, "@CSHORT_BY", DbType.String, 50, poEntity.CSHORT_BY);
               loDb.R_AddCommandParameter(loCmd, "@LPRINT_INACTIVE", DbType.Boolean, 50, poEntity.LPRINT_INACTIVE);
               loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 50, poEntity.CUSER_LOGIN_ID);


               // Log the query using LogDebug
               _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

               var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

               loResult = R_Utility.R_ConvertTo<GSM08500ResultSPPrintStatAccDTO>(loDataTable).ToList();
           }
           catch (Exception ex)
           {
               // Log the exception
               _logger.LogError(ex, "An error occurred while executing the stored procedure.");
               loEx.Add(ex);
           }

           loEx.ThrowExceptionIfErrors();

           return loResult;
       }
    }
}

