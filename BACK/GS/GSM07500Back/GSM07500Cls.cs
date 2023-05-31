using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM07500Common.DTOs;
using System.Data;
using System.Data.Common;

namespace GSM07500Back
{
    public class GSM07500Cls: R_BusinessObject<GSM07500DTO>
    {
        protected override GSM07500DTO R_Display(GSM07500DTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(GSM07500DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GSM07500DTO poEntity)
        {
            throw new NotImplementedException();
        }
        
        public List<GSM07500DTO> GetPeriodDetailDbList (GSM07500DTO poEntity)
        {
            R_Exception loException = new R_Exception();
            List<GSM07500DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            try
            {
                
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                
                lcQuery = $"SELECT * FROM GSM_PERIOD_DT A (NOLOCK) " +
                          $"WHERE A.CCOMPANY_ID = '{poEntity.@CCOMPANY_ID}' " +
                          $"AND A.CCYEAR = '{poEntity.CCYEAR}' ";
                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;
                //
                // loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                // loDb.R_AddCommandParameter(loCmd, "@CCYEAR", System.Data.DbType.String, 50, poEntity.CCYEAR);


                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM07500DTO>(loDataTable).ToList();
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
