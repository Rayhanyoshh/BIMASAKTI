using GSM07500Common.DTOs;
using R_CommonFrontBackAPI;
using System.Collections.Generic;
using System.Text;

namespace GSM07500Common
{
    public interface IGSM07500 : R_IServiceCRUDBase<SaveBatchGSM07500DTO>
    {
        GSM07500ListDTO PeriodDetailList();
        IAsyncEnumerable<GSM07500DTO> PeriodDetailListStream();
        
        GSM07500ListDTO RftPeriodGenerator();
    }
}
