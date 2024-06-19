using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PMR01000Common;
using PMR01000Common.DTO_s;
using PMR01000Common.DTO_s.PrintDTO;
using R_APICommonDTO;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace PMR01000MODEL
{
    public class PMR01000ViewModel: R_ViewModel<PMR01000DTO>
    {
        private PMR01000Model _PMR01000Model = new PMR01000Model();
        
        public List<PropertyListDTO> PropertyList { get; set; } = new List<PropertyListDTO>();
        public List<PMR01000BuildingListDTO> FromBuildingList { get; set; } = new List<PMR01000BuildingListDTO>();
        public List<PMR01000BuildingListDTO> ToBuildingList { get; set; } = new List<PMR01000BuildingListDTO>();
        
        public List<PMR01000PeriodDTDTO> PeriodDetailList { get; set; } = new List<PMR01000PeriodDTDTO>();
        public PMR01000CBSystemParamDTO VAR_CB_SYSTEM_PARAM { get; set; } = new PMR01000CBSystemParamDTO();
        public PMR01000PeriodCompanyDTO VAR_GSM_PERIOD { get; set; } = new PMR01000PeriodCompanyDTO();
        
        public string PropertyDefault = "";
        public string FromBuildingDefault = "";
        public string ToBuildingDefault = "";
        public string loProperty = "";
        public string loDepositType = "";


        public int ToPeriodYear { get; set; }
        public int FromPeriodYear { get; set; }
        public string FromPeriodMonth { get; set; }
        public string ToPeriodMonth { get; set; }
        public DateTime CutOffDate { get; set; } = DateTime.Now;


        public string FromTypeSelected = "";
        public string ToTypeSelected = "";
        public string FromTransTypeSelected = "";
        public string ToTransTypeSelected = "";
        public List<DropDownDTO> FromType = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "Contractor", Text = "Contractor" },
            new DropDownDTO { Id = "Customer", Text = "Customer" }
        };
        public List<DropDownDTO> ToType = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "Contractor", Text = "Contractor" },
            new DropDownDTO { Id = "Customer", Text = "Customer" }
        };
        public List<DropDownDTO> FromTransTypeList = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "LOI", Text = "LOI" },
            new DropDownDTO { Id = "Agreement", Text = "Agreement" }
        };
        public List<DropDownDTO> ToTransTypeList = new List<DropDownDTO>()
        {
            new DropDownDTO { Id = "LOI", Text = "LOI" },
            new DropDownDTO { Id = "Agreement", Text = "Agreement" }
        };

        public List<PMR01000PrintParamDTO> DepositType { get; set; } = new List<PMR01000PrintParamDTO>()
            {
                new PMR01000PrintParamDTO() { CDEPOSIT_TYPE = "1", CDEPOSIT_TYPE_NAME = "List"},
                new PMR01000PrintParamDTO() { CDEPOSIT_TYPE = "2", CDEPOSIT_TYPE_NAME = "Outstanding"},
                new PMR01000PrintParamDTO() { CDEPOSIT_TYPE = "3", CDEPOSIT_TYPE_NAME = "Activity" },
            };

        public string DepositTypeSelected = "";
        
        
        
        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _PMR01000Model.GetProperyListAsync();
                PropertyList = loResult.Data;
                PropertyDefault = PropertyList.FirstOrDefault().CPROPERTY_ID.ToString();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetBuildingList()
        {
            var loEx = new R_Exception();

            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CPROPERTY_ID, PropertyDefault);
                var loResult = await _PMR01000Model.GetBuildingListAsync();
                FromBuildingList = loResult.Data;
                ToBuildingList = loResult.Data;
                FromBuildingDefault = FromBuildingList.FirstOrDefault().CBUILDING_ID;
                ToBuildingDefault = ToBuildingList.FirstOrDefault().CBUILDING_ID;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetAllUniversalData()
        {
            var loEx = new R_Exception();

            try
            {
                // Get Universal Data
                VAR_CB_SYSTEM_PARAM = await _PMR01000Model.GetCBSystemParamAsync();
                VAR_GSM_PERIOD = await _PMR01000Model.GetPeriodYearRangeAsync();
                PeriodDetailList = await _PMR01000Model.GetPeriodDetailAsync();
                

                ToPeriodYear = Convert.ToInt32(VAR_CB_SYSTEM_PARAM.CSOFT_PERIOD_YY);
                FromPeriodYear = Convert.ToInt32(VAR_CB_SYSTEM_PARAM.CSOFT_PERIOD_YY);
                
                
                List<string> monthNames = CultureInfo.CurrentCulture.DateTimeFormat
                    .MonthNames
                    .Where(name => !string.IsNullOrEmpty(name))
                    .ToList();

                // Replace CPERIOD_NAME with the corresponding month name
                for (int i = 0; i < PeriodDetailList.Count; i++)
                {
                    int monthIndex = int.Parse(PeriodDetailList[i].CPERIOD_NO) - 1;
                    if (monthIndex >= 0 && monthIndex < monthNames.Count)
                    {
                        PeriodDetailList[i].CPERIOD_NAME = monthNames[monthIndex];
                    }
                }

        
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}