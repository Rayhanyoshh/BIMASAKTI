using System.Collections.Generic;

namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01003DataResultChild2DTO
{
    public string CCUSTOMER_NAME  { get; set; }
    public string CTRANSACTION_TYPE  { get; set; }
    public string CREFERENCE_NO  { get; set; }
    public string CSTATUS  { get; set; }
    public string CTYPES  { get; set; }
    public string CTRANSACTION_NO  { get; set; }
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
    public List<PMR01003DataResultChild3DTO> Detail3 { get; set; }

  
}