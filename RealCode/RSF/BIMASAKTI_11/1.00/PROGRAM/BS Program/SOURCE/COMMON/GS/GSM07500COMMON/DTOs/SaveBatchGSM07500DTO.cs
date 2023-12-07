using System;
using System.Collections.Generic;

namespace GSM07500Common.DTOs
{
    public class SaveBatchGSM07500DTO
    {
        public string CCOMPANY_ID{ get; set; }
        public string CYEAR{ get; set; }
        public bool LPERIOD_MODE { get; set; }
        public int INO_PERIOD { get; set; }
        public string CACTION { get; set; }
        public string CUSER_ID { get; set; }
        public List<GSM07500DTO> CPERIOD_LIST { get; set; }
        
    }
}

