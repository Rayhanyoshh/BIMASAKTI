using System;
using System.Collections.Generic;
using R_APICommonDTO;

namespace LMM05000Common.DTOs;

public class LMM05010DTO
{
    public string CCOMPANY_ID { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CUNIT_TYPE_ID { get; set; }
    public string CUNIT_TYPE_NAME { get; set; }
    public bool LACTIVE { get; set; }
    public string CDESCRIPTION { get; set; }
    public string CCREATE_BY { get; set; }
    public DateTime DCREATE_DATE { get; set; }
    public string CUPDATE_BY { get; set; }
    public DateTime DUPDATE_DATE { get; set; }

    
}
public class UnitTypeDataDTO : R_APIResultBaseDTO
{
    public List<LMM05010DTO> Data { get; set; }
}