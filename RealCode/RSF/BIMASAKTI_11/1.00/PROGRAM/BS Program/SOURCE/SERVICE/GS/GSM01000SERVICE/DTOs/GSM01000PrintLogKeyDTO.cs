using GSM01000Common.DTOs;
using R_CommonFrontBackAPI.Log;

namespace GSM01000Service.DTOs;

public class GSM01000PrintLogKeyDTO
{
    public GSM01000PrintParamCOADTO poParam { get; set; }
    public R_NetCoreLogKeyDTO poLogKey { get; set; }  
}

public class GSM01000GOAPrintLogKeyDTO
{
    public GSM01000PrintParamGOADTO poParam { get; set; }
    public R_NetCoreLogKeyDTO poLogKey { get; set; }
}