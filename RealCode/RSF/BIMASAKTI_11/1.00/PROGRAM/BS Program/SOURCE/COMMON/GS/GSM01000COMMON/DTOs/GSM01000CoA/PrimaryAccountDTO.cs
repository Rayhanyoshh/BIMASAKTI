using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM01000Common.DTOs
{
    public class PrimaryAccountDTO : R_APIResultBaseDTO
    {
        public string CCOMPANY_ID { get; set; }

        public bool LPRIMARY_ACCOUNT { get; set; }

    }
}