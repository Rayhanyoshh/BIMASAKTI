using R_APICommonDTO;

namespace LMM05000Common.DTOs
{
    public class ActiveInactiveDTO : R_APIResultBaseDTO
    {

    }
    
    public class ActiveInactiveParameterDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CPROPERTY_ID { get; set; }
        public string CUNIT_TYPE_ID { get; set; }
        public string CVALID_INTERNAL_ID { get; set; }
        public bool LACTIVE { get; set; }
        public string CUSER_ID { get; set; }


    }
}