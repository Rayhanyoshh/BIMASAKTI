using LMM01500Common;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LMM01500Common.DTOs;

namespace LMM01500Model.ViewModel
{
    public class LMM01500ViewModel : R_ViewModel<LMM01500DTO>
    {
        private LMM01500Model _LMM01500Model = new LMM01500Model();
        
        public ObservableCollection<LMM01501DTO> InvoinceGroupGrid { get; set; } = new ObservableCollection<LMM01501DTO>();
        public List<LMM01500PropertyDTO> PropertyList { get; set; } = new List<LMM01500PropertyDTO>();
        
        
        public LMM01500DTO InvoiceGroup = new LMM01500DTO();

        public string PropertyValueContext = "";
        public bool StatusChange;
        public bool TabDept = false;
        
        
        public async Task GetPropertyList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _LMM01500Model.GetPropertyAsync();
                PropertyList = loResult;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetInvoiceGroupList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _LMM01500Model.GetInvoiceGrpListAsync(PropertyValueContext);
                InvoinceGroupGrid = new ObservableCollection<LMM01501DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetInvoiceGroup(LMM01500DTO poParam)
        {
            var loEx = new R_Exception();
            try
            {
                var loResult = await _LMM01500Model.R_ServiceGetRecordAsync(poParam);

                loResult.CSEQUENCEInt = int.Parse(loResult.CSEQUENCE);
                TabDept = loResult.LBY_DEPARTMENT;

                InvoiceGroup = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        

    }
}