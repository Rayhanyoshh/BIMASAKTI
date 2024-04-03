using CBT01200Common;
using CBT01200Common.DTOs;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSModel;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CBT01200MODEL
{
    public class CBT01210ViewModel : R_ViewModel<CBT01211DTO>
    {
        #region Model
        private CBT01200InitModel _CBT01200InitModel = new CBT01200InitModel();
        private CBT01200Model _CBT01200Model = new CBT01200Model();
        private CBT01210Model _CBT01210Model = new CBT01210Model();
        #endregion

        #region Initial Data
        public CBT01200GSTransInfoDTO VAR_GSM_TRANSACTION_CODE { get; set; } = new CBT01200GSTransInfoDTO();
        public CBT01200GLSystemParamDTO VAR_GL_SYSTEM_PARAM { get; set; } = new CBT01200GLSystemParamDTO();
        public CBT01200CBSystemParamDTO VAR_CB_SYSTEM_PARAM { get; set; } = new CBT01200CBSystemParamDTO();
        public CBT01200GLSystemEnableOptionInfoDTO VAR_IUNDO_COMMIT_JRN { get; set; } = new CBT01200GLSystemEnableOptionInfoDTO();
        public CBT01200TodayDateDTO VAR_TODAY { get; set; } = new CBT01200TodayDateDTO();
        public CBT01200GSCompanyInfoDTO VAR_GSM_COMPANY { get; set; } = new CBT01200GSCompanyInfoDTO();
        public List<CBT01200GSCurrencyDTO> VAR_CURRENCY_LIST { get; set; } = new List<CBT01200GSCurrencyDTO>();
        public List<CBT01200GSCenterDTO> VAR_CENTER_LIST { get; set; } = new List<CBT01200GSCenterDTO>();
        public CBT01200GSPeriodDTInfoDTO VAR_CCURRENT_PERIOD_START_DATE { get; set; } = new CBT01200GSPeriodDTInfoDTO();
        public CBT01200GSPeriodYearRangeDTO VAR_GSM_PERIOD { get; set; } = new CBT01200GSPeriodYearRangeDTO();
        public List<CBT01200GSGSBCodeDTO> VAR_GSB_CODE_LIST { get; set; } = new List<CBT01200GSGSBCodeDTO>();

        #endregion

        #region Public Property ViewModel
        public DateTime RefDate { get; set; }
        public DateTime? DocDate { get; set; }
        public CBT01210DTO Journal { get; set; } = new CBT01210DTO();
        public ObservableCollection<CBT01201DTO> JournalDetailGrid { get; set; } = new ObservableCollection<CBT01201DTO>();
        public ObservableCollection<CBT01201DTO> JournalDetailGridTemp { get; set; } = new ObservableCollection<CBT01201DTO>();
        #endregion

        public async Task GetAllUniversalData()
        {
            var loEx = new R_Exception();

            try
            {
                // Get Universal Data
                //var loResult = await _CBT01200InitModel.GetTabJournalEntryUniversalVarAsync();

                //Set Universal Data
                VAR_GSM_COMPANY = await _CBT01200InitModel.GetGSCompanyInfoAsync();
                VAR_CB_SYSTEM_PARAM = await _CBT01200InitModel.GetCBSystemParamAsync();
                VAR_GL_SYSTEM_PARAM = await _CBT01200InitModel.GetGLSystemParamAsync();
                VAR_GSM_TRANSACTION_CODE = await _CBT01200InitModel.GetGSTransCodeInfoAsync();
                VAR_TODAY = await _CBT01200InitModel.GetTodayDateAsync();
                VAR_CURRENCY_LIST = await _CBT01200InitModel.GetCurrencyListAsync();
                VAR_IUNDO_COMMIT_JRN = await _CBT01200InitModel.GetGSSystemEnableOptionInfoAsync();
                VAR_GSM_PERIOD = await _CBT01200InitModel.GetGSPeriodYearRangeAsync();
                VAR_GSB_CODE_LIST = await _CBT01200InitModel.GetGSBCodeListAsync();

                var loParam = new CBT01200ParamGSPeriodDTInfoDTO() { CCYEAR = VAR_CB_SYSTEM_PARAM.CCURRENT_PERIOD_YY, CPERIOD_NO = VAR_CB_SYSTEM_PARAM.CCURRENT_PERIOD_MM };
                VAR_CCURRENT_PERIOD_START_DATE = await _CBT01200InitModel.GetGSPeriodDTInfoAsync(loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Journal
        public async Task GetJournal(CBT01210DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _CBT01210Model.GetJournalRecordAsync(poEntity);

                RefDate = DateTime.ParseExact(loResult.CREF_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                DocDate = DateTime.ParseExact(loResult.CDOC_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                Journal = loResult;
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
                poEntity.CRATE_DATE = RefDate.ToString("yyyyMMdd");
                poEntity.CRATETYPE_CODE = VAR_GL_SYSTEM_PARAM.CRATETYPE_CODE;
                var loResult = await _CBT01210Model.GetLastCurrencyAsync(poEntity);

                loRtn = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
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
        public async Task DeleteJournal(CBT01211DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = R_FrontUtility.ConvertObjectToObject<CBT01200UpdateStatusDTO>(poEntity);
                loData.LUNDO_COMMIT = false;
                loData.LAUTO_COMMIT = false;
                loData.CNEW_STATUS = "99";

                await UpdateJournalStatus(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task SaveJournal(CBT01210DTO poEntity, eCRUDMode poCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (poCRUDMode == eCRUDMode.AddMode)
                {
                    poEntity.CACTION = "NEW";
                    poEntity.CREC_ID = "";
                    poEntity.CREF_NO = VAR_GSM_TRANSACTION_CODE.LINCREMENT_FLAG ? "" : poEntity.CREF_NO;
                }
                else if (poCRUDMode == eCRUDMode.EditMode)
                {
                    poEntity.CACTION = "EDIT";
                }
                poEntity.CDOC_DATE = DocDate.Value.ToString("yyyyMMdd");
                poEntity.CREF_DATE = RefDate.ToString("yyyyMMdd");
                poEntity.CTRANS_CODE = ContextConstant.VAR_TRANS_CODE;

                var loResult = await _CBT01210Model.SaveJournalAsync(poEntity);

                Journal = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        #region Detail Journal
        public async Task GetJournalDetailList(CBT01201DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _CBT01200Model.GetJournalDetailListAsync(poEntity);
                loResult.ForEach(x => x.INO = loResult.Count + 1);

                JournalDetailGrid = new ObservableCollection<CBT01201DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveJournalDetail(CBT01211DTO poEntity)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                _CBT01210Model.SaveJournalDetailAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);

            }
            loEx.ThrowExceptionIfErrors();
        }

        #endregion

    }
}
