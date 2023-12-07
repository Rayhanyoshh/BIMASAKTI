using System.Collections.Generic;
using R_APICommonDTO;

namespace LMM05000Common.DTOs;

public class DropDownSqmTotalDTO
{
    public string Id { get; set; }
    public string Text { get; set; }
}

public class DropDownSqmTotalListDTO : R_APIResultBaseDTO
{
    public List<DropDownSqmTotalDTO> Data { get; set; }
}