using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01003PrintResultDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public PMR01003PrintColoumnDTO Column { get; set; }
    public PMR01003PrintColumnDetailDTO ColumnDetail { get; set; }
    public PMR01000PrintParamDTO HeaderParam { get; set; }

    public List<PMR01003DataResultDTO> Data { get; set; }
        
    public PMR01000PrintParamDTO ParamDTO { get; set; }
}

public class PMR01003PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PMR01003PrintResultDTO Data3 { get; set; }
}