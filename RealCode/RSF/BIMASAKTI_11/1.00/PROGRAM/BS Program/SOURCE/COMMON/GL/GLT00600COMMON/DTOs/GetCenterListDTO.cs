using System.Collections.Generic;
using R_APICommonDTO;

namespace GLT00600Common.DTOs
{
    public class GetCenterListDTO  : R_APIResultBaseDTO
    {
        public List<GetCenterDTO> Data { get; set; }
    }
    public class GetCenterDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CCENTER_CODE { get; set; }
        public string CCENTER_NAME { get; set; }
        public string LACTIVE { get; set; }
        public string CCREATE_BY { get; set; }
        public string DCREATE_DATE { get; set; }
        public string CUPDATE_BY { get; set; }
        public string DUPDATE_DATE { get; set; }
    }
}