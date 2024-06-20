using R_CommonFrontBackAPI.Log;
namespace PMR02100Common.DTOs.PrintDTO;

public class PMR02100PrintLogKeyDTO
{
    public PMR02100PrintParamDTO poParam { get; set; }
    public R_NetCoreLogKeyDTO poLogKey { get; set; }  
}