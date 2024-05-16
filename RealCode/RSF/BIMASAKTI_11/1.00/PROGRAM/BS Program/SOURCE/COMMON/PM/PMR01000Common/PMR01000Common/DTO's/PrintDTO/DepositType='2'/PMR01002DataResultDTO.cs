using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01002DataResultDTO
{
    public string CACCOUNT_NO  { get; set; }
    public string CACCOUNT_NAME  { get; set; }
    public string CBUILDING_ID  { get; set; }
    public string CDEPOSIT_TYPE  { get; set; }
    public List<PMR01002DataResultChildDTO> Detail { get; set; }
}