using R_APICommonDTO;
using System;
using System.Collections.Generic;
namespace GSM01000Common.DTOs
{
    public class GSM01200CenterListDTO : R_APIResultBaseDTO
    {

        public List<GSM01200DTO> Data { get; set; }

    }
    
    public class GSM01200CenterGridDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CCENTER_CODE { get; set; }
        public string CCENTER_NAME { get; set; }
        public string CCREATE_BY { get; set; }
        public string DCREATE_DATE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime CUPDATE_DATE { get; set; }
        
        public string CUSER_ID { get; set; }

    }
}