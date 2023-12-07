using R_CommonFrontBackAPI;
using System.Collections.Generic;
using LMM05000Common.DTOs;


namespace LMM05000Common
{
    public interface ILMM05000 : R_IServiceCRUDBase<LMM05000DTO>
    {
        IAsyncEnumerable<PropertyListDTO> GetPropertyList();
        LMM05000ListDTO GetUnitTypePriceList();
        
        ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_Method();
    }
}

