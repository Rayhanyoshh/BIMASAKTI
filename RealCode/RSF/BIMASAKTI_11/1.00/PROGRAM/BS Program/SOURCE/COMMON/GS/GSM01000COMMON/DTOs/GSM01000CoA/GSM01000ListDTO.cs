using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM01000Common.DTOs
{
    public class GSM01000ListDTO : R_APIResultBaseDTO
    {

        public List<GSM01000DTO> Data { get; set; }

    }
    
    public class GSM01000List<T> : R_APIResultBaseDTO
    {
        public List<T> Data { get; set; }
    }

}