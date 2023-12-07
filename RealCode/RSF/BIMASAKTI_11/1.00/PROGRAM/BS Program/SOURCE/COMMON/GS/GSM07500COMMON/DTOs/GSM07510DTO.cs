using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM07500Common.DTOs
{
    public class GSM07510DTO 
    { 
        public bool LSELECTED { get; set; }
        public string CCOMPANY_ID { get; set; }
        public string CYEAR{ get; set; }
        public bool LPERIOD_MODE { get; set; }
        public int INO_PERIOD { get; set; }
        public bool LVALID { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }

        public string DescPeriodMode { get; set; }
        public string CUSER_ID { get; set; }
        public string CACTION { get; set; }
        
        public List<GSM07500DTO> CPERIOD_LIST { get; set; }

    }

    public class GSM07510ListDTO : R_APIResultBaseDTO
    {
        public List<GSM07510DTO> Data { get; set; }
    }
}