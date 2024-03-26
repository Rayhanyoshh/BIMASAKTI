using System.Collections.Generic;
using LMM01500Common.DTOs;
using R_CommonFrontBackAPI;

namespace LMM01500Common;

public interface ILMM01500 : R_IServiceCRUDBase<LMM01500DTO>
{
    IAsyncEnumerable<LMM01500PropertyDTO> GetProperty();

    IAsyncEnumerable<LMM01501DTO> GetInvoiceGrpList();
    
    LMM01500DTO LMM01500ActiveInactive(LMM01500DTO poParam);

    IAsyncEnumerable<LMM01502DTO> LMM01500LookupBank();
}