using R_APICommonDTO;

namespace GSM08500Common.DTOs
{
    public class CopyFromProcess : R_APIResultBaseDTO
    {
    }
    
    public class CopyFromProcessParameter
    {
        public string CLOGIN_COMPANY_ID { get; set; }
        public string CCOPY_FROM_COMPANY_ID { get; set; }
        public string CUSER_ID { get; set; }
    }
}