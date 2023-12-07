using System.Collections.Generic;
using GSM008500Common.DTOs.PrintDTO;

namespace GSM008500Common.DTOs
{
    #region StatAcc DTO
    public class GSM01000PrintStatAccResultDTo
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public GSM08500PrintColoumnStatAccDTO Column { get; set; }
        public List<GSM08500ResultSPPrintStatAccDTO> Data { get; set; }
    }
    
    public class GSM08500PrintStatAccResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
    {
        public GSM01000PrintStatAccResultDTo StatAccountData { get; set; }
    }
    #endregion
}