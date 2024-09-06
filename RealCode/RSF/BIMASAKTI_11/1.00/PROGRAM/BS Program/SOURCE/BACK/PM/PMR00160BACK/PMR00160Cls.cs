using PMR00160COMMON;
using R_BackEnd;
using R_Common;
using System.Data.Common;
using System.Data;
using System.Diagnostics;
using PMR00160COMMON.Utility_Report;

namespace PMR00160BACK
{
    public class PMR00160Cls
    {
        private LoggerPMR00160 _logger;
        private readonly ActivitySource _activitySource;

        public PMR00160Cls()
        {
            //Initial and Get Logger
            _logger = LoggerPMR00160.R_GetInstanceLogger();
            _activitySource = PMR00160Activity.R_GetInstanceActivitySource();
        }
        public List<PMR00160PropertyDTO> GetPropertyList(PMR00160DBParamDTO poParameter)
        {
            string lcMethodName = nameof(GetPropertyList);
            Activity activity = _activitySource.StartActivity(lcMethodName)!;
            _logger.LogInfo(string.Format("START process method {0} on Cls", lcMethodName));
            R_Exception loException = new R_Exception();
            List<PMR00160PropertyDTO> loReturn = null;
            R_Db loDb;
            DbCommand loCommand;

            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();

                var lcQuery = @"RSP_GS_GET_PROPERTY_LIST";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 50, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CUSER_ID", DbType.String, 50, poParameter.CUSER_ID);

                var loDbParam = loCommand.Parameters.Cast<DbParameter>()
                    .Where(x => x != null && x.ParameterName.StartsWith("@"))
                    .ToDictionary(x => x.ParameterName, x => x.Value);
                _logger.LogDebug("{@ObjectQuery} {@Parameter}", loCommand.CommandText, loDbParam);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loReturn = R_Utility.R_ConvertTo<PMR00160PropertyDTO>(loReturnTemp).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                _logger.LogError(loException);
            }
            loException.ThrowExceptionIfErrors();
            _logger.LogInfo(string.Format("END process method {0} on Cls", lcMethodName));

            return loReturn!;
        }
        public PMR00160InitialProcess GetInitialProcess(PMR00160DBParamDTO poParameter)
        {
            string lcMethodName = nameof(GetInitialProcess);
            using Activity activity = _activitySource.StartActivity(lcMethodName)!;
            _logger.LogInfo(string.Format("START process method {0} on Cls", lcMethodName));

            R_Exception loException = new R_Exception();
            PMR00160InitialProcess loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection();
                loCommand = loDb.GetCommand();
                
                //PERIOD RANGE
                 var lcQuery = "RSP_GS_GET_PERIOD_YEAR_RANGE";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;
                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CYEAR", DbType.String, 4, "");
                loDb.R_AddCommandParameter(loCommand, "@CMODE", DbType.String, 10, "");

                var loDbParam = loCommand.Parameters.Cast<DbParameter>()
                 .Where(x => x != null && x.ParameterName.StartsWith("@"))
                 .ToDictionary(x => x.ParameterName, x => x.Value);
                _logger.LogInfo("Execute query initial process to get year range");
                _logger.LogDebug("{@ObjectQuery} {@Parameter}", loCommand.CommandText, loDbParam);
                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, false);
                loResult = R_Utility.R_ConvertTo<PMR00160InitialProcess>(loReturnTemp).FirstOrDefault()!;

                //GET MONTH
                var lcQueryDefaultMonth = "SELECT MONTH(dbo.RFN_GET_DB_TODAY (@CCOMPANY)) as IMONTHS";

