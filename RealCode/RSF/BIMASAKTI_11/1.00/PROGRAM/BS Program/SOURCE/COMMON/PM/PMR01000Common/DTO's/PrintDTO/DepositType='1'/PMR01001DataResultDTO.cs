using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01001DataResultDTO
{
    public string CDEPOSIT_ID  { get; set; }
    public string CDEPOSIT_NAME  { get; set; }
    public List<PMR01001DataResultChild1DTO> Detail1 { get; set; }
}