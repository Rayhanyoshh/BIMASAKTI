using System;
using System.Collections.Generic;
using R_APICommonDTO;

namespace GLT00600Common.DTOs
{
    public class ResultRefreshCurrencyDTO : R_APIResultBaseDTO
    {
        public string CCURRENCY_CODE { get; set; }
        public string CRATETYPE_CODE { get; set; }
        public DateTime CRATE_DATE { get; set; }
        public decimal NLBASE_RATE_AMOUNT { get; set; }
        public decimal NLCURRENCY_RATE_AMOUNT { get; set; }
        public decimal NBBASE_RATE_AMOUNT { get; set; }
        public decimal NBCURRENCY_RATE_AMOUNT { get; set; }
    }

    public class RefreshCurrencyParameterDTO
    {
        public string CCOMPANY_ID { get ; set; }
        public string CUSER_ID { get ; set; }
        public string CCURRENCY_CODE { get; set; }
        public string CRATETYPE_CODE { get; set; }
        public string CRATE_DATE { get; set; }
    }
}