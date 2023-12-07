using System;
using System.Collections.Generic;
using R_APICommonDTO;

namespace GSM01000Common.DTOs
{
    public class GSM01300ListDTO : R_APIResultBaseDTO
    {

        public List<GSM01300DTO> Data { get; set; }

    }
    public class GSM01300DTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGOA_CODE { get; set; }
        public string CGOA_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CBSIS_DESCR { get; set; }
        public string CDBCR { get; set; }
        public string CDBCR_DESCR { get; set; }
        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
        public string CUSER_ID { get; set; }

    }
}