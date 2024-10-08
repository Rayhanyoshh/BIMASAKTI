﻿using CBT01200Back;
using CBT01200Common.Loggers;
using CBT01200Common.DTOs;
using CBT01200Common;
using R_BackEnd;
using R_Common;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;
using System.Transactions;
using CBT01200Common.DTOs;
using R_CommonFrontBackAPI;

namespace CBT01200Back
{
    public class CBT01200Cls  : R_BusinessObject<CBT01200DTO>
    {
        private RSP_CB_DELETE_TRANS_JRNResources.Resources_Dummy_Class loDeleteCBTransJRNRes = new();
        private RSP_CB_SAVE_TRANS_JRNResources.Resources_Dummy_Class loSaveCBTransJRNRes = new();
        private RSP_CB_SAVE_CA_WT_JOURNALResources.Resources_Dummy_Class loSaveCAWTJRNRes = new();
        private RSP_CB_SAVE_SYSTEM_PARAMResources.Resources_Dummy_Class loSaveSystemCBTransJRNRes = new();
        private RSP_CB_UPDATE_TRANS_HD_STATUSResources.Resources_Dummy_Class loUpdateCBTransHDStatusRes
            = new();

        private LoggerCBT01200 _logger;

        private readonly ActivitySource _activitySource;

        public CBT01200Cls()
        {
            _logger = LoggerCBT01200.R_GetInstanceLogger();
            _activitySource = CBT01200Activity.R_GetInstanceActivitySource();
        }
       
        #region log activity
        private void ShowLogDebug(string query, DbParameterCollection parameters)
        {
            var paramValues = string.Join(", ", parameters.Cast<DbParameter>().Select(p => $"{p.ParameterName} '{p.Value}'"));
            _logger.LogDebug($"EXEC {query} {paramValues}");
        }

        private void ShowLogError(Exception ex)
        {
            _logger.LogError(ex);
        }
        #endregion
        
        public List<CBT01200DTO> GetJournalList(CBT01200JournalHDParam poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            List<CBT01200DTO> loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = "RSP_CB_SEARCH_TRANS_HD_LIST";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 50, R_BackGlobalVar.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, 50, ContextConstant.VAR_TRANS_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, 50, poEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CPERIOD", DbType.String, 50, poEntity.CPERIOD);
                loDb.R_AddCommandParameter(loCmd, "@CSTATUS", DbType.String, 50, poEntity.CSTATUS);
                loDb.R_AddCommandParameter(loCmd, "@CSEARCH_TEXT", DbType.String, 50, poEntity.CSEARCH_TEXT);
                loDb.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 50, R_BackGlobalVar.CULTURE);

