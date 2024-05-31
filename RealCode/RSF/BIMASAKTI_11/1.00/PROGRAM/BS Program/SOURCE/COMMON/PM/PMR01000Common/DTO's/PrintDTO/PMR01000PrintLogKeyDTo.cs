using R_CommonFrontBackAPI.Log;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01000PrintLogKeyDTO
{
    public PMR01000PrintParamDTO poParam { get; set; }
    public R_NetCoreLogKeyDTO poLogKey { get; set; }  
}