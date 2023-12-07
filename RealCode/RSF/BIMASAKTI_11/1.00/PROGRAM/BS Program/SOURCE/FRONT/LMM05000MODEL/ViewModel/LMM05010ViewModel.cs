using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using LMM05000Common;
using LMM05000Common.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;

namespace LMM05000Model
{
    public class LMM05010ViewModel: R_ViewModel<LMM05010DTO>
    {
        private LMM05010Model _LMM05010Model = new LMM05010Model();
        public ObservableCollection<LMM05010DTO> UnitTypeList { get; set; } = new ObservableCollection<LMM05010DTO>();
        public LMM05010DTO UnitType = new LMM05010DTO();
        public string propertyId = "";
        public string UnitTypeId = "";

        
        public async Task GetUnitTypeGridList()
        {
            var loEx = new R_Exception();
            R_FrontContext.R_SetStreamingContext(ContextConstant.CPROPERTY_ID, propertyId);
            R_FrontContext.R_SetStreamingContext(ContextConstant.CUNIT_TYPE_ID, UnitTypeId);
            try
            {
                var loReturn = await _LMM05010Model.GetAllUnitTypeAsync();
                UnitTypeList = new ObservableCollection<LMM05010DTO>(loReturn.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
    }
}

