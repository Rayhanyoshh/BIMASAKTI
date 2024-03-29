using System;
using System.Collections.Generic;
using System.Text;

namespace LMM03700Common.DTOs
{
    public class TenantGridDTO : TenantDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CPROPERTY_ID { get; set; }
        public string CTENANT_CLASSIFICATION_GROUP_ID { get; set; }
        public string CTENANT_CLASSIFICATION_ID { get; set; }
        public string CUSER_ID { get; set; }
        public bool LCHECKED { get; set; }
    }
}