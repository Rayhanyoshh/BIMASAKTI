using System.Globalization;
using BlazorClientHelper;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Lookup_GSModel.ViewModel;
using Lookup_PMCOMMON.DTOs;
using Lookup_PMFRONT;
using Lookup_PMModel.ViewModel.LML00600;
using Lookup_PMModel.ViewModel.LML00700;
using Lookup_PMModel.ViewModel.LML00800;
using Microsoft.AspNetCore.Components;
using PMR02200Common.DTOs;
using PMR02200Common.DTOs.PrintDTO;
using PMR02200MODEL;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using R_CommonFrontBackAPI;
using R_LockingFront;

namespace PMR02200FRONT;

public partial class PMR02200 : R_Page
{
    private PMR02200ViewModel _PMR02200ViewModel = new();
    private R_Conductor _ConductorRef;
    
    [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_IReport _reportService { get; set; }
    
    #region LockUnlock
    private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrlPM";
    private const string DEFAULT_MODULE_NAME = "PM";
    protected async override Task<bool> R_LockUnlock(R_LockUnlockEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        var llRtn = false;
        R_LockingFrontResult loLockResult = null;

        try
        {
            var loData = R_FrontUtility.ConvertObjectToObject<PMR02200DTO>(eventArgs.Data);

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
                    Program_Id = "PMR02200",
                    Table_Name = "",
                    Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CPROPERTY_ID)
                };

                loLockResult = await loCls.R_Lock(loLockPar);
            }
            else
            {
                var loUnlockPar = new R_ServiceLockingUnLockParameterDTO
                {
                    Company_Id = _clientHelper.CompanyId,
                    User_Id = _clientHelper.UserId,
                    Program_Id = "PMR02200",
                    Table_Name = "",
                    Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CPROPERTY_ID)
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
    #endregion

    #region init

    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {
            await _PMR02200ViewModel.GetPropertyList();
            await _PMR02200ViewModel.GetPeriodCompany();
            _PMR02200ViewModel.PrintParam.CCUT_OFF_DATE = DateTime.Today.ToString("yyyyMMdd");
            // Mendapatkan bulan hari ini
            int currentMonthIndex = DateTime.Today.Month - 1;

            // Mengambil ID dari _PMR02200ViewModel.PeriodMonthList berdasarkan index bulan hari ini
            _PMR02200ViewModel.FromPeriodMonth = _PMR02200ViewModel.PeriodMonthList[currentMonthIndex].Id;
            _PMR02200ViewModel.ToPeriodMonth = _PMR02200ViewModel.PeriodMonthList[currentMonthIndex].Id;

            
            _PMR02200ViewModel.AggrementRadioSelected = _PMR02200ViewModel.AggrementRadio1.FirstOrDefault().Id;
            _PMR02200ViewModel.DateBaseRadioSelected = _PMR02200ViewModel.DateBaseRadio1.FirstOrDefault().Id;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    
    
    #region Lookup Customer
        private async void BeforeOpen_lookupFromCust(R_BeforeOpenLookupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var param = new LML00600ParameterDTO()
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02200ViewModel.PropertyDefault,
                    CCUSTOMER_TYPE = "01"
                };
                eventArgs.Parameter = param;
                eventArgs.TargetPageType = typeof(LML00600);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private void AfterOpen_lookupFromCust(R_AfterOpenLookupEventArgs eventArgs)
        {
            var loTempResult = (LML00600DTO)eventArgs.Result;
            if (loTempResult == null)
            {
                return;
            }

            _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_ID = loTempResult.CTENANT_ID;
            _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_NAME = loTempResult.CTENANT_NAME;
        }
        private async Task OnLostFocus_LookupFromCust()
        {
            var loEx = new R_Exception();

            try
            {
            if (!string.IsNullOrWhiteSpace(_PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_ID))
            {

                LookupLML00600ViewModel loLookupViewModel = new LookupLML00600ViewModel(); //use GSL's model
                var loParam = new LML00600ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02200ViewModel.PropertyDefault,
                    CCUSTOMER_TYPE = "01",
                    CSEARCH_TEXT = _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_ID, // property that bindded to search textbox
                };
                var loResult = await loLookupViewModel.GetTenant(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                            typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                            "_ErrLookup01"));
                    _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_ID = ""; //kosongin bind textbox name kalo gaada
                    _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_NAME = "";
                }
                else
                    _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_ID = loResult.CTENANT_ID;
                    _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_NAME = loResult.CTENANT_NAME; //assign bind textbox name kalo ada
                }

         
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
               
        private async void BeforeOpen_lookupToCust(R_BeforeOpenLookupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var param = new LML00600ParameterDTO()
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02200ViewModel.PropertyDefault,
                    CCUSTOMER_TYPE = "01"
                };
                eventArgs.Parameter = param;
                eventArgs.TargetPageType = typeof(LML00600);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private void AfterOpen_lookupToCust(R_AfterOpenLookupEventArgs eventArgs)
        {
            var loTempResult = (LML00600DTO)eventArgs.Result;
            if (loTempResult == null)
            {
                return;
            }

            _PMR02200ViewModel.PrintParam.CTO_CUSTOMER_ID = loTempResult.CTENANT_ID;
            _PMR02200ViewModel.PrintParam.CTO_CUSTOMER_NAME = loTempResult.CTENANT_NAME;
        }
        private async Task OnLostFocus_LookupToCust()
        {
            var loEx = new R_Exception();

            try
            {
                LookupLML00600ViewModel loLookupViewModel = new LookupLML00600ViewModel(); //use GSL's model
                var loParam = new LML00600ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02200ViewModel.PropertyDefault,
                    CCUSTOMER_TYPE = "01",
                    CSEARCH_TEXT = _PMR02200ViewModel.PrintParam.CTO_CUSTOMER_ID, // property that bindded to search textbox
                };
                var loResult = await loLookupViewModel.GetTenant(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                            typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                            "_ErrLookup01"));
                    _PMR02200ViewModel.PrintParam.CTO_CUSTOMER_NAME = ""; //kosongin bind textbox name kalo gaada
                    //await GLAccount_TextBox.FocusAsync();
                }
                else
                    _PMR02200ViewModel.PrintParam.CTO_CUSTOMER_NAME = loResult.CTENANT_NAME; //assign bind textbox name kalo ada
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        #endregion
        
