using R_CommonFrontBackAPI;
using System.Collections.Generic;
using PMM05000Common.DTOs;

namespace PMM05000Common
{
    public interface IPMM05010
    {
        PricingDumpResultDTO SavePricingRate(PricingRateSaveParamDTO poParam);
        IAsyncEnumerable<PricingRateDTO> GetPricingRateList();
        IAsyncEnumerable<PricingRateDTO> GetPricingRateDateList();
    }
}

