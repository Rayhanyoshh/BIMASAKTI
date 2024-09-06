using System;

namespace PMR02100Common.DTOs.PrintDTO;

public class PMR02100PrintParamDTO
{
    public string CCOMPANY_ID { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CCUT_OFF_DATE { get; set; }
    public DateTime DCUT_OFF_DATE { get; set; }
    public string CREPORT_TYPE { get; set; }
    public string CFROM_CUSTOMER_ID { get; set; }
    public string CFROM_CUSTOMER_NAME { get; set; }
    public string CTO_CUSTOMER_ID { get; set; }
    public string CTO_CUSTOMER_NAME { get; set; }
    public string CFROM_JRNGRP_CODE { get; set; }
    public string CFROM_JRNGRP_NAME { get; set; }
    public string CTO_JRNGRP_CODE { get; set; }
    public string CTO_JRNGRP_NAME { get; set; }
    
    public string CINV_GRP_CODE { get; set; }
    public string CINV_GRP_NAME { get; set; }
    public string CLANG_ID { get; set; }
    
    public bool LCUSTOMER_INFORMATION { get; set; }
    public bool LUNALLOCATED_RECEIPT { get; set; }
    public bool LPENALTY { get; set; }
    public bool LINVOICE_GROUP{ get; set; }
    public bool LDESCRIPTION{ get; set; }
}