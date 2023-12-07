using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LMM05000Common.DTOs;
using R_APICommonDTO;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;

namespace LMM05000Model
{
    public class LMM05000ViewModel : R_ViewModel<LMM05000DTO>
    {
        private LMM05000Model _LMM05000Model = new LMM05000Model();

        public ObservableCollection<LMM05000DTO> UnitPriceList { get; set; } =
            new ObservableCollection<LMM05000DTO>();

        public List<PropertyListDTO> PropertyList { get; set; } = new List<PropertyListDTO>();

        public string PropertyValue = "";
        public string UnitTypeValue = "";
        public string ValidInternal = "";

        public LMM05000DTO loTmp = new LMM05000DTO();

        // public DateTime ValidDate = DateTime.Now;
        public bool SelectedActiveInactive;
        public List<DropDownSqmTotalDTO> SqmTotalList { get; set; } = new List<DropDownSqmTotalDTO>();



        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _LMM05000Model.GetProperyListAsync();
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
                var loReturn = await _LMM05000Model.GetUnitPriceListAsync();

                var loResult = R_FrontUtility.ConvertCollectionToCollection<LMM05000DTO>(loReturn.Data);
                foreach (var list in loResult)
                {
                    list.DVALID_DATE = DateTime.ParseExact(list.CVALID_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                }

                UnitPriceList = new ObservableCollection<LMM05000DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveUnitPrice(LMM05000DTO poEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                poEntity.CPROPERTY_ID = PropertyValue;
                poEntity.CUNIT_TYPE_ID = UnitTypeValue;
                // poEntity.LACTIVE = SelectedActiveInactive;
                poEntity.CVALID_DATE = poEntity.DVALID_DATE.ToString("yyyyMMdd");
                var loResult = await _LMM05000Model.R_ServiceSaveAsync(poEntity, peCRUDMode);
                loTmp = R_FrontUtility.ConvertObjectToObject<LMM05000DTO>(loResult);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteUnitPrice(LMM05000DTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                poEntity.CPROPERTY_ID = PropertyValue;
                poEntity.CUNIT_TYPE_ID = UnitTypeValue;
                await _LMM05000Model.R_ServiceDeleteAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        public async Task GetUnitPriceById(LMM05000DTO poEntity)
        {
            var loEx = new R_Exception();
            try
            {
                var loResult = await _LMM05000Model.R_ServiceGetRecordAsync(poEntity);
                //Promotion = loResult;
                loTmp = R_FrontUtility.ConvertObjectToObject<LMM05000DTO>(loResult);
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

                await _LMM05000Model.RSP_GS_ACTIVE_INACTIVE_MethodAsync();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}


    
    
