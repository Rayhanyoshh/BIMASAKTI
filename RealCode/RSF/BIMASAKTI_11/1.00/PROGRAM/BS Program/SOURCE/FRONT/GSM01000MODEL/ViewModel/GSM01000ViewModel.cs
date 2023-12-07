using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Helpers;
using GSM01000Common;
using GSM01000Common.DTOs;

namespace GSM01000Model
{
    public class GSM01000ViewModel : R_ViewModel<GSM01000DTO>
    {
        private GSM01000Model _GSM01000Model = new GSM01000Model();
        public GSM01000DTO loEntity = new GSM01000DTO();
        public ObservableCollection<GSM01000DTO> loGridList { get; set; } = new ObservableCollection<GSM01000DTO>();
        public ObservableCollection<CopyFromProcessCompanyDTO> loCompanyList = new ObservableCollection<CopyFromProcessCompanyDTO>();
        
        public CopyFromProcessCompanyListDTO loCompanyRtn = new CopyFromProcessCompanyListDTO();
        public R_ContextHeader _ContextHeader { get; set; }
        
        public string SelectedCopyFromCompanyId = "";
        public string SelectedActiveInactiveCenterCode = "";
        public bool SelectedActiveInactiveLACTIVE;
        
        public List<DropDownDTO> BSIS_Option { get; set; } = new List<DropDownDTO>();
        public List<DropDownDTO> CDBCR_Option { get; set; } = new List<DropDownDTO>();
        
        public PrimaryAccountDTO loHasil = new PrimaryAccountDTO();

        #region Print

        public GSM01000PrintParamCOADTO loPrint = new GSM01000PrintParamCOADTO();

        public List<GSM01000PrintParamCOADTO> SortBy { get; set; } =
        new List<GSM01000PrintParamCOADTO>()
        {
                new GSM01000PrintParamCOADTO() { CSHORT_BY = "01", SortBy = "Code"},
                new GSM01000PrintParamCOADTO() { CSHORT_BY = "02", SortBy = "Name" },
        };


        #endregion

        public async Task GetGridList()
        {
            R_Exception loEx = new R_Exception();
            GSM01000ListDTO loResult = null;
            try
            {   
                loResult = await _GSM01000Model.GetAllAsync();
                loGridList = new ObservableCollection<GSM01000DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetCoASingle(GSM01000DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                GSM01000DTO loParam = new GSM01000DTO();
                loParam = poParam;
                var loResult = await _GSM01000Model.R_ServiceGetRecordAsync(loParam);
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetEntity(GSM01000DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                loEntity = await _GSM01000Model.R_ServiceGetRecordAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task DeleteEntity(GSM01000DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01000Model.R_ServiceDeleteAsync(poEntity);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task SaveEntity(GSM01000DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _GSM01000Model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetCompanyListStreamAsync()
        {
            R_Exception loException = new R_Exception();
            try
            {
                loCompanyRtn = await _GSM01000Model.GetCompanyListStreamAsync();
                loCompanyList = new ObservableCollection<CopyFromProcessCompanyDTO>(loCompanyRtn.Data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
        
        public async Task CopyFromProcessAsync()
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_FrontContext.R_SetContext(ContextConstant.COPY_FROM_COMPANY_ID_CONTEXT, SelectedCopyFromCompanyId);

                await _GSM01000Model.CopyFromProcessAsync();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
        
        public async Task ActiveInactiveProcessAsync()
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_FrontContext.R_SetContext(ContextConstant.ACTIVE_INACTIVE_CGL_NO_CONTEXT, SelectedActiveInactiveCenterCode);
                R_FrontContext.R_SetContext(ContextConstant.ACTIVE_INACTIVE_LACTIVE_CONTEXT, SelectedActiveInactiveLACTIVE);

                await _GSM01000Model.RSP_GS_ACTIVE_INACTIVE_COAMethodAsync();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
        
        public async Task<bool> GetResultPrimaryAcc()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _GSM01000Model.PrimaryAccModel();
                // loHasil.LPRIMARY_ACCOUNT = false;
                loHasil = R_FrontUtility.ConvertObjectToObject<PrimaryAccountDTO>(loResult);
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loHasil.LPRIMARY_ACCOUNT;
        }

   
        public async Task<GSM01000CoAExcelDTO> DownloadTemplate()
        {
            var loEx = new R_Exception();
            GSM01000CoAExcelDTO loResult = null;

            try
            {
                loResult = await _GSM01000Model.GSM01000DownloadTemplateFileModel();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return loResult;
        }
        
        
    }
}
