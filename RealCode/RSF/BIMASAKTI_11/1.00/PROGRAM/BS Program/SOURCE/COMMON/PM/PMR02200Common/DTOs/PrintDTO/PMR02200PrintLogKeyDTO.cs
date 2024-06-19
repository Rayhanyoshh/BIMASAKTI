using R_CommonFrontBackAPI.Log;
namespace PMR02200Common.DTOs.PrintDTO;

public class PMR02200PrintLogKeyDTO
{
    public PMR02200PrintParamDTO poParam { get; set; }
    public R_NetCoreLogKeyDTO poLogKey { get; set; }  
}