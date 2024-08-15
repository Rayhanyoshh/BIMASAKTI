using System;
using System.Collections.Generic;
using System.Text;
using BaseHeaderReportCOMMON;
using TXR00100Common.PrintDTO;

namespace TXR00100Common.Model
{
    public class TXR00100DummyData
    {
        public static PrintResultTXDTO DefaultData()
        {
          PrintResultTXDTO loData = new PrintResultTXDTO()
          {
              Title = "Statement Of Account",
              Header = "",
              Column = new TXR00100PrintColoumnDTO()
          };

          int Data1 = 4;
              
          List<TXR00100DataResultDTO> loCollection = new List<TXR00100DataResultDTO>();
          for (int a = 1; a < Data1; a++)
          {
              loCollection.Add(new TXR00100DataResultDTO()
              {
                  CCUSTOMER_NAME = "PT. RCD",
                  CAGREEMENT_NO = "AGREE-1",
                  CCURRENCY_CODE = "USD",
                  CINVOICE_NO = "INV01",
                  CINVOICE_DATE = "2022-01-01",
                  CUNIT_DESCRIPTION = "Clean",
                  CTAX_ADDRESS = "Everywhere",
                  CWH_TAX_SLIP_DATE = "2022-01-01",
                  CWH_TAX_SLIP_NO = "Tax-01",
                  NWH_TAX_AMOUNT = 100,
                  NWH_TAX_SLIP_AMOUNT = 100,
                  LWH_TAX_COLLECT = true,
              });
          }
              
          loData.Data = loCollection;
          loData.Param = new PrintParamTXDTO()
          {
              CCOMPANY_ID = "RCD",
              CPROPERTY_ID = "ASHMD",
              CTAX_PERIOD_YEAR = "2024",
              CTAX_PERIOD_MONTH = "01",
              CWH_TAX_TYPE = "01",
              CSORT_BY = "01",
              CUSER_LOGIN = "RYC",
          };
          return loData;
      }
      
      public static TXR00100PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
      {
          var loParam = new BaseHeaderDTO()
          {
              CCOMPANY_NAME = "PT Realta Chakradarma",
              CPRINT_CODE = "TXR00100",
              CPRINT_NAME = "Withholding Tax Slip Report",
              CUSER_ID = "RYC",
          };

          TXR00100PrintResultWithBaseHeaderPrintDTO loRtn = new TXR00100PrintResultWithBaseHeaderPrintDTO();

          loRtn.BaseHeaderData = loParam;
          loRtn.Data1 = DefaultData();

          return loRtn;
      }
    }
}
