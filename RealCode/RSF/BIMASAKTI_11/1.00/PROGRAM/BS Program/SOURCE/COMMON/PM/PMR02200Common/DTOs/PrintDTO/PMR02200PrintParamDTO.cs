namespace PMR02200Common.DTOs.PrintDTO;

public class PMR02200PrintParamDTO
{
    public string CCOMPANY_ID { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CDATE_BASE_ON { get; set; }
    public string CFROM_CUSTOMER_ID { get; set; }
    public string CFROM_CUSTOMER_NAME { get; set; }
    public string CTO_CUSTOMER_ID { get; set; }
    public string CTO_CUSTOMER_NAME { get; set; }
    public string CDEPT_CODE { get; set; }
    public string CDEPT_NAME { get; set; }
    public bool LLOI_NO { get; set; }
    public string CFROM_LOI_NO  { get; set; }
    public string CTO_LOI_NO  { get; set; }
    public bool LAGREEMENT_NO { get; set; }
    public string CFROM_AGREEMENT_NO  { get; set; }
    public string CTO_AGREEMENT_NO { get; set; }
    public string CCUT_OFF_DATE { get; set; }
    public string CFROM_PERIOD { get; set; }
    public string CTO_PERIOD { get; set; }
    public string CSTATEMENT_DATE { get; set; }
    public bool LFILTER_INCLUDE_ZERO_BALANCE { get; set; }
    public bool LFILTER_SHOW_AGE_TOTAL { get; set; }
    public string CLANG_ID { get; set; }

}