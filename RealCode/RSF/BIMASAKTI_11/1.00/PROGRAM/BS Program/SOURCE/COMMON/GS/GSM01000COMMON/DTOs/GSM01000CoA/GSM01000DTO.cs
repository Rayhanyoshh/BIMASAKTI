using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM01000Common.DTOs
{
    public class GSM01000DTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CUSER_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; } 
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public bool LACTIVE { get; set; }
        public bool LCENTER_RESTR { get; set; }
        public bool LUSER_RESTR { get; set; }
        public string CCASH_FLOW_GROUP_CODE { get; set; }
        
        public string CCASH_FLOW_NAME { get; set; }
        public string CCASH_FLOW_CODE { get; set; }
        
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
        
        public string CCREATE_BY { get; set; }

        public DateTime DCREATE_DATE { get; set; }

    }
}
