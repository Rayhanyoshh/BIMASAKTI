using System.Collections.Generic;
using BaseHeaderReportCOMMON;
using PMR02200Common.DTOs.PrintDTO;

namespace PMR02200Common.Model;

public class PMR02200DummyDataTest
{
    public static PMR02200PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
    {
        PMR02200PrintResultWithBaseHeaderPrintDTO loRtn = new PMR02200PrintResultWithBaseHeaderPrintDTO();

        loRtn.BaseHeaderData = new BaseHeaderDTO()
        {
            CCOMPANY_NAME = "PT Realta Chakradarma",
            CPRINT_CODE = "010",
            CPRINT_NAME = "Statement Of Account",
            CUSER_ID = "GHC",
        };
        loRtn.Data1 = new PMR02200PrintResultDTO();
        loRtn.Data1.Header = "Statement Of Account";
        loRtn.Data1.Title = "Overtime Detail Report";
        loRtn.Data1.Column = new PMR02200PrintColumnDTO();
        loRtn.Data1.Param = new PMR02200PrintParamDTO()
        {
            CCOMPANY_ID = "RCD",
            CPROPERTY_ID = "ASHMD",
            CFROM_CUSTOMER_ID = "",
            CTO_CUSTOMER_ID = "",
            CDEPT_CODE = "ACC",
            CFROM_LOI_NO = "",
            CTO_LOI_NO = "",
            CFROM_AGREEMENT_NO = "",
            CTO_AGREEMENT_NO = "",
            CCUT_OFF_DATE = "",
            CFROM_PERIOD = "",
            CTO_PERIOD = "",
            CSTATEMENT_DATE = "",
            LFILTER_INCLUDE_ZERO_BALANCE = true,
            CLANG_ID = "",
        };
        loRtn.Data1.Data = new List<PMR02200DataResultDTO>()
        {
           new PMR02200DataResultDTO
            {
                CCOMPANY_ID = "ABC123",
                CPROPERTY_ID = "12345",
                CTENANT_ID = "TEN001",
                CTENANT_NAME = "John Doe",
                CUNIT_DESCRIPTION = "Unit A",
                CREF_NO = "REF001",
                CINVOICE_NO = "INV001",
                CINVOICE_DATE = "2022-01-01",
                CDESCRIPTION = "Description of the invoice",
                CTYPE = "Type A",
                CCURRENCY_CODE = "USD",
                CCURRENCY_NAME = "US Dollar",
                NORIGINAL_AMOUNT = 10050,
                NOUTSTANDING_AMOUNT = 5025,
                NAGE_MORE_THAN_30_DAYS_AMOUNT = 2000,
                NAGE_MORE_THAN_60_DAYS_AMOUNT = 100,
                NAGE_MORE_THAN_90_DAYS_AMOUNT = 500,
                NAGE_MORE_THAN_120_DAYS_AMOUNT = 250
            },
            new PMR02200DataResultDTO
            {
                CCOMPANY_ID = "XYZ456",
                CPROPERTY_ID = "67890",
                CTENANT_ID = "TEN002",
                CTENANT_NAME = "Jane Smith",
                CUNIT_DESCRIPTION = "Unit B",
                CREF_NO = "REF002",
                CINVOICE_NO = "INV002",
                CINVOICE_DATE = "2022-02-01",
                CDESCRIPTION = "Another description",
                CTYPE = "Type B",
                CCURRENCY_CODE = "EUR",
                CCURRENCY_NAME = "Euro",
                NORIGINAL_AMOUNT = 7575,
                NOUTSTANDING_AMOUNT = 3000,
                NAGE_MORE_THAN_30_DAYS_AMOUNT = 1500,
                NAGE_MORE_THAN_60_DAYS_AMOUNT = 500,
                NAGE_MORE_THAN_90_DAYS_AMOUNT = 250,
                NAGE_MORE_THAN_120_DAYS_AMOUNT = 123
            },
            new PMR02200DataResultDTO
            {
                CCOMPANY_ID = "DEF789",
                CPROPERTY_ID = "54321",
                CTENANT_ID = "TEN003",
                CTENANT_NAME = "Alice Johnson",
                CUNIT_DESCRIPTION = "Unit C",
                CREF_NO = "REF003",
                CINVOICE_NO = "INV003",
                CINVOICE_DATE = "2022-03-01",
                CDESCRIPTION = "Yet another description",
                CTYPE = "Type C",
                CCURRENCY_CODE = "GBP",
                CCURRENCY_NAME = "British Pound",
                NORIGINAL_AMOUNT = 12000,
                NOUTSTANDING_AMOUNT = 6000,
                NAGE_MORE_THAN_30_DAYS_AMOUNT = 2500,
                NAGE_MORE_THAN_60_DAYS_AMOUNT = 1250,
                NAGE_MORE_THAN_90_DAYS_AMOUNT = 625,
                NAGE_MORE_THAN_120_DAYS_AMOUNT = 375
            }
        };
        return loRtn;
    }
}