using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSM01000Common.DTOs
{
    public class AssignUserDTO
    {
        public string CUSER_ID { get; set; }
        
        public string CUSER_NAME { get; set; }

        public bool LSELECTED { get; set; } = false;

    }
}