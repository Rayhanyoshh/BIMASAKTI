﻿using CBT01200Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using R_CommonFrontBackAPI;

namespace CBT01200Common
{
    public interface ICBT01200  : R_IServiceCRUDBase<CBT01200DTO>
    {
        IAsyncEnumerable<CBT01200DTO> GetJournalList();
        CBT01200RecordResult<CBT01200UpdateStatusDTO> UpdateJournalStatus(CBT01200UpdateStatusDTO poEntity);
        CBT01200RecordResult<CBT01210LastCurrencyRateDTO> GetLastCurrency(CBT01210LastCurrencyRateDTO poEntity);
        CBT01200RecordResult<CBT01200ValidateUpdateStatusDTO> ValidateUpdateJournalStatus (CBT01200ValidateUpdateStatusDTO poEntity);
    }
}
