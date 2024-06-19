using System.Collections.Generic;
using PMR02200Common.DTOs;

namespace PMR02200Common;

public interface IPMR02200
{
    
    IAsyncEnumerable<PropertyListDTO> GetPropertyList();
    PMR02200RecordResult<PMR02200PeriodCompanyDTO> GetPeriodYearRange();
}