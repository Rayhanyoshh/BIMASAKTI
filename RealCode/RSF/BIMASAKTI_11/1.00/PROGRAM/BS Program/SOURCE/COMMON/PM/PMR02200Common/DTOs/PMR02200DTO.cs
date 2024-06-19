using R_APICommonDTO;

namespace PMR02200Common.DTOs;

public class PMR02200DTO
{
    public string CCOMPANY_ID { get; set; }
    public string CUSER_ID { get; set; }
    public string CCUSTOMER_ID { get; set; }
    public string CCUSTOMER_NAME { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CTENANT_ID { get; set; }
    public string CTENANT_NAME { get; set; }
    public string CUNIT_DESCRIPTION { get; set; }
    public string CREF_NO  { get; set; }
    public string CINVOICE_NO  { get; set; }
    public string CINVOICE_DATE  { get; set; }
    public string CDESCRIPTION { get; set; }
    public string CTYPE { get; set; }
    public string CCURRENCY_CODE { get; set; }
    public string CCURRENCY_NAME { get; set; }
    public decimal NORIGINAL_AMOUNT { get; set; }
    public decimal NOUTSTANDING_AMOUNT { get; set; }
    public decimal NAGE_NOT_DUE_AMOUNT { get; set; }
    public decimal NAGE_CURRENT_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_30_DAYS_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_60_DAYS_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_90_DAYS_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_120_DAYS_AMOUNT { get; set; }

}
public class PMR02200RecordResult<T> : R_APIResultBaseDTO
{
    public T Data { get; set; }
}