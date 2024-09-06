using System.Collections.Generic;
using System.Linq;
using BaseHeaderReportCOMMON;
using PMR01000Common.DTO_s;
using PMR01000Common.DTO_s.PrintDTO;

namespace PMR01000Common.Model;

public static class PMR01003DummyData
{
    public static PMR01003PrintResultDTO DefaultData()
    {
       
        PMR01000PrintParamDTO PrintParam = new PMR01000PrintParamDTO()
        {
            CFROM_YEAR = "2024",
            CFROM_MONTH = "02",
            CTO_YEAR = "2024",
            CTO_MONTH = "05",
            CFROM_BUILDING = "TM01",
            CTO_BUILDING = "TM02",
            CFROM_TYPE = "Contractor",
            CTO_TYPE = "Customer",
            CFROM_TRANS_TYPE = "LOI",
            CTO_TRANS_TYPE = "LOI",
        };

        PMR01003PrintResultDTO loData = new PMR01003PrintResultDTO()
        {
            Title = "Deposit Activity",
            Header = "",
            HeaderParam = PrintParam,
            Column = new PMR01003PrintColoumnDTO(),
            ColumnDetail = new PMR01003PrintColumnDetailDTO(),
        };

        int Data1 = 2;
        int Data2 = 2;
        int Data3 = 2;
        int Data4 = 2;

        List<PMR01003ResultPrintSPDTO> loCollection = new List<PMR01003ResultPrintSPDTO>();
        for (int a = 1; a <= Data1; a++)
        {
            for (int b = 1; b <= Data2; b++)
            {
                for (int c = 1; c <= Data3; c++)
                {
                    for (int d = 1; d <= Data4; d++)
                    {
                        loCollection.Add(new PMR01003ResultPrintSPDTO()
                        {
                            CDEPOSIT_ID = $"211110{a}",
                            CDEPOSIT_NAME = $"Fit Out Deposit{a}",

                            CBUILDING_ID = $"TM{b}",
                            CDEPOSIT_TYPE = $"Contract{b}",

                            CCUSTOMER_NAME = $"BAXTER-CV SINAR MANDIRI{c}",
                            CREFERENCE_NO = $"044/LOO/TM/VII/{c}",
                            CUNIT_DESCRIPTION = $"{c}",
                            CSTATUS = $"Activ{c}",
                            CTYPES = $"INV{c}",
                            CTRANSACTION_TYPE = $"LOI{c}",
                            CTRANSACTION_NO = $"SDE/2021090005{c}",
                            CDEPOSIT_DATE = $"30/09/2021{c}",
                            CPAYMENT_STATUS = $"Paid{c}",
                            CCURRENCY_CODE = $"IDR{c}",
                            NDEPOSIT_AMOUNT =  102340,
                            NCREDIT_NOTE = 44100,
                            NADJUSTMENT = 602340,
                            NREFUND = 602340,
                            NDEPOSIT_BALANCE = 3032520,
                            
                            CDETAIL_DEPOSIT_TYPE = $"DepositCN{d}",
                            CDETAIL_DEPOSIT_DATE = $"30/05/2022{d}",
                            CDETAIL_TRANSACTION_NO= $"CN/20240001{d}",
                            CDETAIL_PAYMENT_STATUS = $"Done{d}",
                            CDETAIL_CURRENCY_CODE = $"USD{d}",
                            NDETAIL_DEPOSIT_BALANCE = 340,
                            NDETAIL_AMOUNT = 340,
                        });
                    }
                }
            }
        

            var loTempData = loCollection
                .GroupBy(data1a => new
                {
                    data1a.CDEPOSIT_ID,
                    data1a.CDEPOSIT_NAME,
                }).Select(data1b => new PMR01003DataResultDTO()
                {
                    CDEPOSIT_ID = data1b.Key.CDEPOSIT_ID,
                    CDEPOSIT_NAME = data1b.Key.CDEPOSIT_NAME,
                    Detail1 = data1b.GroupBy(data2a => new
                    {
                        data2a.CBUILDING_ID,
                        data2a.CDEPOSIT_TYPE,

                    }).Select(data2b => new PMR01003DataResultChild1DTO()
                    {
                        CBUILDING_ID = data2b.Key.CBUILDING_ID,
                        CDEPOSIT_TYPE = data2b.Key.CDEPOSIT_TYPE,
                        Detail2 = data2b.GroupBy(data3a => new
                        {
                            data3a.CCUSTOMER_NAME,
                            data3a.CTRANSACTION_TYPE,
                            data3a.CREFERENCE_NO,
                            data3a.CSTATUS,
                            data3a.CTYPES,
                            data3a.CTRANSACTION_NO,
                            data3a.CUNIT_DESCRIPTION,
                            data3a.CINVOICE_NO,
                            data3a.CDEPOSIT_DATE,
                            data3a.CPAYMENT_STATUS,
                            data3a.CCURRENCY_CODE,
                            data3a.NAMOUNT,
                            data3a.NCREDIT_NOTE,
                            data3a.NADJUSTMENT,
                            data3a.NREFUND,
                            data3a.NDEPOSIT_AMOUNT,
                            data3a.NDEPOSIT_BALANCE,
                        }).Select(data3b => new PMR01003DataResultChild2DTO()
                        {
                            CCUSTOMER_NAME = data3b.Key.CCUSTOMER_NAME,
                            CTRANSACTION_TYPE = data3b.Key.CTRANSACTION_TYPE,
                            CREFERENCE_NO = data3b.Key.CREFERENCE_NO,
                            CSTATUS = data3b.Key.CSTATUS,
                            CTYPES = data3b.Key.CTYPES,
                            CTRANSACTION_NO = data3b.Key.CTRANSACTION_NO,
                            CUNIT_DESCRIPTION = data3b.Key.CUNIT_DESCRIPTION,
                            CINVOICE_NO = data3b.Key.CINVOICE_NO,
                            CDEPOSIT_DATE = data3b.Key.CDEPOSIT_DATE,
                            CPAYMENT_STATUS = data3b.Key.CPAYMENT_STATUS,
                            CCURRENCY_CODE = data3b.Key.CCURRENCY_CODE,
                            NAMOUNT = data3b.Key.NAMOUNT,
                            NCREDIT_NOTE = data3b.Key.NCREDIT_NOTE,
                            NADJUSTMENT = data3b.Key.NADJUSTMENT,
                            NREFUND = data3b.Key.NREFUND,
                            NDEPOSIT_AMOUNT = data3b.Key.NDEPOSIT_AMOUNT,
                            NDEPOSIT_BALANCE = data3b.Key.NDEPOSIT_BALANCE,
                            Detail3 = data3b.GroupBy(data4a => new
                            {
                                data4a.CDETAIL_DEPOSIT_TYPE,
                                data4a.CDETAIL_TRANSACTION_NO,
                                data4a.CDETAIL_DEPOSIT_DATE,
                                data4a.CDETAIL_PAYMENT_STATUS,
                                data4a.CDETAIL_CURRENCY_CODE,
                                data4a.NDETAIL_AMOUNT,
                                data4a.NDETAIL_DEPOSIT_BALANCE,
                            }).Select(data4b => new PMR01003DataResultChild3DTO()
                            {
                                CDETAIL_DEPOSIT_TYPE = data4b.Key.CDETAIL_DEPOSIT_TYPE,
                                CDETAIL_TRANSACTION_NO = data4b.Key.CDETAIL_TRANSACTION_NO,
                                CDETAIL_DEPOSIT_DATE = data4b.Key.CDETAIL_DEPOSIT_DATE,
                                CDETAIL_PAYMENT_STATUS = data4b.Key.CDETAIL_PAYMENT_STATUS,
                                CDETAIL_CURRENCY_CODE = data4b.Key.CDETAIL_CURRENCY_CODE,
                                NDETAIL_AMOUNT = data4b.Key.NDETAIL_AMOUNT,
                                NDETAIL_DEPOSIT_BALANCE = data4b.Key.NDETAIL_DEPOSIT_BALANCE,
                            }).ToList()
                        }).ToList()
                    }).ToList()
                }).ToList();


            loData.Data = loTempData;
        }
        return loData;
    }

    public static PMR01003PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
    {
        var loParam = new BaseHeaderDTO()
        {
            CCOMPANY_NAME = "PT Realta Chakradarma",
            CPRINT_CODE = "010",
            CPRINT_NAME = "DEPOSIT TYPE = ACTIVITY",
            CUSER_ID = "RYC",
        };

        PMR01003PrintResultWithBaseHeaderPrintDTO loRtn = new PMR01003PrintResultWithBaseHeaderPrintDTO();

        loRtn.BaseHeaderData = loParam;
        loRtn.Data3 = DefaultData();

        return loRtn;

    }
}