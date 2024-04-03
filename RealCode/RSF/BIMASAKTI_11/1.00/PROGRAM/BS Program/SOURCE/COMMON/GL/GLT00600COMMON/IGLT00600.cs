using R_CommonFrontBackAPI;
using System.Collections.Generic;
using GLT00600Common.DTOs;

namespace GLT00600Common
{
    public interface IGLT00600 : R_IServiceCRUDBase<GLT00600DTO>
    {
        IAsyncEnumerable<GLT00600JournalGridDTO> GetJournalList();
        IAsyncEnumerable<GLT00600JournalGridDetailDTO> GetAllJournalDetailList();
        GLT00600JournalGridDTO ProcessAuditJournal(GLT00600JournalGridDTO poData);
        GLT00600JournalGridDTO UndoAuditJournal(GLT00600JournalGridDTO poData);
        GLT00600JournalGridDTO JournalProcesStatus(GLT00600JournalGridDTO poData);
        GLT00600JournalGridDTO RefreshCurrencyRate(GLT00600JournalGridDTO poData);


        #region INIT
        VAR_GSM_COMPANYDTO GetCompanyDTO();
        VAR_GL_SYSTEM_PARAMDTO GetSystemParam();
        VAR_USER_DEPARTMENT_LISTDTO GetDeptList();
        VAR_CCURRENT_PERIOD_START_DATEDTO GetCurrentPeriodStartDate(VAR_GL_SYSTEM_PARAMDTO poData);
        VAR_CSOFT_PERIOD_START_DATEDTO GetSoftPeriodStartDate(VAR_GL_SYSTEM_PARAMDTO poData);
        VAR_IUNDO_COMMIT_JRNDTO GetIOption();
        VAR_GSM_TRANSACTION_CODEDTO GetLincementLapproval();
        VAR_GSM_PERIODDTO GetPeriod();
        StatusListDTO GetStatusList();
        CurrencyCodeListDTO GetCurrencyCodeList();
        GetCenterListDTO GetCenterList();
        GSM_TRANSACTION_APPROVALDTO GetTransactionApproval();

        #endregion
    }
}