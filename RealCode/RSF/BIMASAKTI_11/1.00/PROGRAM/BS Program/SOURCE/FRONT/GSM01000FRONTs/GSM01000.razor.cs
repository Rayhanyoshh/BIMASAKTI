using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using GFF00900COMMON.DTOs;
using GSM01000Common;
using GSM01000Common.DTOs;
using GSM01000Model;
using BlazorClientHelper;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid.Columns;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using R_BlazorFrontEnd.Helpers;
using Microsoft.JSInterop;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Controls.Popup;
using R_BlazorFrontEnd.Interfaces;
using R_LockingFront;
using R_BlazorFrontEnd.Controls.Enums;

namespace GSM01000Front
{
    public partial class GSM01000 : R_Page
    {
        private GSM01000ViewModel _GSM01000ViewModel = new GSM01000ViewModel();
        private R_ConductorGrid _conductorGridRef;
        private R_Grid<GSM01000DTO> _gridRef;

        private GSM01010ViewModel _GSM01010ViewModel = new GSM01010ViewModel();
        private R_ConductorGrid _conductorGoARef;
        private R_Grid<GSM01010DTO> _gridGoARef;

        private GSM01100ViewModel _GSM01100ViewModel = new GSM01100ViewModel();
        private R_ConductorGrid _conductorGridCoAUserRef;
        private R_Grid<GSM01100DTO> _gridCoAUserRef;

        private GSM01200ViewModel _GSM01200ViewModel = new GSM01200ViewModel();
        private R_ConductorGrid _conductorGridCoACenterRef;
        private R_Grid<GSM01200DTO> _gridCoACenterRef;

        private GSM01300ViewModel _GSM01300ViewModel = new GSM01300ViewModel();
        private R_ConductorGrid _conductorGridGoARef;
        private R_Grid<GSM01300DTO> _gridGoA1300Ref;

        private GSM01310ViewModel _GSM01310ViewModel = new GSM01310ViewModel();
        private R_ConductorGrid _conductorGoACoARef;
        private R_Grid<GSM01310DTO> _gridGoACoARef;

        private string loLabel = "";


        [Inject] private IClientHelper _clientHelper { get; set; }
        [Inject] private R_ContextHeader _contextHeader { get; set; }
        [Inject] private R_PopupService PopupService { get; set; }

        [Inject] private IJSRuntime JS { get; set; }

        public bool _enableActiveInactive = false;


        #region TAB Predefined

        private void Predef_GOA(R_InstantiateDockEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(GSM01300);
            eventArgs.Parameter = "Group Of Account";
        }

        private void AfterPredef_GOA(R_AfterOpenPredefinedDockEventArgs eventArgs)
        {
        }

        #endregion


        protected override async Task R_Init_From_Master(object poParam)
        {
            var loEx = new R_Exception();

            try
            {
                _GSM01000ViewModel.BSIS_Option = new List<DropDownDTO>
                {
                    new DropDownDTO { Id = "B", Text = @_localizer["_balanceSheet"] },
                    new DropDownDTO { Id = "I", Text = @_localizer["_incomeStatement"] }
                };

                _GSM01000ViewModel.CDBCR_Option = new List<DropDownDTO>
                {
                    new DropDownDTO { Id = "D", Text = @_localizer["_debit"] },
                    new DropDownDTO { Id = "C", Text = @_localizer["_credit"] }
                };
                await _GSM01000ViewModel.GetGridList();
                await _GSM01000ViewModel.GetResultPrimaryAcc();
                // await _GSM01100ViewModel.GetGridCoAUserList();
                // await _GSM01200ViewModel.GetCenterToAssignList();
                await _GSM01300ViewModel.GetGridGoAList();

                await _gridRef.R_RefreshGrid(null);

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_MODULE_NAME = "GS";
        protected async override Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            var llRtn = false;
            R_LockingFrontResult loLockResult = null;

            try
            {
                var loData = (GSM01000DTO)eventArgs.Data;

                var loCls = new R_LockingServiceClient(pcModuleName: DEFAULT_MODULE_NAME,
                    plSendWithContext: true,
                    plSendWithToken: true,
                    pcHttpClientName: DEFAULT_HTTP_NAME);

                if (eventArgs.Mode == R_eLockUnlock.Lock)
                {
                    var loLockPar = new R_ServiceLockingLockParameterDTO
                    {
                        Company_Id = _clientHelper.CompanyId,
                        User_Id = _clientHelper.UserId,
                        Program_Id = "GSM01000",
                        Table_Name = "GSM_COA",
                        Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CGLACCOUNT_NO)
                    };

                    loLockResult = await loCls.R_Lock(loLockPar);
                }
                else
                {
                    var loUnlockPar = new R_ServiceLockingUnLockParameterDTO
                    {
                        Company_Id = _clientHelper.CompanyId,
                        User_Id = _clientHelper.UserId,
                        Program_Id = "GSM01000",
                        Table_Name = "GSM_COA",
                        Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CGLACCOUNT_NO)
                    };

                    loLockResult = await loCls.R_UnLock(loUnlockPar);
                }

                llRtn = loLockResult.IsSuccess;
                if (!loLockResult.IsSuccess && loLockResult.Exception != null)
                    throw loLockResult.Exception;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

            return llRtn;
        }

