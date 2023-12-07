using System.Collections.Generic;
using BaseHeaderReportCOMMON;
using GSM008500Common.DTOs;
using GSM008500Common.DTOs.PrintDTO;

namespace GSM008500Common.Model
{
    public static class GSM08500ModelDummyData
    {
        public static GSM01000PrintStatAccResultDTo DefaultData()
        {
            GSM01000PrintStatAccResultDTo loData = new GSM01000PrintStatAccResultDTo()
            {
                Title = "Statistic Account",
                Header = "",
                Column = new GSM08500PrintColoumnStatAccDTO()
            };

            int Data1 = 4;
            
            List<GSM08500ResultSPPrintStatAccDTO> loCollection = new List<GSM08500ResultSPPrintStatAccDTO>();
            for (int a = 1; a < Data1; a++)
            {
                loCollection.Add(new GSM08500ResultSPPrintStatAccDTO()
                {
                    CGLACCOUNT_NO = $"Account No {a}",
                    CGLACCOUNT_NAME = $"Account Name {a}",
                    CBSIS = $"CBSIS {a}",
                    CDBCR = $"CDBCR {a}",
                    LACTIVE = a % 2 == 0 ? true:false,
                });
            }
            
            loData.Data = loCollection;
            return loData;
        }
    
        public static GSM08500PrintStatAccResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
        {
            var loParam = new BaseHeaderDTO()
            {
                CCOMPANY_NAME = "PT Realta Chakradarma",
                CPRINT_CODE = "010",
                CPRINT_NAME = "Statistic Account",
                CUSER_ID = "RYC",
            };

            GSM08500PrintStatAccResultWithBaseHeaderPrintDTO loRtn = new GSM08500PrintStatAccResultWithBaseHeaderPrintDTO();

            loRtn.BaseHeaderData = loParam;
            loRtn.StatAccountData = DefaultData();

            return loRtn;
        }
    }
}