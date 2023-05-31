using R_CommonFrontBackAPI;
using GSM01000Common;
using System.Collections.Generic;
using GSM01000Common.DTOs;

namespace GSM01000Common
{
    public interface IGSM01310 : R_IServiceCRUDBase<GSM01310DTO>
    {
        GSM01310ListDTO GetGoACoA();
        
        IAsyncEnumerable<GSM01310DTO> GetGoACoAListStream();
        
        IAsyncEnumerable<AssignCoADTO> GetCoAToAssignList();

    }
}