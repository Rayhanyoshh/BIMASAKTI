using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM07500Common.DTOs;
using System.Data;
using System.Data.Common;

namespace GSM07500Back
{
    public class GSM07510Cls : R_BusinessObject<GSM07510DTO>
    {
        protected override GSM07510DTO R_Display(GSM07510DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM07510DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "SELECT * FROM GSM_PERIOD A (NOLOCK) " +
                          "WHERE A.CCOMPANY_ID = @CCOMPANY_ID AND A.CYEAR = @CYEAR ";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CYEAR", System.Data.DbType.String, 4, poEntity.CYEAR);
                
                // loRtn = loDb.SqlExecObjectQuery<GSM02000DTO>(lcQuery, loConn, true, poEntity).FirstOrDefault();
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM07510DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        protected override void R_Saving(GSM07510DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GSM07510DTO poEntity)
        {
            throw new NotImplementedException();
        }
        
        public List<GSM07510DTO> GetPeriodDbList (GSM07510DTO poEntity)
        {
            R_Exception loException = new R_Exception();
            List<GSM07510DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                
                lcQuery = "SELECT * FROM GSM_PERIOD A (NOLOCK) WHERE A.CCOMPANY_ID = @CCOMPANY_ID ORDER BY CYEAR ASC";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM07510DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();

            return loRtn;
        }
    }
}
