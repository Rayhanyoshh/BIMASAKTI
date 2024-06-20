using R_APICommonDTO;

namespace PMR02200Common.DTOs;

public class PMR02100SummaryDTO
{
    public string CCOMPANY_ID { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CJRNGRP_CODE { get; set; }
    public string CJRNGRP_NAME { get; set; }
    public string CCUSTOMER_ID { get; set; }
    public string CCUSTOMER_NAME { get; set; }
    public string CPHONE1 { get; set; }
    public string CEMAIL { get; set; }
    public string CADDRESS { get; set; }
    public decimal NAGE_NOT_DUE_AMOUNT { get; set; }
    public decimal NAGE_MORE_1_30_AMOUNT { get; set; }
    public decimal NAGE_MORE_31_60_AMOUNT { get; set; }
    public decimal NAGE_MORE_61_90_AMOUNT { get; set; }
    public decimal NAGE_MORE_91_120_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_120_AMOUNT { get; set; }
    public decimal NAGE_UNALLOCATED_RECEIPT_AMOUNT { get; set; }
    public decimal NAGE_PENALTY_AMOUNT { get; set; }
    public decimal NAGE_TOTAL_AMOUNT { get; set; }
    public string CUSER_ID { get; set; }
}
public class PMR02100RecordResult<T> : R_APIResultBaseDTO
{
    public T Data { get; set; }
}