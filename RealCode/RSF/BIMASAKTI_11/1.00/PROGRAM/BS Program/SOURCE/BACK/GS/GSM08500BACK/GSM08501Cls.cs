using R_BackEnd;
using R_Common;
using R_CommonFrontBackAPI;
using GSM08500Common.DTOs;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Transactions;
using GSM008500Common.DTOs;
namespace GSM08500Back;

public class GSM08501Cls  : R_IBatchProcess
{
    public void R_BatchProcess(R_BatchProcessPar poBatchProcessPar)
    {
        R_Exception loException = new R_Exception();
        var loDb = new R_Db();

        try
        {
            
            if (loDb.R_TestConnection() == false)
            {
                loException.Add("01", "Database Connection Failed");
                goto EndBlock;
            }

            var loTask = Task.Run(() =>
            {
                _BatchProcess(poBatchProcessPar);
            });

            while (!loTask.IsCompleted)
            {
                Thread.Sleep(100);
            }

            if (loTask.IsFaulted)
            {
                loException.Add(loTask.Exception.InnerException != null ?
                    loTask.Exception.InnerException :
                    loTask.Exception);

                goto EndBlock;
            }
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
          
        EndBlock:

        loException.ThrowExceptionIfErrors();
    }
    
    private async Task _BatchProcess(R_BatchProcessPar poBatchProcessPar)
        {
            R_Exception loException = new R_Exception();
            R_Db loDb = new R_Db();
            DbCommand loCmd = null;
            DbConnection loConn = null;
            var lcQuery = "";

            try
            {
                await Task.Delay(100);
                var loTempObject = R_NetCoreUtility.R_DeserializeObjectFromByte<List<GSM08501ErrorValidateDTO>>(poBatchProcessPar.BigObject);
                var loObject = R_Utility.R_ConvertCollectionToCollection<GSM08501ErrorValidateDTO, GSM08501RequestDTO>(loTempObject);
                
                var loVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.CCOMPANY_ID)).FirstOrDefault().Value;
                var lcCompanyId = ((System.Text.Json.JsonElement)loVar).GetString();

                var loTempVar = poBatchProcessPar.UserParameters.Where((x) => x.Key.Equals(ContextConstant.COVERWRITE)).FirstOrDefault().Value;
                var lbOverwrite = ((System.Text.Json.JsonElement)loTempVar).GetBoolean();

                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                lcQuery += "CREATE TABLE #STATISTIC_ACCOUNT(NO		INT" +
                           ", CCOMPANY_ID	VARCHAR(8)		" +
                           ", CGLACCOUNT_NO	VARCHAR(20)		" +
                           ", CGLACCOUNT_NAME	NVARCHAR(60)	" +
                           ", CBSIS		VARCHAR(2)		" +
                           ", CDBCR		VARCHAR(2)		" +
                           ", LACTIVE		BIT" +
                           ", LCENTER_RESTR	BIT" +
                           ", LUSER_RESTR	BIT" +
                           ", LEXIST		BIT" +
                           ", NonActiveDate	VARCHAR(8))";

                loDb.SqlExecNonQuery(lcQuery, loConn, false);

                loDb.R_BulkInsert<GSM08501RequestDTO>((SqlConnection)loConn, "#STATISTIC_ACCOUNT", loObject);

                lcQuery = "EXECUTE RSP_GS_UPLOAD_STATISTIC_ACCOUNT @CCOMPANY_ID, @CUSER_ID, @CKEY_GUID ";
                loCmd.CommandText = lcQuery;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, poBatchProcessPar.Key.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, poBatchProcessPar.Key.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CKEY_GUID", DbType.String, 50, poBatchProcessPar.Key.KEY_GUID);

                loDb.SqlExecNonQuery(loConn, loCmd, false);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            finally
            {
                if (loConn != null)
                {
                    if (!(loConn.State == ConnectionState.Closed))
                        loConn.Close();
                    loConn.Dispose();
                    loConn = null;
                }

                if (loCmd != null)
                {
                    loCmd.Dispose();
                    loCmd = null;
                }
            }

            if (loException.Haserror)
            {
                lcQuery = $"INSERT INTO GST_UPLOAD_ERROR_STATUS(CCOMPANY_ID,CUSER_ID,CKEY_GUID,ISEQ_NO,CERROR_MESSAGE) VALUES" +
                          $"('{poBatchProcessPar.Key.COMPANY_ID}', '{poBatchProcessPar.Key.USER_ID}', " +
                          $"'{poBatchProcessPar.Key.KEY_GUID}', -1, '{loException.ErrorList[0].ErrDescp}')";
                loDb.SqlExecNonQuery(lcQuery);

                lcQuery = $"EXEC RSP_WriteUploadProcessStatus '{poBatchProcessPar.Key.COMPANY_ID}', " +
                          $"'{poBatchProcessPar.Key.USER_ID}', " +
                          $"'{poBatchProcessPar.Key.KEY_GUID}', " +
                          $"100, '{loException.ErrorList[0].ErrDescp}', 9";

                loDb.SqlExecNonQuery(lcQuery);
            }

        }
}