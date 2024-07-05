using R_BackEnd;
using GSM01000Common.DTOs;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using GSM01000Back;
using GSM01000Common;
using R_Common;
using R_CommonFrontBackAPI;

namespace GSM001000Back
{
    public class GSM01000Cls : R_BusinessObject<GSM01000DTO>
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;
        private RSP_GS_MAINTAIN_COAResources.Resources_Dummy_Class _rspCOA = new();

        public GSM01000Cls()
        {
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_GetInstanceActivitySource();
        }
        
        protected override GSM01000DTO R_Display(GSM01000DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("R_Display");
            R_Exception loEx = new R_Exception();
            GSM01000DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_COA_DETAIL";
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

                // Log the query and parameter values using your custom logging method
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);
                _logger.LogDebug("Parameters: {loDbParam}", loDbParam);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01000DTO>(loDataTable).FirstOrDefault();
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


        
       protected override void R_Saving(GSM01000DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            using Activity activity = _activitySource.StartActivity("R_Saving");

            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn = null;
            string lcAction = "";

            try
            {
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

                lcQuery = "RSP_GS_MAINTAIN_COA";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                // Log the query and parameter values
                _logger.LogDebug("QueryExecuting stored procedure: {lcQuery}", lcQuery);
                _logger.LogDebug("Parameters: CCOMPANY_ID = {CCOMPANY_ID}, CGLACCOUNT_NO = {CGLACCOUNT_NO}, CGLACCOUNT_NAME = {CGLACCOUNT_NAME}, CBSIS = {CBSIS}, CDBCR = {CDBCR}, LACTIVE = {LACTIVE}, LUSER_RESTR = {LUSER_RESTR}, LCENTER_RESTR = {LCENTER_RESTR}, CCASH_FLOW_GROUP_CODE = {CCASH_FLOW_GROUP_CODE}, CCASH_FLOW_CODE = {CCASH_FLOW_CODE}, CACTION = {CACTION}, CUSER_ID = {CUSER_ID}",
                    poNewEntity.CCOMPANY_ID, poNewEntity.CGLACCOUNT_NO, poNewEntity.CGLACCOUNT_NAME, poNewEntity.CBSIS, poNewEntity.CDBCR, poNewEntity.LACTIVE, poNewEntity.LUSER_RESTR, poNewEntity.LCENTER_RESTR, poNewEntity.CCASH_FLOW_GROUP_CODE, poNewEntity.CCASH_FLOW_CODE, lcAction, poNewEntity.CUSER_ID);

                // Add command parameters
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", DbType.String, 20, poNewEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NAME", DbType.String, 60, poNewEntity.CGLACCOUNT_NAME);
                loDb.R_AddCommandParameter(loCmd, "@CBSIS", DbType.String, 1, poNewEntity.CBSIS);
                loDb.R_AddCommandParameter(loCmd, "@CDBCR", DbType.String, 1, poNewEntity.CDBCR);
                loDb.R_AddCommandParameter(loCmd, "@LACTIVE", DbType.Boolean, 2, poNewEntity.LACTIVE);
                loDb.R_AddCommandParameter(loCmd, "@LUSER_RESTR", DbType.Boolean, 2, poNewEntity.LUSER_RESTR);
                loDb.R_AddCommandParameter(loCmd, "@LCENTER_RESTR", DbType.Boolean, 2, poNewEntity.LCENTER_RESTR);
                loDb.R_AddCommandParameter(loCmd, "@CCASH_FLOW_GROUP_CODE", DbType.String, 20, poNewEntity.CCASH_FLOW_GROUP_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CCASH_FLOW_CODE", DbType.String, 20, poNewEntity.CCASH_FLOW_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, 10, lcAction);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 8, poNewEntity.CUSER_ID);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
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
                _logger.LogError(ex, "An error occurred in the outer catch block.");
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
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();
        }


        protected override void R_Deleting(GSM01000DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("R_Deleting");

            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loComm;
            DbConnection loConn = null;
            string lcAction = "DELETE";  // Set the action as "DELETE"

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loComm = loDb.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);

                lcQuery = "RSP_GS_MAINTAIN_COA";
                loComm.CommandType = CommandType.StoredProcedure;
                loComm.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loComm, "@CCOMPANY_ID", DbType.String, 8, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loComm, "@CGLACCOUNT_NO", DbType.String, 20, poEntity.CGLACCOUNT_NO);
                loDb.R_AddCommandParameter(loComm, "@CGLACCOUNT_NAME", DbType.String, 60, poEntity.CGLACCOUNT_NAME);
                loDb.R_AddCommandParameter(loComm, "@CBSIS", DbType.String, 1, poEntity.CBSIS);
                loDb.R_AddCommandParameter(loComm, "@CDBCR", DbType.String, 1, poEntity.CDBCR);
                loDb.R_AddCommandParameter(loComm, "@LACTIVE", DbType.Boolean, 2, poEntity.LACTIVE);
                loDb.R_AddCommandParameter(loComm, "@LUSER_RESTR", DbType.Boolean, 2, poEntity.LUSER_RESTR);
                loDb.R_AddCommandParameter(loComm, "@LCENTER_RESTR", DbType.Boolean, 2, poEntity.LCENTER_RESTR);
                loDb.R_AddCommandParameter(loComm, "@CCASH_FLOW_GROUP_CODE", DbType.String, 20, poEntity.CCASH_FLOW_GROUP_CODE);
                loDb.R_AddCommandParameter(loComm, "@CCASH_FLOW_CODE", DbType.String, 20, poEntity.CCASH_FLOW_CODE);
                loDb.R_AddCommandParameter(loComm, "@CACTION", DbType.String, 10, lcAction);
                loDb.R_AddCommandParameter(loComm, "@CUSER_ID", DbType.String, 8, poEntity.CUSER_ID);

