using System.Collections.Generic;
using LMM01500Common.DTOs;
using R_CommonFrontBackAPI;

namespace LMM01500Common;

public interface ILMM01510 : R_IServiceCRUDBase<LMM01511DTO>
{
    IAsyncEnumerable<LMM01510DTO> LMM01510TemplateAndBankAccountList();
}