                loCommand.CommandText = lcQueryDefaultMonth;
                loCommand.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCommand, "CCOMPANY", DbType.String, 8, poParameter.CCOMPANY_ID);
                var loDbParamMonth = loCommand.Parameters.Cast<DbParameter>()
                    .Where(x => x != null && x.ParameterName.StartsWith("@"))
                    .ToDictionary(x => x.ParameterName, x => x.Value);

                _logger.LogInfo("Execute query initial process to get months");
                _logger.LogDebug("{@ObjectQuery} {@Parameter}", loCommand.CommandText, loDbParamMonth);
                var loReturnMonths = loDb.SqlExecQuery(loConn, loCommand, false);
                var looResult = R_Utility.R_ConvertTo<PMR00160InitialProcess>(loReturnMonths).FirstOrDefault()!;

                //ASSIGN TO OBJECT RESULT
                loResult.IMONTHS = looResult.IMONTHS;

                //GET YEAR
                var lcQueryDefaultYear = $"SELECT YEAR(dbo.RFN_GET_DB_TODAY ('{poParameter.CCOMPANY_ID}')) as IYEAR";

                //var lcQueryDefaultYear = $"SELECT YEAR(dbo.RFN_GET_DB_TODAY ('{poParameter.CCOMPANY_ID}')) as CYEAR";

                loCommand.CommandText = lcQueryDefaultYear;
                loCommand.CommandType = CommandType.Text;
              //  loDb.R_AddCommandParameter(loCommand, "CCOMPANY", DbType.String, 8, poParameter.CCOMPANY_ID);
                var loDbParamYear = loCommand.Parameters.Cast<DbParameter>()
                    .Where(x => x != null && x.ParameterName.StartsWith("@"))
                    .ToDictionary(x => x.ParameterName, x => x.Value);

                _logger.LogInfo("Execute query initial process to get months");
                _logger.LogDebug("{@ObjectQuery} {@Parameter}", loCommand.CommandText, loDbParamYear);
                var loReturnYear = loDb.SqlExecQuery(loConn, loCommand, false);
                var loResultYear = R_Utility.R_ConvertTo<PMR00160InitialProcess>(loReturnYear).FirstOrDefault()!;

                //ASSIGN TO OBJECT RESULT
                loResult.IYEAR = loResultYear.IYEAR;

            }
            catch (Exception ex)
            {
                loException.Add(ex);
                _logger.LogError(loException);
            }
            loException.ThrowExceptionIfErrors();
            _logger.LogInfo(string.Format("END process method {0} on Cls", lcMethodName));

            return loResult!;
        }

        public List<PMR00160DataSummaryDbDTO> GetLOCStatusSummaryReportData(PMR00160DBParamDTO poParameter)
        {
            string lcMethodName = nameof(GetLOCStatusSummaryReportData);
            using Activity activity = _activitySource.StartActivity(lcMethodName)!;
            _logger.LogInfo(string.Format("START process method {0} on Cls", lcMethodName));
            R_Exception loException = new R_Exception();
            List<PMR00160DataSummaryDbDTO>? loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
                loCommand = loDb.GetCommand();
                var lcQuery = @"RSP_PMR00160_GET_REPORT";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CPROPERTY_ID", DbType.String, 20, poParameter.CPROPERTY_ID);

                loDb.R_AddCommandParameter(loCommand, "@CFROM_DEPT_CODE", DbType.String, 20, poParameter.CFROM_DEPARTMENT_ID);
                loDb.R_AddCommandParameter(loCommand, "@CTO_DEPT_CODE", DbType.String, 20, poParameter.CTO_DEPARTMENT_ID);
                loDb.R_AddCommandParameter(loCommand, "@CFROM_SALESMAN_ID", DbType.String, 8, poParameter.CFROM_SALESMAN_ID);
                loDb.R_AddCommandParameter(loCommand, "@CTO_SALESMAN_ID", DbType.String, 8, poParameter.CTO_SALESMAN_ID);
                loDb.R_AddCommandParameter(loCommand, "@CFROM_PERIOD", DbType.String, 6, poParameter.CFROM_PERIOD);
                loDb.R_AddCommandParameter(loCommand, "@CTO_PERIOD", DbType.String, 6, poParameter.CTO_PERIOD); 
                loDb.R_AddCommandParameter(loCommand, "@CREPORT_TYPE", DbType.String, 6, poParameter.CREPORT_TYPE);
                loDb.R_AddCommandParameter(loCommand, "@CLANG_ID", DbType.String, 2, poParameter.CLANG_ID);

                var loDbParam = loCommand.Parameters.Cast<DbParameter>()
                    .Where(x => x != null && x.ParameterName.StartsWith("@"))
                    .ToDictionary(x => x.ParameterName, x => x.Value);
                _logger.LogDebug("{@ObjectQuery} {@Parameter}", loCommand.CommandText, loDbParam);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<PMR00160DataSummaryDbDTO>(loReturnTemp).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                _logger.LogError(loException);
            }
            loException.ThrowExceptionIfErrors();
            _logger.LogInfo(string.Format("END process method {0} on Cls", lcMethodName));

            return loResult!;
        }

        public List<PMR00160DataDetailDbDTO> GetLOCStatusDetailReportData(PMR00160DBParamDTO poParameter)
        {
            string lcMethodName = nameof(GetLOCStatusDetailReportData);
            using Activity activity = _activitySource.StartActivity(lcMethodName)!;
            _logger.LogInfo(string.Format("START process method {0} on Cls", lcMethodName));
            R_Exception loException = new R_Exception();
            List<PMR00160DataDetailDbDTO>? loResult = null;
            R_Db loDb;
            DbCommand loCommand;
            try
            {
                loDb = new R_Db();
                var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
                loCommand = loDb.GetCommand();
                var lcQuery = @"RSP_PMR00160_GET_REPORT";
                loCommand.CommandText = lcQuery;
                loCommand.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCommand, "@CCOMPANY_ID", DbType.String, 8, poParameter.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCommand, "@CPROPERTY_ID", DbType.String, 20, poParameter.CPROPERTY_ID);

                loDb.R_AddCommandParameter(loCommand, "@CFROM_DEPT_CODE", DbType.String, 20, poParameter.CFROM_DEPARTMENT_ID);
                loDb.R_AddCommandParameter(loCommand, "@CTO_DEPT_CODE", DbType.String, 20, poParameter.CTO_DEPARTMENT_ID);
                loDb.R_AddCommandParameter(loCommand, "@CFROM_SALESMAN_ID", DbType.String, 8, poParameter.CFROM_SALESMAN_ID);
                loDb.R_AddCommandParameter(loCommand, "@CTO_SALESMAN_ID", DbType.String, 8, poParameter.CTO_SALESMAN_ID);
                loDb.R_AddCommandParameter(loCommand, "@CFROM_PERIOD", DbType.String, 6, poParameter.CFROM_PERIOD);
                loDb.R_AddCommandParameter(loCommand, "@CTO_PERIOD", DbType.String, 6, poParameter.CTO_PERIOD); //CTO_PERIOD
                loDb.R_AddCommandParameter(loCommand, "@CREPORT_TYPE", DbType.String, 6, poParameter.CREPORT_TYPE);
                loDb.R_AddCommandParameter(loCommand, "@CLANG_ID", DbType.String, 2, poParameter.CLANG_ID);

                var loDbParam = loCommand.Parameters.Cast<DbParameter>()
                    .Where(x => x != null && x.ParameterName.StartsWith("@"))
                    .ToDictionary(x => x.ParameterName, x => x.Value);
                _logger.LogDebug("{@ObjectQuery} {@Parameter}", loCommand.CommandText, loDbParam);

                var loReturnTemp = loDb.SqlExecQuery(loConn, loCommand, true);
                loResult = R_Utility.R_ConvertTo<PMR00160DataDetailDbDTO>(loReturnTemp).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
                _logger.LogError(loException);
            }
            loException.ThrowExceptionIfErrors();
            _logger.LogInfo(string.Format("END process method {0} on Cls", lcMethodName));

            return loResult!;
        }

        public PMR00160DataSummaryDbDTO GetBaseHeaderLogoCompany(PMR00160DBParamDTO poParameter)
        {
            using Activity activity = _activitySource.StartActivity("GetBaseHeaderLogoCompany");
            var loEx = new R_Exception();
            PMR00160DataSummaryDbDTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
                var loCmd = loDb.GetCommand();


                var lcQuery = "SELECT dbo.RFN_GET_COMPANY_LOGO(@CCOMPANY_ID) as CLOGO";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.Text;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poParameter.CCOMPANY_ID);

                //Debug Logs
                var loDbParam = loCmd.Parameters.Cast<DbParameter>()
                .Where(x => x != null && x.ParameterName.StartsWith("@")).Select(x => x.Value);
                _logger.LogDebug("SELECT dbo.RFN_GET_COMPANY_LOGO({@CCOMPANY_ID}) as CLOGO", loDbParam);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<PMR00160DataSummaryDbDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
    }
}