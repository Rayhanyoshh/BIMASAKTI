namespace GSM008500Common.DTOs.PrintDTO
{
    public class GSM08500PrintParamStatAccDTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGL_ACCOUNT_FROM { get; set; }
        public string CGL_ACCOUNT_NAME_FROM { get; set; }
        public string CGL_ACCOUNT_TO { get; set; }
        public string CGL_ACCOUNT_NAME_TO { get; set; }
        public bool LBALANCE_SHEET { get; set; }
        public bool LINCOME_STATEMENT { get; set; }
        public string CSHORT_BY { get; set; }
        public string SortBy { get; set; }  
        public bool LPRINT_INACTIVE { get; set; }
        
        public string CUSER_LOGIN_ID { get; set; }
    }
    public class GSM08500PrintColoumnStatAccDTO
    {
        public string COLOUMN_ACCOUNT_NO { get; set; } = "Account No";
        public string COLOUMN_ACCOUNT_NAME { get; set; } = "Account Name";
        public string COLOUMN_CATEGORY { get; set; } = "BS/IS";
        public string COLOUMN_ACTIVE { get; set; } = "Active";
    }

    public class GSM08500ResultSPPrintStatAccDTO
    {
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public bool LACTIVE { get; set; }
    }
}