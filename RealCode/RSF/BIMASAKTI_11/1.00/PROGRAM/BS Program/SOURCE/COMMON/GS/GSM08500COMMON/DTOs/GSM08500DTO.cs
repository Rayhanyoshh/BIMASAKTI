using R_APICommonDTO;   
using System;
using System.Collections.Generic;

namespace GSM008500Common.DTOs
{
    public class GSM08500DTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CUSER_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public bool LACTIVE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
    }
    
    public class GSM08500ListDTO : R_APIResultBaseDTO
    {

        public List<GSM08500DTO> Data { get; set; }

    }

      

    public class GSM08500CoAExcelDTO : R_APIResultBaseDTO
    {
        public byte[] FileBytes { get; set; }
    }
}