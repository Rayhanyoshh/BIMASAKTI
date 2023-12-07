using GSM01500COMMON.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data.Common;

namespace GSM01500BACK
{
    public class GSM01500Cls : R_BusinessObject<CreateUpdateDeleteParameterDTO>
    {
        public List<GSM01500DTO> GetCenterList(GetCenterListParameterDTO poEntity)
        {
            R_Exception loException = new R_Exception();
            List<GSM01500DTO> loResult = null;

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_GET_CENTER_LIST " +
                    $"'{poEntity.CCOMPANY_ID}', " +
                    $"'{poEntity.CUSER_ID}'";
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loResult = R_Utility.R_ConvertTo<GSM01500DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

            return loResult;
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
                                WHERE A.LPRIMARY_ACCOUNT = 1";
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

        public void CopyFromProcess(CopyFromProcessParameter poEntity)
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_COPY_CENTER " +
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

        public void RSP_GS_ACTIVE_INACTIVE_CENTERMethod(ActiveInactiveParameterDTO poEntity)
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_ACTIVE_INACTIVE_CENTER " +
                    $"'{poEntity.CCOMPANY_ID}', " +
                    $"'{poEntity.CCENTER_CODE}', " +
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

        private void RSP_GS_MAINTAIN_CENTERMethod(string pcCommand)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = loDb.GetConnection();

            try
            {
                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = pcCommand;

                R_ExternalException.R_SP_Init_Exception(loConn);

                try
                {
                    loDb.SqlExecNonQuery(loConn, loCmd, false);
                }
                catch (Exception ex)
                {
                    loException.Add(ex);
                }

                loException.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (loConn.State != System.Data.ConnectionState.Closed)
                        loConn.Close();

                    loConn.Dispose();
                    loConn = null;
                }
            }
            loException.ThrowExceptionIfErrors();

        }

        protected override void R_Deleting(CreateUpdateDeleteParameterDTO poEntity)
        {
            R_Exception loException = new R_Exception();

            try
            {
                string lcQuery = $"EXEC RSP_GS_MAINTAIN_CENTER " +
                    $"'{poEntity.CCOMPANY_ID}', " +
                    $"'{poEntity.Data.CCENTER_CODE}', " +
                    $"'{poEntity.Data.CCENTER_NAME}', " +
                    $"'{poEntity.Data.LACTIVE}', " +
                    $"'DELETE', " +
                    $"'{poEntity.CUSER_ID}'";

                RSP_GS_MAINTAIN_CENTERMethod(lcQuery);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }

        protected override CreateUpdateDeleteParameterDTO R_Display(CreateUpdateDeleteParameterDTO poEntity)
        {
            var loEx = new R_Exception();
            CreateUpdateDeleteParameterDTO loResult = new CreateUpdateDeleteParameterDTO();

            try
            {
                R_Db loDb = new R_Db();
                DbConnection loConn = loDb.GetConnection("R_DefaultConnectionString");

                string lcQuery = $"EXEC RSP_GS_GET_CENTER_DETAIL " +
                    $"'{poEntity.CCOMPANY_ID}', " +
                    $"'{poEntity.Data.CCENTER_CODE}', " +
                    $"'{poEntity.CUSER_ID}'";

                DbCommand loCmd = loDb.GetCommand();
                loCmd.CommandText = lcQuery;

                loResult.Data = loDb.SqlExecObjectQuery<GSM01500DTO>(lcQuery, loConn, true).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }

        protected override void R_Saving(CreateUpdateDeleteParameterDTO poNewEntity, eCRUDMode poCRUDMode)
        {
            R_Exception loException = new R_Exception();
            string Mode = "";

            try
            {
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    Mode = "ADD";
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    Mode = "EDIT";
                }

                string lcQuery = $"EXEC RSP_GS_MAINTAIN_CENTER " +
                    $"'{poNewEntity.CCOMPANY_ID}', " +
                    $"'{poNewEntity.Data.CCENTER_CODE}', " +
                    $"'{poNewEntity.Data.CCENTER_NAME}', " +
                    $"'{poNewEntity.Data.LACTIVE}', " +
                    $"'{Mode}', " +
                    $"'{poNewEntity.CUSER_ID}'";

                RSP_GS_MAINTAIN_CENTERMethod(lcQuery);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}
