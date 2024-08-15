using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Lookup_PMCOMMON.DTOs;
using PMR02200Common.DTOs;
using PMR02200Common.DTOs.PrintDTO;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using MonthDTO = PMR02200Common.DTOs.MonthDTO;

namespace PMR02200MODEL
{
    public class PMR02200ViewModel
    {
        private PMR02200Model _PMR02200Model = new PMR02200Model();
        public PMR02200PrintParamDTO PrintParam { get; set; } = new PMR02200PrintParamDTO();
        public DateTime DCUT_OFF_DATE = DateTime.Now;
        public DateTime DSTATEMENT_DATE = DateTime.Now;
        public int FromPeriodYear { get; set; } = DateTime.Now.Year;
        public int ToPeriodYear { get; set; } = DateTime.Now.Year;


        #region Property
        public string PropertyDefault = "";
        public List<PropertyListDTO> PropertyList { get; set; } = new List<PropertyListDTO>();
        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _PMR02200Model.GetProperyListAsync();
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
        #region Company Period
        public PMR02200PeriodCompanyDTO VAR_YEAR_RANGE { get; set; } = new PMR02200PeriodCompanyDTO();
        public string FromPeriodMonth { get; set; }
        public string ToPeriodMonth { get; set; }
        
        public List<MonthDTO> PeriodMonthList { get; set; } = new List<MonthDTO>
        {
            new MonthDTO { Id = "01", Text = "January" },
            new MonthDTO { Id = "02", Text = "February" },
            new MonthDTO { Id = "03", Text = "March" },
            new MonthDTO { Id = "04", Text = "April" },
            new MonthDTO { Id = "05", Text = "May" },
            new MonthDTO { Id = "06", Text = "June" },
            new MonthDTO { Id = "07", Text = "July" },
            new MonthDTO { Id = "08", Text = "August" },
            new MonthDTO { Id = "09", Text = "September" },
            new MonthDTO { Id = "10", Text = "October" },
            new MonthDTO { Id = "11", Text = "November" },
            new MonthDTO { Id = "12", Text = "December" }
        };
        public async Task GetPeriodCompany()
        {
            var loEx = new R_Exception();

            try
            {
                List<string> monthNames = CultureInfo.CurrentCulture.DateTimeFormat
                    .MonthNames
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList();

                // Replace CPERIOD_NAME with the corresponding month name
                for (int i = 0; i < PeriodMonthList.Count; i++)
                {
                    int monthIndex = int.Parse(PeriodMonthList[i].Id) - 1;
                    if (monthIndex >= 0 && monthIndex < monthNames.Count)
                    {
                        PeriodMonthList[i].Text = monthNames[monthIndex];
                    }
                }
                
                R_FrontContext.R_SetStreamingContext(ContextConstantPublicLookup.CPROPERTY_ID, PropertyDefault);
                R_FrontContext.R_SetStreamingContext(ContextConstantPublicLookup.CCUSTOMER_TYPE, "01");
                VAR_YEAR_RANGE = await _PMR02200Model.GetPeriodYearRangeAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #endregion
        #region Agreement No. Based On
        public List<DropDownDTO> AggrementRadio1 = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "1", Text = "Customer" },
        };
        public List<DropDownDTO> AggrementRadio2 = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "2", Text = "Department" }
        };
        public string AggrementRadioSelected = "";
        #endregion
        #region Date Based On
        public List<DropDownDTO> DateBaseRadio1 = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "1", Text = "Cut Off" },
        };
        public List<DropDownDTO> DateBaseRadio2 = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "2", Text = "Period" }
        };
        public string DateBaseRadioSelected = "";
        #endregion

        public List<CheckBoxDTO> LOINoOption = new List<CheckBoxDTO>()
        {
            new CheckBoxDTO { Id = "1", Value = true },
               new CheckBoxDTO { Id = "2", Value = false },

        };
    }
}