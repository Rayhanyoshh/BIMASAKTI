using CBT01200Common.DTOs.CBT01210;
using CBT01200Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBT01200Common
{
    public interface ICBT01210
    {
        CBT01200RecordResult<CBT01210LastCurrencyRateDTO> GetLastCurrency(CBT01210LastCurrencyRateDTO poEntity);
        CBT01200RecordResult<CBT01210DTO> GetJournalRecord(CBT01210DTO poEntity);
        CBT01200RecordResult<CBT01211DTO> GetJournalDetailRecord(CBT01211DTO poEntity);
        CBT01200RecordResult<CBT01210DTO> SaveJournal(CBT01210DTO poEntity);
        CBT01200RecordResult<CBT01210DTO> SaveJournalDetail(CBT01211DTO poEntity);
        CBT01200RecordResult<CBT01210DTO> DeleteJournalDetail(CBT01211DTO poEntity);


    }
}
