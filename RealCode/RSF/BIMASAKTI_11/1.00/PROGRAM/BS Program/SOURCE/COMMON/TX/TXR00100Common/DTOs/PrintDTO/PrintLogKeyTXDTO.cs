using R_CommonFrontBackAPI.Log;

namespace TXR00100Common.PrintDTO;

public class PrintLogKeyTXDTO
{
    public PrintParamTXDTO poParam { get; set; }
    public R_NetCoreLogKeyDTO poLogKey { get; set; }  
}