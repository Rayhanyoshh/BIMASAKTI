namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01003ResultPrintSPDTO
{
    public string CDEPOSIT_ID  { get; set; }
    public string CDEPOSIT_NAME  { get; set; }
    public string CBUILDING_ID  { get; set; }
    public string CDEPOSIT_TYPE  { get; set; }
    public string CCUSTOMER_NAME  { get; set; }
    public string CTRANSACTION_TYPE  { get; set; }
    public string CTRANSACTION_NO  { get; set; }
    public string CREFERENCE_NO  { get; set; }
    public string CSTATUS  { get; set; }
    public string CTYPES  { get; set; }
    public string CUNIT_DESCRIPTION { get; set; }
    public string CINVOICE_NO { get; set; }
    public string CDEPOSIT_DATE  { get; set; }
    public string CPAYMENT_STATUS  { get; set; }
    public string CCURRENCY_CODE  { get; set; }
    public decimal NAMOUNT  { get; set; }
    public decimal NCREDIT_NOTE  { get; set; }
    public decimal NADJUSTMENT  { get; set; }
    public decimal NREFUND  { get; set; }
    public decimal NDEPOSIT_AMOUNT  { get; set; }
    public decimal NDEPOSIT_BALANCE  { get; set; }
    
    //DETAIL
    public string CDETAIL_DEPOSIT_TYPE  { get; set; }
    public string CDETAIL_TRANSACTION_NO  { get; set; }
    public string CDETAIL_DEPOSIT_DATE  { get; set; }
    public string CDETAIL_PAYMENT_STATUS  { get; set; }
    public string CDETAIL_CURRENCY_CODE  { get; set; }
    public decimal? NDETAIL_AMOUNT  { get; set; }
    public decimal? NDETAIL_DEPOSIT_BALANCE  { get; set; }
}