                // Log the query and parameter values
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);
                _logger.LogDebug("Parameters: CCOMPANY_ID = {CCOMPANY_ID}, CGLACCOUNT_NO = {CGLACCOUNT_NO}, CGLACCOUNT_NAME = {CGLACCOUNT_NAME}, CBSIS = {CBSIS}, CDBCR = {CDBCR}, LACTIVE = {LACTIVE}, LUSER_RESTR = {LUSER_RESTR}, LCENTER_RESTR = {LCENTER_RESTR}, CCASH_FLOW_GROUP_CODE = {CCASH_FLOW_GROUP_CODE}, CCASH_FLOW_CODE = {CCASH_FLOW_CODE}, CACTION = {CACTION}, CUSER_ID = {CUSER_ID}",
                    poEntity.CCOMPANY_ID, poEntity.CGLACCOUNT_NO, poEntity.CGLACCOUNT_NAME, poEntity.CBSIS, poEntity.CDBCR, poEntity.LACTIVE, poEntity.LUSER_RESTR, poEntity.LCENTER_RESTR, poEntity.CCASH_FLOW_GROUP_CODE, poEntity.CCASH_FLOW_CODE, lcAction, poEntity.CUSER_ID);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loComm, false);
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
                _logger.LogError(ex, "An error occurred in the outer catch block.");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        
        public List<GSM01000DTO> GetCoaListDb(COAListDbParameter poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetCoaListDb");

            R_Exception loException = new R_Exception();
            List<GSM01000DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_COA_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", System.Data.DbType.String, 50, poEntity.CUSER_ID);

                // Log the query and parameter values
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);
                _logger.LogDebug("Parameters: CCOMPANY_ID = {CCOMPANY_ID}, CUSER_ID = {CUSER_ID}", poEntity.CCOMPANY_ID, poEntity.CUSER_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01000DTO>(loDataTable).ToList();
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

        
        public void CopyFromProcessGSM01000Cls(CopyFromProcessParameter poEntity)
        {
            using Activity activity = _activitySource.StartActivity("CopyFromProcessGSM01000Cls");
            R_Exception loException = new R_Exception();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_COPY_COA " +
                                 $"'{poEntity.CLOGIN_COMPANY_ID}', " +
                                 $"'{poEntity.CCOPY_FROM_COMPANY_ID}', " +
                                 $"'{poEntity.CUSER_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                // Log the query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                loDb.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();
        }

        
        public List<CopyFromProcessCompanyDTO> GetCompanyList(COAListDbParameter poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetCompanyList");

            R_Exception loException = new R_Exception();
            List<CopyFromProcessCompanyDTO> loResult = null;

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_GET_COMPANY_INFO " +
                                 $"'{poEntity.CCOMPANY_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                // Log the query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<CopyFromProcessCompanyDTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
        }

        
        public void ActiveInactiveProcess()
        {
            using Activity activity = _activitySource.StartActivity("ActiveInactiveProcess");

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
        
        public void RSP_GS_ACTIVE_INACTIVE_COA_Method(ActiveInactiveParameterDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("RSP_GS_ACTIVE_INACTIVE_COA_Method");
            R_Exception loException = new R_Exception();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_ACTIVE_INACTIVE_COA " +
                                 $"'{poEntity.CCOMPANY_ID}', " +
                                 $"'{poEntity.CGLACCOUNT_NO}', " +
                                 $"'{poEntity.LACTIVE}', " +
                                 $"'{poEntity.CUSER_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                // Log the query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                loDb.SqlExecNonQuery(loConn, loCmd, true);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();
        }

        public PrimaryAccountDTO PrimaryAccountCheckCls(GSM01000DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("PrimaryAccountCheckCls");

            R_Exception loEx = new R_Exception();
            PrimaryAccountDTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_COMPANY_INFO";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);

                // Log the query using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<PrimaryAccountDTO>(loDataTable).FirstOrDefault();
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

        
        public GSM01000UploadHeaderDTO CompanyDetailCls(COAListDbParameter poEntity)
        {
            using Activity activity = _activitySource.StartActivity("CompanyDetailCls");

            R_Exception loEx = new R_Exception();
            GSM01000UploadHeaderDTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = " select * from SAM_COMPANIES where CCOMPANY_ID = @CCOMPANY_ID ";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);

                // Log the query using LogDebug
                _logger.LogDebug("Executing SQL query: {lcQuery}", lcQuery);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01000UploadHeaderDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the SQL query.");
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        
       public List<GSM01000ResultSPPrintCOADTO> GetPrintDataResult(GSM01000PrintParamCOADTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetPrintDataResult");

            R_Exception loEx = new R_Exception();
            List<GSM01000ResultSPPrintCOADTO> loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
                var loCmd = loDb.GetCommand();

                var lcQuery = "RSP_GS_PRINT_COA";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGL_ACCOUNT_FROM", DbType.String, 50, poEntity.CGL_ACCOUNT_FROM);
                loDb.R_AddCommandParameter(loCmd, "@CGL_ACCOUNT_TO", DbType.String, 50, poEntity.CGL_ACCOUNT_TO);
                loDb.R_AddCommandParameter(loCmd, "@LBALANCE_SHEET", DbType.Boolean, 2, poEntity.LBALANCE_SHEET);
                loDb.R_AddCommandParameter(loCmd, "@LINCOME_STATEMENT", DbType.Boolean, 2, poEntity.LINCOME_STATEMENT);
                loDb.R_AddCommandParameter(loCmd, "@CSHORT_BY", DbType.String, 50, poEntity.CSHORT_BY);
                loDb.R_AddCommandParameter(loCmd, "@LPRINT_INACTIVE", DbType.Boolean, 50, poEntity.LPRINT_INACTIVE);
                loDb.R_AddCommandParameter(loCmd, "@LPRINT_USER_RESTRICT", DbType.Boolean, 50, poEntity.LPRINT_USER_RESTRICT);
                loDb.R_AddCommandParameter(loCmd, "@LPRINT_CENTER_RESTRICT", DbType.Boolean, 50, poEntity.LPRINT_CENTER_RESTRICT);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 50, poEntity.CUSER_LOGIN_ID);

                // Log the query using LogDebug
                _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<GSM01000ResultSPPrintCOADTO>(loDataTable).ToList();
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


        public PrintLogoResultDTO GetBaseHeaderLogoCompany(string pcCompanyId)
        {
            using Activity activity = _activitySource.StartActivity("GetBaseHeaderLogoCompany");
            var loEx = new R_Exception();
            PrintLogoResultDTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
                var loCmd = loDb.GetCommand();


                var lcQuery = "SELECT dbo.RFN_GET_COMPANY_LOGO(@CCOMPANY_ID) as CLOGO";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, pcCompanyId);

                //Debug Logs
                var loDbParam = loCmd.Parameters.Cast<DbParameter>()
                    .Where(x => x != null && x.ParameterName.StartsWith("@")).Select(x => x.Value);
                _logger.LogDebug("SELECT dbo.RFN_GET_COMPANY_LOGO({@CCOMPANY_ID}) as CLOGO", loDbParam);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<PrintLogoResultDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

       public List<GSM01000ResultSPPrintGOADTO> GetPrintDataResultGOA(GSM01000PrintParamGOADTO poEntity)
       {
           using Activity activity = _activitySource.StartActivity("GetPrintDataResultGOA");

           R_Exception loEx = new R_Exception();
           List<GSM01000ResultSPPrintGOADTO> loResult = null;

           try
           {
               var loDb = new R_Db();
               var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
               var loCmd = loDb.GetCommand();

               var lcQuery = "RSP_GS_PRINT_GOA";
               loCmd.CommandText = lcQuery;
               loCmd.CommandType = CommandType.StoredProcedure;

               loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poEntity.CCOMPANY_ID);
               loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE_FROM", DbType.String, 50, poEntity.CGOA_CODE_FROM);
               loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE_TO", DbType.String, 50, poEntity.CGOA_CODE_TO);
               loDb.R_AddCommandParameter(loCmd, "@LBALANCE_SHEET", DbType.Boolean, 2, poEntity.LBALANCE_SHEET);
               loDb.R_AddCommandParameter(loCmd, "@LINCOME_STATEMENT", DbType.Boolean, 2, poEntity.LINCOME_STATEMENT);
               loDb.R_AddCommandParameter(loCmd, "@CSHORT_BY", DbType.String, 50, poEntity.CSHORT_BY);
               loDb.R_AddCommandParameter(loCmd, "@LPRINT_GL_ACCOUNT", DbType.Boolean, 50, poEntity.LPRINT_GL_ACCOUNT);
               loDb.R_AddCommandParameter(loCmd, "@LPRINT_INACTIVE", DbType.Boolean, 50, poEntity.LPRINT_INACTIVE);
               loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN_ID", DbType.String, 50, poEntity.CUSER_LOGIN_ID);

               // Log the query using LogDebug
               _logger.LogDebug("Executing stored procedure: {lcQuery}", lcQuery);

               var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

               loResult = R_Utility.R_ConvertTo<GSM01000ResultSPPrintGOADTO>(loDataTable).ToList();
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
