using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM07500Common.DTOs
{
    public class GSM07500DTO 
    { 
        public string CCOMPANY_ID { get; set; }
        public string CCYEAR { get; set; }
        public string CPERIOD_NO { get; set; }
        public string CSTART_DATE { get; set; } 
        public DateTime DSTART_DATE { get; set; } 
        public string CEND_DATE { get; set; }
        public DateTime DEND_DATE { get; set; }
        
        public int INO_USED { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set;}
        public string CUSER_ID { get; set; }
    }

    public class GSM07500ListDTO : R_APIResultBaseDTO
    {
        public List<GSM07500DTO> Data { get; set; }
    }
}
