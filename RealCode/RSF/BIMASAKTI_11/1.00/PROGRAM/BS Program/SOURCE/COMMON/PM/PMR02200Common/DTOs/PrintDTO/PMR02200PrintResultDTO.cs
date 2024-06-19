using System.Collections.Generic;
using PMR02200Common.DTOs;

namespace PMR02200Common.DTOs.PrintDTO;

public class PMR02200PrintResultDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public PMR02200PrintColumnDTO Column { get; set; }
    public PMR02200PrintParamDTO HeaderParam { get; set; }
    public List<PMR02200DataResultDTO> Data { get; set; }
    public List<PMR02200DTO> DataResult { get; set; }
    public PMR02200PrintParamDTO Param { get; set; }
}


public class PMR02200PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PMR02200PrintResultDTO Data1 { get; set; }
}