using System.Collections.Generic;
using PMR02100Common.DTOs.PrintDTO;

namespace PMR02200Common.DTOs;

public class PMR02100SummaryPrintResultDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public PMR02100PrintColoumnDTO Column { get; set; }
    public PMR02100PrintParamDTO Param { get; set; }
    public List<PMR02100SummaryDTO> DataResult { get; set; }
}

public class PMR02100PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PMR02100SummaryPrintResultDTO Data { get; set; }
}