using R_CommonFrontBackAPI;
using GSM01000Common;
using System.Collections.Generic;
using GSM01000Common.DTOs;

namespace GSM01000Common
{
    public interface IGSM01200: R_IServiceCRUDBase<GSM01200DTO>
    {
        GSM01200CenterListDTO GetCoACenterList();
        
        IAsyncEnumerable<GSM01200DTO> GetCoACenterListStream();
        
        IAsyncEnumerable<AssignCenterDTO> GetCenterToAssignList();
        
        GSM01200DTO GetParameterInfo();
        
        AssignCenterResultDTO AssignCenterAction (CentertoAssignParam poParam);

    }
}