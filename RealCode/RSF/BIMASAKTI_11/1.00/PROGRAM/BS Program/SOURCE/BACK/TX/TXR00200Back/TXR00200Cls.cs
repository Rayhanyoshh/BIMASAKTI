using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using R_BackEnd;
using R_Common;
using TXR00200Common;
using TXR00200Common.DTOs;
using TXR00200Common.DTOs.PrintDTO;
using TXR00200Common.PrintDTO;

namespace TXR00200Back;

public class TXR00200Cls
{
    private LoggerTXR00200 _logger;
    private readonly ActivitySource _activitySource;
    
    public TXR00200Cls()
    {
        _logger = LoggerTXR00200.R_GetInstanceLogger();
        _activitySource = TXR00200Activity.R_GetInstanceActivitySource();
    }

    public List<TXR00200DTO> GetPrintDataResult (PrintParamTXDTO poEntity)
    {
       using Activity activity = _activitySource.StartActivity("GetPrintDataResultGOA");

       R_Exception loEx = new R_Exception();
       List<TXR00200DTO> loResult = null;

       try
       {
           var loDb = new R_Db();
           var loConn = loDb.GetConnection(R_Db.eDbConnectionStringType.ReportConnectionString);
           var loCmd = loDb.GetCommand();

           var lcQuery = "RSP_TXR00200_EFAKTUR_REPORT";
           loCmd.CommandText = lcQuery;
           loCmd.CommandType = CommandType.StoredProcedure;

           loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poEntity.CCOMPANY_ID);
           loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", DbType.String, 50, poEntity.CPROPERTY_ID);
           loDb.R_AddCommandParameter(loCmd, "@CTAX_PERIOD_YEAR", DbType.String, 50, poEntity.CTAX_PERIOD_YEAR);
           loDb.R_AddCommandParameter(loCmd, "@CTAX_PERIOD_MONTH", DbType.String, 50, poEntity.CTAX_PERIOD_MONTH);
           loDb.R_AddCommandParameter(loCmd, "@CTAX_TYPE", DbType.String, 50, poEntity.CTAX_TYPE);
           loDb.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, 50, poEntity.CTRANS_CODE);
           loDb.R_AddCommandParameter(loCmd, "@CUSER_LOGIN", DbType.String, 50, poEntity.CUSER_LOGIN);

           // Log the query using LogDebug
           _logger.LogError("Executing stored procedure: {lcQuery}", lcQuery);

           var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

           loResult = R_Utility.R_ConvertTo<TXR00200DTO>(loDataTable).ToList();
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
    
    public List<PropertyListDTO> PropertyListDB(PrintParamTXDTO poParameter)
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
            loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poParameter.CUSER_LOGIN);

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
    
    public List<TXR00200PeriodDetailDTO> PeriodDetailListDB (PrintParamTXDTO poEntity)
    {
        R_Exception loException = new R_Exception();
        List<TXR00200PeriodDetailDTO> loResult = null;
        try
        {
            var loDb = new R_Db();
            var loConn = loDb.GetConnection("");
            var loCmd = loDb.GetCommand();

            var lcQuery = "RSP_GS_GET_PERIOD_DT_LIST";
            loCmd.CommandText = lcQuery;
            loCmd.CommandType = CommandType.StoredProcedure;

            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 15, poEntity.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CYEAR", DbType.String, 50, poEntity.CTAX_PERIOD_YEAR);

            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
            loResult = R_Utility.R_ConvertTo<TXR00200PeriodDetailDTO>(loDataTable).ToList();
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loResult;
    }
}