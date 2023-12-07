using System.Collections.Generic;
using R_APICommonDTO;

namespace GLT00600Common.DTOs
{
    public class GetMonthDTO
    {
        public string Id { get; set; }
    }
    public class GetMonthListDTO : R_APIResultBaseDTO
    {
        public List<GetMonthDTO> Data { get; set; }
    }
}