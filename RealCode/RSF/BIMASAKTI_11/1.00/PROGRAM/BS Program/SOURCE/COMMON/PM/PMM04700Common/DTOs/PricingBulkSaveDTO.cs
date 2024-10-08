namespace PMM04700Common.DTOs;


public class PricingBulkSaveDTO : PricingDTO
{
    public int ISEQ { get; set; } = 0;
    public string CCHARGES_TYPE_ID { get; set; }
}
public class PricingDBSaveBulkDTO
{
    public int ISEQ { get; set; } = 0;
    public string CVALID_INTERNAL_ID { get; set; } = "";
    public string CCHARGES_TYPE { get; set; }
    public string CCHARGES_ID { get; set; }
    public string CPRICE_MODE { get; set; }
    public decimal NNORMAL_PRICE { get; set; }
    public decimal NBOTTOM_PRICE { get; set; }
    public bool LOVERWRITE { get; set; }
}
