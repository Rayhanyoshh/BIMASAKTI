using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01002DataResultDTO
{
    public string CACCOUNT_NO  { get; set; }
    public string CACCOUNT_NAME  { get; set; }
    public List<PMR01002DataResultChild1DTO> Detail1 { get; set; }
}