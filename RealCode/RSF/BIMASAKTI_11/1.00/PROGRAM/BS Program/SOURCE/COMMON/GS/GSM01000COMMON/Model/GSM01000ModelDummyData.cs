using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using BaseHeaderReportCOMMON.Models;
using BaseHeaderReportCOMMON;
using GSM01000Common.DTOs;

namespace GSM01000Common.Model
{
    public static class GSM01000ModelDummyData
    {
        public static GSM01000PrintCOAResultDTo DefaultData()
        {
            
            GSM01000PrintCOAResultDTo loData = new GSM01000PrintCOAResultDTo()
            {
                Title = "Chart Of Account",
                Header = "",
                Column = new GSM01000PrintColoumnCOADTO()
            };

            int Data1 = 4;
            
            List<GSM01000ResultSPPrintCOADTO> loCollection = new List<GSM01000ResultSPPrintCOADTO>();
            for (int a = 1; a < Data1; a++)
            {
                loCollection.Add(new GSM01000ResultSPPrintCOADTO()
                {
                    CGLACCOUNT_NO = $"Account No {a}",
                    CGLACCOUNT_NAME = $"Account Name {a}",
                    CBSIS = $"BS/IS {a}",
                    CDBCR = $"D/C {a}",
                    CCASH_FLOW_CODE = $"CCASH_FLOW_CODE {a}",
                    CCASH_FLOW_NAME = $"CCASH_FLOW_NAME {a}",
                    LACTIVE = a % 2 == 0 ? true:false,
                    CUSER_NAME_LIST =$"Alief{a}, AOC, cp, Ericson, Ivan, Reni",
                    CCENTER_NAME_LIST=$"Marketing{a}, HRD{a}",
                });
            }
            
            loData.Data = loCollection;
            loData.ParamDTO = new GSM01000PrintParamCOADTO
            {
                LPRINT_USER_RESTRICT = true,
                LPRINT_CENTER_RESTRICT = true,
            };
            return loData;
        }

        public static GSM01000PrintCOAResultWithBaseHeaderPrintDTO DefaultDataWithHeader()
        {
            var loParam = new BaseHeaderDTO()
            {
                CCOMPANY_NAME = "PT Realta Chakradarma",
                CPRINT_CODE = "010",
                CPRINT_NAME = "Chart OF Account",
                CUSER_ID = "RYC",
            };

            GSM01000PrintCOAResultWithBaseHeaderPrintDTO loRtn = new GSM01000PrintCOAResultWithBaseHeaderPrintDTO();

            loRtn.BaseHeaderData = loParam;
            loRtn.CenterData = DefaultData();

            return loRtn;
        }
        
        
    }
    
    
}
