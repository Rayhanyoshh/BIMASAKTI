using System.Collections.Generic;

namespace CBT01200Common.DTOs
{
    public class JournalEntryFormAndDetailDTO
    {
        public CBT01200DTO MainData { get; set; }
        public List<CBT01200JournalGridDetailDTO> DetailList { get; set; }
    }
}