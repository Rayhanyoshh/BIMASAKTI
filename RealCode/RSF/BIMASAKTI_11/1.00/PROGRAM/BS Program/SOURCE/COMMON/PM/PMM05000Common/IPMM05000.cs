using R_CommonFrontBackAPI;
using System.Collections.Generic;
using PMM05000Common.DTOs;


namespace PMM05000Common
{
    public interface IPMM05000 : R_IServiceCRUDBase<PMM05000DTO>
    {
        IAsyncEnumerable<PropertyListDTO> GetPropertyList();
        PMM05000ListDTO GetUnitTypePriceList();
        
        ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_Method();
    }
}

