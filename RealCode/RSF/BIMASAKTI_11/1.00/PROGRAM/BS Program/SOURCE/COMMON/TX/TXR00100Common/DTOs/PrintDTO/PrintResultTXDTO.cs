using System.Collections.Generic;

namespace TXR00100Common.PrintDTO;

public class PrintResultTXDTO
{
    public string Title { get; set; }
    public string Header { get; set; }
    public TXR00100PrintColoumnDTO Column { get; set; }
    public PrintParamTXDTO Param { get; set; }
    public List<TXR00100DataResultDTO> Data { get; set; }
}


public class TXR00100PrintResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
{
    public PrintResultTXDTO Data1 { get; set; }
}
