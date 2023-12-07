using R_CommonFrontBackAPI;
using System.Collections.Generic;
using GSM01000Common.DTOs;

namespace GSM01000Common
{
    public interface IGSM01300 : R_IServiceCRUDBase<GSM01300DTO>
    {
        GSM01300ListDTO GetAllGOA();
        IAsyncEnumerable<GSM01300DTO> GetAllGOAStream();
        
        AssignCOAResultDTO AssignCOAAction (COAtoAssignParam poParam);

        
    }
}