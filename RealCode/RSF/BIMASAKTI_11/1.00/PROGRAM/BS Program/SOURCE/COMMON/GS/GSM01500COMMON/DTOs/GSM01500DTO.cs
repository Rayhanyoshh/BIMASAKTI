using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSM01500COMMON.DTOs
{
    public class GSM01500DTO : R_APIResultBaseDTO
    {
        public string CCENTER_CODE { get; set; }
        public string CCENTER_NAME { get; set; }
        public bool LACTIVE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
    }
}
