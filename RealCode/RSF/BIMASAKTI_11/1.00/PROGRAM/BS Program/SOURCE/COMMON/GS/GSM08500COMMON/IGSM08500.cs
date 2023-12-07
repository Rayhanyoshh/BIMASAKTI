using R_CommonFrontBackAPI;
using System.Collections.Generic;
using GSM008500Common.DTOs;
using GSM08500Common.DTOs;

namespace GSM008500Common
{
    public interface IGSM08500 : R_IServiceCRUDBase<GSM08500DTO>
    {
        GSM08500ListDTO GetStatisticAccDbList();
        
        IAsyncEnumerable<GSM08500DTO> GetStatisticAccDbListStream();
        
        IAsyncEnumerable<CopyFromProcessCompanyDTO> GetCompanyList();
        
        CopyFromProcess CopyFromProcess();   
        
        ActiveInactiveDTO RSP_GS_ACTIVE_INACTIVE_StatAccMethod();
        
        GSM08500CoAExcelDTO CoAExcelTemplate(); 
        
        GSM08500UploadHeaderDTO CompanyDetail();

    }
}