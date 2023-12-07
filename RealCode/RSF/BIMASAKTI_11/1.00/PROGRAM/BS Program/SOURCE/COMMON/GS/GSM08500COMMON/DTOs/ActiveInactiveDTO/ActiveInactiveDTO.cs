using R_APICommonDTO;

namespace GSM08500Common.DTOs
{
    public class ActiveInactiveDTO : R_APIResultBaseDTO
    {

    }
    
    public class ActiveInactiveParameterDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public bool LACTIVE { get; set; }
        public string CUSER_ID { get; set; }
    }
}