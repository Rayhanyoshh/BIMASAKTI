using GSM008500Common.DTOs.PrintDTO;
using R_CommonFrontBackAPI.Log;

namespace GSM08500Service.DTOs;

public class GSM08500PrintLogKeyDTO
{
    public GSM08500PrintParamStatAccDTO poParam { get; set; }
    public R_NetCoreLogKeyDTO poLogKey { get; set; }  
}