using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using PMM05000Common;
using PMM05000Common.DTOs;
using R_BackEnd;
using R_Common;

namespace PMM05000Back;

public class PMM05000InitCls
{
    RSP_PM_MAINTAIN_PRICINGResources.Resources_Dummy_Class _rspMaintainPricing = new();

    RSP_PM_MAINTAIN_PRICING_RATEResources.Resources_Dummy_Class _rspMaintainPricingRate = new();

    RSP_PM_ACTIVE_INACTIVE_PRICINGResources.Resources_Dummy_Class _rspActiveInactivePricing = new();

    private LoggerPMM05000 _logger;
    private readonly ActivitySource _activitySource;

    public PMM05000InitCls()
    {
        _logger = LoggerPMM05000.R_GetInstanceLogger();
        _activitySource = PMM05000Activity.R_GetInstanceActivitySource();
    }

    public List<PropertyDTO> GetPropertyList(PropertyDTO poEntity)
    {
        using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
        R_Exception loEx = new R_Exception();
        List<PropertyDTO> loRtn = null;
        R_Db loDB;
        DbConnection loConn;
        DbCommand loCmd;
        string lcQuery;
        try
        {
            loDB = new R_Db();
            loConn = loDB.GetConnection();
            loCmd = loDB.GetCommand();

            lcQuery = "RSP_GS_GET_PROPERTY_LIST";
            loCmd.CommandType = CommandType.StoredProcedure;
            loCmd.CommandText = lcQuery;

            loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poEntity.CCOMPANY_ID);
            loDB.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poEntity.CUSER_ID);

            ShowLogDebug(lcQuery, loCmd.Parameters);
            var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
            loRtn = R_Utility.R_ConvertTo<PropertyDTO>(loRtnTemp).ToList();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
            ShowLogError(loEx);
        }
        loEx.ThrowExceptionIfErrors();
        return loRtn;
    }

    public List<TypeDTO> GetChargesPriceTypeList(TypeParam poParam)
    {
        using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
        R_Exception loEx = new();
        List<TypeDTO> loRtn = null;
        R_Db loDB;
        DbConnection loConn;
        DbCommand loCmd;
        string lcQuery;
        try
        {
            loDB = new R_Db();
            loConn = loDB.GetConnection();
            loCmd = loDB.GetCommand();

            lcQuery = "SELECT CCODE, CDESCRIPTION FROM RFT_GET_GSB_CODE_INFO(@CLASS_APPLICATION, @CCOMPANY_ID, @CLASS_ID, @REC_ID_LIST, @LANG_ID)";
            loCmd.CommandType = CommandType.Text;
            loCmd.CommandText = lcQuery;

            loDB.R_AddCommandParameter(loCmd, "@CLASS_APPLICATION", DbType.String, int.MaxValue, poParam.CLASS_APPLICATION);
            loDB.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poParam.CCOMPANY_ID);
            loDB.R_AddCommandParameter(loCmd, "@CLASS_ID", DbType.String, int.MaxValue, poParam.CLASS_ID);
            loDB.R_AddCommandParameter(loCmd, "@REC_ID_LIST", DbType.String, int.MaxValue, poParam.REC_ID_LIST);
            loDB.R_AddCommandParameter(loCmd, "@LANG_ID", DbType.String, int.MaxValue, poParam.LANG_ID);

            ShowLogDebug(lcQuery, loCmd.Parameters);
            var loRtnTemp = loDB.SqlExecQuery(loConn, loCmd, true);
            loRtn = R_Utility.R_ConvertTo<TypeDTO>(loRtnTemp).ToList();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
            ShowLogError(loEx);
        }
        loEx.ThrowExceptionIfErrors();
        return loRtn;
    }

    #region log method helper

    private void ShowLogDebug(string query, DbParameterCollection parameters)
    {
        var paramValues = string.Join(", ", parameters.Cast<DbParameter>().Select(p => $"{p.ParameterName} '{p.Value}'"));
        _logger.LogDebug($"EXEC {query} {paramValues}");
    }

    private void ShowLogError(Exception ex)
    {
        _logger.LogError(ex);
    }

    #endregion
        
}
