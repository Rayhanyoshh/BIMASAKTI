namespace PMR01000Common.DTO_s.PrintDTO;

public class PMR01000PrintParamDTO
{
    public string CPROPERTY_ID { get; set; }
    public string CDEPOSIT_TYPE  { get; set; }
    public string CFROM_PERIOD { get { return CFROM_YEAR + CFROM_MONTH; } }
    public string CFROM_YEAR { get; set; }
    public string CFROM_MONTH { get; set; }
    public string CTO_PERIOD { get { return CTO_YEAR + CTO_MONTH; } }
    public string CTO_MONTH { get; set; }
    public string CTO_YEAR { get; set; }
    public string CFROM_BUILDING { get; set; }
    public string CTO_BUILDING { get; set; }
    public string CCUT_OFF_DATE { get; set; }
    public string CFROM_TYPE { get; set; }
    public string CTO_TYPE { get; set; }
    public string CFROM_TRANS_TYPE { get; set; }
    public string CTO_TRANS_TYPE { get; set; }
    public string CLANGUAGE_ID { get; set; }
}