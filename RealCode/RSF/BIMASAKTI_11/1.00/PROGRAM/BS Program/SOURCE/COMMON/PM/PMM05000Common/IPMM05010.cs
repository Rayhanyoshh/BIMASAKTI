using R_CommonFrontBackAPI;
using System.Collections.Generic;
using PMM05000Common.DTOs;

namespace PMM05000Common
{
    public interface IPMM05010: R_IServiceCRUDBase<PMM05010DTO>
    {
        UnitTypeDataDTO GetUnitTypeList();
    }
}

