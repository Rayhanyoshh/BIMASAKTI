using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM01000Common;
using GSM01000Common.DTOs;
using System.Data;
using System.Net.Http.Headers;
using System.Data.Common;
using System.Reflection.Metadata;
using GSM001000Back;
using System.Windows.Input;

namespace GSM01000Back
{
    public class GSM01300Cls : R_BusinessObject<GSM01300DTO>
    {
        protected override GSM01300DTO R_Display(GSM01300DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            GSM01300DTO loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_GS_GET_GOA_DETAIL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CGOA_CODE", System.Data.DbType.String, 50, poEntity.CGOA_CODE);


                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01300DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            EndBlock:
            loEx.ThrowExceptionIfErrors();

            return loRtn;
        }

        protected override void R_Saving(GSM01300DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            throw new NotImplementedException();
        }

        protected override void R_Deleting(GSM01300DTO poEntity)
        {
            throw new NotImplementedException();
        }

        public List<GSM01300DTO> GetGoAListDb(GOAHeadListDbParameter poEntity)
        {
            R_Exception loException = new R_Exception();
            List<GSM01300DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn = null;
            DbCommand loCmd;
            string lcQuery = null;
            
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                
                
                lcQuery = "RSP_GS_GET_GOA_LIST";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;
                
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", System.Data.DbType.String, 50, poEntity.CCOMPANY_ID);
                
                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);

                loRtn = R_Utility.R_ConvertTo<GSM01300DTO>(loDataTable).ToList();
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
