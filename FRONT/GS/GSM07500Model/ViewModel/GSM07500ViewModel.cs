using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GSM07500Common;
using GSM07500Common.DTOs;

namespace GSM07500Model.ViewModel
{
    public class GSM07500ViewModel  : R_ViewModel<GSM07500DTO>
    {
        private GSM07500Model _GSM07500Model = new GSM07500Model();
        public GSM07500DTO loEntity = new GSM07500DTO();
        public ObservableCollection<GSM07500DTO> loGridPeriodDetailList { get; set; } = new ObservableCollection<GSM07500DTO>();
        public string _cCYear { get; set; }
        
        public async Task GetGridPeriodDetailList()
        {
            R_Exception loEx = new R_Exception();
            
            try
            {   
                R_FrontContext.R_SetStreamingContext(ContextConstant.CCYEAR, _cCYear);
                var loResult = await _GSM07500Model.GetPeriodDetailListAsync();
                loGridPeriodDetailList = new ObservableCollection<GSM07500DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
    
    
}