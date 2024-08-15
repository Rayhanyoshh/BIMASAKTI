using System;
using System.Collections.Generic;
using System.Text;
using BaseHeaderReportCOMMON;
using TXR00200Common.PrintDTO;

namespace TXR00200Common.Model
{
    public class TXR00200DummyData
    {
        public static PrintResultTXDTO DefaultData()
        {
          PrintResultTXDTO loData = new PrintResultTXDTO()
          {
              Title = "Statement Of Account",
              Header = "",
              Column = new TXR00200PrintColoumnDTO()
          };

          int Data1 = 4;
              
          List<TXR00200DataResultDTO> loCollection = new List<TXR00200DataResultDTO>();
          for (int a = 1; a < Data1; a++)
          {
              loCollection.Add(new TXR00200DataResultDTO()
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
              CTAX_TYPE = "01",
              CTRANS_CODE = "01",
              CUSER_LOGIN = "RYC",
          };
          return loData;
      }
      
      public static TXR00200PrintResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
      {
          var loParam = new BaseHeaderDTO()
          {
              CCOMPANY_NAME = "PT Realta Chakradarma",
              CPRINT_CODE = "TXR00200",
              CPRINT_NAME = "Withholding Tax Slip Report",
              CUSER_ID = "RYC",
          };

          TXR00200PrintResultWithBaseHeaderPrintDTO loRtn = new TXR00200PrintResultWithBaseHeaderPrintDTO();

          loRtn.BaseHeaderData = loParam;
          loRtn.Data1 = DefaultData();

          return loRtn;
      }
    }
}
