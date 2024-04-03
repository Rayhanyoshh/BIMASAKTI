using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;
using PMM05000Common.DTOs;
using PMM05000Back;

namespace PMM05000Back;

public class PMM05010Cls : R_BusinessObject<PMM05010DTO>
{
    protected override PMM05010DTO R_Display(PMM05010DTO poEntity)
    {
        R_Exception loException = new R_Exception();
        PMM05010DTO loRtn = null;
        R_Db loDb;
        DbConnection loConn = null;
        DbCommand loCmd;
        string lcQuery;
        try
        {
                
            loDb = new R_Db();
            loConn = loDb.GetConnection("R_DefaultConnectionString");
            loCmd = loDb.GetCommand();
                
            lcQuery = "RSP_GS_GET_UNIT_TYPE_DETAIL";
            loCmd.CommandType = CommandType.StoredProcedure;
            loCmd.CommandText = lcQuery;
                
            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", System.Data.DbType.String, 50, poEntity.CPROPERTY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CUNIT_TYPE_ID", System.Data.DbType.String, 50, poEntity.CUNIT_TYPE_ID);
            
            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

            loRtn = R_Utility.R_ConvertTo<PMM05010DTO>(loDataTable).FirstOrDefault();
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loRtn;
    }

    protected override void R_Saving(PMM05010DTO poNewEntity, eCRUDMode poCRUDMode)
    {
        throw new NotImplementedException();
    }

    protected override void R_Deleting(PMM05010DTO poEntity)
    {
        throw new NotImplementedException();
    }
    
    public List<PMM05010DTO> UnitTypeListDB(BackParameter poParameter)
    {
        R_Exception loEx = new R_Exception();
        List<PMM05010DTO> loRtn = null;
        R_Db loDb;
        DbConnection loConn = null;
        DbCommand loCmd;
        string lcQuery = null;
        try
        {
            loDb = new R_Db();
            loConn = loDb.GetConnection("R_DefaultConnectionString");
            loCmd = loDb.GetCommand();
            
            lcQuery = "RSP_GS_GET_UNIT_TYPE_LIST";
            loCmd.CommandType = CommandType.StoredProcedure;
            loCmd.CommandText = lcQuery;
                
            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poParameter.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", System.Data.DbType.String, 50, poParameter.CPROPERTY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CUNIT_TYPE_CATEGORY_ID", System.Data.DbType.String, 50, poParameter.CUNIT_TYPE_CATEGORY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", System.Data.DbType.String, 50, poParameter.CUSER_ID);
            

            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
            loRtn = R_Utility.R_ConvertTo<PMM05010DTO>(loDataTable).ToList();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
        return loRtn;
    }
}