                //Debug Logs
                ShowLogDebug(lcQuery, loCmd.Parameters);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<CBT01200DTO>(loDataTable).ToList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        
        protected override CBT01200DTO R_Display(CBT01200DTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01200DTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = "RSP_CB_GET_TRANS_HD";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 20, poEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 100, poEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, 100, poEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CLANGUAGE_ID", DbType.String, 3, poEntity.CLANGUAGE_ID);


                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<CBT01200DTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
    
            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        
        public void UpdateJournalStatus(CBT01200UpdateStatusDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            R_Exception loEx = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = null;
            DbCommand loCmd = null;
            string lcQuery;

            try
            {
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_CB_UPDATE_TRANS_HD_STATUS";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, R_BackGlobalVar.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CAPPROVE_BY", DbType.String, 20, R_BackGlobalVar.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID_LIST", DbType.String, int.MaxValue, poEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CNEW_STATUS", DbType.String, 20, poEntity.CNEW_STATUS);
                loDb.R_AddCommandParameter(loCmd, "@LAUTO_COMMIT", DbType.Boolean, 20, poEntity.LAUTO_COMMIT);
                loDb.R_AddCommandParameter(loCmd, "@LUNDO_COMMIT", DbType.Boolean, 20, poEntity.LUNDO_COMMIT);

                R_ExternalException.R_SP_Init_Exception(loConn);

                try
                {
                    ShowLogDebug(lcQuery, loCmd.Parameters);
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
                loEx.Add(ex); ShowLogError(loEx);
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
                if (loCmd != null)
                {
                    loCmd.Dispose();
                    loCmd = null;
                }
                if (loDb != null)
                {
                    loDb = null;
                }
            }
            loEx.ThrowExceptionIfErrors();
        }
        
        public CBT01210LastCurrencyRateDTO GetLastCurrency(CBT01210LastCurrencyRateDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            var loEx = new R_Exception();
            CBT01210LastCurrencyRateDTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection("R_DefaultConnectionString");
                var loCmd = loDb.GetCommand();

                var lcQuery = @"RSP_GS_GET_LAST_CURRENCY_RATE";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, 50, string.IsNullOrWhiteSpace(poEntity.CCURRENCY_CODE) ? "" : poEntity.CCURRENCY_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CRATETYPE_CODE", DbType.String, 50, poEntity.CRATETYPE_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CRATE_DATE", DbType.String, 50, string.IsNullOrWhiteSpace(poEntity.CRATE_DATE) ? "" : poEntity.CRATE_DATE);

                //Debug Logs
                ShowLogDebug(lcQuery, loCmd.Parameters);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<CBT01210LastCurrencyRateDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                ShowLogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        
        public CBT01200DTO SaveJournal(CBT01200DTO poNewEntity, eCRUDMode poCRUDMode, ePARAM_CALLER poPARAMCALLER)
        {
            using Activity activity = _activitySource.StartActivity("SaveJournal");
            var loEx = new R_Exception();
            CBT01200DTO loRtn = null;

            try
            {
                using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required))
                {
                    if (poPARAMCALLER == ePARAM_CALLER.TRANSACTION)
                    {
                        R_Saving(poNewEntity, poCRUDMode);
                    }
                    else
                    {
                        R_Saving_Deposit(poNewEntity, poCRUDMode);
                    }

                    transactionScope.Complete();
                }

                loRtn = R_Display(poNewEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }

        
        protected override void R_Saving(CBT01200DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            using Activity activity = _activitySource.StartActivity("R_Saving");
            var loEx = new R_Exception();
            try
            {
                
                if (poNewEntity.SaveParam.PARAM_CALLER == ePARAM_CALLER.TRANSACTION)
                {
                    R_SavingSP(poNewEntity, poCRUDMode);
                }
                else
                {
                    R_Saving_Deposit(poNewEntity, poCRUDMode);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        private void R_SavingSP (CBT01200DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            using Activity activity = _activitySource.StartActivity("R_Saving");

            R_Exception loEx = new R_Exception();
            string lcQuery = null;
            R_Db loDb;
            DbCommand loCmd;
            DbConnection loConn = null;

            try
            {
                loDb = new R_Db();
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);

                // set action 
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    poNewEntity.CACTION = "NEW";
                    poNewEntity.CREC_ID = "";
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    poNewEntity.CACTION = "EDIT";
                }

                lcQuery = "RSP_CB_SAVE_CA_WT_JOURNAL";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                // Log the query and parameter values
                _logger.LogDebug("QueryExecuting stored procedure: {lcQuery}", lcQuery);

                // Add command parameters
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poNewEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, int.MaxValue, poNewEntity.CACTION);
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, int.MaxValue, poNewEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, int.MaxValue, poNewEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, int.MaxValue, poNewEntity.CTRANS_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CPAYMENT_TYPE", DbType.String, int.MaxValue, poNewEntity.CPAYMENT_TYPE);
                loDb.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, int.MaxValue, poNewEntity.CREF_NO);
                loDb.R_AddCommandParameter(loCmd, "@CREF_DATE", DbType.String, int.MaxValue, poNewEntity.CREF_DATE);
                loDb.R_AddCommandParameter(loCmd, "@CDOC_NO", DbType.String, int.MaxValue, poNewEntity.CDOC_NO);
                loDb.R_AddCommandParameter(loCmd, "@CDOC_DATE", DbType.String, int.MaxValue, poNewEntity.CDOC_DATE);
                loDb.R_AddCommandParameter(loCmd, "@CCB_CODE", DbType.String, int.MaxValue, poNewEntity.CCB_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CCB_ACCOUNT_NO", DbType.String, int.MaxValue, poNewEntity.CCB_ACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CTRANS_DESC", DbType.String, int.MaxValue, poNewEntity.CTRANS_DESC);
                loDb.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, int.MaxValue, poNewEntity.CCURRENCY_CODE);
                loDb.R_AddCommandParameter(loCmd, "@NTRANS_AMOUNT", DbType.Decimal, int.MaxValue, poNewEntity.NTRANS_AMOUNT);
                loDb.R_AddCommandParameter(loCmd, "@NLBASE_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NLBASE_RATE);
                loDb.R_AddCommandParameter(loCmd, "@NLCURRENCY_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NLCURRENCY_RATE);
                loDb.R_AddCommandParameter(loCmd, "@NBBASE_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NBBASE_RATE);
                loDb.R_AddCommandParameter(loCmd, "@NBCURRENCY_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NBCURRENCY_RATE);
              
                try
                {
                    // loDb.SqlExecNonQuery(loConn, loCmd, false);
                    var loDataTable = loDb.SqlExecQuery(loConn, loCmd, false);
                    var loTempResult = R_Utility.R_ConvertTo<CBT01200JournalHDParam>(loDataTable).FirstOrDefault();
                    poNewEntity.CREC_ID = loTempResult.CREC_ID;
                    
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                    loEx.Add(ex);
                }

                loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred in the outer catch block.");
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
        private void R_Saving_Deposit(CBT01200DTO poNewEntity, eCRUDMode poCRUDMode)
        {
            using Activity activity = _activitySource.StartActivity("R_Saving_Deposit");

            R_Exception loEx = new R_Exception();
            R_Db loDb = null;
            DbConnection loConn = null;
            DbCommand loCmd = null;
            string lcQuery;

            try
            {
                // set action 
                // set action 
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    poNewEntity.CACTION = "NEW";
                    poNewEntity.CREC_ID = "";
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    poNewEntity.CACTION = "EDIT";
                }
                
                loDb = new R_Db();
                loConn = loDb.GetConnection("R_DefaultConnectionString");
                loCmd = loDb.GetCommand();
                R_ExternalException.R_SP_Init_Exception(loConn);


                lcQuery = "RSP_CB_SAVE_CA_WT_JOURNAL_DEPOSIT";
                loCmd.CommandType = CommandType.StoredProcedure;
                loCmd.CommandText = lcQuery;

                // Log the query and parameter values
                _logger.LogDebug("QueryExecuting stored procedure: {lcQuery}", lcQuery);

                // Add command parameters
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, int.MaxValue, poNewEntity.CUSER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CACTION", DbType.String, int.MaxValue, poNewEntity.CACTION);
                loDb.R_AddCommandParameter(loCmd, "@CREC_ID", DbType.String, int.MaxValue, poNewEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, int.MaxValue, poNewEntity.CCOMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CDEPT_CODE", DbType.String, int.MaxValue, poNewEntity.CDEPT_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CTRANS_CODE", DbType.String, int.MaxValue, poNewEntity.CTRANS_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CPAYMENT_TYPE", DbType.String, int.MaxValue, poNewEntity.CPAYMENT_TYPE);
                
                loDb.R_AddCommandParameter(loCmd, "@CREF_NO", DbType.String, int.MaxValue, poNewEntity.CREF_NO);
                loDb.R_AddCommandParameter(loCmd, "@CREF_DATE", DbType.String, int.MaxValue, poNewEntity.CREF_DATE);
                loDb.R_AddCommandParameter(loCmd, "@CDOC_NO", DbType.String, int.MaxValue, poNewEntity.CDOC_NO);
                loDb.R_AddCommandParameter(loCmd, "@CDOC_DATE", DbType.String, int.MaxValue, poNewEntity.CDOC_DATE);
                
                loDb.R_AddCommandParameter(loCmd, "@CCB_CODE", DbType.String, int.MaxValue, poNewEntity.CCB_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CCB_ACCOUNT_NO", DbType.String, int.MaxValue, poNewEntity.CCB_ACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CTRANS_DESC", DbType.String, int.MaxValue, poNewEntity.CTRANS_DESC);
                loDb.R_AddCommandParameter(loCmd, "@CCURRENCY_CODE", DbType.String, int.MaxValue, poNewEntity.CCURRENCY_CODE);
                loDb.R_AddCommandParameter(loCmd, "@NTRANS_AMOUNT", DbType.Decimal, int.MaxValue, poNewEntity.NTRANS_AMOUNT);
                loDb.R_AddCommandParameter(loCmd, "@NLBASE_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NLBASE_RATE);
                loDb.R_AddCommandParameter(loCmd, "@NLCURRENCY_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NLCURRENCY_RATE);
                loDb.R_AddCommandParameter(loCmd, "@NBBASE_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NBBASE_RATE);
                loDb.R_AddCommandParameter(loCmd, "@NBCURRENCY_RATE", DbType.Decimal, int.MaxValue, poNewEntity.NBCURRENCY_RATE);
              
                loDb.R_AddCommandParameter(loCmd, "@CSTRANS_CODE", DbType.String, 10, poNewEntity.PARAM_CALLER_TRANS_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CSREF_NO", DbType.String, 30, poNewEntity.PARAM_CALLER_REF_NO);
                loDb.R_AddCommandParameter(loCmd, "@CSMODULE", DbType.String, 10, poNewEntity.PARAM_CALLER_ID.Substring(0, 2));
                loDb.R_AddCommandParameter(loCmd, "@CDP_GLACCOUNT_NO", DbType.String, 20, poNewEntity.PARAM_GLACCOUNT_NO);
                loDb.R_AddCommandParameter(loCmd, "@CDP_CENTER_CODE", DbType.String, 8, poNewEntity.PARAM_CENTER_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CDP_CASH_FLOW_GROUP_CODE", DbType.String, 20, poNewEntity.PARAM_CASH_FLOW_GROUP_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CDP_CASH_FLOW_CODE", DbType.String, 20, poNewEntity.PARAM_CASH_FLOW_CODE);

                
                try
                {
                    //Debug Logs
                    //Debug Logs
                    var loDbParam = loCmd.Parameters.Cast<DbParameter>()
                        .Where(x => x != null && x.ParameterName.StartsWith("@")).Select(x => x.Value);
                    _logger.LogDebug("EXEC RSP_CB_SAVE_CA_WT_JOURNAL_DEPOSIT {@poParameter}", loDbParam);

                    var loDataTable = loDb.SqlExecQuery(loConn, loCmd, false);

                    var loTempResult = R_Utility.R_ConvertTo<CBT01200DTO>(loDataTable).FirstOrDefault();
                    poNewEntity.CREC_ID = loTempResult.CREC_ID;
                    
                }
                catch (Exception ex)
                {
                    // Log the exception
                    _logger.LogError(ex, "An error occurred while executing the stored procedure.");
                    loEx.Add(ex);
                }

                loEx.Add(R_ExternalException.R_SP_Get_Exception(loConn));
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An error occurred in the outer catch block.");
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
        protected override void R_Deleting(CBT01200DTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity(MethodBase.GetCurrentMethod().Name);
            R_Exception loEx = new R_Exception();
            R_Db loDb = new R_Db();
            DbConnection loConn = null;
            DbCommand loCmd = null;
            string lcQuery;

            try
            {
                loConn = loDb.GetConnection();
                loCmd = loDb.GetCommand();

                lcQuery = "RSP_CB_UPDATE_TRANS_HD_STATUS";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.StoredProcedure;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 8, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CUSER_ID", DbType.String, 20, R_BackGlobalVar.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CAPPROVE_BY", DbType.String, 20, R_BackGlobalVar.USER_ID);
                loDb.R_AddCommandParameter(loCmd, "@CJRN_ID_LIST", DbType.String, int.MaxValue, poEntity.CREC_ID);
                loDb.R_AddCommandParameter(loCmd, "@CNEW_STATUS", DbType.String, 20,"99");
                loDb.R_AddCommandParameter(loCmd, "@LAUTO_COMMIT", DbType.Boolean, 20, false);
                loDb.R_AddCommandParameter(loCmd, "@LUNDO_COMMIT", DbType.Boolean, 20, false);

                R_ExternalException.R_SP_Init_Exception(loConn);

                try
                {
                    ShowLogDebug(lcQuery, loCmd.Parameters);
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
                loEx.Add(ex); ShowLogError(loEx);
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
                if (loCmd != null)
                {
                    loCmd.Dispose();
                    loCmd = null;
                }
                if (loDb != null)
                {
                    loDb = null;
                }
            }
            loEx.ThrowExceptionIfErrors();
        }
        
        public CBT01200ValidateUpdateStatusDTO GetValidateUpdateJournalStatus(CBT01200ValidateUpdateStatusDTO poEntity)
        {
            using Activity activity = _activitySource.StartActivity("GetValidateUpdateJournalStatus");
            var loEx = new R_Exception();
            CBT01200ValidateUpdateStatusDTO loResult = null;

            try
            {
                var loDb = new R_Db();
                var loConn = loDb.GetConnection();
                var loCmd = loDb.GetCommand();

                var lcQuery = "SELECT dbo.RFN_CB_GET_ACCOUNT_BALANCE(@CCOMPANY_ID, @CCB_CODE, @CCB_ACCOUNT_NO, 1) AS NVALUE";
                loCmd.CommandText = lcQuery;
                loCmd.CommandType = CommandType.Text;

                loDb.R_AddCommandParameter(loCmd, "@CCOMPANY_ID", DbType.String, 50, R_BackGlobalVar.COMPANY_ID);
                loDb.R_AddCommandParameter(loCmd, "@CCB_CODE", DbType.String, 30, poEntity.CCB_CODE);
                loDb.R_AddCommandParameter(loCmd, "@CCB_ACCOUNT_NO", DbType.String, 30, poEntity.CCB_ACCOUNT_NO);

                //Debug Logs
                var loDbParam = loCmd.Parameters.Cast<DbParameter>()
                    .Where(x => x != null && x.ParameterName.StartsWith("@")).Select(x => x.Value);
                _logger.LogDebug(lcQuery, loDbParam);

                var loDataTable = loDb.SqlExecQuery(loConn, loCmd, true);
                loResult = R_Utility.R_ConvertTo<CBT01200ValidateUpdateStatusDTO>(loDataTable).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
                _logger.LogError(loEx);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
    }
}
