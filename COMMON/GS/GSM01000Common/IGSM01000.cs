using R_CommonFrontBackAPI;
using System.Collections.Generic;
using GSM01000Common.DTOs;

namespace GSM01000Common
{
    public interface IGSM01000 : R_IServiceCRUDBase<GSM01000DTO>
    {
        GSM01000ListDTO GetAllCOA();
        IAsyncEnumerable<GSM01000DTO> GetAllCOAStream();
        
        IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList();
        
        CopyFromProcessDTO CopyFromProcess();   
        
        ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_COAMethod();
        
    }
}