using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GLT00600Common;
using R_BlazorFrontEnd.Helpers;
using GLT00600Common.DTOs;
using R_APICommonDTO;

namespace GLT00600Model.ViewModel
{
    public class GLT00600ViewModel : R_ViewModel<GLT00600DTO>
    {
        private GLT00600Model _JournalListModel = new GLT00600Model();
        public ObservableCollection<GLT00600JournalGridDTO> JournalList = new();
        public GLT00600JournalGridDTO JournalEntity = new();
        public GLT00600ParameterDTO Parameter = new();
        public List<GLT00600JournalGridDTO> loGetApprovalList = new List<GLT00600JournalGridDTO>();

        public Action StateChangeAction { get; set; }
        public Action ShowSuccessAction { get; set; }

        public ObservableCollection<GLT00600JournalGridDetailDTO> JournaDetailList { get; set; } = new();
        public ObservableCollection<GLT00600JournalGridDetailDTO> JournaDetailListTemp { get; set; } = new();
        public GLT00600DTO Journal = new();

        #region Collection
        public VAR_CCURRENT_PERIOD_START_DATEDTO CurrentPeriodStartCollection = new();
        public VAR_CSOFT_PERIOD_START_DATEDTO SoftPeriodStartCollection = new();
        public VAR_GL_SYSTEM_PARAMDTO SystemParamCollection = new();
        public VAR_GSM_COMPANYDTO CompanyCollection = new();
        public VAR_GSM_PERIODDTO GSMPeriodCollection = new();
        public VAR_GSM_TRANSACTION_CODEDTO TransactionCodeCollection = new();
        public GSM_TRANSACTION_APPROVALDTO TransactionApprovalCollection = new();
        public VAR_IUNDO_COMMIT_JRNDTO IundoCollection = new();
        public List<StatusDTO> allStatusData = new();
        public List<CurrencyCodeDTO> currencyData = new();
        public List<GetCenterDTO> CenterListData { get; set; } = new();
        public List<GLT00600JournalGridDTO> loProcessRapidApproveOrCommitList = new();
        public List<GetMonthDTO> GetMonthList { get; set; }
        public List<VAR_USER_DEPARTMENTDTO> AllDeptData = new();
        public Dictionary<string, string> statusMappings = new Dictionary<string, string>
        {
            [""] = "All",
            ["00"] = "Draft",
            ["10"] = "Submitted",
            ["20"] = "Approved",
            ["80"] = "Commited",
            ["99"] = "Deleted"
        };
        #endregion
        #region property
        public string LcCrecID = "";
        public string lcSearch = "";
        public string lcPeriod = "";
        public string lcStatus = "";
        public string lcDeptCode = "";
        public string COMPANYID;
        public string USERID;
        public string CCURRENT_PERIOD_YY = "";
        public string CCURRENT_PERIOD_MM = "";
        public string CSOFT_PERIOD_YY = "";
        public string CSOFT_PERIOD_MM = "";
        public string ProgressBarMessage = "";
        public int ProgressBarPercentage = 0;
        public string ResultProcessList = null;
        public string ResultFailedProcessList = null;

        public DateTime Drefdate = DateTime.Now;
        public DateTime Ddocdate = DateTime.Now;
        public DateTime Drevdate = DateTime.Now;
        //public bool EnableDept = false;
        public bool EnableDept = false;
        public bool buttonRapidApprove = true;
        public bool buttonRapidCommit = true;
        public bool EnableButton = false;
        public bool PredefineJournalList = false;
        public bool EnableDelete = false;
        public bool EnableSubmit = false;
        public bool EnableApprove = false;
        public bool EnableCommit = false;
        public bool EnablePrint = false;
        public bool EnableCopy =false;
        public string CSTATUS_TEMP { get; set; }
        public string CommitLabel = "Commit";
        public string SubmitLabel = "Submit";
        public string lcRapidOrdCommit = "";
        #endregion
        
