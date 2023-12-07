using GSM07500Common.DTOs;
using R_CommonFrontBackAPI;
using System.Collections.Generic;
using System.Text;

namespace GSM07510Common
{
    public interface IGSM07510 : R_IServiceCRUDBase<GSM07510DTO>
    {
        GSM07510ListDTO PeriodList();
        IAsyncEnumerable<GSM07510DTO> PeriodListStream();
        
        LastYearDTO GetLastYear();
    }
}