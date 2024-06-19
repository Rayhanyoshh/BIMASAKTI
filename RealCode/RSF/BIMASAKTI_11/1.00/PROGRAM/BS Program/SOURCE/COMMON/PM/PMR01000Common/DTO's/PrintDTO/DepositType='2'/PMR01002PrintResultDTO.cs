using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01002PrintResultDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public PMR01002PrintColoumnDTO Column { get; set; }
    public PMR01000PrintParamDTO HeaderParam { get; set; }
    public List<PMR01002DataResultDTO> Data { get; set; }
        
}

public class PMR01002PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PMR01002PrintResultDTO Data2 { get; set; }
}