using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using R_BackEnd;
using R_Common;
using PMR01000Common;
using PMR01000Common.DTO_s;
using PMR01000Common.DTO_s.PrintDTO;
using PMR01000Common.DTOs.PrintDTO;

namespace PMR01000Back;

public class PMR01000Cls
{
    private LoggerPMR01000 _logger;
    private readonly ActivitySource _activitySource;
    
    public PMR01000Cls()
    {
        _logger = LoggerPMR01000.R_GetInstanceLogger();
        _activitySource = PMR01000Activity.R_GetInstanceActivitySource();
    }
    public List<PMR01000ResultPrintSPDTO> GetPrintDataResult (PMR01000PrintParamDTO poEntity)
    {
       using Activity activity = _activitySource.StartActivity("GetPrintDataResultGOA");

       R_Exception loEx = new R_Exception();
       List<PMR01000ResultPrintSPDTO> loResult = null;

       try
       {
           var loDb = new R_Db();
           var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
           var loCmd = loDb.GetCommand();

           var lcQuery = "RSP_PMR01000_GET_REPORT";
           loCmd.CommandText = lcQuery;
           loCmd.CommandType = CommandType.StoredProcedure;

           loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poEntity.CCOMPANY_ID);
           loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, poEntity.CPROPERTY_ID);
           loDb.R_AddCommandParameter(loCmd, "@CDEPOSIT_TYPE", DbType.String, 50, poEntity.CDEPOSIT_TYPE);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_PERIOD", DbType.String, 2, poEntity.CFROM_PERIOD);
           loDb.R_AddCommandParameter(loCmd, "@CTO_PERIOD", DbType.String, 2, poEntity.CTO_PERIOD);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_BUILDING", DbType.String, 50, poEntity.CFROM_BUILDING);
           loDb.R_AddCommandParameter(loCmd, "@CTO_BUILDING", DbType.String, 50, poEntity.CTO_BUILDING);
           loDb.R_AddCommandParameter(loCmd, "@CCUT_OFF_DATE", DbType.String, 50, poEntity.CCUT_OFF_DATE);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_TYPE", DbType.String, 50, poEntity.CFROM_TYPE);
           loDb.R_AddCommandParameter(loCmd, "@CTO_TYPE", DbType.String, 50, poEntity.CTO_TYPE);
           loDb.R_AddCommandParameter(loCmd, "@CFROM_TRANS_TYPE", DbType.String, 50, poEntity.CFROM_TRANS_TYPE);
           loDb.R_AddCommandParameter(loCmd, "@CTO_TRANS_TYPE", DbType.String, 50, poEntity.CTO_TRANS_TYPE);
           loDb.R_AddCommandParameter(loCmd, "@CLANG_ID", DbType.String, 50, poEntity.CLANGUAGE_ID);

           // Log the query using LogDebug
           _logger.LogError("Executing stored procedure: {lcQuery}", lcQuery);

           var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

           loResult = R_Utility.R_ConvertTo<PMR01000ResultPrintSPDTO>(loDataTable).ToList();
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
    
    public PMR01000CBSystemParamDTO GetCBSystemParamRecord()
    {
        using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
        var loEx = new R_Exception();
        PMR01000CBSystemParamDTO loResult = null;

        try
        {
            var loDb = new R_Db();
            var loConn = loDb.GetConnection("R_DefaultConnectionString");
            var loCmd = loDb.GetCommand();

            var lcQuery = "RSP_CB_GET_SYSTEM_PARAM";
            loCmd.CommandText = lcQuery;
            loCmd.CommandType = CommandType.StoredProcedure;

            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, R_BackGlobalVar.COMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, R_BackGlobalVar.CULTURE);

            //Debug Logs
            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
            loResult = R_Utility.R_ConvertTo<PMR01000CBSystemParamDTO>(loDataTable).FirstOrDefault();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();

        return loResult;
    }
    
    
    public List<PropertyListDTO> PropertyListDB(PMR01000DTO poParameter)
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
    
    public List<PMR01000BuildingListDTO> BuildingListCLs(PMR01000DTO poParameter)
    {
        R_Exception loException = new R_Exception();
        List<PMR01000BuildingListDTO> loResult = null;
        try
        {
            var loDb = new R_Db();
            var loConn = loDb.GetConnection("R_DefaultConnectionString");
            var loCmd = loDb.GetCommand();

            var lcQuery = "EXEC RSP_GS_GET_BUILDING_LIST @CCOMPANY_ID, @CPROPERTY_ID, @CUSER_ID, @LAGREEMENT";
            loCmd.CommandText = lcQuery;

            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poParameter.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 20, poParameter.CPROPERTY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poParameter.CUSER_ID);
            loDb.R_AddCommandParameter(loCmd, "@LAGREEMENT", DbType.Int32,1, 1);

            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
            loResult = R_Utility.R_ConvertTo<PMR01000BuildingListDTO>(loDataTable).ToList();
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loResult;
    }
    
    public PMR01000PeriodCompanyDTO GetYearRange()
    {
        using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
        var loEx = new R_Exception();
        PMR01000PeriodCompanyDTO loResult = null;

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
            loResult = R_Utility.R_ConvertTo<PMR01000PeriodCompanyDTO>(loDataTable).FirstOrDefault();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();

        return loResult;
    }
    
    public List<PMR01000PeriodDTDTO> PeriodDetailCls (PMR01000DTO poParameter)
    {
        R_Exception loException = new R_Exception();
        List<PMR01000PeriodDTDTO> loResult = null;
        try
        {
            var loDb = new R_Db();
            var loConn = loDb.GetConnection("R_DefaultConnectionString");
            var loCmd = loDb.GetCommand();

            var lcQuery = "EXEC RSP_GS_GET_PERIOD_DT_LIST @CCOMPANY_ID, @CYEAR";
            loCmd.CommandText = lcQuery;

            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poParameter.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CYEAR", DbType.String, 4, DateTime.Now.Year.ToString());

            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
            loResult = R_Utility.R_ConvertTo<PMR01000PeriodDTDTO>(loDataTable).ToList();
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loResult;
    }
    
}