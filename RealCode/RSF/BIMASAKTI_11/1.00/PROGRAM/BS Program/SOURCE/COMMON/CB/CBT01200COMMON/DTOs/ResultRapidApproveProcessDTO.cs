using System.Collections.Generic;
using R_APICommonDTO;

namespace CBT01200Common.DTOs
{

    public class ResultRapidApproveProcessDTO
    {
        public bool LSUCCESSED { get; set; }
        public string CREC_ID { get; set; }
    }

    public class ResultRapidApproveProcessListDTO : R_APIResultBaseDTO
    {
        public List<ResultRapidApproveProcessDTO> Data { get; set; }
    }
}