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
    public class GSM01200ViewModel : R_ViewModel<GSM01200DTO>
    {
        private GSM01200Model _GSM01200Model = new GSM01200Model();
        public ObservableCollection<AssignCenterDTO> CenterToAssignList { get; set; } = new ObservableCollection<AssignCenterDTO>();

        public List<AssignCenterDTO> loGetCenterAssignList = new List<AssignCenterDTO>();
        public ObservableCollection<GSM01200DTO> loGridCoACenterList { get; set; } = new ObservableCollection<GSM01200DTO>();

        public GSM1200CenterCoAParameterDTO loParameter { get; set; } = new GSM1200CenterCoAParameterDTO();
        public GSM01200DTO CenterToAssign { get; set; } = new GSM01200DTO();

        public GSM01200DTO loEntity = new GSM01200DTO();

        public R_ContextHeader _ContextHeader { get; set; }

        public string UserId { get; set; } = "";
        public string cglAccount { get; set; } = "";

        public async Task GetGridCoACenterList()
        {
            R_Exception loEx = new R_Exception();
            GSM01200CenterListDTO loResult = null;

            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);
                loResult = await _GSM01200Model.GetAllCoACenterAsync();
                loGridCoACenterList = new ObservableCollection<GSM01200DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetCoACenterSingle(GSM01200DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                GSM01200DTO loParam = new GSM01200DTO();
                loParam = poParam;
                var loResult = await _GSM01200Model.R_ServiceGetRecordAsync(loParam);
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetCenterToAssignList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);
                var loResult = await _GSM01200Model.GetCenterToAssignListAsync();
                CenterToAssignList = new ObservableCollection<AssignCenterDTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        public async Task AssignCenterToCOA(GSM01200DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);
                var loResult = await _GSM01200Model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                CenterToAssign = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetParameterInfo()
        {
            var loEx = new R_Exception();

            try
            {
                GSM1200CenterCoAParameterDTO loResult = await _GSM01200Model.GetParameterInfoAsync();
                loParameter = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveCenterAssign(string lcCombinedCenterWithCommaSeparator)
        {
            var loException = new R_Exception();
            R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);

            try
            {
                CentertoAssignParam loContent = new CentertoAssignParam()
                {
                    CCOMPANY_ID = loEntity.CCOMPANY_ID,
                    CGLACCOUNT_NO = cglAccount,
                    CUSER_ID = loEntity.CUSER_ID,
                    CCENTER_LIST = lcCombinedCenterWithCommaSeparator
                };

                await _GSM01200Model.AssignCenterActionAsync(poParameter: loContent);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}