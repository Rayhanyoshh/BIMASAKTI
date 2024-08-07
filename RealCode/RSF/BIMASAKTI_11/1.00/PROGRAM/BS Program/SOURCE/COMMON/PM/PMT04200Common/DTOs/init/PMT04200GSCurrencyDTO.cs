﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMT04200Common.DTOs
{
    public class PMT04200GSCurrencyDTO
    {
        public string CCURRENCY_CODE { get; set; }
        public string CCURRENCY_SYMBOL { get; set; }
        public string CCURRENCY_NAME { get; set; }

        public string CCREATE_BY { get; set; }
        public DateTime DCREATE_DATE { get; set; }
        public string CUPDATE_BY { get; set; }
        public DateTime DUPDATE_DATE { get; set; }
    }
}
