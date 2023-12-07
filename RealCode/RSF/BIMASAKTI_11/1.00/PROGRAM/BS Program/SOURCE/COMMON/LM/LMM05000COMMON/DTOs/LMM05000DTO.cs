using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using R_APICommonDTO;

namespace LMM05000Common.DTOs;

public class LMM05000DTO
{
    public string CCOMPANY_ID { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CUNIT_TYPE_ID { get; set; }
    public string CVALID_INTERNAL_ID { get; set; }
    public string CVALID_DATE { get; set; }
    public DateTime DVALID_DATE { get; set; }
    public string CBY_SQM_TOTAL { get; set; }
    public decimal NNORMAL_PRICE_SQM { get; set;}
    public decimal NNORMAL_SELLING_PRICE { get; set; }
    public decimal NBOTTOM_PRICE_SQM { get; set; }
    public decimal NBOTTOM_SELLING_PRICE { get; set;}
    public bool LOVERWRITE { get; set; }
    public bool LACTIVE { get; set; }
    public string CCREATE_BY { get; set; }
    public DateTime DCREATE_DATE { get; set; }
    public string CUPDATE_BY { get; set; }
    public DateTime DUPDATE_DATE { get; set; }
    public string CUSER_ID { get; set; }
}
public class LMM05000ListDTO : R_APIResultBaseDTO
{
    public List<LMM05000DTO> Data { get; set; }
}