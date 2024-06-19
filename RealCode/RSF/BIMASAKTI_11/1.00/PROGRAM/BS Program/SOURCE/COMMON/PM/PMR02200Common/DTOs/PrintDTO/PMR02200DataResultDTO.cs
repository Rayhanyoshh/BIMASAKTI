namespace PMR02200Common.DTOs.PrintDTO;

public class PMR02200DataResultDTO
{
    public string CCOMPANY_ID { get; set; }
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
    public decimal NAGE_MORE_THAN_30_DAYS_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_60_DAYS_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_90_DAYS_AMOUNT { get; set; }
    public decimal NAGE_MORE_THAN_120_DAYS_AMOUNT { get; set; }
}