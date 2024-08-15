using System.Collections.Generic;

namespace TXR00200Common.PrintDTO;

public class PrintResultTXDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public TXR00200PrintColoumnDTO Column { get; set; }
    public PrintParamTXDTO Param { get; set; }
    public List<TXR00200DataResultDTO> Data { get; set; }
}


public class TXR00200PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PrintResultTXDTO Data1 { get; set; }
}
