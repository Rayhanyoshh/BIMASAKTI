using System.Collections.Generic;
using System.Linq;
using BaseHeaderReportCOMMON;
using PMR01000Common.DTO_s.PrintDTO;

namespace PMR01000Common.Model;

public static class PMR01002DummyData
{
    public static PMR01002PrintResultDTO DefaultData()
    {
        PMR01000PrintParamDTO PrintParam = new PMR01000PrintParamDTO()
        {
            CFROM_YEAR = "2024",
            CFROM_MONTH = "02",
            CTO_YEAR = "2024",
            CTO_MONTH = "05",
            CCUT_OFF_DATE = "30 Sep 2024"
        };
        
        PMR01002PrintResultDTO loData = new PMR01002PrintResultDTO()
        {
            Title = "Deposit Outstanding",
            Header = "",
            HeaderParam = PrintParam,
            Column = new PMR01002PrintColoumnDTO()
        };
        
        int Data1 = 3;
        int Data2 = 3;
        int Data3 = 5;

        List<PMR01000ResultPrintSPDTO> loCollection = new List<PMR01000ResultPrintSPDTO>();
        for (int a = 1; a <= Data1; a++)
        {
            for (int b = 1; b <= Data2; b++)
            {
                for (int c = 1; c <= Data3; c++)
                {
                    loCollection.Add(new PMR01000ResultPrintSPDTO()
                    {
                        CACCOUNT_NO = $"2111.001{a}",
                        CACCOUNT_NAME = $"Fit Out Deposit Contractor{a}",
                        
                        CBUILDING_ID = $"TM{b}",
                        CDEPOSIT_TYPE = $"Contractor{b}",
                    
                        CCUSTOMER_NAME = $"CIMB - PT BANK CIMB NIAGA TBK{c}",
                        CTRANSACTION_TYPE = $"LOI{c}",
                        CREFERENCE_NO = $"044/LOO/TM/VII/2020{c}",
                        CSTATUS = $"Active{c}",
                        CUNIT_DESC = $"{c}",
                        CDEPOSIT_ID = $"211110{c}",
                        CDEPOSIT_NAME = $"Fit Out Deposit{c}",
                        CINVOICE_NO = $"SDE/2021090005{c}",
                        CDEPOSIT_DATE = $"30/09/2021{c}",
                        CPAYMENT_STATUS = $"Paid{c}",
                        CCURRENCY_CODE = $"IDR{c}",
                        NDEPOSIT_AMOUNT = 100,
                        NDEPOSIT_BALANCE = 300,
                        NLOCAL_DEPOSIT_BALANCE = 200
                    });
                }
            }
        }

        
      var loTempData = loCollection
            .GroupBy(data1a => new
            {
                data1a.CACCOUNT_NO,
                data1a.CACCOUNT_NAME,
            }).Select(data1b => new PMR01002DataResultDTO()
            {
                CACCOUNT_NO = data1b.Key.CACCOUNT_NO,
                CACCOUNT_NAME = data1b.Key.CACCOUNT_NAME,
                Detail1 = data1b.GroupBy(data2a => new
                {
                   data2a.CBUILDING_ID,
                   data2a.CDEPOSIT_TYPE,
                   
                }).Select(data2b => new PMR01002DataResultChild1DTO()
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
                        data2b.CDEPOSIT_ID,
                        data2b.CDEPOSIT_NAME,
                        data2b.CINVOICE_NO,
                        data2b.CDEPOSIT_DATE,
                        data2b.CPAYMENT_STATUS,
                        data2b.CCURRENCY_CODE,
                        data2b.NDEPOSIT_AMOUNT,
                        data2b.NDEPOSIT_BALANCE,
                        data2b.NLOCAL_DEPOSIT_BALANCE
                    }).Select(data3b => new PMR01002DataResultChild2DTO()
                    {
                        CCUSTOMER_NAME = data3b.Key.CCUSTOMER_NAME, 
                        CTRANSACTION_TYPE = data3b.Key.CTRANSACTION_TYPE,
                        CREFERENCE_NO = data3b.Key.CREFERENCE_NO,
                        CSTATUS = data3b.Key.CSTATUS,
                        CUNIT_DESC = data3b.Key.CUNIT_DESC,
                        CDEPOSIT_ID = data3b.Key.CDEPOSIT_ID,
                        CDEPOSIT_NAME = data3b.Key.CDEPOSIT_NAME,
                        CDEPOSIT_DATE = data3b.Key.CDEPOSIT_DATE,
                        CPAYMENT_STATUS = data3b.Key.CPAYMENT_STATUS,
                        CCURRENCY_CODE = data3b.Key.CCURRENCY_CODE,
                        NDEPOSIT_AMOUNT = data3b.Key.NDEPOSIT_AMOUNT,
                        NDEPOSIT_BALANCE = data3b.Key.NDEPOSIT_BALANCE,
                        NLOCAL_DEPOSIT_BALANCE = data3b.Key.NDEPOSIT_AMOUNT
                    }).ToList()
                }).ToList()
            }).ToList();
           

        loData.Data = loTempData;
        return loData;
    }

    public static PMR01002PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
    {
        var loParam = new BaseHeaderDTO()
        {
            CCOMPANY_NAME = "PT Realta Chakradarma",
            CPRINT_CODE = "010",
            CPRINT_NAME = "DEPOSIT TYPE = OUTSTANDING",
            CUSER_ID = "RYC",
        };

        PMR01002PrintResultWithBaseHeaderPrintDTO loRtn = new PMR01002PrintResultWithBaseHeaderPrintDTO();

        loRtn.BaseHeaderData = loParam;
        loRtn.Data2 = DefaultData();

        return loRtn;

    }
}