using System.Collections.Generic;
using R_APICommonDTO;

namespace GLT00600Common.DTOs
{
    public class GLT00600JournalGridDetailDTO
    {
        public string CDEPT_CODE { get; set; }

        public string CREC_ID { get; set; }
        public int INO { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CCENTER_NAME { get; set; }
        public string CDBCR { get; set; }
        public decimal NDEBIT { get; set; }
        public decimal NCREDIT { get; set; }
        public string CDETAIL_DESC { get; set; } = "";
        public string CDOCUMENT_NO { get; set; }
        public string CDOCUMENT_DATE { get; set; }
        public decimal NLDEBIT { get; set; }
        public decimal NLCREDIT { get; set; }
        public decimal NBDEBIT { get; set; }
        public decimal NBCREDIT { get; set; }
        public string CCENTER_CODE { get; set; }
        public decimal NAMOUNT { get; set; }
        public char CBSIS { get; set; }
    }

    public class GLT00600JournalGridDetailListDTO : R_APIResultBaseDTO
    {
        public List<GLT00600JournalGridDetailDTO> Data { get; set; }

    }
}