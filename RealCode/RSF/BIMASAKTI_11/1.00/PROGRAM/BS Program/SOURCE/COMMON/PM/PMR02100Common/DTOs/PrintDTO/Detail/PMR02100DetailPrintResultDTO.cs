using System.Collections.Generic;
using PMR02100Common.DTOs;
using PMR02100Common.DTOs.PrintDTO;

namespace PMR02200Common.DTOs;

public class PMR02100DetailPrintResultDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public PMR02100PrintColoumnDTO Column { get; set; }
    public PMR02100PrintParamDTO Param { get; set; }
    public List<PMR02100DTO> DataResult { get; set; }
}

public class PMR02100DetailPrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PMR02100DetailPrintResultDTO Data { get; set; }
}