using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;
using LMM05000Common.DTOs;

namespace LMM05000Back;

public class LMM05000Cls : R_BusinessObject<LMM05000DTO>
{
    protected override LMM05000DTO R_Display(LMM05000DTO poEntity)
    {
        R_Exception loException = new R_Exception();
        LMM05000DTO loRtn = null;
        R_Db loDb;
        DbConnection loConn = null;
        DbCommand loCmd;
        string lcQuery;
        try
        {
                
            loDb = new R_Db();
            loConn = loDb.GetConnection("R_DefaultConnectionString");
            loCmd = loDb.GetCommand();
                
            lcQuery = "RSP_GS_GET_UNIT_TYPE_PRICE_DETAIL";
            loCmd.CommandType = CommandType.StoredProcedure;
            loCmd.CommandText = lcQuery;
                
            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", System.Data.DbType.String, 50, poEntity.CPROPERTY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CVALID_DATE", System.Data.DbType.String, 50, poEntity.CVALID_DATE);
            loDb.R_AddCommandParameter(loCmd, "@CUNIT_TYPE_ID", System.Data.DbType.String, 50, poEntity.CUNIT_TYPE_ID);
            loDb.R_AddCommandParameter(loCmd, "@LACTIVE_ONLY", System.Data.DbType.Boolean, 50, poEntity.LACTIVE);
            loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", System.Data.DbType.String, 50, poEntity.CUSER_ID);

            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

            loRtn = R_Utility.R_ConvertTo<LMM05000DTO>(loDataTable).FirstOrDefault();
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();

        return loRtn;
    }

    protected override void R_Saving(LMM05000DTO poNewEntity, eCRUDMode poCRUDMode)
    {
          var loEx = new R_Exception();
          var loDb = new R_Db();
          DbCommand loCmd;
          var loConn = loDb.GetConnection("R_DefaultConnectionString");
          string lcAction = "";
          string lcQuery = "";
          
          try
            {
                loCmd = loDb.GetCommand();
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    lcAction = "ADD";
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    lcAction = "EDIT";
                }

                lcQuery = "RSP_LM_MAINTAIN_UNIT_TYPE_PRICE";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "CCOMPANY_ID", DbType.String, 8, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "CPROPERTY_ID", DbType.String, 20, poNewEntity.CPROPERTY_ID);
                loDb.R_AddCommandParameter(loCmd, "CUNIT_TYPE_ID", DbType.String, 20, poNewEntity.CUNIT_TYPE_ID);
                loDb.R_AddCommandParameter(loCmd, "CVALID_INTERNAL_ID", DbType.String, 20, poNewEntity.CVALID_INTERNAL_ID);
                loDb.R_AddCommandParameter(loCmd, "CVALID_DATE", DbType.String, 20, poNewEntity.CVALID_DATE);
                loDb.R_AddCommandParameter(loCmd, "CBY_SQM_TOTAL", DbType.String, 20, poNewEntity.CBY_SQM_TOTAL);
                loDb.R_AddCommandParameter(loCmd, "NNORMAL_PRICE_SQM", DbType.Decimal, 20, poNewEntity.NNORMAL_PRICE_SQM);
                loDb.R_AddCommandParameter(loCmd, "NNORMAL_SELLING_PRICE", DbType.Decimal, 20, poNewEntity.NNORMAL_SELLING_PRICE);
                loDb.R_AddCommandParameter(loCmd, "NBOTTOM_PRICE_SQM", DbType.Decimal, 20, poNewEntity.NBOTTOM_PRICE_SQM);
                loDb.R_AddCommandParameter(loCmd, "NBOTTOM_SELLING_PRICE", DbType.Decimal, 20, poNewEntity.NBOTTOM_SELLING_PRICE);
                loDb.R_AddCommandParameter(loCmd, "LOVERWRITE", DbType.Boolean, 20, poNewEntity.LOVERWRITE);
                loDb.R_AddCommandParameter(loCmd, "LACTIVE", DbType.Boolean, 20, poNewEntity.LACTIVE);
                loDb.R_AddCommandParameter(loCmd, "CACTION", DbType.String, 10, lcAction);
                loDb.R_AddCommandParameter(loCmd, "CUSER_ID", DbType.String, 20, poNewEntity.CUSER_ID);

