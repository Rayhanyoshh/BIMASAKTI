using System;
using System.Collections.Generic;
using System.Text;
using LMM03700Common.DTOs;

namespace LMM03700Common.DTOs
{
    public class TenantParamDTO : TenantGridDTO
    {
        public string CTENANT_ID_LIST_COMMA_SEPARATOR { get; set; }
    }
}