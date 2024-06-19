using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01001PrintResultDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public PMR01001PrintColoumnDTO Column { get; set; }
    public PMR01000PrintParamDTO HeaderParam { get; set; }
    public List<PMR01001DataResultDTO> Data { get; set; }
    
    public List<PMR01000ResultPrintSPDTO> DataResult { get; set; }
    
    public PMR01000PrintParamDTO Param { get; set; }
}

public class PMR01001PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PMR01001PrintResultDTO Data1 { get; set; }
}