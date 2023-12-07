using System;
using System.Collections.Generic;

namespace GSM01000Common.DTOs
{
    #region COA DTO
    public class GSM01000PrintCOAResultDTo
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public GSM01000PrintColoumnCOADTO Column { get; set; }
        public List<GSM01000ResultSPPrintCOADTO> Data { get; set; }
        
        public GSM01000PrintParamCOADTO ParamDTO { get; set; }
    }

    public class GSM01000PrintCOAResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
    {
        public GSM01000PrintCOAResultDTo CenterData { get; set; }
    }
    #endregion


    #region GOA DTO
    public class GSM01000PrintGOAResultDTo
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public GSM01000PrintColoumnGOADTO Column { get; set; }
        public List<GSM01000DataResultGOADTO> Data { get; set; }
    }

    public class GSM01000PrintGOAResultWithBaseHeaderPrintDTO : BaseHeaderReportCOMMON.BaseHeaderResult
    {
        public GSM01000PrintGOAResultDTo GOAData { get; set; }
    }
    #endregion


}