using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GSM01000Common;
using GSM01000Common.DTOs;

namespace GSM01000Model
{
    public class GSM01310ViewModel : R_ViewModel<GSM01310DTO>
    {
        private GSM01310Model _GSM01310Model = new GSM01310Model();
        public ObservableCollection<GSM01310DTO> loGridGoACoAList { get; set; } = new ObservableCollection<GSM01310DTO>();
        public ObservableCollection<GSM01310DTO> CoAToAssignList { get; set; } = new ObservableCollection<GSM01310DTO>();

        public GSM01310DTO loEntity = new GSM01310DTO();
        public R_ContextHeader _ContextHeader { get; set; }
        public string _cGOACode { get; set; }
        public string loGOA { get; set; }

        public async Task GetGoACoAList()
        {
            var loEx = new R_Exception();

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGOA_CODE, _cGOACode);
                
                var loResult = await _GSM01310Model.GetAllGoACoAAsync();
                loGridGoACoAList = new ObservableCollection<GSM01310DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetCoAAssignList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _GSM01310Model.GetCoAToAssignListAsync();
                CoAToAssignList = new ObservableCollection<GSM01310DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task AssignCoAToGOA(GSM01310DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var loResult = await _GSM01310Model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                // poNewEntity.CGLACCOUNT_NO = "11.10.1000";
                // poNewEntity.CGLACCOUNT_NAME = "BANK";
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }    }
}