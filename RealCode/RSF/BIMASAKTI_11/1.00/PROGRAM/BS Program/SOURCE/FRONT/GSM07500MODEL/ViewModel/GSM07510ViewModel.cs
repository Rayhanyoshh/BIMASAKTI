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
using System.Security.Cryptography.X509Certificates;

namespace GSM07500Model.ViewModel
{
    public class GSM07510ViewModel : R_ViewModel<GSM07510DTO>
    {
        private GSM07510Model _GSM07510Model = new GSM07510Model();
        public GSM07510DTO Period = new GSM07510DTO();
        public LastYearDTO LastYear = new LastYearDTO();
        public ObservableCollection<GSM07510DTO> loGridPeriodList { get; set; } = new ObservableCollection<GSM07510DTO>();
        public string DescriptionPeriod = "";
        
        
        public string Cyear = "";
        public int InoPeriod;
        public bool LperiodMode;
        
        public async Task GetGridPeriodList()
        {
            R_Exception loEx = new R_Exception();
            
            try
            {   
                 var loResult = await _GSM07510Model.GetPeriodListAsync();
                //R_FrontContext.R_SetContext(ContextConstant.CCYEAR, Cyear);

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
                R_FrontContext.R_SetStreamingContext(ContextConstant.CYEAR, poParam.CYEAR);
                R_FrontContext.R_SetStreamingContext(ContextConstant.LPERIOD_MODE, poParam.LPERIOD_MODE);
                R_FrontContext.R_SetStreamingContext(ContextConstant.INO_PERIOD, poParam.INO_PERIOD);
                var loResult = await _GSM07510Model.R_ServiceGetRecordAsync(poParam);
                Period = loResult;
                // Period = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteEntity(GSM07510DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM07510Model.R_ServiceDeleteAsync(poEntity);

            }
            catch (Exception ex)
            {
                loEx.Add(ex); 
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task<string> GetLastYearSingle(GSM07510DTO poEntity)
         {
            var loEx = new R_Exception();

            try
            {
                //R_FrontContext.R_SetStreamingContext(ContextConstant.CCOMPANY_ID, poEntity.CCOMPANY_ID);
                var loResult = await _GSM07510Model.GetLastYearAsync();
                LastYear = R_FrontUtility.ConvertObjectToObject<LastYearDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return LastYear.NEXT_YEAR;
        }

        public List<GSM07510DTO> PeriodModeOption { get; set; } = new List<GSM07510DTO>
        {
    
            new GSM07510DTO { LPERIOD_MODE = true, DescPeriodMode = "Normal Calendar"},
            new GSM07510DTO { LPERIOD_MODE = false,  DescPeriodMode = "Custom Period"},
        };
        
        public async Task SavePeriodDetailGenerator(GSM07510DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();
        
            try
            {
                var loResult = await _GSM07510Model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                // R_FrontContext.R_SetStreamingContext(ContextConstant.CYEAR, Cyear);
                // R_FrontContext.R_SetStreamingContext(ContextConstant.LPERIOD_MODE, true);
                // R_FrontContext.R_SetStreamingContext(ContextConstant.INO_PERIOD, 12);
                Period = loResult;
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        
            loEx.ThrowExceptionIfErrors();
        }
        
        // public async Task PeriodModeCondition()
        // {
        //     if (Data.LPERIOD_MODE == true)
        //     {
        //         
        //     }
        // }



    }
}