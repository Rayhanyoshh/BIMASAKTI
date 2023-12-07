using R_CommonFrontBackAPI;
using System.Collections.Generic;
using LMM05000Common.DTOs;

namespace LMM05000Common
{
    public interface ILMM05010: R_IServiceCRUDBase<LMM05010DTO>
    {
        UnitTypeDataDTO GetUnitTypeList();
    }
}

