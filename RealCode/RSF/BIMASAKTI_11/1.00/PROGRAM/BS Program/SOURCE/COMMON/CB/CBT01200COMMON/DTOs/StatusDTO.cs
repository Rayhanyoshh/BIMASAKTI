using System.Collections.Generic;
using R_APICommonDTO;

namespace CBT01200Common.DTOs
{
    public class StatusDTO
    {
        public string CCODE { get; set; }
        public string CNAME { get; set; }
    }

    public class StatusListDTO : R_APIResultBaseDTO
    {
        public List<StatusDTO> Data { get; set; }

    }
}