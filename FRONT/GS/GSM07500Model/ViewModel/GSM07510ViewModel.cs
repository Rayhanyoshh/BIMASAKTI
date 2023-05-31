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
using R_BlazorFrontEnd.Helpers;

namespace GSM07500Model.ViewModel
{
    public class GSM07510ViewModel : R_ViewModel<GSM07510DTO>
    {
        private GSM07510Model _GSM07510Model = new GSM07510Model();
        public GSM07510DTO Period = new GSM07510DTO();
        public ObservableCollection<GSM07510DTO> loGridPeriodList { get; set; } = new ObservableCollection<GSM07510DTO>();
        
        
        public async Task GetGridPeriodList()
        {
            R_Exception loEx = new R_Exception();
            
            try
            {   
                 var loResult = await _GSM07510Model.GetPeriodListAsync();
                 loGridPeriodList = new ObservableCollection<GSM07510DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetPeriodSingle(GSM07510DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _GSM07510Model.R_ServiceGetRecordAsync(poParam);
                Period = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        
        public List<RadioModel> PeriodModeOption { get; set; } = new List<RadioModel>
        {
            new RadioModel { Id = "False", Text = "Custom Period"},
            new RadioModel { Id = "True", Text = "Normal Calendar" },
        };

        public class RadioModel
        {
            public string Id { get; set; }
            public string Text { get; set; }
        }
    }
}