using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using PMM05000Common.DTOs;
using R_APICommonDTO;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace PMM05000Model
{
    public class PMM05000ViewModel : R_ViewModel<PMM05000DTO>
    {
        private PMM05000Model _PMM05000Model = new PMM05000Model();

        public ObservableCollection<PMM05000DTO> UnitPriceList { get; set; } =
            new ObservableCollection<PMM05000DTO>();

        public List<PropertyListDTO> PropertyList { get; set; } = new List<PropertyListDTO>();

        public string PropertyValue = "";
        public string UnitTypeValue = "";
        public string ValidInternal = "";

        public PMM05000DTO loTmp = new PMM05000DTO();

        // public DateTime ValidDate = DateTime.Now;
        public bool SelectedActiveInactive;
        public List<DropDownSqmTotalDTO> SqmTotalList { get; set; } = new List<DropDownSqmTotalDTO>();



        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _PMM05000Model.GetProperyListAsync();
                PropertyList = loResult.Data;
                PropertyValue = PropertyList.FirstOrDefault().CPROPERTY_ID.ToString();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetUnitTypePriceList()
        {
            var loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CUNIT_TYPE_ID, UnitTypeValue);
                R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, PropertyValue);
                R_FrontContext.R_SetStreamingContext(ContextConstant.LACTIVE_ONLY, SelectedActiveInactive);
                var loReturn = await _PMM05000Model.GetUnitPriceListAsync();

                var loResult = R_FrontUtility.ConvertCollectionToCollection<PMM05000DTO>(loReturn.Data);
                foreach (var list in loResult)
                {
                    list.DVALID_DATE = DateTime.ParseExact(list.CVALID_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                }

                UnitPriceList = new ObservableCollection<PMM05000DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveUnitPrice(PMM05000DTO poEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                poEntity.CPROPERTY_ID = PropertyValue;
                poEntity.CUNIT_TYPE_ID = UnitTypeValue;
                // poEntity.LACTIVE = SelectedActiveInactive;
                poEntity.CVALID_DATE = poEntity.DVALID_DATE.ToString("yyyyMMdd");
                var loResult = await _PMM05000Model.R_ServiceSaveAsync(poEntity, peCRUDMode);
                loTmp = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(loResult);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteUnitPrice(PMM05000DTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                poEntity.CPROPERTY_ID = PropertyValue;
                poEntity.CUNIT_TYPE_ID = UnitTypeValue;
                await _PMM05000Model.R_ServiceDeleteAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        public async Task GetUnitPriceById(PMM05000DTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                var loResult = await _PMM05000Model.R_ServiceGetRecordAsync(poEntity);
                //Promotion = loResult;
                loTmp = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(loResult);
                //R_FrontContext.R_SetContext(ContextConstant.LACTIVE_ONLY, loTmp.LACTIVE);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task ActiveInactiveProcessAsync()

        {
            R_Exception loException = new R_Exception();

            try
            {
                R_FrontContext.R_SetContext(ContextConstant.CPROPERTY_ID, PropertyValue);
                R_FrontContext.R_SetContext(ContextConstant.CUNIT_TYPE_ID, UnitTypeValue);
                R_FrontContext.R_SetContext(ContextConstant.CVALID_INTERNAL_ID, loTmp.CVALID_INTERNAL_ID);
                R_FrontContext.R_SetContext(ContextConstant.LACTIVE_ONLY, loTmp.LACTIVE);

                await _PMM05000Model.RSP_GS_ACTIVE_INACTIVE_MethodAsync();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}


    
    