    #region LookupDept
 
        private async void BeforeOpen_lookupDept(R_BeforeOpenLookupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var param = new GSL00700ParameterDTO()
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId
                };
                eventArgs.Parameter = param;
                eventArgs.TargetPageType = typeof(GSL00700);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
        private void AfterOpen_lookupDept(R_AfterOpenLookupEventArgs eventArgs)
        {
            var loTempResult = (GSL00700DTO)eventArgs.Result;
            if (loTempResult == null)
            {
                return;
            }

            _PMR02200ViewModel.PrintParam.CDEPT_CODE = loTempResult.CDEPT_CODE;
            _PMR02200ViewModel.PrintParam.CDEPT_NAME = loTempResult.CDEPT_NAME;
           
        }
        private async Task OnLostFocus_LookupDept()
        {
            var loEx = new R_Exception();

            try
            {
                LookupGSL00700ViewModel loLookupViewModel = new LookupGSL00700ViewModel(); //use GSL's model
                var loParam = new GSL00700ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CSEARCH_TEXT = _PMR02200ViewModel.PrintParam.CDEPT_CODE, // property that bindded to search textbox
                };


                var loResult = await loLookupViewModel.GetDepartment(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                            typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                            "_ErrLookup01"));
                    _PMR02200ViewModel.PrintParam.CDEPT_NAME = ""; //kosongin bind textbox name kalo gaada
                    //await GLAccount_TextBox.FocusAsync();
                }
                else
                    _PMR02200ViewModel.PrintParam.CDEPT_NAME = loResult.CDEPT_NAME; //assign bind textbox name kalo ada
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }

    #endregion

    #region LookupLOI

    private async void BeforeOpen_lookupLOI(R_BeforeOpenLookupEventArgs eventArgs)
    {
        R_Exception loEx = new R_Exception();
        try
        {
            var param = new LML00800ParameterDTO()
            {
                CCOMPANY_ID = _clientHelper.CompanyId,
                CPROPERTY_ID = _PMR02200ViewModel.PropertyDefault,
                CDEPT_CODE = _PMR02200ViewModel.PrintParam.CDEPT_CODE,
                CAGGR_STTS = _PMR02200ViewModel.AggrementRadioSelected,
                CLANG_ID = _clientHelper.Culture.Name
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(LML00800);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();

    }
    private void AfterOpen_TolookupLOI(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (LML00800DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _PMR02200ViewModel.PrintParam.CTO_LOI_NO = loTempResult.CREF_NO;

    }
    private void AfterOpen_fromlookupLOI(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (LML00800DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _PMR02200ViewModel.PrintParam.CFROM_LOI_NO = loTempResult.CREF_NO;

    }
    
    private void AfterOpen_FromLookupAgreement(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (LML00800DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _PMR02200ViewModel.PrintParam.CFROM_AGREEMENT_NO = loTempResult.CREF_NO;

    }
    private void AfterOpen_ToLookupAgreement(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (LML00800DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _PMR02200ViewModel.PrintParam.CTO_AGREEMENT_NO = loTempResult.CREF_NO;

    }
    private async Task OnLostFocus_FromLookupLOI()
    {
        var loEx = new R_Exception();

        try
        {
            LookupLML00800ViewModel loLookupViewModel = new LookupLML00800ViewModel(); //use GSL's model
            var loParam = new LML00800ParameterDTO // use match param as GSL's dto, send as type in search texbox
            {
                CCOMPANY_ID = _clientHelper.CompanyId,
                CPROPERTY_ID = _PMR02200ViewModel.PropertyDefault,
                CDEPT_CODE = _PMR02200ViewModel.PrintParam.CDEPT_CODE,
                CAGGR_STTS = _PMR02200ViewModel.AggrementRadioSelected,
                CLANG_ID = _clientHelper.Culture.Name,
                CSEARCH_TEXT = _PMR02200ViewModel.PrintParam.CFROM_LOI_NO, // property that bindded to search textbox
            };


            var loResult = await loLookupViewModel.GetAgreement(loParam); //retrive single record 

            //show result & show name/related another fields
            if (loResult == null)
            {
                loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                _PMR02200ViewModel.PrintParam.CFROM_LOI_NO = ""; //kosongin bind textbox name kalo gaada
                                                               //await GLAccount_TextBox.FocusAsync();
            }
            else
                _PMR02200ViewModel.PrintParam.CFROM_LOI_NO = loResult.CREF_NO; //assign bind textbox name kalo ada
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task OnLostFocus_FromLookupAgree()
    {
        var loEx = new R_Exception();

        try
        {
            LookupLML00800ViewModel loLookupViewModel = new LookupLML00800ViewModel(); //use GSL's model
            var loParam = new LML00800ParameterDTO // use match param as GSL's dto, send as type in search texbox
            {
                CSEARCH_TEXT = _PMR02200ViewModel.PrintParam.CFROM_AGREEMENT_NO, // property that bindded to search textbox
            };


            var loResult = await loLookupViewModel.GetAgreement(loParam); //retrive single record 

            //show result & show name/related another fields
            if (loResult == null)
            {
                loEx.Add(R_FrontUtility.R_GetError(
                    typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                    "_ErrLookup01"));
                _PMR02200ViewModel.PrintParam.CFROM_AGREEMENT_NO = ""; //kosongin bind textbox name kalo gaada
                //await GLAccount_TextBox.FocusAsync();
            }
            else
                _PMR02200ViewModel.PrintParam.CFROM_AGREEMENT_NO = loResult.CREF_NO; //assign bind textbox name kalo ada
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    private async Task OnLostFocus_ToLookupLOI()
    {
        var loEx = new R_Exception();

        try
        {
            LookupLML00800ViewModel loLookupViewModel = new LookupLML00800ViewModel(); //use GSL's model
            var loParam = new LML00800ParameterDTO // use match param as GSL's dto, send as type in search texbox
            {
                CSEARCH_TEXT = _PMR02200ViewModel.PrintParam.CTO_LOI_NO, // property that bindded to search textbox
            };


            var loResult = await loLookupViewModel.GetAgreement(loParam); //retrive single record 

            //show result & show name/related another fields
            if (loResult == null)
            {
                loEx.Add(R_FrontUtility.R_GetError(
                    typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                    "_ErrLookup01"));
                _PMR02200ViewModel.PrintParam.CTO_LOI_NO = ""; //kosongin bind textbox name kalo gaada
                //await GLAccount_TextBox.FocusAsync();
            }
            else
                _PMR02200ViewModel.PrintParam.CTO_LOI_NO = loResult.CREF_NO; //assign bind textbox name kalo ada
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }

    private async Task OnLostFocus_ToLookupAgree()
    {
        var loEx = new R_Exception();

        try
        {
            LookupLML00800ViewModel loLookupViewModel = new LookupLML00800ViewModel(); //use GSL's model
            var loParam = new LML00800ParameterDTO // use match param as GSL's dto, send as type in search texbox
            {
                CSEARCH_TEXT = _PMR02200ViewModel.PrintParam.CTO_AGREEMENT_NO, // property that bindded to search textbox
            };


            var loResult = await loLookupViewModel.GetAgreement(loParam); //retrive single record 

            //show result & show name/related another fields
            if (loResult == null)
            {
                loEx.Add(R_FrontUtility.R_GetError(
                    typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                    "_ErrLookup01"));
                _PMR02200ViewModel.PrintParam.CTO_AGREEMENT_NO = ""; //kosongin bind textbox name kalo gaada
                //await GLAccount_TextBox.FocusAsync();
            }
            else
                _PMR02200ViewModel.PrintParam.CTO_AGREEMENT_NO = loResult.CREF_NO; //assign bind textbox name kalo ada
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    
    private async Task OnChangedCutOff(object poParam)
    {
        var loEx = new R_Exception();
        try
        {
          if(_PMR02200ViewModel.DateBaseRadioSelected == "1")
            {
                _PMR02200ViewModel.PrintParam.LFILTER_INCLUDE_ZERO_BALANCE = false;
            }

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private async Task Button_OnClickProcessAsync()
    {
        var loEx = new R_Exception();
        PMR02200PrintParamDTO loParam;
        try
        {
            loParam = new PMR02200PrintParamDTO()
            {
                CCOMPANY_ID = _clientHelper.CompanyId,
                CPROPERTY_ID = _PMR02200ViewModel.PropertyDefault,
                CDATE_BASE_ON = _PMR02200ViewModel.AggrementRadioSelected == "1" ? "Cut Off Date" : "Period",
                CFROM_CUSTOMER_ID = _PMR02200ViewModel.AggrementRadioSelected == "2" ? "" : _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_ID,
                CFROM_CUSTOMER_NAME = _PMR02200ViewModel.AggrementRadioSelected == "2" ? "" : _PMR02200ViewModel.PrintParam.CFROM_CUSTOMER_NAME,
                CTO_CUSTOMER_ID = _PMR02200ViewModel.AggrementRadioSelected == "2" ? "" : _PMR02200ViewModel.PrintParam.CTO_CUSTOMER_ID,
                CTO_CUSTOMER_NAME = _PMR02200ViewModel.AggrementRadioSelected == "2" ? "" : _PMR02200ViewModel.PrintParam.CTO_CUSTOMER_NAME,
                CDEPT_CODE = _PMR02200ViewModel.AggrementRadioSelected == "1" ? "" : _PMR02200ViewModel.PrintParam.CDEPT_CODE,
                CDEPT_NAME = _PMR02200ViewModel.AggrementRadioSelected == "1" ? "" : _PMR02200ViewModel.PrintParam.CDEPT_NAME,
                LLOI_NO = _PMR02200ViewModel.PrintParam.LLOI_NO,
                LAGREEMENT_NO = _PMR02200ViewModel.PrintParam.LAGREEMENT_NO,
                CFROM_LOI_NO = _PMR02200ViewModel.AggrementRadioSelected == "1" ? "" : _PMR02200ViewModel.PrintParam.CFROM_LOI_NO,
                CTO_LOI_NO = _PMR02200ViewModel.AggrementRadioSelected == "1" ? "" : _PMR02200ViewModel.PrintParam.CTO_LOI_NO,
                CFROM_AGREEMENT_NO = _PMR02200ViewModel.AggrementRadioSelected == "1" ? "" : _PMR02200ViewModel.PrintParam.CFROM_AGREEMENT_NO,
                CTO_AGREEMENT_NO = _PMR02200ViewModel.AggrementRadioSelected == "1" ? "" : _PMR02200ViewModel.PrintParam.CTO_AGREEMENT_NO,
                CCUT_OFF_DATE = _PMR02200ViewModel.DateBaseRadioSelected == "1" ? _PMR02200ViewModel.DCUT_OFF_DATE.ToString("yyyyMMdd") : string.Empty,
                CFROM_PERIOD = _PMR02200ViewModel.DateBaseRadioSelected == "1" ? "" : _PMR02200ViewModel.FromPeriodYear.ToString() + _PMR02200ViewModel.FromPeriodMonth,
                CTO_PERIOD = _PMR02200ViewModel.DateBaseRadioSelected == "1" ? "" : _PMR02200ViewModel.ToPeriodYear.ToString() + _PMR02200ViewModel.ToPeriodMonth,
                CSTATEMENT_DATE = _PMR02200ViewModel.DSTATEMENT_DATE.ToString("yyyyMMdd"),
                LFILTER_INCLUDE_ZERO_BALANCE = _PMR02200ViewModel.PrintParam.LFILTER_INCLUDE_ZERO_BALANCE,
                LFILTER_SHOW_AGE_TOTAL = _PMR02200ViewModel.PrintParam.LFILTER_SHOW_AGE_TOTAL,
                CLANG_ID = _clientHelper.Culture.Name,
            };
            
            await _reportService.GetReport(
                "R_DefaultServiceUrlPM",
                "PM",
                "rpt/PMR02200Print/DownloadResultPrintPost",
                "rpt/PMR02200Print/StatementAccountReportListGet",
                loParam);
            
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        EndBlock:
        loEx.ThrowExceptionIfErrors();
    }
       
        
}