             #region initial process
        public async Task GetCurrentPeriodStart(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            var loEx = new R_Exception();

            try
            {
                poData.CCURRENT_PERIOD_MM = CCURRENT_PERIOD_MM;
                poData.CCURRENT_PERIOD_YY = CCURRENT_PERIOD_YY;
                var loReturn = await _JournalListModel.GetCurrentPeriodStartDateAsync(poData);
                CurrentPeriodStartCollection = loReturn;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetSoftPeriodStart(VAR_GL_SYSTEM_PARAMDTO poData)
        {
            var loEx = new R_Exception();

            try
            {
                poData.CSOFT_PERIOD_MM = CSOFT_PERIOD_MM;
                poData.CSOFT_PERIOD_YY = CSOFT_PERIOD_YY;
                var loReturn = await _JournalListModel.GetSoftPeriodStartDateAsync(poData);
                SoftPeriodStartCollection = loReturn;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetSystemParam()
        {
            var loEx = new R_Exception();

            try
            {
                var loreturn = await _JournalListModel.GetSystemParamAsync();
                SystemParamCollection = loreturn;
                SystemParamCollection.ISOFT_PERIOD_YY = Convert.ToInt32(SystemParamCollection.CSOFT_PERIOD_YY);
                CCURRENT_PERIOD_MM = SystemParamCollection.CCURRENT_PERIOD_MM;
                CCURRENT_PERIOD_YY = SystemParamCollection.CCURRENT_PERIOD_YY;
                CSOFT_PERIOD_MM = SystemParamCollection.CSOFT_PERIOD_MM;
                CSOFT_PERIOD_YY = SystemParamCollection.CSOFT_PERIOD_YY;
                Data.CSOFT_PERIOD_MM = CSOFT_PERIOD_MM;
                Data.ISOFT_PERIOD_YY = SystemParamCollection.ISOFT_PERIOD_YY;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetGsmCompany()
        {
            var loEx = new R_Exception();

            try
            {
                var loReturn = await _JournalListModel.GetCompanyDTOAsync();
                CompanyCollection = loReturn;
                Journal.CCURRENCY_CODE = CompanyCollection.CLOCAL_CURRENCY_CODE;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetGSMPeriod()
        {
            var loEx = new R_Exception();

            try
            {
                var loReturn = await _JournalListModel.GetPeriodAsync();
                GSMPeriodCollection = loReturn;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetTransactionCode()
        {
            var loEx = new R_Exception();

            try
            {
                var loReturn = await _JournalListModel.GetLincementLapprovalAsync();
                TransactionCodeCollection = loReturn;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetIundo()
        {
            var loEx = new R_Exception();

            try
            {
                var loReturn = await _JournalListModel.GetIOptionAsync();
                IundoCollection = loReturn;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }
        public async Task GetStatusList()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loReturn = await _JournalListModel.GetStatusListAsync();
                allStatusData = loReturn.Data;

                allStatusData.Insert(0, new StatusDTO() { CCODE = "", CNAME = "All" });

                if (allStatusData.Count > 0)
                {
                    Data.CSTATUS = allStatusData[0].CCODE;
                }

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }
        public async Task GetDepartmentList()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loReturn = await _JournalListModel.GetDeptListAsync();
                AllDeptData = loReturn.Data;
                Data.CDEPT_CODE = AllDeptData.Select(m => m.CDEPT_CODE).FirstOrDefault();
                Data.CDEPT_NAME = AllDeptData.Select(m => m.CDEPT_NAME).FirstOrDefault();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }
        public async Task GetCurrencyList()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loReturn = await _JournalListModel.GetCurrencyCodeListAsync();
                currencyData = loReturn.Data;

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }
        public async Task GetCenterList()
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loReturn = await _JournalListModel.GetCenterListAsync();
                CenterListData = loReturn.Data;

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
        }
        public async Task GetTransactionApproval()
        {
            var loEx = new R_Exception();

            try
            {
                var loReturn = await _JournalListModel.GetTransactionApprovalAsync();
                TransactionApprovalCollection = loReturn;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion
        
        
        public async Task GetJournal(GLT00600DTO loParam)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _JournalListModel.R_ServiceGetRecordAsync(loParam);
                Journal = loResult;

                Journal.DREF_DATE = DateTime.ParseExact(Journal.CREF_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                Journal.DDOC_DATE = DateTime.ParseExact(Journal.CDOC_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                if (!string.IsNullOrWhiteSpace(Journal.CREVERSE_DATE))
                {
                    // Membersihkan spasi di sekitar nilai tanggal sebelum parsing
                    string cleanedDate = Journal.CREVERSE_DATE.Trim();

                    if (DateTime.TryParseExact(cleanedDate, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime reverseDate))
                    {
                        Journal.DREVERSE_DATE = reverseDate;
                    }
                    else
                    {
                        Journal.DREVERSE_DATE = DateTime.MinValue;
                    }
                }

                Journal.CUPDATE_DATE = Journal.DUPDATE_DATE.ToLongDateString();
                Journal.CCREATE_DATE = Journal.DCREATE_DATE.ToLongDateString();
                LcCrecID = Journal.CREC_ID;
                Data.CSTATUS = Journal.CSTATUS;
                await GetJournalDetailList();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveJournal(GLT00600DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();
            try
            {
                poNewEntity.CTRANS_CODE = "000088";
                poNewEntity.CREVERSE_DATE = "";
                poNewEntity.LREVERSE = false;
                var loResult = await _JournalListModel.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task ShowAllJournals()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                lcPeriod = Data.ISOFT_PERIOD_YY + "88";
                lcSearch = Data.CSEARCH_TEXT ?? "";
                lcDeptCode = Data.CDEPT_CODE;
                lcStatus = Data.CSTATUS ?? "";

                var loResult = await _JournalListModel.GetJournalListAsync(lcPeriod, lcSearch, lcDeptCode, lcStatus);
                JournalList = new ObservableCollection<GLT00600JournalGridDTO>(loResult.Data);
                foreach (var item in JournalList)
                {
                    DateTime parsedCrefDate;
                    if (DateTime.TryParseExact(item.CREF_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedCrefDate))
                    {
                        item.DREF_DATE = parsedCrefDate;
                    }
                    DateTime parsedReverseDate;
                    if (DateTime.TryParseExact(item.CREVERSE_DATE, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedReverseDate))
                    {
                        item.DREVERSE_DATE = parsedReverseDate;
                    }
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetJournalDetailList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _JournalListModel.GetAllJournalDetailListAsync(LcCrecID);

                if (loResult != null)
                {
                    var modifiedList = loResult.Data.Select((m, Index) =>
                    {
                        m.INO = Index + 1;
                        m.NAMOUNT = m.NDEBIT + m.NCREDIT;
                        if (m.CCENTER_CODE == null)
                        {
                            foreach (var item in CenterListData)
                            {
                                if (m.CCENTER_NAME == item.CCENTER_NAME)
                                {
                                    m.CCENTER_CODE = item.CCENTER_CODE;
                                }
                            }
                        }
                        return m;
                    }).ToList();
                    
                    JournaDetailList = new ObservableCollection<GLT00600JournalGridDetailDTO>(modifiedList);
                }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        
          #region ProcessStatus
        public async Task ProcessCommitJournal(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                await _JournalListModel.ProcessReversingJournalAsync(poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task ApproveJournal(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                CSTATUS_TEMP = JournalEntity.CSTATUS;
                poData.CSTATUS = "20";
                poData.LCOMMIT_APRJRN = SystemParamCollection.LCOMMIT_APRJRN; 
                poData.CREC_ID = JournalEntity.CREC_ID;
                await _JournalListModel.JournalProcessAsync(poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task CommitJournal(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                CSTATUS_TEMP = Journal.CSTATUS;
                poData.CREC_ID = JournalEntity.CREC_ID;
                if (Journal.CSTATUS == "80")
                {
                    poData.CSTATUS = "20";
                    poData.LUNDO_COMMIT = true;
                }
                else
                {
                    poData.CSTATUS = "80";
                    poData.LUNDO_COMMIT = false;
                }
                await _JournalListModel.JournalProcessAsync(poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task SubmitJournal(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                CSTATUS_TEMP = Journal.CSTATUS;
                if (Journal.CSTATUS == "00")
                {
                    poData.CSTATUS = "10";
                }
                else
                {
                    poData.CSTATUS = "00";
                }

                await _JournalListModel.JournalProcessAsync(poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task DeleteJournal(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                CSTATUS_TEMP = Journal.CSTATUS;
                poData.CSTATUS = "99";
                poData.LCOMMIT_APRJRN = false;
                poData.LUNDO_COMMIT = false;
                await _JournalListModel.JournalProcessAsync(poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        public async Task UndoReversingJournal(GLT00600JournalGridDTO poData)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                await _JournalListModel.UndoReversingJournalProcessAsync(poData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region "Rapid Approve and Commit"
        public async Task RapidApproveOrCommitJournal()
        {
            R_Exception loEx = new R_Exception();
            List<GLT00600JournalGridDTO> tempDataSelected = new List<GLT00600JournalGridDTO>();
            try
            {
                foreach (var item in loProcessRapidApproveOrCommitList)
                {
                    if (item.LSELECTED == true)
                    {
                        tempDataSelected.Add(item);
                    }
                }

                if (loProcessRapidApproveOrCommitList.Count == 0)
                {
                    loEx.Add(new Exception("Please select Journal to process!"));
                    goto EndDetail;
                }

                loProcessRapidApproveOrCommitList = tempDataSelected;
                // await ProcessDataSelected(COMPANYID, USERID);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        EndDetail:
            loEx.ThrowExceptionIfErrors();
        }
        // public async Task ProcessDataSelected(string COMPANYID, string USERID)
        // {
        //     var loEx = new R_Exception();
        //     List<GLT00600JournalGridDTO> loTemp = new List<GLT00600JournalGridDTO>();
        //     loTemp = loProcessRapidApproveOrCommitList;
        //     foreach (var item in loTemp)
        //     {
        //         if (item.CSTATUS == "80")
        //         {
        //             item.CSTATUS = "20";
        //         }
        //         else
        //         {
        //             item.CSTATUS = "80";
        //         }
        //     }
        //
        //     try
        //     {
        //         var loUserParameters = new List<R_KeyValue>();
        //         loUserParameters.Add(new R_KeyValue() { Key = ContextConstant.CRAPIDORCOMMIT, Value = lcRapidOrdCommit });
        //
        //         //Instantiate ProcessClient
        //         R_ProcessAndUploadClient loCls = new R_ProcessAndUploadClient(
        //             pcModuleName: "GL",
        //             plSendWithContext: true,
        //             plSendWithToken: true,
        //             pcHttpClientName: "R_DefaultServiceUrlGL",
        //             poProcessProgressStatus: this);
        //
        //         //prepare Batch Parameter
        //         R_BatchParameter loDbPar = new R_BatchParameter();
        //         loDbPar.COMPANY_ID = COMPANYID;
        //         loDbPar.USER_ID = USERID;
        //         loDbPar.UserParameters = loUserParameters;
        //         loDbPar.ClassName = "GLT00600Back.GLT00600RapidApproveAndCommitCls";
        //         loDbPar.BigObject = loTemp;
        //
        //         await loCls.R_BatchProcess<List<GLT00600JournalGridDTO>>(loDbPar, loTemp.Count);
        //     }
        //     catch (Exception ex)
        //     {
        //         loEx.Add(ex);
        //     }
        // }

        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            if (poProcessResultMode == eProcessResultMode.Success)
            {
                ProgressBarMessage = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                ShowSuccessAction();
            }

            if (poProcessResultMode == eProcessResultMode.Fail)
            {
                ProgressBarMessage = string.Format("Process Complete but fail with GUID {0}", pcKeyGuid);
            }

            await Task.CompletedTask;
        }
        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            ProgressBarMessage = string.Format("Process Error with GUID {0}", pcKeyGuid);
            StateChangeAction();


            await Task.CompletedTask;
        }
        public async Task ReportProgress(int pnProgress, string pcStatus)
        {
            ProgressBarMessage = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            ProgressBarPercentage = pnProgress;
            ProgressBarMessage = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            StateChangeAction();

            await Task.CompletedTask;
        }
        #endregion

        public async Task RapidApprove (string pcIdSelectedCREF_NOCombinedWithCommaSeparator)
        {
            var loException = new R_Exception();

            try
            {
                GLT00600JournalGridDTO loContent = new GLT00600JournalGridDTO()
                {
                    CCOMPANY_ID = Parameter.CCOMPANY_ID,
                    CUSER_ID = Parameter.CUSER_ID,
                    CREC_ID = pcIdSelectedCREF_NOCombinedWithCommaSeparator,
                    CSTATUS = "20",
                    LCOMMIT_APRJRN = Parameter.LCOMMIT_APRJRN,
                    LUNDO_COMMIT = false
                        
                };

                await _JournalListModel.JournalProcessAsync(poData : loContent);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }

        public async Task RapidCommit(string pcIdSelectedCREC_IDCombinedWithCommaSeparator)
        {
            var loException = new R_Exception();

            try
            {
                GLT00600JournalGridDTO loContent = new GLT00600JournalGridDTO()
                {
                    CCOMPANY_ID = Parameter.CCOMPANY_ID,
                    CUSER_ID = Parameter.CUSER_ID,
                    CREC_ID = pcIdSelectedCREC_IDCombinedWithCommaSeparator,
                    CSTATUS = "80",
                    LCOMMIT_APRJRN = Parameter.LCOMMIT_APRJRN,
                    LUNDO_COMMIT = false

                };

                await _JournalListModel.JournalProcessAsync(poData: loContent);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}
