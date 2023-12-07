using System;
using System.Collections.Generic;
using System.Text;
using R_APICommonDTO;

namespace GSM01000Common.DTOs
{
    #region COA
    public class GSM01000PrintParamCOADTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGL_ACCOUNT_FROM { get; set; }
        public string CGL_ACCOUNT_NAME_FROM { get; set; }
        public string CGL_ACCOUNT_TO { get; set; }
        public string CGL_ACCOUNT_NAME_TO { get; set; }
        public bool LBALANCE_SHEET { get; set; }
        public bool LINCOME_STATEMENT { get; set; }
        public string CSHORT_BY { get; set; }
        public bool LPRINT_INACTIVE { get; set; }
        public bool LPRINT_USER_RESTRICT { get; set; }
        public bool LPRINT_CENTER_RESTRICT { get; set; }
        public string CUSER_LOGIN_ID { get; set; }
        public string SortBy { get; set; }

    }

    public class GSM01000PrintColoumnCOADTO
    {
        public string COLOUMN_ACCOUNT_NO { get; set; } = "Account No";
        public string COLOUMN_ACCOUNT_NAME { get; set; } = "Account Name";
        public string COLOUMN_CATEGORY { get; set; } = "Category";
        public string COLOUMN_CASHFLOW { get; set; } = "Cash Flow";
        public string COLOUMN_ACTIVE { get; set; } = "Active";
    }

    public class GSM01000ResultSPPrintCOADTO
    {
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public string CCASH_FLOW_CODE { get; set; }
        public string CCASH_FLOW_NAME { get; set; }
        public bool LACTIVE { get; set; }
        public string CUSER_NAME_LIST { get; set; }
        public string CCENTER_NAME_LIST { get; set; }
    }

    #endregion

    #region GOA

    public class GSM01000PrintParamGOADTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGOA_CODE_FROM { get; set; }
        public string CGOA_CODE_NAME_FROM { get; set; }
        public string CGOA_CODE_TO { get; set; }
        public string CGOA_CODE_NAME_TO { get; set; }
        public bool LBALANCE_SHEET { get; set; }
        public bool LINCOME_STATEMENT { get; set; }
        public string CSHORT_BY { get; set; }
        public string SortBy { get; set; }
        public bool LPRINT_GL_ACCOUNT { get; set; }
        public bool LPRINT_INACTIVE { get; set; }
        public string CUSER_LOGIN_ID { get; set; }

    }
    
    public class GSM01000ResultSPPrintGOADTO
    {
        public string CCOMPANY_ID { get; set; }
        public string CGOA_CODE { get; set; }
        public string CGOA_NAME { get; set; }
        public string CGOA_BSIS { get; set; }
        public string CGOA_DBCR { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public string CCASH_FLOW_CODE  { get; set; }
        public string CCASH_FLOW_NAME  { get; set; }
        public bool LACTIVE  { get; set; }
    }

    public class GSM01000DataResultGOADTO
    {
        public string CGOA_CODE { get; set; }
        public string CGOA_NAME { get; set; }
        public string CGOA_BSIS { get; set; }
        public string CGOA_DBCR { get; set; }
        
        public List<GSM01001DataResultGOADTO> GSM01001DataResultGOADTO { get; set; }
    }
    
    public class GSM01001DataResultGOADTO
    {
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public string CCASH_FLOW_CODE  { get; set; }
        public string CCASH_FLOW_NAME  { get; set; }
        public bool LACTIVE  { get; set; }
    }

    
    public class GSM01000PrintColoumnGOADTO
    {
        public string COLOUMN_CODE { get; set; } = "No";
        public string COLOUMN_NAME { get; set; } = "Name";
        public string COLOUMN_CATEGORY { get; set; } = "Category";
        public string COLOUMN_CASHFLOW { get; set; } = "Cash Flow";
        public string COLOUMN_ACTIVE { get; set; } = "Active";
    }
    
    #endregion


}