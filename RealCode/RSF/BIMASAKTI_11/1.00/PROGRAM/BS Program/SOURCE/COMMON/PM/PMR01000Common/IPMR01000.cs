using System.Collections.Generic;
using PMR01000Common.DTO_s;

namespace PMR01000Common;

public interface IPMR01000
{
    IAsyncEnumerable<PropertyListDTO> GetPropertyList();
    
    PMR01000RecordResult<PMR01000CBSystemParamDTO> GetCBSystemParam();
    
    PMR01000RecordResult<PMR01000PeriodCompanyDTO> GetPeriodYearRange();
    
    IAsyncEnumerable<PMR01000PeriodDTDTO> GetPeriodDetailList();
    IAsyncEnumerable<PMR01000BuildingListDTO> GetBuildinglList();

}