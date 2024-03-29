﻿using GSM01500COMMON.DTOs;
using R_CommonFrontBackAPI;
using System.Collections.Generic;

namespace GSM01500COMMON
{
    public interface IGSM01500 : R_IServiceCRUDBase<GSM01500DTO>
    {
        IAsyncEnumerable<GSM01500DTO> GetCenterList();
        IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList();
        CopyFromProcessDTO CopyFromProcess();
        ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_CENTERMethod();
    }
}
