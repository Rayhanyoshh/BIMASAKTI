using R_CommonFrontBackAPI;
using GSM01000Common;
using System.Collections.Generic;
using GSM01000Common.DTOs;

namespace GSM01000Common
{
    public interface IGSM01100 : R_IServiceCRUDBase<GSM01100DTO>
    {
        GSM01100UserListDTO GetCoAUserList();
        IAsyncEnumerable<GSM01100DTO> GetCoAUserListStream();
        
        IAsyncEnumerable<AssignUserDTO> GetUserToAssignList();
        
        AssignUserResultDTO AssignUserAction (UsertoAssignParam poParam);
        
        GSM01100DTO GetParameterInfo();
        
    }
}