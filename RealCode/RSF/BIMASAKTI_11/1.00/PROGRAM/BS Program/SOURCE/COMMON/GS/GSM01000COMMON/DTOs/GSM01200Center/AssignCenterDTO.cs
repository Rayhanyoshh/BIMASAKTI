using R_APICommonDTO;

namespace GSM01000Common.DTOs
{
    public class AssignCenterDTO
    {
        public string CCENTER_CODE { get; set; }
        
        public string CCENTER_NAME  { get; set; }

        public bool LSELECTED { get; set; }
    }
    
    public class AssignCenterResultDTO : R_APIResultBaseDTO
    {
        
    }
    
    public class CentertoAssignParam
    {
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CCENTER_LIST { get; set; }
        public string CUSER_ID { get; set; }
    }
}