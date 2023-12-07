using System.Collections.Generic;
using R_APICommonDTO;

namespace GSM07500Common.DTOs
{
    public class LastYearDTO: R_APIResultBaseDTO
    {
        public string NEXT_YEAR { get; set; }
    }
    
    public class LastYearListDTO : R_APIResultBaseDTO
    {
        public LastYearDTO Data { get; set; }
    }
}