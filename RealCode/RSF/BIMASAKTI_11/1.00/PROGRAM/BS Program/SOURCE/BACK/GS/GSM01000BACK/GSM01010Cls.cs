using GSM001000Back;
using GSM01000Common.DTOs;
using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using GSM01000Common;

namespace GSM01000Back
{
    public class GSM01010Cls : R_BusinessObject<GSM01010DTO>
    {
        private LoggerGSM01000 _logger;
        private readonly ActivitySource _activitySource;


        public GSM01010Cls()
        {
            _logger = LoggerGSM01000.R_GetInstanceLogger();
            _activitySource = GSM01000Activity.R_GetInstanceActivitySource();
        }
        
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
        
        public List<GSM01010DTO> GetGoAListByGlAccount(GOAHeadListDbParameter poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetGoAListByGlAccount");

            var loEx = new R_Exception();
            List<GSM01010DTO> loRtn = null;
            R_Db loDb;
            DbConnection loConn;
            DbCommand loCmd;
            string lcQuery;
            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
                lcQuery =
                    $"EXEC RSP_GS_GET_GOA_BY_COA_LIST" +
                    $"'{poEntity.CCOMPANY_ID}', " +
                    $"'{poEntity.CGLACCOUNT_NO}', " +
                    $"'{poEntity.CUSER_LOGIN_ID}' ";

                loCmd.CommandType = CommandType.Text;
                loCmd.CommandText = lcQuery;

                // Log the query using LogDebug
                _logger.LogDebug("Executing query: {lcQuery}", lcQuery);

                // Log each parameter
                _logger.LogDebug("Parameters:");
                _logger.LogDebug("CCOMPANY_ID = {CCOMPANY_ID}", poEntity.CCOMPANY_ID);
                _logger.LogDebug("CGLACCOUNT_NO = {CGLACCOUNT_NO}", poEntity.CGLACCOUNT_NO);
                _logger.LogDebug("CUSER_LOGIN_ID = {CUSER_LOGIN_ID}", poEntity.CUSER_LOGIN_ID);

                loRtn = loDb.SqlExecObjectQuery<GSM01010DTO>(lcQuery, loConn, true);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred while executing the query.");
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

    }
}
