using System.Collections.Generic;
using BaseHeaderReportCOMMON;
using PMR02100Common.DTOs.PrintDTO;
using PMR02200Common.DTOs;

namespace PMR02200Common.Model;

public class PMR02102SummaryDummyData
{
    public static PMR02100SummaryPrintResultDTO DefaultData()
    {
        PMR02100SummaryPrintResultDTO loData = new PMR02100SummaryPrintResultDTO()
        {
            Title = "AR AGEING SUMMARY",
            Header = "",
            Column = new PMR02100PrintColoumnDTO()
        };

        loData.Param = new PMR02100PrintParamDTO
        {
            CCOMPANY_ID = "RCD",
            CPROPERTY_ID = "ASHMD",
            CCUT_OFF_DATE = "20240101",
            CFROM_CUSTOMER_ID = "TenantA",
            CTO_CUSTOMER_ID = "TenantA",
            CREPORT_TYPE = "Summary",
            LCUSTOMER_INFORMATION = true,
            LUNALLOCATED_RECEIPT = true,
            LPENALTY = false,
            CFROM_JRNGRP_CODE = "JRNGRP001",
            CTO_JRNGRP_CODE = "JRNGRP002",
            CINV_GRP_CODE = "INV_GRP001",
            CLANG_ID = "en",
        };
        
        int Data1 = 4;
        List<PMR02100SummaryDTO> loCollection = new List<PMR02100SummaryDTO>();
        for (int a = 1; a < Data1; a++)
        {
            loCollection.Add(new PMR02100SummaryDTO()
            {
                CCOMPANY_ID = $"RCD{a}",
                CPROPERTY_ID = $"ASHMD{a}",
                CJRNGRP_CODE = $"JRNGRP{a}",
                CJRNGRP_NAME = $"JRNGRPName{a}",
                CCUSTOMER_ID = $"Customer{a}",
                CCUSTOMER_NAME = $"CustomerName{a}",
                CPHONE1 = $"087234343787{a}",
                CEMAIL = $"rayhan{a}@gmail.com",
                CADDRESS = $"Jl.Petojo Binatu{a}",
                NAGE_NOT_DUE_AMOUNT = 100,
                NAGE_MORE_1_30_AMOUNT = 200,
                NAGE_MORE_31_60_AMOUNT = 300,
                NAGE_MORE_61_90_AMOUNT = 400,
                NAGE_MORE_91_120_AMOUNT = 500,
                NAGE_MORE_THAN_120_AMOUNT = 600,
                NAGE_UNALLOCATED_RECEIPT_AMOUNT = 700,
                NAGE_PENALTY_AMOUNT = 800,
                NAGE_TOTAL_AMOUNT = 1500,
                CUSER_ID = $"RYC"
            });
        }
            
        loData.DataResult = loCollection;
        return loData;
    }
    
    public static PMR02100PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
    {
        var loParam = new BaseHeaderDTO()
        {
            CCOMPANY_NAME = "PT Realta Chakradarma",
            CPRINT_CODE = "010",
            CPRINT_NAME = "AR AGEING SUMMARY",
            CUSER_ID = "RYC",
        };

        PMR02100PrintResultWithBaseHeaderPrintDTO loRtn = new PMR02100PrintResultWithBaseHeaderPrintDTO();

        loRtn.BaseHeaderData = loParam;
        loRtn.Data = DefaultData();

        return loRtn;
    }
}