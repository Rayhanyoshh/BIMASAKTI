using System.Collections.Generic;
using BaseHeaderReportCOMMON;
using PMR02100Common.DTOs;
using PMR02100Common.DTOs.PrintDTO;
using PMR02200Common.DTOs;

namespace PMR02200Common.Model;

public class PMR02102DetailDummyData
{
    public static PMR02100DetailPrintResultDTO DefaultData()
    {
        PMR02100DetailPrintResultDTO loData = new PMR02100DetailPrintResultDTO()
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
            CREPORT_TYPE = "Detail",
            LCUSTOMER_INFORMATION = true,
            LUNALLOCATED_RECEIPT = true,
            CFROM_JRNGRP_CODE = "JRNGRP001",
            CTO_JRNGRP_CODE = "JRNGRP002",
            CINV_GRP_CODE = "INV_GRP001",
            CLANG_ID = "en",
        };
        
        int Data1 = 4;
        int Data2 = 4;
        List<PMR02100DTO> loCollection = new List<PMR02100DTO>();
        for (int a = 1; a < Data1; a++)
        {
            for (int b = 1; b < Data2; b++)
            {
                loCollection.Add(new PMR02100DTO()
                {
                    CCOMPANY_ID = $"RCD{a}",
                    CPROPERTY_ID = $"ASHMD{a}",
                    CJRNGRP_CODE = $"JRNGRP{a}",
                    CJRNGRP_NAME = $"JRNGRPName{a}",
                    CCUSTOMER_ID = $"Customer{b}",
                    CCUSTOMER_NAME = $"CustomerName{b}",
                    CPHONE = $"087234343787{b}",
                    CEMAIL = $"rayhan{b}@gmail.com",
                    CADDRESS = $"Jl.Petojo Binatu{b}",
                    NAGE_NOT_DUE_AMOUNT = 1000000,
                    NAGE_MORE_1_30_AMOUNT = 2000000,
                    NAGE_MORE_31_60_AMOUNT = 300000,
                    NAGE_MORE_61_90_AMOUNT = 400000,
                    NAGE_MORE_91_120_AMOUNT = 500000,
                    NAGE_MORE_THAN_120_AMOUNT = 600000,
                    NAGE_UNALLOCATED_RECEIPT_AMOUNT = 7000000,
                    NAGE_PENALTY_AMOUNT = 8000000,
                    NAGE_TOTAL_AMOUNT = 1500000,
                    CUSER_ID = $"RYC", 
                    CINVOICE_NO = $"InvoiceNo{b}",
                    CINVOICE_DESCRIPTION = $"InvoiceDesc{b}",
                    CUNIT_DESCRIPTION = $"UnitDesc{b}",
                    CINVGRP_NAME = $"InvGroupName{b}",
                    CINVGRP_CODE = $"InvGroupCode{b}",
                    CINVGRP_DUE_DATE = $"13-12-202{b}",
                    CAGREEMENT_NO = $"AgreementNo{b}"
                });
            }
        }
            
        loData.DataResult = loCollection;
        return loData;
    }
    
    public static PMR02100DetailPrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
    {
        var loParam = new BaseHeaderDTO()
        {
            CCOMPANY_NAME = "PT Realta Chakradarma",
            CPRINT_CODE = "010",
            CPRINT_NAME = "AR AGEING SUMMARY",
            CUSER_ID = "RYC",
        };

        PMR02100DetailPrintResultWithBaseHeaderPrintDTO loRtn = new PMR02100DetailPrintResultWithBaseHeaderPrintDTO();

        loRtn.BaseHeaderData = loParam;
        loRtn.Data = DefaultData();

        return loRtn;
    }
}