        #region COA

        private void COA_BeforeAdd(R_BeforeAddEventArgs eventArgs)
        {
            _enableActiveInactive = false;

        }

        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01000ViewModel.GetGridList();
                arg.ListEntityResult = _GSM01000ViewModel.loGridList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private bool EditModePrimary = true;
        private bool AddModePrimary = true;
        private bool DeleteModePrimary = true;
        private bool AssignUserButton = true;
        private bool AssignCenterButton = true;
        private R_Popup PopupCopyFrom;

        private bool CopyFromButton;

        private async Task Grid_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM01000DTO)eventArgs.Data;
               
                await _gridGoARef.R_RefreshGrid(loParam);
                
                _GSM01000ViewModel._cglCode = loParam.CGLACCOUNT_NO;
                _GSM01000ViewModel._cglName= loParam.CGLACCOUNT_NAME;

                // Active/inActive
                _GSM01000ViewModel.SelectedActiveInactiveCenterCode = loParam.CGLACCOUNT_NO;
                _GSM01000ViewModel.SelectedActiveInactiveLACTIVE = loParam.LACTIVE;

                // Switching Tab
                _GSM01100ViewModel.cglAccount = loParam.CGLACCOUNT_NO;
                _GSM01200ViewModel.cglAccount = loParam.CGLACCOUNT_NO;

           
                await _GSM01100ViewModel.GetGridCoAUserList();
                await _GSM01200ViewModel.GetGridCoACenterList();
                await _GSM01200ViewModel.GetCenterToAssignList();

                //Head Info

                
                if (_GSM01000ViewModel.loHasil.LPRIMARY_ACCOUNT == true)
                {
                    EditModePrimary = true;
                    AddModePrimary = true;
                    DeleteModePrimary = true;
                    CopyFromButton = false;
                    // PopupCopyFrom.Enabled = false;
                }
                else
                {
                    EditModePrimary = false;
                    AddModePrimary = false;
                    DeleteModePrimary = false;
                    CopyFromButton = true;
                }

                if (loParam.LUSER_RESTR == false)
                {
                    AssignUserButton = false;
                }
                else
                {
                    AssignUserButton = true;
                }

                if (loParam.LCENTER_RESTR == false)
                {
                    AssignCenterButton = false;
                }
                else
                {
                    AssignCenterButton = true;
                }

