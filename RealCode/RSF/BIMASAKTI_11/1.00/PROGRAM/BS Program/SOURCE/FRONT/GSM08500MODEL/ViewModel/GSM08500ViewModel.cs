using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GSM008500Common.DTOs;
using GSM08500Common.DTOs;
using GSM008500Common.DTOs.PrintDTO;

namespace GSM08500Model.ViewModel
{
    public class GSM08500ViewModel : R_ViewModel<GSM08500DTO>
    {
        private GSM08500Model _GSM08500Model = new GSM08500Model();
        public GSM08500DTO loEntity = new GSM08500DTO();
        public ObservableCollection<GSM08500DTO> loGridList { get; set; } = new ObservableCollection<GSM08500DTO>();

        public ObservableCollection<CopyFromProcessCompanyDTO> loCompanyList = new ObservableCollection<CopyFromProcessCompanyDTO>();
        
        public CopyFromProcessCompanyListDTO loCompanyRtn = new CopyFromProcessCompanyListDTO();
        public R_ContextHeader _ContextHeader { get; set; }
        
        public string SelectedCopyFromCompanyId = "";
        public string SelectedActiveInactiveCenterCode = "";
        public bool SelectedActiveInactiveLACTIVE;
        
        public List<DropDownDTO> BSIS_Option { get; set; } = new List<DropDownDTO>();
        public List<DropDownDTO> CDBCR_Option { get; set; } = new List<DropDownDTO>();


        #region Print

        public GSM08500PrintParamStatAccDTO loPrint = new GSM08500PrintParamStatAccDTO();

        public List<GSM08500PrintParamStatAccDTO> SortBy { get; set; } =
            new List<GSM08500PrintParamStatAccDTO>()
            {
                new GSM08500PrintParamStatAccDTO() { CSHORT_BY = "01", SortBy = "Account No"},
                new GSM08500PrintParamStatAccDTO() { CSHORT_BY = "02", SortBy = "Account Name" },
            };
        #endregion

        public async Task GetGridList()
        {
            R_Exception loEx = new R_Exception();
            GSM08500ListDTO loResult = null;
            try
            {   
                loResult = await _GSM08500Model.GetAllStatAccAsync();
                loGridList = new ObservableCollection<GSM08500DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetStatAccSingle(GSM08500DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                GSM08500DTO loParam = new GSM08500DTO();
                loParam = poParam;
                var loResult = await _GSM08500Model.R_ServiceGetRecordAsync(loParam);
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetEntity(GSM08500DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                loEntity = await  _GSM08500Model.R_ServiceGetRecordAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task SaveEntity(GSM08500DTO poNewEntity, eCRUDMode peCRUDMode)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _GSM08500Model.R_ServiceSaveAsync(poNewEntity, peCRUDMode);
                loEntity = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task DeleteEntity(GSM08500DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM08500Model.R_ServiceDeleteAsync(poEntity);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #region Copy From Process
        public async Task GetCompanyListStreamAsync()
        {
            R_Exception loException = new R_Exception();
            try
            {
                loCompanyRtn = await _GSM08500Model.GetCompanyListStreamAsync();
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

                await _GSM08500Model.CopyFromProcessAsync();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
        #endregion
        
        public async Task ActiveInactiveProcessAsync()
        {
            R_Exception loException = new R_Exception();

            try
            {
                R_FrontContext.R_SetContext(ContextConstant.ACTIVE_INACTIVE_CGL_NO_CONTEXT, SelectedActiveInactiveCenterCode);
                R_FrontContext.R_SetContext(ContextConstant.ACTIVE_INACTIVE_LACTIVE_CONTEXT, SelectedActiveInactiveLACTIVE);

                await _GSM08500Model.RSP_GS_ACTIVE_INACTIVE_StatMethodAsync();
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }

        public async Task<GSM08500CoAExcelDTO> DownloadTemplate()
        {
            var loEx = new R_Exception();
            GSM08500CoAExcelDTO loResult = null;

            try
            {
                loResult = await _GSM08500Model.GSM01000DownloadTemplateFileModel();
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