using BaseHeaderReportCOMMON;
using GSM01000Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using BaseHeaderReportCOMMON.Models;
using System.Text;
using BaseHeaderReportCOMMON;


namespace GSM001000Common.Model
{
    public static class GSM01000GOAModelDummyData
    {
        public static GSM01000PrintGOAResultDTo DefaultData()
        {
            GSM01000PrintGOAResultDTo loData = new GSM01000PrintGOAResultDTo()
            {
                Title = "Group Of Account",
                Header = "",
                Column = new GSM01000PrintColoumnGOADTO()
            };

            int Data1 = 4;
            int Data2 = 5;

            List<GSM01000ResultSPPrintGOADTO> loCollection = new List<GSM01000ResultSPPrintGOADTO>();
            for (int a = 1; a < Data1; a++)
            {
                for (int b = 1; b < Data2; b++)
                {
                    loCollection.Add(new GSM01000ResultSPPrintGOADTO()
                    {
                        CGOA_CODE = $"GOA No {a}",
                        CGOA_NAME = $"GOA Name {a}",
                        CGOA_BSIS = $"CGOABSIS {a}",
                        CGOA_DBCR = $"CGOADBCR {a}",
                        
                        CGLACCOUNT_NO = $"Account No {b}",
                        CGLACCOUNT_NAME = $"Account Name {b}",
                        CBSIS = $"CBSIS {b}",
                        CDBCR = $"CDBCR {b}",
                        CCASH_FLOW_CODE = $"CASHCODE {b}",
                        CCASH_FLOW_NAME = $"CASHNAME {b}",
                        LACTIVE = b % 2 == 0 ? true : false,
                    });
                }
            }

            var loTempData = loCollection
                .GroupBy(data1a => new
                {
                    data1a.CGOA_CODE,
                    data1a.CGOA_NAME,
                    data1a.CGOA_BSIS,
                    data1a.CGOA_DBCR,

                }).Select(data1b => new GSM01000DataResultGOADTO()
                {
                    CGOA_CODE = data1b.Key.CGOA_CODE,
                    CGOA_NAME = data1b.Key.CGOA_NAME,
                    CGOA_BSIS = data1b.Key.CGOA_BSIS,
                    CGOA_DBCR = data1b.Key.CGOA_DBCR,
                    
                    
                    GSM01001DataResultGOADTO = data1b.GroupBy(data2a => new
                    {
                        data2a.CGLACCOUNT_NO,
                        data2a.CGLACCOUNT_NAME,
                        data2a.CBSIS,
                        data2a.CDBCR,
                        data2a.CCASH_FLOW_CODE,
                        data2a.CCASH_FLOW_NAME,
                        data2a.LACTIVE,
                    }).Select(data2b => new GSM01001DataResultGOADTO()
                    {
                        CGLACCOUNT_NO = data2b.Key.CGLACCOUNT_NO,
                        CGLACCOUNT_NAME = data2b.Key.CGLACCOUNT_NAME,
                        CBSIS = data2b.Key.CBSIS,
                        CDBCR = data2b.Key.CDBCR,
                        CCASH_FLOW_CODE = data2b.Key.CCASH_FLOW_CODE,
                        CCASH_FLOW_NAME = data2b.Key.CCASH_FLOW_NAME,
                        LACTIVE = data2b.Key.LACTIVE,   
                    }).ToList()
                }).ToList();
               

            loData.Data = loTempData;
            return loData;
        }

        public static GSM01000PrintGOAResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
        {
            var loParam = new BaseHeaderDTO()
            {
                CCOMPANY_NAME = "PT Realta Chakradarma",
                CPRINT_CODE = "010",
                CPRINT_NAME = "Group Of Account",
                CUSER_ID = "RYC",
            };

            GSM01000PrintGOAResultWithBaseHeaderPrintDTO loRtn = new GSM01000PrintGOAResultWithBaseHeaderPrintDTO();

            loRtn.BaseHeaderData = loParam;
            loRtn.GOAData = DefaultData();

            return loRtn;
        }

    }
}
