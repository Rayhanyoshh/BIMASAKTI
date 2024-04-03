using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using PMM05000Common;
using PMM05000Common.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;

namespace PMM05000Model
{
    public class PMM05010ViewModel: R_ViewModel<PMM05010DTO>
    {
        private PMM05010Model _PMM05010Model = new PMM05010Model();
        public ObservableCollection<PMM05010DTO> UnitTypeList { get; set; } = new ObservableCollection<PMM05010DTO>();
        public PMM05010DTO UnitType = new PMM05010DTO();
        public string propertyId = "";
        public string UnitTypeId = "";

        
        public async Task GetUnitTypeGridList()
        {
            var loEx = new R_Exception();
            R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, propertyId);
            R_FrontContext.R_SetStreamingContext(ContextConstant.CUNIT_TYPE_ID, UnitTypeId);
            try
            {
                var loReturn = await _PMM05010Model.GetAllUnitTypeAsync();
                UnitTypeList = new ObservableCollection<PMM05010DTO>(loReturn.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
    }
}

