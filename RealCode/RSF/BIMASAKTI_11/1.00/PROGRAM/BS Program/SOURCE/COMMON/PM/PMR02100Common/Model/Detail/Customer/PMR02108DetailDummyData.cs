using System.Collections.Generic;
using BaseHeaderReportCOMMON;
using PMR02100Common.DTOs;
using PMR02100Common.DTOs.PrintDTO;
using PMR02200Common.DTOs;

namespace PMR02200Common.Model;

public class PMR02108DetailDummyData
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
            LPENALTY = true,
            LINVOICE_GROUP = true,
            CFROM_JRNGRP_CODE = "JRNGRP001",
            CTO_JRNGRP_CODE = "JRNGRP002",
            CINV_GRP_CODE = "INV_GRP001",
            CLANG_ID = "en",
        };
        
          int Data1 = 4;
        int Data2 = 4;
        int Data3 = 4;
        int Data4 = 4;
        List<PMR02100DTO> loCollection = new List<PMR02100DTO>();
        for (int a = 1; a < Data1; a++)
        {
            for (int b = 1; b < Data2; b++)
            {
                for (int c = 1; c < Data3; c++)
                {
                    for (int d = 1; d < Data4; d++)
                    {
                        loCollection.Add(new PMR02100DTO()
                        {
                            CCOMPANY_ID = $"RCD{a}",
                            CPROPERTY_ID = $"ASHMD{a}",
                            CJRNGRP_CODE = $"JRNGRP{a}",
                            CJRNGRP_NAME = $"JRNGRPName{a}",
                            CCUSTOMER_ID = $"MU0{b}",
                            CCUSTOMER_NAME = $"Muhammad Andika Putra{b}",
                            CPHONE = $"087234343787{b}",
                            CEMAIL = $"rayhan{b}@gmail.com",
                            CADDRESS = $"Jl.Petojo Binatu{b}",
                            NAGE_NOT_DUE_AMOUNT = 100,
                            NAGE_MORE_1_30_AMOUNT = 200,
                            NAGE_MORE_31_60_AMOUNT = 300,
                            NAGE_MORE_61_90_AMOUNT = 400,
                            NAGE_MORE_91_120_AMOUNT = 500,
                            NAGE_MORE_THAN_120_AMOUNT = 600,
                            NAGE_UNALLOCATED_RECEIPT_AMOUNT = 700,
                            NAGE_PENALTY_AMOUNT = 800,
                            NAGE_TOTAL_AMOUNT = 1500,
                            CUNIT_DESCRIPTION = $"PT. Pasti Bisa{c}",
                            CAGREEMENT_NO = $"AGREE-REF-0{c}",
                            CINVOICE_NO = $"INV-REF-01-0{d}",
                            CINVGRP_DUE_DATE = $"13-Jan-202{d}",
                            CINVGRP_CODE = $"SC{d}",
                            CINVGRP_NAME = $"Service Charge{d}",
                            CINVOICE_DESCRIPTION = $"Air,Listrik,Service Charge{d}",
                        });
                    }
                }
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