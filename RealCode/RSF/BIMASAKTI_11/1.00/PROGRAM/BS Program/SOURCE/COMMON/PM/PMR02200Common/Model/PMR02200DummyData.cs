using System.Collections.Generic;
using BaseHeaderReportCOMMON;
using PMR02200Common.DTOs;
using PMR02200Common.DTOs.PrintDTO;

namespace PMR02200Common.Model;

public class PMR02200DummyData
{
    public static PMR02200PrintResultDTO DefaultData()
    {
        PMR02200PrintResultDTO loData = new PMR02200PrintResultDTO()
        {
            Title = "Statement Of Account",
            Header = "",
            Column = new PMR02200PrintColumnDTO()
        };

        int Data1 = 4;
            
        List<PMR02200DTO> loCollection = new List<PMR02200DTO>();
        for (int a = 1; a < Data1; a++)
        {
            loCollection.Add(new PMR02200DTO()
            {
                CCOMPANY_ID = $"RCD{a}",
                CPROPERTY_ID = $"ASHMD{a}",
                CTENANT_ID = $"TEN001{a}",
                CTENANT_NAME = $"John Doe{a}",
                CUNIT_DESCRIPTION = $"Service Charge{a}",
                CREF_NO = $"REF-AGREE-0{a}",
                CINVOICE_NO = $"REF-INV_AGREE-1-{a}",
                CINVOICE_DATE = $"2022-01-0{a}",
                CDESCRIPTION = $"Description of the invoice{a}",
                CTYPE = $"Type A{a}",
                CCURRENCY_CODE = $"USD",
                CCURRENCY_NAME = $"US Dollar",
                NORIGINAL_AMOUNT = 10050,
                NOUTSTANDING_AMOUNT = 5025,
                NAGE_NOT_DUE_AMOUNT = 0,
                NAGE_CURRENT_AMOUNT = 10050,
                NAGE_MORE_THAN_30_DAYS_AMOUNT = 2000,
                NAGE_MORE_THAN_60_DAYS_AMOUNT = 100,
                NAGE_MORE_THAN_90_DAYS_AMOUNT = 500,
                NAGE_MORE_THAN_120_DAYS_AMOUNT = 250
            });
        }
            
        loData.DataResult = loCollection;
        loData.Param = new PMR02200PrintParamDTO
        {
            CCOMPANY_ID = "RCD",
            CPROPERTY_ID = "ASHMD",
            CFROM_CUSTOMER_ID = "TenantA",
            CTO_CUSTOMER_ID = "TenantA",
            CDEPT_CODE = "ACC",
            CFROM_LOI_NO = "LOI1",
            CTO_LOI_NO = "LOI1",
            CFROM_AGREEMENT_NO = "AGREE1",
            CTO_AGREEMENT_NO = "AGREE2",
            CCUT_OFF_DATE = "20240101",
            CFROM_PERIOD = "20240101",
            CTO_PERIOD = "20241201",
            CSTATEMENT_DATE = "20240614",
            LFILTER_INCLUDE_ZERO_BALANCE = true,
            LFILTER_SHOW_AGE_TOTAL = false,
            CLANG_ID = "en",
        };
        return loData;
    }
    
    public static PMR02200PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
    {
        var loParam = new BaseHeaderDTO()
        {
            CCOMPANY_NAME = "PT Realta Chakradarma",
            CPRINT_CODE = "010",
            CPRINT_NAME = "Statement Of Account",
            CUSER_ID = "RYC",
        };

        PMR02200PrintResultWithBaseHeaderPrintDTO loRtn = new PMR02200PrintResultWithBaseHeaderPrintDTO();

        loRtn.BaseHeaderData = loParam;
        loRtn.Data1 = DefaultData();

        return loRtn;
    }
}