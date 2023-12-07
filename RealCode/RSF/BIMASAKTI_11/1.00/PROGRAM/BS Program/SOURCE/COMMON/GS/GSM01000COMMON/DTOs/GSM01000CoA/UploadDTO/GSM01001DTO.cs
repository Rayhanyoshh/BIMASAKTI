using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM01000Common.DTOs
{
    public class  GSM01001DTO
    {
        // Result
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string BSIS { get; set; }
        public string DC { get; set; }
        public bool Active { get; set; }
        public bool CenterRestriction { get; set; }
        public bool UserRestriction { get; set; }
        public string NonActiveDate { get; set; }
        public string ErrorMessage { get; set; }
        public bool Var_Exists { get; set; }
        public bool Var_Overwrite { get; set; }
    
    }
    
    public class  GSM01001ExcelDTO
    {
        // Result
        public string AccountNo { get; set; }
        public string AccountName { get; set; }
        public string BSIS { get; set; }
        public string DC { get; set; }
        public bool Active { get; set; }
        public bool CenterRestriction { get; set; }
        public bool UserRestriction { get; set; }
        public string NonActiveDate { get; set; }
        public string ErrorMessage { get; set; }
    }
    
    public class GSM01001ErrorValidateDTO
    {
        // result
        public int NO { get; set; }
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public bool LACTIVE { get; set; }
        public bool LCENTER_RESTR { get; set; }
        public bool LUSER_RESTR { get; set; } 
        public bool LEXIST { get; set; } 
        public string NonActiveDate { get; set; }
        public DateTime?  NonActiveDateDisplay { get; set; }
        public string ErrorMessage { get; set; }
        public string Valid { get; set; }
        public bool Var_Exists { get; set; }
        public bool Var_Overwrite { get; set; }
    }
    
    
    public class GSM01001RequestDTO
    {
        // Result
        public int NO { get; set; }
        public string CCOMPANY_ID { get; set; }
        public string CGLACCOUNT_NO { get; set; }
        public string CGLACCOUNT_NAME { get; set; }
        public string CBSIS { get; set; }
        public string CDBCR { get; set; }
        public bool LACTIVE { get; set; }
        public bool LCENTER_RESTR { get; set; }
        public bool LUSER_RESTR { get; set; } 
        public bool LEXIST { get; set; } 
        public string NonActiveDate { get; set; }
    }
    
    public class GSM01001ErrorValidateParamDTO
    {
        // Result
        public string CCOMPANY_ID { get; set; }
        public string CUSER_ID { get; set; }
        public string KEY_GUID { get; set; }
    }
}