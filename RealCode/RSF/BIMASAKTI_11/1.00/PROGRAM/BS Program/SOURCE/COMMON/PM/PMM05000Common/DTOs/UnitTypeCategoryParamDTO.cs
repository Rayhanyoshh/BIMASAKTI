using System;
using System.Collections.Generic;
using R_CommonFrontBackAPI;

namespace PMM05000Common.DTOs;

public class UnitTypeCategoryParamDTO : PricingDTO
{
 
    public string CACTION { get; set; }
    public string CTYPE { get; set; }
    public string CVALID_FROM_DATE { get; set; }
}
