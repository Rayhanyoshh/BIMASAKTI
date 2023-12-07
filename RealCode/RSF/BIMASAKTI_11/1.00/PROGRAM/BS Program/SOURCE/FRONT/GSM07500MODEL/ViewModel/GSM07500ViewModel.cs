using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using GSM07500Common;
using GSM07500Common.DTOs;
using R_BlazorFrontEnd.Helpers;

namespace GSM07500Model.ViewModel
{
    public class GSM07500ViewModel  : R_ViewModel<GSM07500DTO>
    {
        private GSM07500Model _GSM07500Model = new GSM07500Model();
        private GSM07510ViewModel _GSM07510ViewModel = new GSM07510ViewModel();
        public GSM07500DTO loEntity = new GSM07500DTO();

        public SaveBatchGSM07500DTO loDetailEntity = new SaveBatchGSM07500DTO();
        public ObservableCollection<GSM07500DTO> loGridPeriodDetailList { get; set; } = new ObservableCollection<GSM07500DTO>();
        public string _cCYear { get; set; }
        public string _lPeriodMode { get; set; }
        public string _iNoPeriod { get; set; }

        public string _CCYEAR = "";



        public async Task GetGridPeriodDetailList(GSM07500DTO poParam)
        {
            R_Exception loEx = new R_Exception();
            
            try
            {   
                R_FrontContext.R_SetContext(ContextConstant.CCYEAR, _CCYEAR);
                
                var loReturn = await _GSM07500Model.GetPeriodDetailListAsync();
                
                var loResult = R_FrontUtility.ConvertCollectionToCollection<GSM07500DTO>(loReturn.Data);
               
                foreach (var list in loResult)
                {
                    list.DSTART_DATE = DateTime.ParseExact(list.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                    list.DEND_DATE = DateTime.ParseExact(list.CEND_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                loGridPeriodDetailList = new ObservableCollection<GSM07500DTO>(loResult);
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetGenaratorListPeriod(GSM07510DTO poParam, string poYearParam)
        {
            R_Exception loEx = new R_Exception();

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CYEAR, poYearParam);
                R_FrontContext.R_SetStreamingContext(ContextConstant.LPERIOD_MODE, true);
                R_FrontContext.R_SetStreamingContext(ContextConstant.INO_PERIOD, 12);
                var loReturn = await _GSM07500Model.GetGenerateGSMPeriodStream();
               
                var loResult = R_FrontUtility.ConvertCollectionToCollection<GSM07500DTO>(loReturn.Data);

                
                foreach (var list in loResult)
                {
                    list.DSTART_DATE = DateTime.ParseExact(list.CSTART_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                    list.DEND_DATE = DateTime.ParseExact(list.CEND_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
                }
                
                
                loGridPeriodDetailList = new ObservableCollection<GSM07500DTO>(loResult);
                // loDetailEntity.CYEAR = Data.CCYEAR;
                // loDetailEntity.LPERIOD_MODE = _GSM07510ViewModel.Data.LPERIOD_MODE;
                // loDetailEntity.CPERIOD_LIST = new List<GSM07500DTO>(loResult);
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetPeriodDetailSingle(SaveBatchGSM07500DTO poEntity)
        {
            var loEx = new R_Exception();
        
            try
            {
                
            
                // await _GSM07500Model.R_ServiceSaveAsync(poEntity, peCRUDMode) ;
                var LoTempResult = await _GSM07500Model.R_ServiceGetRecordAsync(poEntity);
                loDetailEntity = LoTempResult;
            }
            
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        
            loEx.ThrowExceptionIfErrors();
        }
        
        

        // public async Task SaveMultipleUserAssignmentAsync()
        // {
        //     R_Exception loException = new R_Exception();
        //
        //     try
        //     {
        //         SaveMultipleUserAssignmentParameterDTO loParam = new SaveMultipleUserAssignmentParameterDTO();
        //
        //         loParam.CSELECTED_USER_ID_LIST = new List<string>();
        //
        //         foreach (var item in loSelectedUser)
        //         {
        //             if (item.LSELECTED)
        //             {
        //                 loParam.CSELECTED_USER_ID_LIST.Add(item.CUSER_ID);
        //             }
        //         }
        //         loParam.CAPPROVAL_CODE = GSM07011_SELECTED_APPROVAL_CODE;
        //
        //         R_FrontContext.R_SetContext(ContextConstant.GSM07011_SAVE_MULTIPLE_USER_ASSIGNMENT_CONTEXT, loParam);
        //         await loModel.SaveMultipleUserAssignmentAsync();
        //     }
        //     catch (Exception ex)
        //     {
        //         loException.Add(ex);
        //     }
        //     EndBlock:
        //     loException.ThrowExceptionIfErrors();
        // }
    }
    
    
}