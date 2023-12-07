using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM01000Common.DTOs
{
    public class GSM01100DTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }   
        public string CGLACCOUNT_NAME { get; set; }   
        public string CUSER_ID { get; set; }
        public string CUSER_NAME { get; set; }

        public string CCREATE_BY { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
        public bool LSELECTED { get; set; } = false;
        public string CUSER_LIST { get; set; }
    }

    public class UsertoAssignDTO
    {
        public string CUSER_ID { get; set; }
        public bool LSELECTED { get; set; }


    }

    public class UsertoAssignParam
    {
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CUSER_LIST { get; set; }
        public string CUSER_ID { get; set; }
    }

    public class AssignUserResultDTO : R_APIResultBaseDTO
    {
        
    }
}