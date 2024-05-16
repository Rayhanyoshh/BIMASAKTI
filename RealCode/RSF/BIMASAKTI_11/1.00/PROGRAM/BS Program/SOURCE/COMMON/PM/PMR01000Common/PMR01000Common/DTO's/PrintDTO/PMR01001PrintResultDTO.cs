using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01001PrintResultDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public PMR01001PrintColoumnDTO Column { get; set; }
    public List<PMR01001ResultPrintSPDTO> Data { get; set; }
        
    public PMR01000PrintParamDTO ParamDTO { get; set; }
}

public class PMR01000PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public GSM01000PrintGOAResultDTo GOAData { get; set; }
}