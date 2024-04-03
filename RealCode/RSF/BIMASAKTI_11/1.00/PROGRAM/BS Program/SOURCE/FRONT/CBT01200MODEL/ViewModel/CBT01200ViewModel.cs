using CBT01200Common;
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

namespace CBT01200MODEL
{
    public class CBT01200ViewModel : R_ViewModel<CBT01200DTO>
    {
        #region Model
        private CBT01200InitModel _CBT01200InitModel = new CBT01200InitModel();
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

        #region Public Property ViewModel
        public int JournalPeriodYear { get; set; }
        public string JournalPeriodMonth { get; set; }
        public CBT01200ParamDTO JournalParam { get; set; } = new CBT01200ParamDTO();
        public ObservableCollection<CBT01200DTO> JournalGrid { get; set; } = new ObservableCollection<CBT01200DTO>();
        public ObservableCollection<CBT01201DTO> JournalDetailGrid { get; set; } = new ObservableCollection<CBT01201DTO>();
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
                JournalParam.CDEPT_CODE = VAR_DEPARTEMENT_LIST.Any(loDeptList => loDeptList.CDEPT_CODE == VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_CODE) ? VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_CODE : "";
                JournalParam.CDEPT_NAME = VAR_DEPARTEMENT_LIST.Any(loDeptList => loDeptList.CDEPT_NAME == VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_NAME) ? VAR_GL_SYSTEM_PARAM.CCLOSE_DEPT_NAME : "";

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

        public async Task GetJournalDetailList(CBT01201DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _CBT01200Model.GetJournalDetailListAsync(poEntity);

                JournalDetailGrid = new ObservableCollection<CBT01201DTO>(loResult);
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
    }
}