                R_ExternalException.R_SP_Init_Exception(loConn);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                }
                catch (Exception ex)
                {
                    loEx.Add(ex);
                }

                loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));

            }
            catch (Exception ex)
            {
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
            loEx.ThrowExceptionIfErrors();
    }

    protected override void R_Deleting(LMM05000DTO poNewEntity)
    {
        var loEx = new R_Exception();
        var loDb = new R_Db();
        DbConnection loConn = null;
        DbCommand loCmd;
        string lcAction = "DELETE";
        string lcQuery = "";

        try
        {
            loConn = loDb.GetConnection("R_DefaultConnectionString");
            loCmd = loDb.GetCommand();

            lcQuery = "RSP_LM_MAINTAIN_UNIT_TYPE_PRICE";
            loCmd.CommandType = CommandType.StoredProcedure;
            loCmd.CommandText = lcQuery;

            loDb.R_AddCommandParameter(loCmd, "CCOMPANY_ID", DbType.String, 8, poNewEntity.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "CPROPERTY_ID", DbType.String, 20, poNewEntity.CPROPERTY_ID);
            loDb.R_AddCommandParameter(loCmd, "CUNIT_TYPE_ID", DbType.String, 20, poNewEntity.CUNIT_TYPE_ID);
            loDb.R_AddCommandParameter(loCmd, "CVALID_INTERNAL_ID", DbType.String, 20, poNewEntity.CVALID_INTERNAL_ID);
            loDb.R_AddCommandParameter(loCmd, "CVALID_DATE", DbType.String, 20, poNewEntity.CVALID_DATE);
            loDb.R_AddCommandParameter(loCmd, "CBY_SQM_TOTAL", DbType.String, 20, poNewEntity.CBY_SQM_TOTAL);
            loDb.R_AddCommandParameter(loCmd, "NNORMAL_PRICE_SQM", DbType.Decimal, 20, poNewEntity.NNORMAL_PRICE_SQM);
            loDb.R_AddCommandParameter(loCmd, "NNORMAL_SELLING_PRICE", DbType.Decimal, 20,
                poNewEntity.NNORMAL_SELLING_PRICE);
            loDb.R_AddCommandParameter(loCmd, "NBOTTOM_PRICE_SQM", DbType.Decimal, 20, poNewEntity.NBOTTOM_PRICE_SQM);
            loDb.R_AddCommandParameter(loCmd, "NBOTTOM_SELLING_PRICE", DbType.Decimal, 20,
                poNewEntity.NBOTTOM_SELLING_PRICE);
            loDb.R_AddCommandParameter(loCmd, "LOVERWRITE", DbType.Boolean, 20, poNewEntity.LOVERWRITE);
            loDb.R_AddCommandParameter(loCmd, "LACTIVE", DbType.Boolean, 20, poNewEntity.LACTIVE);
            loDb.R_AddCommandParameter(loCmd, "CACTION", DbType.String, 10, lcAction);
            loDb.R_AddCommandParameter(loCmd, "CUSER_ID", DbType.String, 20, poNewEntity.CUSER_ID);

            R_ExternalException.R_SP_Init_Exception(loConn);
            
            
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
    }

    public List<LMM05000DTO> GetTypePriceListDb (BackParameter poEntity)
    {
        R_Exception loException = new R_Exception();
        List<LMM05000DTO> loRtn = null;
        R_Db loDb;
        DbConnection loConn = null;
        DbCommand loCmd;
        string lcQuery = null;
        try
        {
                
            loDb = new R_Db();
            loConn = loDb.GetConnection("R_DefaultConnectionString");
            loCmd = loDb.GetCommand();
                
            lcQuery = "RSP_GS_GET_UNIT_TYPE_PRICE_LIST";
            loCmd.CommandType = CommandType.StoredProcedure;
            loCmd.CommandText = lcQuery;
                
            loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CPROPERTY_ID", System.Data.DbType.String, 50, poEntity.CPROPERTY_ID);
            loDb.R_AddCommandParameter(loCmd, "@CUNIT_TYPE_ID", System.Data.DbType.String, 50, poEntity.CUNIT_TYPE_ID);
            loDb.R_AddCommandParameter(loCmd, "@LACTIVE_ONLY", System.Data.DbType.String, 50, false);
            loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", System.Data.DbType.String, 50, poEntity.CUSER_ID);

            var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

            loRtn = R_Utility.R_ConvertTo<LMM05000DTO>(loDataTable).ToList();
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
            loException.ThrowExceptionIfErrors();

        return loRtn;
    }
    
    public List<PropertyListDTO> PropertyListDB(BackParameter poParameter)
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
        
    public void RSP_GS_ACTIVE_INACTIVE_PRICE_Method(ActiveInactiveParameterDTO poEntity)
    {
        R_Exception loException = new R_Exception();
        var loEx = new R_Exception();
        var loDb = new R_Db();
        var loConn = loDb.GetConnection("R_DefaultConnectionString");

        try
        {
            //R_Db loDb = new R_Db();
            //DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");
            string lcQuery = $"EXEC RSP_LM_ACTIVE_INACTIVE_UNIT_PRICE" +
                             $"'{poEntity.CCOMPANY_ID}', " +
                             $"'{poEntity.CPROPERTY_ID}', " +
                             $"'{poEntity.CUNIT_TYPE_ID}', " +
                             $"'{poEntity.CVALID_INTERNAL_ID}', " +
                             $"'{!poEntity.LACTIVE}', " +
                             $"'{poEntity.CUSER_ID}'";

            DbCommand loCmd = loDb.GetCommand();
            loCmd.CommandText = lcQuery;

            R_ExternalException.R_SP_Init_Exception(loConn);

            try
            {
                loDb.SqlExecNonQuery(loConn, loCmd, false);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));

        }
        catch (Exception ex)
        {
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
        loEx.ThrowExceptionIfErrors();
    }
}