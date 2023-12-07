using System.Collections.Generic;

namespace GLT00600Common.DTOs
{
    public class JournalEntryFormAndDetailDTO
    {
        public GLT00600DTO MainData { get; set; }
        public List<GLT00600JournalGridDetailDTO> DetailList { get; set; }
    }
}