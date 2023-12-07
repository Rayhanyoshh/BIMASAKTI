using System;
using System.Collections.Generic;
using System.Text;
using R_APICommonDTO;

namespace GSM08500Common.DTOs
{
    public class CopyFromProcessCompanyDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CCOMPANY_NAME { get; set; }
    }
    public class CopyFromProcessCompanyListDTO : R_APIResultBaseDTO
    {
        public List<CopyFromProcessCompanyDTO> Data { get; set; }
    }
}