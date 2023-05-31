using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GSM01000Common.DTOs;

namespace GSM01000Model.ViewModel
{
    public class GSM01300ViewModel : R_ViewModel<GSM01300DTO>
    {
        private GSM01300Model _GSM01300Model = new GSM01300Model();
        
        public ObservableCollection<AssignCoADTO> CoAToAssignList { get; set; } = new ObservableCollection<AssignCoADTO>();

        public ObservableCollection<GSM01300DTO> loGridGoAList { get; set; } = new ObservableCollection<GSM01300DTO>();
        public GSM01300DTO loEntity = new GSM01300DTO();
        
        public R_ContextHeader _ContextHeader { get; set; }
        
        public async Task GetGridGoAList()
        {
            R_Exception loEx = new R_Exception();
            GSM01300ListDTO loResult = null;
            try
            {
                loResult = await _GSM01300Model.GetAllGOAAsync();
                loGridGoAList = new ObservableCollection<GSM01300DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetGoASingle(GSM01300DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                GSM01300DTO loParam = new GSM01300DTO();
                loParam = poParam;
                var loResult = await _GSM01300Model.R_ServiceGetRecordAsync(loParam);
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetEntity(GSM01300DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                loEntity = await _GSM01300Model.R_ServiceGetRecordAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
    }
}