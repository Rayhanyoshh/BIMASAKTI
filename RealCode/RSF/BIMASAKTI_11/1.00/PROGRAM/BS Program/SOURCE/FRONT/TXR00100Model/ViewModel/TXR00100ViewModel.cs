using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;
using TXR00100Common.DTOs;
using TXR00100Common.PrintDTO;
using TXR00100FrontResources;

namespace TXR00100MODEL.ViewModel
{
    public class TXR00100ViewModel
    {
        private TXR00100Model _TXR00100Model = new TXR00100Model();
        public PrintParamTXDTO PrintParam { get; set; } = new PrintParamTXDTO();
        
        #region Property
        public string PropertyDefault = "";
        public List<PropertyListDTO> PropertyList { get; set; } = new List<PropertyListDTO>();
        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _TXR00100Model.GetProperyListAsync();
                PropertyList = loResult.Data;
                PropertyDefault = PropertyList.FirstOrDefault().CPROPERTY_ID.ToString();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion
        
        #region Periodetail
        public string PeriodMonthDefault = "";
        public int PeriodYear = Convert.ToInt32(DateTime.Now.Year);
        public List<TXR00100PeriodDetailDTO> PeriodDetailList { get; set; } = new List<TXR00100PeriodDetailDTO>();
        public async Task GetPeriodDetailList()
        {
            var loEx = new R_Exception();

            try
            {
                string lcYear = Convert.ToString(PeriodYear);
                var loResult = await _TXR00100Model.GetPerioDetailModel(lcYear);
                PeriodDetailList = loResult.Data;
                PeriodMonthDefault = PeriodDetailList.FirstOrDefault().CPERIOD_NO.ToString();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        #endregion

        
        public async Task InitProcess(R_ILocalizer<ResourcesDummyTXR00100> poParamLocalizer)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                WHTaxRadio = new List<PrintParamTXDTO>()
                {
                    new PrintParamTXDTO() { CWH_TAX_TYPE = "01", CWH_TAX_TYPE_NAME = poParamLocalizer["_radioAll"]},
                    new PrintParamTXDTO() { CWH_TAX_TYPE = "02", CWH_TAX_TYPE_NAME = poParamLocalizer["_radioCollect"]},
                    new PrintParamTXDTO() { CWH_TAX_TYPE = "03", CWH_TAX_TYPE_NAME = poParamLocalizer["_radioNotCollect"] },
                };
                
                SortByRadio  = new List<PrintParamTXDTO>()
                {
                    new PrintParamTXDTO() { CSORT_BY = "01", CSORT_BY_NAME = "Customer Name & Agreement No."},
                    new PrintParamTXDTO() { CSORT_BY = "02", CSORT_BY_NAME = "Invoice No."},
                    new PrintParamTXDTO() { CSORT_BY = "03", CSORT_BY_NAME = "Invoice Date" },
                };
                
                SortByRadio1 = new List<PrintParamTXDTO>()
                {
                    new PrintParamTXDTO { CSORT_BY = "01", CSORT_BY_NAME = poParamLocalizer["_radioCustAndAgree"] },
                };
                SortByRadio2 = new List<PrintParamTXDTO>()
                {
                    new PrintParamTXDTO { CSORT_BY = "02", CSORT_BY_NAME = poParamLocalizer["_radioInvoiceNo"] }
                };
                SortByRadio3 = new List<PrintParamTXDTO>()
                {
                    new PrintParamTXDTO { CSORT_BY = "03", CSORT_BY_NAME = poParamLocalizer["_radioInvoiceDate"] }
                };
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        
        #region WH_TAX

        public List<PrintParamTXDTO> WHTaxRadio { get; set; } = new List<PrintParamTXDTO>();

        public string WHTaxRadioSelected = "";

        #endregion
        #region SortBy

        public List<PrintParamTXDTO> SortByRadio { get; set; } = new List<PrintParamTXDTO>();
        public List<PrintParamTXDTO> SortByRadio1 = new List<PrintParamTXDTO>();
        public List<PrintParamTXDTO> SortByRadio2 = new List<PrintParamTXDTO>();
        public List<PrintParamTXDTO> SortByRadio3 = new List<PrintParamTXDTO>();

        public string SortByRadioSelected = "";

        #endregion
 
    }
}