using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using PMR02200Common.DTOs;
using PMR02200Common.DTOs.PrintDTO;
using PMR02200Common;
using R_BackEnd;
using R_Common;
namespace PMR02200Back;

public class PMR02200Cls
{
    private LoggerPMR02200 _logger;
    private readonly ActivitySource _activitySource;
    
    public PMR02200Cls()
    {
        _logger = LoggerPMR02200.R_GetInstanceLogger();
        _activitySource = PMR02200Activity.R_GetInstanceActivitySource();
    }

    public List<PMR02200DTO> GetPrintDataResult (PMR02200PrintParamDTO poEntity)
    {
       using Activity activity = _activitySource.StartActivity("GetPrintDataResultGOA");

       R_Exception loEx = new R_Exception();
       List<PMR02200DTO> loResult = null;

       try
       {
           var loDb = new R_Db();
           var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
           var loCmd = loDb.GetCommand();

           var lcQuery = "RSP_PMR02200_GET_REPORT";
           loCmd.CommandText = lcQuery;
           loCmd.CommandType = CommandType.StoredProcedure;

           loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poEntity.CCOMPANY_ID);
           loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, poEntity.CPROPERTY_ID);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_CUSTOMER_ID", DbType.String, 50, poEntity.CFROM_CUSTOMER_ID);
           loDb.R_AddCommandParameter(loCmd, "@CTO_CUSTOMER_ID", DbType.String, 2, poEntity.CTO_CUSTOMER_ID);
           loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 2, poEntity.CDEPT_CODE);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_LOI_NO", DbType.String, 50, poEntity.CFROM_LOI_NO);
           loDb.R_AddCommandParameter(loCmd, "@CTO_LOI_NO", DbType.String, 50, poEntity.CTO_LOI_NO);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_AGREEMENT_NO", DbType.String, 50, poEntity.CFROM_AGREEMENT_NO);
           loDb.R_AddCommandParameter(loCmd, "@CTO_AGREEMENT_NO", DbType.String, 50, poEntity.CTO_AGREEMENT_NO);
           loDb.R_AddCommandParameter(loCmd, "@CCUT_OFF_DATE", DbType.String, 50, poEntity.CCUT_OFF_DATE);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_PERIOD", DbType.String, 50, poEntity.CFROM_PERIOD);
           loDb.R_AddCommandParameter(loCmd, "@CTO_PERIOD", DbType.String, 50, poEntity.CTO_PERIOD);
           loDb.R_AddCommandParameter(loCmd, "@CSTATEMENT_DATE", DbType.String, 50, poEntity.CSTATEMENT_DATE);
           loDb.R_AddCommandParameter(loCmd, "@LFILTER_INCLUDE_ZERO_BALANCE", DbType.Boolean, 50, poEntity.LFILTER_INCLUDE_ZERO_BALANCE);
           loDb.R_AddCommandParameter(loCmd, "@CLANG_ID", DbType.String, 50, poEntity.CLANG_ID);

           // Log the query using LogDebug
           _logger.LogError("Executing stored procedure: {lcQuery}", lcQuery);

           var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

           loResult = R_Utility.R_ConvertTo<PMR02200DTO>(loDataTable).ToList();
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
            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, R_BackGlobalVar.COMPANY_ID);

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
    
    public List<PropertyListDTO> PropertyListDB(PMR02200DTO poParameter)
    {
        R_Exception loException = new R_Exception();
        List<PropertyListDTO> loResult = null;
        try
        {
            var loDb = new R_Db();
            var loConn = loDb.GetConnection("R_DefaultConnectionString");
            var loCmd = loDb.GetCommand();

            var lcQuery = "EXEC RSP_GS_GET_PROPERTY_LIST @CCOMPANY_ID, @CUSER_ID";
            loCmd.CommandText = lcQuery;

            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poParameter.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poParameter.CUSER_ID);

            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
            loResult = R_Utility.R_ConvertTo<PropertyListDTO>(loDataTable).ToList();
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loResult;
    }
    
    public PMR02200PeriodCompanyDTO GetYearRange()
    {
        using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
        var loEx = new R_Exception();
        PMR02200PeriodCompanyDTO loResult = null;

        try
        {
            var loDb = new R_Db();
            var loConn = loDb.GetConnection("R_DefaultConnectionString");
            var loCmd = loDb.GetCommand();

            var lcQuery = "RSP_GS_GET_PERIOD_YEAR_RANGE";
            loCmd.CommandText = lcQuery;
            loCmd.CommandType = CommandType.StoredProcedure;

            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, R_BackGlobalVar.COMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CYEAR", DbType.String, 50, "");
            loDb.R_AddCommandParameter(loCmd, "@CMODE", DbType.String, 50, "");

            //Debug Logs
            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
            loResult = R_Utility.R_ConvertTo<PMR02200PeriodCompanyDTO>(loDataTable).FirstOrDefault();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();

        return loResult;
    }

}