using R_APICommonDTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSM01000Common.DTOs
{
    public class GSM01000UploadHeaderDTO : R_APIResultBaseDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CCOMPANY_NAME { get; set; }
    }
}