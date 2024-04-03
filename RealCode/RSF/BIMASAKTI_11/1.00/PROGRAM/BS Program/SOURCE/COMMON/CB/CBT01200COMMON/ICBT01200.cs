using CBT01200Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBT01200Common
{
    public interface ICBT01200
    {
        IAsyncEnumerable<CBT01200DTO> GetJournalList();
        IAsyncEnumerable<CBT01201DTO> GetJournalDetailList();
        CBT01200RecordResult<CBT01200UpdateStatusDTO> UpdateJournalStatus(CBT01200UpdateStatusDTO poEntity);
    }
}
