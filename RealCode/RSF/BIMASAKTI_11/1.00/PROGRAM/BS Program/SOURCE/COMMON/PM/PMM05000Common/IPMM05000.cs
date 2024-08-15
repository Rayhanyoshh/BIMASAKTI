using R_CommonFrontBackAPI;
using System.Collections.Generic;
using PMM05000Common.DTOs;


namespace PMM05000Common
{
    public interface IPMM05000  
    {
        IAsyncEnumerable<PropertyDTO> GetPropertyList();
        IAsyncEnumerable<UnitTypeCategoryDTO> GetUnitTypeCategoryList();
        IAsyncEnumerable<PricingDTO> GetPricingList();
        IAsyncEnumerable<PricingDTO> GetPricingDateList();
        PricingDumpResultDTO SavePricing(PricingSaveParamDTO poParam);
        IAsyncEnumerable<TypeDTO> GetPriceChargesType();
        PricingDumpResultDTO ActiveInactivePricing(PricingParamDTO poParam);
    }
}

