using System;
using R_APICommonDTO;

namespace GSM01000Common.DTOs
{
    public class GSM1200CenterCoAParameterDTO : R_APIResultBaseDTO 
    {
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CCENTER_CODE { get; set; }
        public string CCENTER_NAME { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATED_DATE { get; set; }
    }
}