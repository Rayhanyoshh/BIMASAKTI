using System.Collections.Generic;
using R_APICommonDTO;

namespace TXR00200Common.DTOs;

public class TXR00200PeriodDetailDTO
{
    public string CCYEAR { get; set; }
    public string CPERIOD_NO { get; set; }
    public string CSTART_DATE { get; set; }
    public string CEND_DATE { get; set; }
}
public class PeriodDetailListDataDTO :R_APIResultBaseDTO
{
    public List<TXR00200PeriodDetailDTO> Data { get; set; }
}