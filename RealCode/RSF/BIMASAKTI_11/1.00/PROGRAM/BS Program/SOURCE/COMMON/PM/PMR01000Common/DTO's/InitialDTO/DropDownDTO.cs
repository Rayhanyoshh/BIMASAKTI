using System.Collections.Generic;
using R_APICommonDTO;

namespace PMR01000Common.DTO_s;

public class DropDownDTO
{
    public string Id { get; set; }
    public string Text { get; set; }
}
    
public class DropDownDataDTO : R_APIResultBaseDTO
{
    public List<DropDownDTO> Data { get; set; }
}