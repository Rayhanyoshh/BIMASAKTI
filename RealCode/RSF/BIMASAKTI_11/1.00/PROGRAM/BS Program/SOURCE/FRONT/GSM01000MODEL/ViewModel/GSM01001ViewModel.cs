using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd;
using System.Collections.ObjectModel;
using System.Data;
using R_CommonFrontBackAPI;
using R_BlazorFrontEnd.Helpers;
using R_ProcessAndUploadFront;
using R_APICommonDTO;
using System.Linq;
using System.Globalization;
using System.Text.Json;
using GSM01000Common;
using GSM01000Common.DTOs;
using R_BlazorFrontEnd.Excel;

namespace GSM01000Model
{
    public class GSM01001ViewModel   : R_ViewModel<GSM01001ErrorValidateDTO>, R_IProcessProgressStatus
    {
        private GSM01000Model _GSM01000Model = new GSM01000Model();
        
        // Action StateHasChanged
        public Action StateChangeAction { get; set; }
        
        // Action Get Error Unhandle
        public Action<R_APIException> ShowErrorAction { get; set; }
        
        //func process is Success
        public Func<Task> ShowSuccessAction { get; set; }
        
        // Action Get DataSet
        public Func<Task> ActionDataSetExcel { get; set; }
        




        // DataSet Excel 
        public DataSet ExcelDataSet { get; set; }
        
        
        public string CompanyValue { get; set; } = "";
        public string SourceFileName { get; set; } = "";
        public string Message { get; set; } = "";
        public int Percentage { get; set; } = 0;
        public bool OverwriteData { get; set; } = false;
        public bool VisibleError { get; set; } = false;
        public bool BtnSave { get; set; } = true;
        public int SumListCOAExcel { get; set; }
        public int SumValidDataCOAExcel { get; set; }
        public int SumInvalidDataCOAExcel { get; set; }
        public string CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string UserId { get; set; }
        
        public GSM01000UploadHeaderDTO loCompany = new GSM01000UploadHeaderDTO();

        // Grid Display COA Upload
        public ObservableCollection<GSM01001ErrorValidateDTO> COAValidateUploadError { get; set; } = new ObservableCollection<GSM01001ErrorValidateDTO>();

        
        public async Task ConvertGrid(List<GSM01001DTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                // Onchange Visible Error
                VisibleError = false;
                SumValidDataCOAExcel = 0;
                SumInvalidDataCOAExcel = 0;

                R_FrontContext.R_SetContext(ContextConstant.CCOMPANY_ID, CompanyID);

                // Convert Excel DTO and add SeqNo
                var loData = poEntity.Select((loTemp, i) => new GSM01001ErrorValidateDTO()
                {
                    NO = i+1,
                    CCOMPANY_ID = CompanyID,
                    CGLACCOUNT_NO = loTemp.AccountNo,
                    CGLACCOUNT_NAME = loTemp.AccountName,
                    LACTIVE = loTemp?.Active ?? false,
                    CBSIS = loTemp.BSIS,
                    CDBCR = loTemp.DC,
                    CCASH_FLOW_CODE = loTemp?.CashFlowCode ?? "",
                    CCASH_FLOW_GROUP_CODE = loTemp?.CashFlowGroupCode ?? "",
                    LUSER_RESTR = loTemp?.UserRestriction ?? false,
                    LCENTER_RESTR = loTemp?.CenterRestriction ?? false,
                    NonActiveDate = loTemp?.NonActiveDate ?? "",
                    NonActiveDateDisplay = !string.IsNullOrWhiteSpace(loTemp.NonActiveDate) ? DateTime.ParseExact(loTemp.NonActiveDate, "yyyyMMdd", CultureInfo.InvariantCulture) : default,
                    Valid = loTemp?.Valid ?? "",
                    ErrorMessage = "",
                    Var_Exists = loTemp.Var_Exists,
                    Var_Overwrite = loTemp.Var_Overwrite,
                    
                }).ToList();

                SumListCOAExcel = loData.Count;

                COAValidateUploadError = new ObservableCollection<GSM01001ErrorValidateDTO>(loData);

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }

        public async Task SaveBulkFile()
        {
            var loEx = new R_Exception();
            R_BatchParameter loBatchPar;
            R_ProcessAndUploadClient loCls;
            List<GSM01001ErrorValidateDTO> Bigobject;
            List<R_KeyValue> loUserParameneters;

            try
            {
                // set Param
                loUserParameneters = new List<R_KeyValue>();
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.CCOMPANY_ID, Value = CompanyID });
                loUserParameneters.Add(new R_KeyValue() { Key = ContextConstant.COVERWRITE, Value = OverwriteData });

                //Instantiate ProcessClient
                loCls = new R_ProcessAndUploadClient(
                    pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl",
                    poProcessProgressStatus: this);

                //Set Data
                if (COAValidateUploadError.Count == 0)
                    return;

                Bigobject = COAValidateUploadError.ToList<GSM01001ErrorValidateDTO>();

                //preapare Batch Parameter
                loBatchPar = new R_BatchParameter();

                loBatchPar.COMPANY_ID = CompanyID;
                loBatchPar.USER_ID = UserId; ;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = "GSM01000Back.GSM01001Cls";
                loBatchPar.BigObject = Bigobject;

