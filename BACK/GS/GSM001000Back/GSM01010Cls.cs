using GSM001000Back;
using GSM01000Common.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;

namespace GSM01000Back
{
    public class GSM01010Cls : R_BusinessObject<GSM01010DTO>
    {
        
        protected override GSM01010DTO R_Display(GSM01010DTO poEntity)
        {
            throw new NotImplementedException();
        }

        protected override void R_Saving(GSM01010DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GSM01010DTO poEntity)
        {
            throw new NotImplementedException();
        }
        
        public List<GSM01010DTO> GetGoAListByGlAccount (GOAHeadListDbParameter poParameter)
        {
            var loEx = new R_Exception();
            List<GSM01010DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                lcQuery =
                    $"SELECT A.CGOA_CODE, B.CGOA_NAME,  A.CUPDATE_BY, A.DUPDATE_DATE FROM GSM_GOA_COA A (NOLOCK) " +
                    $"INNER JOIN GSM_GOA B ON A.CCOMPANY_ID = B.CCOMPANY_ID AND A.CGOA_CODE = B.CGOA_CODE " +
                    $"WHERE A.CCOMPANY_ID = '{poParameter.CCOMPANY_ID}' " +
                    $"AND A.CGLACCOUNT_NO = '{poParameter.CGLACCOUNT_NO}' ";
                        

                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                // loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 20, poParameter.CCOMPANY_ID);
                // loDb.R_AddCommandParameter(loCmd, "@CGLACCOUNT_NO", System.Data.DbType.String, 100, poParameter.CGLACCOUNT_NO);

                loRtn = loDb.SqlExecObjectQuery<GSM01010DTO>(lcQuery, loConn,true);
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
    }
}
