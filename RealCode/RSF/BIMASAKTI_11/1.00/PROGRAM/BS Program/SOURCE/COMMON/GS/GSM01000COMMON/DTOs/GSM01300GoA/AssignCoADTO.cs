using R_APICommonDTO;

namespace GSM01000Common.DTOs
{
    public class AssignCoADTO
    {
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public bool LSELECTED { get; set; }

    }

    public class COAtoAssignParam
    {
        public string CCOMPANY_ID { get; set; }
        public string CGOA_CODE { get; set; }
        public string CCOA_LIST { get; set; }
        public string CUSER_ID { get; set; }
       
    }
    
    public class AssignCOAResultDTO : R_APIResultBaseDTO
    {
        
    }

}