                var lcGuid = await loCls.R_BatchProcess<List<GSM01001ErrorValidateDTO>>(loBatchPar, 13);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        
        
        
         #region Status
        public async Task ProcessComplete(string pcKeyGuid, eProcessResultMode poProcessResultMode)
        {
            var loEx = new R_Exception();

            try
            {
                if (poProcessResultMode == eProcessResultMode.Success)
                {
                    Message = string.Format("Process Complete and success with GUID {0}", pcKeyGuid);
                    VisibleError = false;
                    await ShowSuccessAction();
                }

                if (poProcessResultMode == eProcessResultMode.Fail)
                {
                    Message = $"Process Complete but fail with GUID {pcKeyGuid}";
                    await ServiceGetError(pcKeyGuid);
                    VisibleError = true;
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            // Call Method Action StateHasChange
            StateChangeAction();

            loEx.ThrowExceptionIfErrors();
        }

        public async Task ProcessError(string pcKeyGuid, R_APIException ex)
        {
            Message = string.Format("Process Error with GUID {0}", pcKeyGuid);

            // Call Method Action Error Unhandle
            ShowErrorAction(ex);

            // Call Method Action StateHasChange
            StateChangeAction();

            await Task.CompletedTask;
        }

        public async Task ReportProgress(int pnProgress, string pcStatus)
        {
            Message = string.Format("Process Progress {0}% with status {1}", pnProgress, pcStatus);

            Percentage = pnProgress;
            Message = string.Format("Process Progress {0}% with status {1}", pnProgress, pcStatus);

            // Call Method Action StateHasChange
            StateChangeAction();

            await Task.CompletedTask;
        }

        private async Task ServiceGetError(string pcKeyGuid)
        {
            R_Exception loException = new R_Exception();

            List<R_ErrorStatusReturn> loResultData;
            R_GetErrorWithMultiLanguageParameter loParameterData;
            R_ProcessAndUploadClient loCls;

            try
            {
                // Add Parameter
                loParameterData = new R_GetErrorWithMultiLanguageParameter()
                {
                    COMPANY_ID = CompanyID,
                    USER_ID = UserId,
                    KEY_GUID = pcKeyGuid,
                    RESOURCE_NAME = "RSP_GS_UPLOAD_COAResources"
                };

                loCls = new R_ProcessAndUploadClient(pcModuleName: "GS",
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: "R_DefaultServiceUrl");

                // Get error result
                loResultData = await loCls.R_GetStreamErrorProcess(loParameterData);

                // check error if unhandle
                if (loResultData.Any(y => y.SeqNo <= 0))
                {
                    var loUnhandleEx = loResultData.Where(y => y.SeqNo <= 0).Select(x => new R_BlazorFrontEnd.Exceptions.R_Error(x.SeqNo.ToString(), x.ErrorMessage)).ToList();
                    loUnhandleEx.ForEach(x => loException.Add(x));
                }

                if (loResultData.Any(y => y.SeqNo > 0))
                {
                    // Display Error Handle if get seq
                    COAValidateUploadError.ToList().ForEach(x =>
                    {
                        //Assign ErrorMessage, Valid and Set Valid And Invalid Data
                        if (loResultData.Any(y => y.SeqNo == x.NO))
                        {
                            x.ErrorMessage = loResultData.Where(y => y.SeqNo == x.NO).FirstOrDefault().ErrorMessage;
                            x.Valid = "N";
                            SumInvalidDataCOAExcel++;
                        }
                        else
                        {
                            x.Valid = "Y";
                            SumValidDataCOAExcel++;
                        }
                    });

                    
                    //Set DataSetTable and get error
                    var loExcelData = COAValidateUploadError.Select(x => new GSM01001ExcelDTO
                    {
                        // Mapping properti x ke GSM01001ExcelDTO
                        No = x.NO,
                        AccountNo = x.CGLACCOUNT_NO,
                        AccountName = x.CGLACCOUNT_NAME,
                        Active = x.LACTIVE,
                        BSIS = x.CBSIS,
                        DC = x.CDBCR,
                        CashFlowCode = x.CCASH_FLOW_CODE,
                        CashFlowGroupCode = x.CCASH_FLOW_GROUP_CODE,
                        UserRestriction = x.LUSER_RESTR,
                        CenterRestriction = x.LCENTER_RESTR,
                        NonActiveDate = x.NonActiveDate,
                        ErrorMessage = x.ErrorMessage,
                    }).ToList();

                    var loDataTable = R_FrontUtility.R_ConvertTo<GSM01001ExcelDTO>(loExcelData);
                    loDataTable.TableName = "COA";

                    var loDataSet = new DataSet();
                    loDataSet.Tables.Add(loDataTable);

                    // Asign Dataset
                    ExcelDataSet = loDataSet;

                    // Dowload if get Error
                    //await ActionDataSetExcel.Invoke();
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }
        #endregion
        
       
      
    
       
          
        public async Task GetCompanyDetailAsync()
        {
            var loEx = new R_Exception();

            try
            {

                var loResult = await _GSM01000Model.CompanyDetailModel();

                loCompany = R_FrontUtility.ConvertObjectToObject<GSM01000UploadHeaderDTO>(loResult);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
       
    }
}