using System;
using System.Collections.Generic;
using R_APICommonDTO;

namespace PMR01000Common.DTO_s;

public class PMR01000DTO
{
    public string CCOMPANY_ID { get; set; }
    public string CUSER_ID { get; set; }
    public string CLANGUAGE_ID { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CPROPERTY_NAME { get; set; }
    public string CDEPOSIT_ID  { get; set; }
    public string CDEPOSIT_NAME  { get; set; }
    public string CDEPOSIT_TYPE  { get; set; }
    public string CFROM_YEAR { get; set; }
    public string CTO_YEAR { get; set; }
    public string CFROM_MONTH { get; set; }
    public string CTO_MONTH { get; set; }
    public string CFROM_BUILDING { get; set; }
    public string CTO_BUILDING { get; set; }
    public string CCUT_OFF_DATE { get; set; }
    public string CFROM_TYPE { get; set; }
    public string CTO_TYPE { get; set; }
    public string CFROM_TRANS_TYPE { get; set; }
    public string CTO_TRANS_TYPE { get; set; }
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
public class PropertyListDataDTO :R_APIResultBaseDTO
{
    public List<PropertyListDTO> Data { get; set; }
}
public class PMR01000RecordResult<T> : R_APIResultBaseDTO
{
    public T Data { get; set; }
}




