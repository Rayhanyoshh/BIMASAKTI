using GSM01000Common.DTOs;
using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;
namespace GSM01000Common.DTOs
{
    public class GSM01010DTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CGOA_CODE { get; set; }
        public string CGOA_NAME { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
    }
}