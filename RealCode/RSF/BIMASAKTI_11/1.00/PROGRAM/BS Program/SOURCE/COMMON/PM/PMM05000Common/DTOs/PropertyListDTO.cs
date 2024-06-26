using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace PMM05000Common.DTOs
{
    public class PropertyListDataDTO :R_APIResultBaseDTO
    {
        public List<PropertyListDTO> Data { get; set; }
    }
    
    public class PropertyListDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CPROPERTY_ID { get; set; }
        public string CPROPERTY_NAME { get; set; }
        public bool LACTIVE { get; set; }
        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
    }
}

