﻿using CBT01200Common;
using CBT01200Common.DTOs;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSModel;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace CBT01200MODEL
{
    public class CBT01200ViewModel : R_ViewModel<CBT01200DTO>
    {
        #region Model
        private CBT01200InitModel _CBT01200InitModel = new CBT01200InitModel();
        private CBT01210ViewModel _CBT01210ViewModel = new CBT01210ViewModel();
        private PublicLookupModel _PublicLookupModel = new PublicLookupModel();
        private CBT01200Model _CBT01200Model = new CBT01200Model();
        #endregion

        #region Initial Data
        public CBT01200GSCompanyInfoDTO VAR_GSM_COMPANY { get; set; } = new CBT01200GSCompanyInfoDTO();
        public CBT01200GLSystemParamDTO VAR_GL_SYSTEM_PARAM { get; set; } = new CBT01200GLSystemParamDTO();
        public CBT01200CBSystemParamDTO VAR_CB_SYSTEM_PARAM { get; set; } = new CBT01200CBSystemParamDTO();
        public CBT01200GSTransInfoDTO VAR_GSM_TRANSACTION_CODE { get; set; } = new CBT01200GSTransInfoDTO();
        public CBT01200GLSystemEnableOptionInfoDTO VAR_IUNDO_COMMIT_JRN { get; set; } = new CBT01200GLSystemEnableOptionInfoDTO();
        public CBT01200GSPeriodYearRangeDTO VAR_GSM_PERIOD { get; set; } = new CBT01200GSPeriodYearRangeDTO();
        public List<CBT01200GSGSBCodeDTO> VAR_GSB_CODE_LIST { get; set; } = new List<CBT01200GSGSBCodeDTO>();
        public List<GSL00700DTO> VAR_DEPARTEMENT_LIST { get; set; } = new List<GSL00700DTO>();
        #endregion
        
        #region property
        public DateTime?
            Drefdate = DateTime.Now;

        public DateTime?
            Ddocdate = DateTime.Now;

        public int ProgressBarPercentage = 0;
        public string
            ProgressBarMessage = "",
            CCURRENT_PERIOD_YY = "",
            CCURRENT_PERIOD_MM = "",
            CSOFT_PERIOD_YY = "",
            CSOFT_PERIOD_MM = "",
            LcCrecID = "",
            lcSearch = "",
            lcPeriod = "",
            lcStatus = "",
            lcDeptCode = "",
            lcDeptName = "",
            lcRapidOrdCommit = "",
            lcLocalCurrency = "",
            lcBaseCurrency = "";

        public string COMPANYID;
        public string USERID;
        public string CommitLabel { get; set; }
        public string SubmitLabel = "Submit";
        public bool
            EnableDept = false,
            EnableButton = false,
            EnableDelete = false,
            EnableSubmit = false,
            EnableApprove = false,
            EnableCommit = false,
            EnablePrint = false,
            EnableCopy = false,
            EnableEdit = false,
            EnableNLBASE_RATE = false,
            EnableNLCURRENCY_RATE = false,
            EnableNBBASE_RATE = false,
            EnableNBCURRENCY_RATE = false,
            EnableRefNo = false,
            loCopy = false;
        


        public bool buttonRapidApprove = true;
        public bool buttonRapidCommit = true;
        public string CSTATUS_TEMP { get; set; }
        #endregion

        #region Public Property ViewModel
        public int JournalPeriodYear { get; set; }
        public string JournalPeriodMonth { get; set; }
        public CBT01200JournalHDParam JournalParam { get; set; } = new CBT01200JournalHDParam();
        public ObservableCollection<CBT01200DTO> JournalGrid { get; set; } = new ObservableCollection<CBT01200DTO>();
        
        public List<CBT01200DTO> JournalHeaderDTO { get; set; } = new ();

        
        public CBT01200DTO Journal = new();

        public CBT01200JournalHDParam JournalRecord = new();
        public ObservableCollection<CBT01201DTO> JournalDetailGrid { get; set; } = new ObservableCollection<CBT01201DTO>();
        public DateTime? RefDate { get; set; }
        public DateTime? DocDate { get; set; }
        #endregion
        
        #region Universal DTO
        
        public CBT01200GSCompanyInfoDTO CompanyCollection = new();
        
        #endregion  

        #region ComboBox ViewModel

        public List<KeyValuePair<string, string>> PeriodMonthList { get; } = new List<KeyValuePair<string, string>>()
        {
            new KeyValuePair<string, string>("01", "01"),
            new KeyValuePair<string, string>("02", "02"),
            new KeyValuePair<string, string>("03", "03"),
            new KeyValuePair<string, string>("04", "04"),
            new KeyValuePair<string, string>("05", "05"),
            new KeyValuePair<string, string>("06", "06"),
            new KeyValuePair<string, string>("07", "07"),
            new KeyValuePair<string, string>("08", "08"),
            new KeyValuePair<string, string>("09", "09"),
            new KeyValuePair<string, string>("10", "10"),
            new KeyValuePair<string, string>("11", "11"),
            new KeyValuePair<string, string>("12", "12")
        };

        #endregion
        public async Task GetAllUniversalData()
        {
            var loEx = new R_Exception();

            try
            {
                // Get Universal Data
                VAR_GSM_COMPANY = await _CBT01200InitModel.GetGSCompanyInfoAsync();
                VAR_GL_SYSTEM_PARAM = await _CBT01200InitModel.GetGLSystemParamAsync();
                VAR_CB_SYSTEM_PARAM = await _CBT01200InitModel.GetCBSystemParamAsync();
                VAR_GSM_TRANSACTION_CODE = await _CBT01200InitModel.GetGSTransCodeInfoAsync();
                VAR_IUNDO_COMMIT_JRN = await _CBT01200InitModel.GetGSSystemEnableOptionInfoAsync();
                VAR_GSM_PERIOD = await _CBT01200InitModel.GetGSPeriodYearRangeAsync();
                VAR_GSB_CODE_LIST = await _CBT01200InitModel.GetGSBCodeListAsync();

                //Add all data 
                VAR_GSB_CODE_LIST.Add(new CBT01200GSGSBCodeDTO { CCODE = "", CNAME = "ALL" });

                //Get And Set List Dept Code
                VAR_DEPARTEMENT_LIST = await _PublicLookupModel.GSL00700GetDepartmentListAsync(new GSL00700ParameterDTO());

                //Set default Dept Code
                if (VAR_DEPARTEMENT_LIST != null && VAR_DEPARTEMENT_LIST.Any() && VAR_GL_SYSTEM_PARAM != null)
                {
                    JournalParam.CDEPT_CODE = VAR_DEPARTEMENT_LIST.Any(loDeptList => loDeptList.CDEPT_CODE == VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_CODE)
                                                ? VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_CODE
                                                : "";
                }
                else
                {
                    // Penanganan kesalahan, misalnya mengatur nilai default untuk JournalParam.CDEPT_CODE
                    JournalParam.CDEPT_CODE = "";
                }

                if (VAR_DEPARTEMENT_LIST != null && VAR_DEPARTEMENT_LIST.Any() && VAR_GL_SYSTEM_PARAM != null)
                {
                    JournalParam.CDEPT_NAME = VAR_DEPARTEMENT_LIST.Any(loDeptList => loDeptList.CDEPT_NAME == VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_NAME)
                                                ? VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_NAME
                                                : "";
                }
                else
                {
                    // Penanganan kesalahan, misalnya mengatur nilai default untuk JournalParam.CDEPT_CODE
                    JournalParam.CDEPT_CODE = "";
                }


                //Set default Journal Period
                JournalPeriodYear = int.Parse(VAR_CB_SYSTEM_PARAM.CSOFT_PERIOD_YY);
                JournalPeriodMonth = VAR_CB_SYSTEM_PARAM.CSOFT_PERIOD_MM;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetJournalList()
        {
            var loEx = new R_Exception();

            try
            {
                JournalParam.CPERIOD = JournalPeriodYear + JournalPeriodMonth;
                
                var loResult = await _CBT01200Model.GetJournalListAsync(JournalParam);

                JournalGrid = new ObservableCollection<CBT01200DTO>(loResult);
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetJournalRecord(CBT01200DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _CBT01200Model.R_ServiceGetRecordAsync(poParam);
                Journal = loResult;


                DocDate= DateTime.ParseExact(Journal.CDOC_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                RefDate = DateTime.ParseExact(Journal.CREF_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                Journal.CUPDATE_DATE = Journal.DUPDATE_DATE.Value.ToLongDateString();
                Journal.CCREATE_DATE = Journal.DCREATE_DATE.Value.ToLongDateString();
                Journal.CLOCAL_CURRENCY_CODE = CompanyCollection.CLOCAL_CURRENCY_CODE;
                Journal.CBASE_CURRENCY_CODE = CompanyCollection.CBASE_CURRENCY_CODE;
                LcCrecID = Journal.CREC_ID;
                Data.CSTATUS = Journal.CSTATUS;
                Data.CCB_ACCOUNT_NAME = Journal.CCB_ACCOUNT_NAME;
                Data.LALLOW_APPROVE = Journal.LALLOW_APPROVE;

                //var loParam = new CBT01201DTO()
                //{
                //    CREC_ID = JournalRecord.CREC_ID
                //};
                //await _CBT01210ViewModel.GetJournalDetailList();

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            }

        public async Task SaveJournal(CBT01200DTO poEntity, eCRUDMode poCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                poEntity.CREF_NO = string.IsNullOrWhiteSpace(poEntity.CREF_NO) ? "" : poEntity.CREF_NO;
                poEntity.CREF_DATE = Drefdate.Value.ToString("yyyyMMdd");
                poEntity.CDOC_DATE = Ddocdate.Value.ToString("yyyyMMdd");
                poEntity.SaveParam = new()
                {
                    PARAM_CALLER = new()
                };
                var loResult = await _CBT01200Model.R_ServiceSaveAsync(poEntity, poCRUDMode);

                Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task DeleteJournal(CBT01200JournalHDParam poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = R_FrontUtility.ConvertObjectToObject<CBT01200JournalHDParam>(poEntity);
                poEntity.LUNDO_COMMIT = false;
                loData.LAUTO_COMMIT = false;
                loData.CNEW_STATUS = "99";

                await _CBT01200Model.R_ServiceDeleteAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task UpdateJournalStatus(CBT01200UpdateStatusDTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await _CBT01200Model.UpdateJournalStatusAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task<CBT01210LastCurrencyRateDTO> GetLastCurrency(CBT01210LastCurrencyRateDTO poEntity)
        {
            var loEx = new R_Exception();
            CBT01210LastCurrencyRateDTO loRtn = null;
            try
            {
                poEntity.CRATE_DATE = Drefdate.Value.ToString("yyyyMMdd");
                poEntity.CRATETYPE_CODE = VAR_CB_SYSTEM_PARAM.CRATETYPE_CODE;
                var loResult = await _CBT01200Model.GetLastCurrencyAsync(poEntity);

                loRtn = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
        
        public async Task<CBT01200ValidateUpdateStatusDTO> ValidateUpdateJournalStatus(object poEntity)
        {
            var loEx = new R_Exception();
            CBT01200ValidateUpdateStatusDTO loRtn = null;

            try
            {
                var loParam = R_FrontUtility.ConvertObjectToObject<CBT01200ValidateUpdateStatusDTO>(poEntity);
                loRtn = await _CBT01200Model.ValidateUpdateJournalStatusAsync(loParam);
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

