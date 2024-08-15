using System.Collections.Generic;
using R_APICommonDTO;

namespace TXR00100Common.DTOs;

public class TXR00100PeriodDetailDTO
{
    public string CCYEAR { get; set; }
    public string CPERIOD_NO { get; set; }
    public string CSTART_DATE { get; set; }
    public string CEND_DATE { get; set; }
}
public class PeriodDetailListDataDTO :R_APIResultBaseDTO
{
    public List<TXR00100PeriodDetailDTO> Data { get; set; }
}