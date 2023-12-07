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
    public class GSM01010ViewModel: R_ViewModel<GSM01010DTO>
    {
        private GSM01010Model _GSM01010Model = new GSM01010Model();
        public ObservableCollection<GSM01010DTO> loGridGoAList { get; set; } = new ObservableCollection<GSM01010DTO>();
        public GSM01010DTO loEntity = new GSM01010DTO();
        public R_ContextHeader _ContextHeader { get; set; }
        public string _cglAccountNo { get; set; }
        
        public async Task GetGOAListbyGL()
        {
            var loEx = new R_Exception();

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, _cglAccountNo);
                
                var loResult = await _GSM01010Model.GetAllGoAAsync();
                loGridGoAList = new ObservableCollection<GSM01010DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}