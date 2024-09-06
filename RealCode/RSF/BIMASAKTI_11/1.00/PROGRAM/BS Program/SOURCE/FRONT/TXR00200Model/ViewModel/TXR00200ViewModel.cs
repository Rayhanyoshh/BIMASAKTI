using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Extensions;
using R_BlazorFrontEnd.Helpers;
using TXR00200Common.DTOs;
using TXR00200Common.PrintDTO;

namespace TXR00200MODEL.ViewModel
{
    public class TXR00200ViewModel
    {
        private TXR00200Model _TXR00200Model = new TXR00200Model();
        public PrintParamTXDTO PrintParam { get; set; } = new PrintParamTXDTO();
        
        public DataSet ExcelDataSet { get; set; }
        #region Property
        public string PropertyDefault = "";
        public string PropertyDefaultName = "";
        public List<PropertyListDTO> PropertyList { get; set; } = new List<PropertyListDTO>();
        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _TXR00200Model.GetProperyListAsync();
                PropertyList = loResult.Data;
                var property = PropertyList.FirstOrDefault();
                PropertyDefault = property.CPROPERTY_ID.ToString();
                PropertyDefaultName = property.CPROPERTY_NAME; // tambahkan baris ini
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
        public List<TXR00200PeriodDetailDTO> PeriodDetailList { get; set; } = new List<TXR00200PeriodDetailDTO>();
        public async Task GetPeriodDetailList()
        {
            var loEx = new R_Exception();

            try
            {
                string lcYear = Convert.ToString(PeriodYear);
                var loResult = await _TXR00200Model.GetPerioDetailModel(lcYear);
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

        #region WH_TAX

        public List<PrintParamTXDTO> TaxTypeRadio { get; set; } = new List<PrintParamTXDTO>()
        {
            new PrintParamTXDTO() { CTAX_TYPE = "I", CTAX_TYPE_NAME = "In"},
            new PrintParamTXDTO() { CTAX_TYPE = "O", CTAX_TYPE_NAME = "Out"},
       
        };

        public string TaxTypeRadioSelected = "";

        #endregion
        
        #region SortBy

        public List<PrintParamTXDTO> TransCodeComboBox { get; set; } = new List<PrintParamTXDTO>()
        {
            new PrintParamTXDTO() { CTRANS_CODE = "01", CTRANC_CODE_NAME = "Sales Invoice"},
            new PrintParamTXDTO() { CTRANS_CODE = "02", CTRANC_CODE_NAME = "Sales Return"},
            new PrintParamTXDTO() { CTRANS_CODE = "03", CTRANC_CODE_NAME = "Sales Debit Note" },
            new PrintParamTXDTO() { CTRANS_CODE = "04", CTRANC_CODE_NAME = "Sales Credit Note" },
        };
        public List<PrintParamTXDTO> TransCodeComboBox1 { get; set; } = new List<PrintParamTXDTO>()
        {
            new PrintParamTXDTO() { CTRANS_CODE = "11", CTRANC_CODE_NAME = "Purchase  Invoice"},
            new PrintParamTXDTO() { CTRANS_CODE = "12", CTRANC_CODE_NAME = "Purchase  Return"},
            new PrintParamTXDTO() { CTRANS_CODE = "13", CTRANC_CODE_NAME = "Purchase  Debit Note" },
            new PrintParamTXDTO() { CTRANS_CODE = "14", CTRANC_CODE_NAME = "Purchase  Credit Note" },
        };

        public List<PrintParamTXDTO> TransCodeComboBoxList { get; set; } = new List<PrintParamTXDTO>();
        
        public string TransCodeSelected = "";

        #endregion

        public List<TXR00200DTO> EfakturList { get; set; } = new List<TXR00200DTO>();

        public async Task GetEfakturList(PrintParamTXDTO poParam)
        {
            var loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, poParam.CPROPERTY_ID);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CTAX_PERIOD_YEAR, poParam.CTAX_PERIOD_YEAR);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CTAX_PERIOD_MONTH, poParam.CTAX_PERIOD_MONTH);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CTAX_TYPE, poParam.CTAX_TYPE);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CTRANS_CODE, poParam.CTRANS_CODE);
                var loResult = await _TXR00200Model.GetEFakturListAsync();
                EfakturList = loResult.Data;  
                var loExcelData = R_FrontUtility.ConvertCollectionToCollection<TXR00200DTO>(EfakturList);

                var loDataTable = R_FrontUtility.R_ConvertTo<TXR00200DTO>(loExcelData);
                loDataTable.TableName = "EFaktur";

                var loDataSet = new DataSet();
                loDataSet.Tables.Add(loDataTable);

                // Asign Dataset
                ExcelDataSet = loDataSet;
                

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
    }
}