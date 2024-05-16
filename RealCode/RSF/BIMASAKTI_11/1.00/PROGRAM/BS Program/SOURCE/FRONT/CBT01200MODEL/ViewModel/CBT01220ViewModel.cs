using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CBT01200Common.DTOs;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;

namespace CBT01200MODEL;

public class CBT01220ViewModel
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
    public CBT01200GSPeriodDTInfoDTO VAR_SOFT_PERIOD_START_DATE { get; set; } = new CBT01200GSPeriodDTInfoDTO();
    public CBT01200GSPeriodYearRangeDTO VAR_GSM_PERIOD { get; set; } = new CBT01200GSPeriodYearRangeDTO();
    public List<CBT01200GSGSBCodeDTO> VAR_GSB_CODE_LIST { get; set; } = new List<CBT01200GSGSBCodeDTO>();
    #endregion
    
    #region Public Property ViewModel
    public string _CREC_ID { get; set; } = "";
    public DateTime? RefDate { get; set; }
    public DateTime? DocDate { get; set; }
    public CBT01200DTO Journal { get; set; } = new CBT01200DTO();
    #endregion
    
    
    
    public async Task SaveJournalDeposit(CBT01200DTO poEntity, eCRUDMode poCRUDMode)
    {
        var loEx = new R_Exception();
        ePARAM_CALLER loParamCAller = ePARAM_CALLER.DEPOSIT;
        try
        {
            poEntity.CREF_NO = string.IsNullOrWhiteSpace(poEntity.CREF_NO) ? "" : poEntity.CREF_NO;
            poEntity.CREF_DATE = RefDate.Value.ToString("yyyyMMdd");
            poEntity.CDOC_DATE = DocDate.Value.ToString("yyyyMMdd");
            poEntity.SaveParam = new()
            {
                PARAM_CALLER = loParamCAller
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
}