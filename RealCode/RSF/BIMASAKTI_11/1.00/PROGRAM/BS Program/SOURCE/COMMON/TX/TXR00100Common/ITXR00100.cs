using System.Collections.Generic;
using TXR00100Common.DTOs;

namespace PMR02200Common;

public interface ITXR00100
{
    
    IAsyncEnumerable<PropertyListDTO> GetPropertyList();
    IAsyncEnumerable<TXR00100PeriodDetailDTO> GetPeriodDetailList(string poYear);
}