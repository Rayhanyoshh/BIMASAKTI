using R_CommonFrontBackAPI;
using GSM01000Common;
using System.Collections.Generic;
using GSM01000Common.DTOs;

namespace GSM01000Common
{
    public interface IGSM01010 : R_IServiceCRUDBase<GSM01010DTO>
    {
        GSM01010ListDTO GetGoA();
        
        IAsyncEnumerable<GSM01010DTO> GetGOAListByGLAccountStream();
        
        

    }
}