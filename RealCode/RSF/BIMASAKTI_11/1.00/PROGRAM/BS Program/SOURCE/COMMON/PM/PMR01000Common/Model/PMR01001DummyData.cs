using System.Collections.Generic;
using System.Linq;
using BaseHeaderReportCOMMON;
using PMR01000Common.DTO_s;
using PMR01000Common.DTO_s.PrintDTO;

namespace PMR01000Common.Model;

public static class PMR01001DummyData
{
    public static PMR01001PrintResultDTO DefaultData()
    {
        PMR01000PrintParamDTO PrintParam = new PMR01000PrintParamDTO()
        {
            CFROM_YEAR = "2024",
            CFROM_MONTH = "02",
            
            CTO_YEAR = "2024",
            CTO_MONTH = "05",
            CFROM_BUILDING = "TM01",
            CTO_BUILDING = "TM02",
        };
        
        PMR01001PrintResultDTO loData = new PMR01001PrintResultDTO()
        {
            Title = "Deposit List",
            Header = "",
            HeaderParam = PrintParam,
            Column = new PMR01001PrintColoumnDTO()
        };

        int Data1 = 4;
        int Data2 = 2;
        int Data3 = 4;

        List<PMR01000ResultPrintSPDTO> loCollection = new List<PMR01000ResultPrintSPDTO>();
        for (int a = 1; a <= Data1; a++)
        {
            for (int b = 1; b <= Data2; b++)
            {
                for (int c = 1; c <= Data3; c++)
                {
                    loCollection.Add(new PMR01000ResultPrintSPDTO()
                    {
                        CDEPOSIT_ID = $"DepId{a}",
                        CDEPOSIT_NAME = $"DepoName{a}",
                
                        CBUILDING_ID = $"TM{b}",
                        CDEPOSIT_TYPE = $"Contractor{b}",
                
                        CCUSTOMER_NAME = $"BAXTER-CV SINAR MANDIRI GLASS INDO{c}",
                        CTRANSACTION_TYPE = $"LOI{c}",
                        CREFERENCE_NO = $"044/LOO/TM/VII/{c}",
                        CSTATUS = $"Activ{c}",
                        CUNIT_DESC = $"Desc{c}",
                        CINVOICE_NO = $"SDE/2021090005{c}",
                        CDEPOSIT_DATE = $"30/09/2021{c}",
                        CPAYMENT_STATUS = $"Paid{c}",
                        CCURRENCY_CODE = $"IDR{c}",
                        NDEPOSIT_AMOUNT = 3000000,
                        NDEPOSIT_BALANCE = 3000000,
                    });
                }
            }
        }

        var loTempData = loCollection
            .GroupBy(data1a => new
            {
                data1a.CDEPOSIT_ID,
                data1a.CDEPOSIT_NAME,
            }).Select(data1b => new PMR01001DataResultDTO()
            {
                CDEPOSIT_ID = data1b.Key.CDEPOSIT_ID,
                CDEPOSIT_NAME = data1b.Key.CDEPOSIT_NAME,
                Detail1 = data1b.GroupBy(data2a => new
                {
                   data2a.CBUILDING_ID,
                   data2a.CDEPOSIT_TYPE,
                   
                }).Select(data2b => new PMR01001DataResultChild1DTO()
                {
                    CBUILDING_ID = data2b.Key.CBUILDING_ID,
                    CDEPOSIT_TYPE = data2b.Key.CDEPOSIT_TYPE,
                    Detail2 = data2b.GroupBy(data2b => new
                    {
                        data2b.CCUSTOMER_NAME,
                        data2b.CTRANSACTION_TYPE,
                        data2b.CREFERENCE_NO,
                        data2b.CSTATUS,
                        data2b.CUNIT_DESC,
                        data2b.CINVOICE_NO,
                        data2b.CDEPOSIT_DATE,
                        data2b.CPAYMENT_STATUS,
                        data2b.CCURRENCY_CODE,
                        data2b.NDEPOSIT_AMOUNT,
                        data2b.NDEPOSIT_BALANCE,
                    }).Select(data3b => new PMR01001DataResultChild2DTO()
                    {
                        CCUSTOMER_NAME = data3b.Key.CCUSTOMER_NAME,
                        CTRANSACTION_TYPE = data3b.Key.CTRANSACTION_TYPE,
                        CREFERENCE_NO = data3b.Key.CREFERENCE_NO,
                        CSTATUS = data3b.Key.CSTATUS,
                        CUNIT_DESC = data3b.Key.CUNIT_DESC,
                        CINVOICE_NO = data3b.Key.CINVOICE_NO,
                        CDEPOSIT_DATE = data3b.Key.CDEPOSIT_DATE,
                        CPAYMENT_STATUS = data3b.Key.CPAYMENT_STATUS,
                        CCURRENCY_CODE = data3b.Key.CCURRENCY_CODE,
                        NDEPOSIT_AMOUNT = data3b.Key.NDEPOSIT_AMOUNT,
                        NDEPOSIT_BALANCE = data3b.Key.NDEPOSIT_BALANCE,
                    }).ToList()
                }).ToList()
            }).ToList();
           

        loData.Data = loTempData;
        return loData;
    }

    public static PMR01001PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
    {
        var loParam = new BaseHeaderDTO()
        {
            CCOMPANY_NAME = "PT Realta Chakradarma",
            CPRINT_CODE = "010",
            CPRINT_NAME = "DEPOSIT TYPE = LIST",
            CUSER_ID = "RYC",
        };

        PMR01001PrintResultWithBaseHeaderPrintDTO loRtn = new PMR01001PrintResultWithBaseHeaderPrintDTO();

        loRtn.BaseHeaderData = loParam;
        loRtn.Data1 = DefaultData();

        return loRtn;

    }
}