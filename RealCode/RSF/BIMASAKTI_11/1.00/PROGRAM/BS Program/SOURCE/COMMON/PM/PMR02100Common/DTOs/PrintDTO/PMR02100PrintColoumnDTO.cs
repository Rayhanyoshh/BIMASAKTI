namespace PMR02100Common.DTOs.PrintDTO;

public class PMR02100PrintColoumnDTO
{
    public string HEADER_PROPERTY { get; set; } = "Property";
    public string HEADER_CUT_OFF_DATE { get; set; } = "Cut Off Date";
    public string HEADER_CUSTOMER { get; set; } = "Customer";
    public string HEADER_JOURNAL_GROUP { get; set; } = "Journal Group";
    public string HEADER_REPORT_TYPE { get; set; } = "Report Type";
    
    public string CHECKBOX_CUSTOMER_INFORMATION { get; set; } = "Customer Information";
    public string CHECKBOX_UNALLOCATED_RECEIPT { get; set; } = "Unallocated Receipt";
    public string CHECKBOX_PENALTY { get; set; } = "Penalty";
    public string CHECKBOX_INVOICE_GROUP { get; set; } = "Invoice Group";
    
    public string COLOUMN_JRN_GRP { get; set; } = "Journal Group";
    public string COLOUMN_UNIT_NAME { get; set; } = "Unit Name";
    public string COLOUMN_AGREEMENT_NO { get; set; } = "Agreement No";
    public string COLOUMN_INVOICE_NO { get; set; } = "Invoice No";
    public string COLOUMN_DUE_DATE { get; set; } = "Due Date";
    public string COLOUMN_CUSTOMER { get; set; } = "Customer";
    public string COLOUMN_PHONE { get; set; } = "Phone";
    public string COLOUMN_EMAIL { get; set; } = "Email";
    public string COLOUMN_ADDRESS { get; set; } = "Address";
    public string COLOUMN_NOT_DUE { get; set; } = "Not Due";
    public string COLOUMN_UNALLOCATED_RECEIPT { get; set; } = "Unallocated Receipt";
    public string COLOUMN_PENALTY { get; set; } = "Penalty";
    public string COLOUMN_1_30DAYS { get; set; } = "1-30 Days";
    public string COLOUMN_31_60DAYS { get; set; } = "31-60 Days";
    public string COLOUMN_61_90DAYS { get; set; } = "61-90 Days";
    public string COLOUMN_91_120DAYS { get; set; } = "91-120 Days";
    public string COLOUMN_120DAYS { get; set; } = "> 120 Days";
    public string COLOUMN_TOTAL { get; set; } = "Total";
    
    public string FOOTER_DESCRIPTION { get; set; } = "Description";
    public string FOOTER_GRAND_TOTAL { get; set; } = "Grand Total";
    public string FOOTER_SUB_TOTAL_BASED_ON_JRN_GRP { get; set; } = "Sub Total Based on";
}