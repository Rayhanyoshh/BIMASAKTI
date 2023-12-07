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
using GSM008500Common.DTOs;
using GSM08500Common;
using GSM08500Common.DTOs;
using R_BlazorFrontEnd.Excel;

namespace GSM08500Model
{
    public class GSM08501ViewModel   : R_ViewModel<GSM08501ErrorValidateDTO>, R_IProcessProgressStatus
    {
        private GSM08500Model _GSM08500Model = new GSM08500Model();
        
        // Action StateHasChanged
        public Action StateChangeAction { get; set; }

        // Action Get Error Unhandle
        public Action<R_APIException> ShowErrorAction { get; set; }

        // Action Get DataSet
        public Func<Task> ActionDataSetExcel { get; set; }

        // DataSet Excel 
        public DataSet ExcelDataSet { get; set; }
        
        
        public string CompanyValue { get; set; } = "";
        public string CompanyName { get; set; } = "";
        public string SourceFileName { get; set; } = "";
        public string Message { get; set; } = "";
        public int Percentage { get; set; } = 0;
        public bool OverwriteData { get; set; } = false;
        public bool VisibleError { get; set; } = false;
        public bool BtnSave { get; set; } = true;
        public int SumListStaffExcel { get; set; }
        public int SumValidDataStaffExcel { get; set; }
        public int SumInvalidDataStaffExcel { get; set; }
        public string CompanyID { get; set; }
        public string UserId { get; set; }
        
        public GSM08500UploadHeaderDTO loCompany = new GSM08500UploadHeaderDTO();

        // Grid Display Staff Upload
        public ObservableCollection<GSM08501ErrorValidateDTO> STATISTIC_ACCOUNTValidateUploadError { get; set; } = new ObservableCollection<GSM08501ErrorValidateDTO>();

        
        public async Task ConvertGrid(List<GSM08501DTO> poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                // Onchange Visible Error
                VisibleError = false;
                SumValidDataStaffExcel = 0;
                SumInvalidDataStaffExcel = 0;

                R_FrontContext.R_SetContext(ContextConstant.CCOMPANY_ID, CompanyID);

                // Convert Excel DTO and add SeqNo
                var loData = poEntity.Select((loTemp, i) => new GSM08501ErrorValidateDTO()
                {
                    NO = i+1,
                    CCOMPANY_ID = CompanyID,
                    CGLACCOUNT_NO = loTemp.AccountNo,
                    CGLACCOUNT_NAME = loTemp.AccountName,
                    LACTIVE = loTemp.Active,
                    CBSIS = loTemp.BSIS,
                    CDBCR = loTemp.DC,
                    LUSER_RESTR = loTemp.UserRestriction,
                    LCENTER_RESTR = loTemp.CenterRestriction,
                    NonActiveDate = loTemp.NonActiveDate,
                    NonActiveDateDisplay = !string.IsNullOrWhiteSpace(loTemp.NonActiveDate) ? DateTime.ParseExact(loTemp.NonActiveDate, "yyyyMMdd", CultureInfo.InvariantCulture) : default,
                    Valid = "",
                    ErrorMessage = "",
                    Var_Exists = loTemp.Var_Exists,
                    Var_Overwrite = loTemp.Var_Overwrite
                }).ToList();

                SumListStaffExcel = loData.Count;

                STATISTIC_ACCOUNTValidateUploadError = new ObservableCollection<GSM08501ErrorValidateDTO>(loData);

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
            List<GSM08501ErrorValidateDTO> Bigobject;
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
                if (STATISTIC_ACCOUNTValidateUploadError.Count == 0)
                    return;

                Bigobject = STATISTIC_ACCOUNTValidateUploadError.ToList<GSM08501ErrorValidateDTO>();

                //preapare Batch Parameter
                loBatchPar = new R_BatchParameter();

                loBatchPar.COMPANY_ID = CompanyID;
                loBatchPar.USER_ID = UserId;
                loBatchPar.UserParameters = loUserParameneters;
                loBatchPar.ClassName = "GSM08500Back.GSM08501Cls";
                loBatchPar.BigObject = Bigobject;

                var lcGuid = await loCls.R_BatchProcess<List<GSM08501ErrorValidateDTO>>(loBatchPar, 11);
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
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

            Percentage = pnProgress;
            Message = string.Format("Process Progress {0} with status {1}", pnProgress, pcStatus);

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
                    RESOURCE_NAME = "RSP_GS_UPLOAD_STATISTIC_ACCOUNTResources"
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
                    STATISTIC_ACCOUNTValidateUploadError.ToList().ForEach(x =>
                    {
                        //Assign ErrorMessage, Valid and Set Valid And Invalid Data
                        if (loResultData.Any(y => y.SeqNo == x.NO))
                        {
                            x.ErrorMessage = loResultData.Where(y => y.SeqNo == x.NO).FirstOrDefault().ErrorMessage;
                            x.Valid = "N";
                            SumInvalidDataStaffExcel++;
                        }
                        else
                        {
                            x.Valid = "Y";
                            SumValidDataStaffExcel++;
                        }
                    });

                    //Set DataSetTable and get error
                    var loExcelData = R_FrontUtility.ConvertCollectionToCollection<GSM08501ExcelDTO>(STATISTIC_ACCOUNTValidateUploadError);

                    var loDataTable = R_FrontUtility.R_ConvertTo<GSM08501ExcelDTO>(loExcelData);
                    loDataTable.TableName = "STATISTIC_ACCOUNT";

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
                R_FrontContext.R_SetContext(ContextConstant.CCOMPANY_ID, CompanyID);
        
                var loResult = await _GSM08500Model.CompanyDetailModel();
        
                loCompany = R_FrontUtility.ConvertObjectToObject<GSM08500UploadHeaderDTO>(loResult);
        
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
        
            loEx.ThrowExceptionIfErrors();
        }
       
    }
}