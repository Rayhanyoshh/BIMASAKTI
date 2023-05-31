using R_APICommonDTO;
using System;
using System.Collections.Generic;

namespace GSM01000Common.DTOs
{
    public class GSM01100UserListDTO : R_APIResultBaseDTO
    {

        public List<GSM01100DTO> Data { get; set; }

    }

    public class GSM01100UserCOAParameterDTO: R_APIResultBaseDTO 

    {
    public string CCOMPANY_ID { get; set; }
    public string CGLACCOUNT_NO { get; set; }
    public string CGLACCOUNT_NAME { get; set; }
    public string CUSER_ID { get; set; }
    public string CUSER_NAME { get; set; }
    public string CUPDATE_BY { get; set; }
    public DateTime DUPDATED_DATE { get; set; }

    }
}