namespace TXR00100Common.PrintDTO;

public class TXR00100DataResultDTO
{
    public string CCUSTOMER_NAME { get; set; }
    public string CTAX_ADDRESS { get; set; }
    public string CAGREEMENT_NO { get; set; }
    public string CUNIT_DESCRIPTION { get; set; }
    public string CINVOICE_NO { get; set; }
    public string CINVOICE_DATE { get; set; }
    public string CCURRENCY_CODE { get; set; }
    public decimal NWH_TAX_AMOUNT { get; set; }
    public string CWH_TAX_SLIP_NO { get; set; }
    public string CWH_TAX_SLIP_DATE { get; set; }
    public decimal NWH_TAX_SLIP_AMOUNT { get; set; }
    public bool LWH_TAX_COLLECT { get; set; }
}