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
    public class GSM01100ViewModel: R_ViewModel<GSM01100DTO>
    {
        private GSM01100Model _GSM01100Model = new GSM01100Model();
        public ObservableCollection<GSM01100DTO> loGridCoAUserList { get; set; } = new ObservableCollection<GSM01100DTO>();

        public List<AssignUserDTO> loGetUserAssignList = new List<AssignUserDTO>();
        
        public ObservableCollection<AssignUserDTO> UsersToAssignList { get; set; } = new ObservableCollection<AssignUserDTO>();

        public GSM01100DTO loEntity = new GSM01100DTO();

        public UsertoAssignDTO loUserEntity = new UsertoAssignDTO();
        public GSM01100DTO UserToAssign { get; set; } = new GSM01100DTO();
        public GSM01100UserCOAParameterDTO loParameter { get; set; } = new GSM01100UserCOAParameterDTO();

        public string lcCompany = "";
        public string lcUserList = "";
        public string UserID { get; set; } = "";
        public string cglAccount = ""; // for filter
        public R_ContextHeader _ContextHeader { get; set; }
        
        public async Task GetGridCoAUserList()
        {
            R_Exception loEx = new R_Exception();
            GSM01100UserListDTO loResult = null;
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);
                loResult = await _GSM01100Model.GetAllCoAUserAsync();
                loGridCoAUserList = new ObservableCollection<GSM01100DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetCoAUserSingle(GSM01100DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                GSM01100DTO loParam = new GSM01100DTO();
                loParam = poParam;
                var loResult = await _GSM01100Model.R_ServiceGetRecordAsync(loParam);
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task AssignUserToCOA(GSM01100DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);
                var loResult = await _GSM01100Model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                UserToAssign = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetUserToAssignList()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);
                var loResult = await _GSM01100Model.GetUserToAssignListAsync();
                UsersToAssignList = new ObservableCollection<AssignUserDTO>(loResult);
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
                GSM01100UserCOAParameterDTO loResult = await _GSM01100Model.GetParameterInfoAsync(pcCGLACCOUNT_NO: "");
                loParameter = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        public async Task SaveUserAssign(string pcIdSelectedUserCombinedWithCommaSeparator)
        {
            var loException = new R_Exception();
            R_FrontContext.R_SetStreamingContext(ContextConstant.CGLACCOUNT_NO, cglAccount);

            try
            {
                UsertoAssignParam loContent = new UsertoAssignParam()
                {
                    CCOMPANY_ID = loEntity.CCOMPANY_ID,
                    CGLACCOUNT_NO = cglAccount,
                    CUSER_ID = loEntity.CUSER_ID,
                    CUSER_LIST = pcIdSelectedUserCombinedWithCommaSeparator
                };

                await _GSM01100Model.AssignUserActionAsync(poParameter: loContent);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }

    }
    
   
}
