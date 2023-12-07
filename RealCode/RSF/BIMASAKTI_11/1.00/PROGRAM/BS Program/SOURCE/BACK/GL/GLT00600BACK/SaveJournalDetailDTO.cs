namespace GLT00600Back;

public class SaveJournalDetailDTO
{
    public string CGLACCOUNT_NO { get; set; }
    public string CDBCR { get; set; }
    public string CDETAIL_DESC { get; set; } = "";
    public string CDOCUMENT_NO { get; set; } = "";
    public string CDOCUMENT_DATE { get; set; } = "";
    public string CCENTER_CODE { get; set; }
    public decimal NAMOUNT { get; set; }
}