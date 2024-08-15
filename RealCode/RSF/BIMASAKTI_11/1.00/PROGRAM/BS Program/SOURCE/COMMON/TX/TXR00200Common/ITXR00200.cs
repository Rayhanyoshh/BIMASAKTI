using System.Collections.Generic;
using TXR00200Common.DTOs;

namespace PMR02200Common;

public interface ITXR00200
{
    
    IAsyncEnumerable<PropertyListDTO> GetPropertyList();
    IAsyncEnumerable<TXR00200DTO> GerEFakturList();
    IAsyncEnumerable<TXR00200PeriodDetailDTO> GetPeriodDetailList(string poYear);
}