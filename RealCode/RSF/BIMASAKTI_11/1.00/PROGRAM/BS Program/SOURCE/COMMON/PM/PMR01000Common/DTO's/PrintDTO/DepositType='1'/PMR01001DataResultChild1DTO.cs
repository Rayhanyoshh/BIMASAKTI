using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01001DataResultChild1DTO
{  
    public string CBUILDING_ID  { get; set; }
    public string CDEPOSIT_TYPE  { get; set; }
    public List<PMR01001DataResultChild2DTO> Detail2 { get; set; }
}