                if (loParam.LACTIVE)
                {
                    loLabel = @_localizer["_active"];
                    _GSM01000ViewModel.SelectedActiveInactiveLACTIVE = false;
                }
                else
                {
                    loLabel = @_localizer["_inactive"];
                    _GSM01000ViewModel.SelectedActiveInactiveLACTIVE = true;
                }


            }
        }

        private async Task Grid_R_ServiceGetGridCoA(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                // var lcParam = ((GSM01000DTO)eventArgs.Data).CGLACCOUNT_NO;
                // _contextHeader.R_Context.R_SetStreamingContext(ContextConstant.CGL_ACCOUNT, lcParam);

                var loParam = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(eventArgs.Data);
                _GSM01100ViewModel.cglAccount = loParam.CGLACCOUNT_NO;
                await _GSM01000ViewModel.GetCoASingle(loParam);

                eventArgs.Result = _GSM01000ViewModel.loEntity;


            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Conductor_Validation(R_ValidationEventArgs arg)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = arg.Data as GSM01000DTO;
                // nanti kerjakan resources nya
                if (_conductorGridRef.R_ConductorMode == R_eConductorMode.Add )
                {
                    if (loData.CCASH_FLOW_CODE == "")
                    {
                        loEx.Add("", @_localizer["validationCashflow"]);
                    }
                }
                if (_conductorGridRef.R_ConductorMode == R_eConductorMode.Edit )
                {
                    if (loData.CCASH_FLOW_CODE == "")
                    {
                        loEx.Add("", @_localizer["validationCashflow"]);
                    }
                }
                
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            arg.Cancel = loEx.HasError;
            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_ServiceSave(R_ServiceSaveEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM01000ViewModel.SaveEntity((GSM01000DTO)eventArgs.Data, (eCRUDMode)eventArgs.ConductorMode);
                eventArgs.Result = _GSM01000ViewModel.loEntity;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task Grid_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loData = (GSM01000DTO)eventArgs.Data;
                await _GSM01000ViewModel.DeleteEntity(loData);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task BeforeDeleteCOA(R_BeforeDeleteEventArgs eventArgs)
        {
            var loEx = new R_Exception();
            try
            {
                var loData = (GSM01000DTO)eventArgs.Data;
                await _GSM01310ViewModel.GetGoACoAList();
                var coaList = _GSM01310ViewModel.loGridGoACoAList;
                // Ambil CGLACCOUNT_NO dari loData
                var selectedCGLACCOUNT_NO = loData.CGLACCOUNT_NO;

                // Periksa apakah CGLACCOUNT_NO ada dalam daftar
                if (coaList.Any(coa => coa.CGLACCOUNT_NO == selectedCGLACCOUNT_NO))
                {
                    // Jika ada, tampilkan pesan kesalahan atau lakukan tindakan yang sesuai
                    loEx.Add(new Exception("Cannot Delete Paired Account"));
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();
        }

        private async Task R_ConvertToGridEntity(R_ConvertToGridEntityEventArgs arg)
        {
            arg.GridData = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(arg.Data);
        }

        #endregion

        #region GOA

        private async Task GridGoA_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loCGLParam = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(eventArgs.Parameter);
                _GSM01010ViewModel._cglAccountNo = loCGLParam.CGLACCOUNT_NO;
                await _GSM01010ViewModel.GetGOAListbyGL();
                eventArgs.ListEntityResult = _GSM01010ViewModel.loGridGoAList;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        private async Task GridGoA1300_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                var loCGLParam = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(arg.Parameter);
                _GSM01010ViewModel._cglAccountNo = loCGLParam.CGLACCOUNT_NO;
                await _GSM01300ViewModel.GetGridGoAList();
                arg.ListEntityResult = _GSM01300ViewModel.loGridGoAList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task GridGoACoA_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                var loCGOAcodeParam = R_FrontUtility.ConvertObjectToObject<GSM01300DTO>(eventArgs.Parameter);
                _GSM01310ViewModel._cGOACode = loCGOAcodeParam.CGOA_CODE;
                await _GSM01310ViewModel.GetGoACoAList();
                eventArgs.ListEntityResult = _GSM01310ViewModel.loGridGoACoAList;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private async Task GridGoA_Display(R_DisplayEventArgs eventArgs)
        {
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loParam = (GSM01300DTO)eventArgs.Data;
                var loHeadGridGOACOA = (GSM01310DTO)_conductorGoACoARef.R_GetCurrentData();
                loHeadGridGOACOA.CGOA_CODE = loParam.CGOA_CODE;
                loHeadGridGOACOA.CGOA_NAME = loParam.CGOA_NAME;

                await _gridGoACoARef.R_RefreshGrid(loParam);
            }
        }

        private async Task Grid_R_ServiceGetGridGoA(R_ServiceGetRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                // var lcParam = ((GSM01000DTO)eventArgs.Data).CGLACCOUNT_NO;
                // _contextHeader.R_Context.R_SetStreamingContext(ContextConstant.CGL_ACCOUNT, lcParam);

                var loParam = R_FrontUtility.ConvertObjectToObject<GSM01300DTO>(eventArgs.Data);
                await _GSM01300ViewModel.GetGoASingle(loParam);

                eventArgs.Result = _GSM01300ViewModel.loEntity;


            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private async Task GridCoAUser_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                var loCGLParam = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(arg.Parameter);
                // _GSM01010ViewModel._cglAccountNo = loCGLParam.CGLACCOUNT_NO;
                await _GSM01100ViewModel.GetGridCoAUserList();
                arg.ListEntityResult = _GSM01100ViewModel.loGridCoAUserList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        private async Task GridCoACenter_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs arg)
        {
            var loEx = new R_Exception();

            try
            {
                var loCGLParam = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(arg.Parameter);

                await _GSM01200ViewModel.GetGridCoACenterList();
                arg.ListEntityResult = _GSM01200ViewModel.loGridCoACenterList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

        #endregion

        #region CopyFrom

        private void R_Before_Open_Popup_CopyFrom(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(CopyFromModal);
        }

        private async Task R_After_Open_Popup_CopyFrom(R_AfterOpenPopupEventArgs eventArgs)
        {
            await _gridRef.R_RefreshGrid(null);

        }

        #endregion

        #region Active/Inactive

        private async Task R_Before_Open_Popup_ActivateInactive(R_BeforeOpenPopupEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();
            try
            {
                var loValidateViewModel = new GFF00900Model.ViewModel.GFF00900ViewModel();
                loValidateViewModel.ACTIVATE_INACTIVE_ACTIVITY_CODE =
                    "GSM01001"; //Uabh Approval Code sesuai Spec masing masing
                await loValidateViewModel
                    .RSP_ACTIVITY_VALIDITYMethodAsync(); //Jika IAPPROVAL_CODE == 3, maka akan keluar RSP_ERROR disini

                //Jika Approval User ALL dan Approval Code 1, maka akan langsung menjalankan ActiveInactive
                if (loValidateViewModel.loRspActivityValidityList.FirstOrDefault().CAPPROVAL_USER == "ALL" &&
                    loValidateViewModel.loRspActivityValidityResult.Data.FirstOrDefault().IAPPROVAL_MODE == 1)
                {
                    await _GSM01000ViewModel
                        .ActiveInactiveProcessAsync(); //Ganti jadi method ActiveInactive masing masing
                    return;
                }
                else //Disini Approval Code yang didapat adalah 2, yang berarti Active Inactive akan dijalankan jika User yang diinput ada di RSP_ACTIVITY_VALIDITY
                {
                    eventArgs.Parameter = new GFF00900ParameterDTO()
                    {
                        Data = loValidateViewModel.loRspActivityValidityList,
                        IAPPROVAL_CODE = "GSM01001" //Uabh Approval Code sesuai Spec masing masing
                    };
                    eventArgs.TargetPageType = typeof(GFF00900FRONT.GFF00900);
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }


        private async Task R_After_Open_Popup_ActivateInactive(R_AfterOpenPopupEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();
            try
            {
                if (eventArgs.Success == false)
                {
                    return;
                }

                bool result = (bool)eventArgs.Result;
                if (result == true)
                {
                    await _GSM01000ViewModel.ActiveInactiveProcessAsync();
                    await _gridRef.R_RefreshGrid(null);
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();

        }

        #endregion

        #region Upload

        private void R_Before_Open_Popup_Upload(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.TargetPageType = typeof(UploadPopUp);
        }

        private async Task R_After_Open_Popup_Upload(R_AfterOpenPopupEventArgs eventArgs)
        {
            await _gridRef.R_RefreshGrid(null);

        }

        #endregion

        #region Lookup

        private R_GridLookupColumn LookupColumn;

        private void Cash_Before_Open_Lookup(R_BeforeOpenGridLookupColumnEventArgs eventArgs)
        {
            var loParam = new GSL01500ParameterGroupDTO();
            eventArgs.Parameter = loParam;
            eventArgs.TargetPageType = typeof(GSL01500);
        }

        private void Cash_After_Open_Lookup(R_AfterOpenGridLookupColumnEventArgs eventArgs)
        {
            //mengambil result dari popup dan set ke data row

            var loTempResult = R_FrontUtility.ConvertObjectToObject<GSM01000DTO>(eventArgs.Result);

            if (loTempResult == null)
            {
                return;
            }

            ((GSM01000DTO)eventArgs.ColumnData).CCASH_FLOW_GROUP_CODE = loTempResult.CCASH_FLOW_GROUP_CODE;
            ((GSM01000DTO)eventArgs.ColumnData).CCASH_FLOW_CODE = loTempResult.CCASH_FLOW_CODE;
            ((GSM01000DTO)eventArgs.ColumnData).CCASH_FLOW_NAME = loTempResult.CCASH_FLOW_NAME;

        }

        #endregion

        #region Assign USer

        private void R_Before_Open_PopupUserAssign(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.Parameter = _GSM01100ViewModel.cglAccount;
            eventArgs.TargetPageType = typeof(GSM01100PopUp);

        }

        private async Task R_After_Open_PopupUserAssign(R_AfterOpenPopupEventArgs eventArgs)
        {


            await _gridCoAUserRef.R_RefreshGrid(null);
        }
        #endregion

        #region CenterAssign
        private void R_Before_Open_PopupCenter(R_BeforeOpenPopupEventArgs eventArgs)
        {
            eventArgs.Parameter = _GSM01200ViewModel.cglAccount;
            eventArgs.TargetPageType = typeof(GSM01200PopUp);
        }

        private async Task R_After_Open_PopupCenter(R_AfterOpenPopupEventArgs eventArgs)
        {
            //var loTempResult = (AssignCenterDTO)eventArgs.Result;
            //var loParam = (GSM01200DTO)_conductorGridCoACenterRef.R_GetCurrentData();
            //// var loGlAccountTab = _GSM01200ViewModel.loParameter;

            //if (loTempResult==null)
            //{
            //    return;
            //}

            //loParam.CCENTER_CODE = loTempResult.CCENTER_CODE;
            //loParam.CGLACCOUNT_NO = _GSM01200ViewModel.cglAccount;

            ////await _GSM01200ViewModel.AssignCenterToCOA(loParam, 
            ////    eCRUDMode.AddMode);
            await _gridCoACenterRef.R_RefreshGrid(null);
        }
        #endregion


        #region Assign COA

        private void R_Before_Open_PopupCOA(R_BeforeOpenPopupEventArgs eventArgs)
        {
            var loCGOAcodeParam = R_FrontUtility.ConvertObjectToObject<GSM01300DTO>(eventArgs.Parameter);

            _GSM01310ViewModel._cGOACode = loCGOAcodeParam.CGOA_CODE;
            eventArgs.Parameter = _GSM01310ViewModel._cGOACode;
            eventArgs.TargetPageType = typeof(GSM01300PopUp);
        }

        private async Task R_After_Open_PopupCOA(R_AfterOpenPopupEventArgs eventArgs)
        {
            //var loTempResult = (AssignCoADTO)eventArgs.Result;
            //var loCoa = (GSM01310DTO)_conductorGoACoARef.R_GetCurrentData();
            //if (loTempResult==null)
            //{
            //    return;
            //}

            //await _GSM01310ViewModel.AssignCoAToGOA(new GSM01310DTO()
            //    {
            //        CGOA_CODE = loCoa.CGOA_CODE,
            //        CGLACCOUNT_NO = loTempResult.CGLACCOUNT_NO,
            //    }, 
            //    eCRUDMode.AddMode);
            await _gridGoACoARef.R_RefreshGrid(null);
        }

        #endregion


        #region Tab

        private async Task ChangeTab(R_TabStripTab arg)
        {
            var loEx = new R_Exception();

            try
            {
                if (arg.Id == "TabUser")
                {
                    await _GSM01100ViewModel.GetGridCoAUserList();
                    await _gridCoAUserRef.R_RefreshGrid(null);
                }
                else if (arg.Id == "TabCenter")
                {
                    await _GSM01200ViewModel.GetGridCoACenterList();
                    await _gridCoACenterRef.R_RefreshGrid(null);

                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        #endregion

        #region Template

        private async Task DownloadTemplate()
        {
            var loEx = new R_Exception();

            try
            {
                var loValidate = await R_MessageBox.Show("", "Are you sure download this template?",
                    R_eMessageBoxButtonType.YesNo);

                if (loValidate == R_eMessageBoxResult.Yes)
                {
                    var loByteFile = await _GSM01000ViewModel.DownloadTemplate();

                    var saveFileName = $"CHART_OF_ACCOUNT.xlsx";

                    await JS.downloadFileFromStreamHandler(saveFileName, loByteFile.FileBytes);
                }
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        #endregion

        #region Print

        private void R_Before_Open_Popup_Print(R_BeforeOpenPopupEventArgs eventArgs)
        {

            eventArgs.TargetPageType = typeof(PrintPopUp);
        }

        private async void R_After_Open_Popup_Print(R_AfterOpenPopupEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();
            try
            {
                await _gridRef.R_RefreshGrid(null);

            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }

        #endregion

        #region BeforeAdd
        private void R_After_Add_COA(R_AfterAddEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                // var loTempParam = R_FrontUtility.ConvertObjectToObject<GSM07510DTO>(eventArgs.Data);
                var loData = (GSM01000DTO)eventArgs.Data;
                loData.LACTIVE = true;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();

        }


        private async Task GridAddCOA_Validation(R_ValidationEventArgs eventArgs)
        {
            R_Exception loException = new R_Exception();
            R_PopupResult loResult = null;
            GFF00900ParameterDTO loParam = null;
            GSM01000DTO loData = null;
            try
            {
                loData = (GSM01000DTO)eventArgs.Data;
                if (loData.LACTIVE == true && _conductorGridRef.R_ConductorMode == R_eConductorMode.Add)
                {
                    var loValidateViewModel = new GFF00900Model.ViewModel.GFF00900ViewModel();
                    loValidateViewModel.ACTIVATE_INACTIVE_ACTIVITY_CODE = "GSM01001";
                    await loValidateViewModel.RSP_ACTIVITY_VALIDITYMethodAsync();

                    if (loValidateViewModel.loRspActivityValidityList.FirstOrDefault().CAPPROVAL_USER == "ALL" && loValidateViewModel.loRspActivityValidityResult.Data.FirstOrDefault().IAPPROVAL_MODE == 1)
                    {
                        eventArgs.Cancel = false;
                    }
                    else
                    {
                        loParam = new GFF00900ParameterDTO()
                        {
                            Data = loValidateViewModel.loRspActivityValidityList,
                            IAPPROVAL_CODE = "GSM01001"
                        };
                        loResult = await PopupService.Show(typeof(GFF00900FRONT.GFF00900), loParam);
                        if (loResult.Success == false || (bool)loResult.Result == false)
                        {
                            eventArgs.Cancel = true;
                        }
                    }
                }
                  
                    // nanti kerjakan resources nya
                if (_conductorGridRef.R_ConductorMode == R_eConductorMode.Add )
                {
                    if (loData.CCASH_FLOW_CODE == "")
                    {
                        loException.Add("", @_localizer["validationCashflow"]);
                    }
                }
                if (_conductorGridRef.R_ConductorMode == R_eConductorMode.Edit )
                {
                    if (loData.CCASH_FLOW_CODE == "")
                    {
                        loException.Add("", @_localizer["validationCashflow"]);
                    }
                }
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
        #endregion
    }
}