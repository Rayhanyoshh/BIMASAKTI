using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM01000Common.DTOs;
using System.Data;
using System.Data.Common;

namespace GSM001000Back
{
    public class GSM01000Cls : R_BusinessObject<GSM01000DTO>
    {
        protected override GSM01000DTO R_Display(GSM01000DTO poEntity) //menunggu RSP_GET_COA_DETAIL
        {
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
               

                // loRtn = loDb.SqlExecObjectQuery<GSM02000DTO>(lcQuery, loConn, true, poEntity).FirstOrDefault();
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01000DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        protected override void R_Saving(GSM01000DTO poNewEntity, eCRUDMode poCRUDMode)
        {
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
                
                loDb.SqlExecNonQuery(loConn, loCmd, true);
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
            
                    EndBlock:
                    loEx.ThrowExceptionIfErrors();
        }

        protected override void R_Deleting(GSM01000DTO poEntity)
        {
           R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loComm;
            DbConnection loConn = null;
            string lcAction = "";
        
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loComm = loDb.GetCommand();

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
                loDb.R_AddCommandParameter(loComm, "@CACTION", DbType.String, 10, "DELETE");
                loDb.R_AddCommandParameter(loComm, "@CUSER_ID", DbType.String, 8, poEntity.CUSER_ID);

                loDb.SqlExecNonQuery(loConn, loComm, true);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public List<GSM01000DTO> GetCoaListDb (COAListDbParameter poEntity)
        {
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

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01000DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
        
        public void CopyFromProcess(CopyFromProcessParameter poEntity)
        {
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

                loDb.SqlExecNonQuery(loConn, loCmd, true);

            }
            catch (Exception ex)
            {
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

                string lcQuery = @"SELECT A.CCOMPANY_ID, B.CCOMPANY_NAME FROM GSM_COMPANY A (NOLOCK) 
                                   INNER JOIN SAM_COMPANIES B (NOLOCK) ON A.CCOMPANY_ID = B.CCOMPANY_ID 
                                   WHERE A.LPRIMARY_ACCOUNT = 1 --[TRUE] ";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<CopyFromProcessCompanyDTO>(loDataTable).ToList();
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
        
        public void RSP_GS_ACTIVE_INACTIVE_COA_Method(ActiveInactiveParameterDTO poEntity)
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_ACTIVE_INACTIVE_COA" +
                                 $"'{poEntity.CCOMPANY_ID}', " +
                                 $"'{poEntity.CGLACCOUNT_NO}', " +
                                 $"'{poEntity.LACTIVE}', " +
                                 $"'{poEntity.CUSER_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                loDb.SqlExecNonQuery(loConn, loCmd, true);
    
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            EndBlock:
            loException.ThrowExceptionIfErrors();
        }
    }
}
