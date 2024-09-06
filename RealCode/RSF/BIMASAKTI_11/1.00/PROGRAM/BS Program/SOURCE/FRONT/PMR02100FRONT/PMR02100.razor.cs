using System.Globalization;
using BlazorClientHelper;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Lookup_GSModel.ViewModel;
using Lookup_PMCOMMON.DTOs;
using Lookup_PMCOMMON.DTOs.PML01200;
using Lookup_PMFRONT;
using Lookup_PMModel.ViewModel.LML00600;
using Lookup_PMModel.ViewModel.LML00800;
using Lookup_PMModel.ViewModel.PML01200;
using Microsoft.AspNetCore.Components;
using PMR02100Common.DTOs;
using PMR02100Common.DTOs.PrintDTO;
using PMR02100FrontResources;
using PMR02200FrontResources;
using PMR02100MODEL;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd.Interfaces;
using R_CommonFrontBackAPI;
using R_LockingFront;

namespace PMR02100FRONT;

public partial class PMR02100 : R_Page
{
    private PMR02100ViewModel _PMR02100ViewModel = new();
    private R_Conductor _ConductorRef;
    [Inject] private IClientHelper _clientHelper { get; set; }
    [Inject] private R_IReport _reportService { get; set; }
    [Inject] private R_ILocalizer<ResourcesDummyClsPMR2100> _localizer { get; set; }
    
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
            var loData = R_FrontUtility.ConvertObjectToObject<PMR02100DTO>(eventArgs.Data);

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
                    Program_Id = "PMR02100",
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
                    Program_Id = "PMR02100",
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
            await _PMR02100ViewModel.InitProcess(_localizer);

            await _PMR02100ViewModel.GetPropertyList();
            _PMR02100ViewModel.PrintParam.CCUT_OFF_DATE = DateTime.Today.ToString("yyyyMMdd");
            // Mendapatkan bulan hari ini
            _PMR02100ViewModel.ReportTypeRadioSelected = _PMR02100ViewModel.ReportTypeRadio1.FirstOrDefault().Id;
            _PMR02100ViewModel.BasedOnRadioSelected = _PMR02100ViewModel.BasedOnRadio1.FirstOrDefault().Id;
            _PMR02100ViewModel.DateBaseRadioSelected = _PMR02100ViewModel.DateBaseRadio1.FirstOrDefault().Id;
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
                CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
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

        _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_ID = loTempResult.CTENANT_ID;
        _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_NAME = loTempResult.CTENANT_NAME;
    }
    private async Task OnLostFocus_LookupFromCust()
    {
        var loEx = new R_Exception();

        try
        {
            if (!string.IsNullOrWhiteSpace(_PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_ID))
            {

                LookupLML00600ViewModel loLookupViewModel = new LookupLML00600ViewModel(); //use GSL's model
                var loParam = new LML00600ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                    CCUSTOMER_TYPE = "01",
                    CSEARCH_TEXT = _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_ID, // property that bindded to search textbox
                };
                var loResult = await loLookupViewModel.GetTenant(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                    _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_ID = ""; 
                    _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_NAME = "";
                }
                else
                {
                    _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_ID = loResult?.CTENANT_ID; 
                    _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_NAME = loResult?.CTENANT_NAME; 
                }
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
                CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
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

        _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_ID = loTempResult.CTENANT_ID;
        _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_NAME = loTempResult.CTENANT_NAME;
    }
    private async Task OnLostFocus_LookupToCust()
    {
        var loEx = new R_Exception();

        try
        {
            if (!string.IsNullOrWhiteSpace(_PMR02100ViewModel.PrintParam.CTO_CUSTOMER_ID))
            {

                LookupLML00600ViewModel loLookupViewModel = new LookupLML00600ViewModel(); //use GSL's model
                var loParam = new LML00600ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CUSER_ID = _clientHelper.UserId,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                    CCUSTOMER_TYPE = "01",
                    CSEARCH_TEXT = _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_ID, // property that bindded to search textbox
                };
                var loResult = await loLookupViewModel.GetTenant(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                    _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_ID = ""; //kosongin bind textbox name kalo gaada
                    _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_NAME = "";
                }
                else
                    _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_ID = loResult?.CTENANT_ID;
                    _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_NAME = loResult?.CTENANT_NAME; //assign bind textbox name kalo ada
            }

         
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    #region LookupJrn
    private async void BeforeOpen_lookupJrn(R_BeforeOpenLookupEventArgs eventArgs)
    {
        R_Exception loEx = new R_Exception();
        try
        {
            var param = new GSL00400ParameterDTO()
            {
                CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                CCOMPANY_ID = _clientHelper.CompanyId,
                CJRNGRP_TYPE = "10",
                CUSER_LOGIN_ID = _clientHelper.UserId
                
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(GSL00400);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();

    }
    private void AfterOpen_lookupToJrn(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (GSL00400DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }
        _PMR02100ViewModel.PrintParam.CTO_JRNGRP_CODE = loTempResult.CJRNGRP_CODE;
        _PMR02100ViewModel.PrintParam.CTO_JRNGRP_NAME = loTempResult.CJRNGRP_NAME;
    }
    private void AfterOpen_lookupFromJrn(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (GSL00400DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }
        _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_CODE = loTempResult.CJRNGRP_CODE;
        _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_NAME = loTempResult.CJRNGRP_NAME;
    }
    private async Task OnLostFocus_LookupFromJrn()
    {
        var loEx = new R_Exception();

        try
        {
            if (!string.IsNullOrWhiteSpace(_PMR02100ViewModel.PrintParam.CFROM_JRNGRP_CODE))
            {

                LookupGSL00400ViewModel loLookupViewModel = new LookupGSL00400ViewModel(); //use GSL's model
                var loParam = new GSL00400ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CJRNGRP_TYPE = "10",
                    CUSER_LOGIN_ID = _clientHelper.UserId,
                    CSEARCH_TEXT = _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_CODE, // property that bindded to search textbox
                };
                var loResult = await loLookupViewModel.GetJournalGroup(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                    _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_NAME = ""; //kosongin bind textbox name kalo gaada
                    //await GLAccount_TextBox.FocusAsync();
                }
                _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_NAME = loResult?.CJRNGRP_NAME; //assign bind textbox name kalo ada
            }
            else
            {
                _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_CODE = ""; //kosongin bind textbox name kalo gaada
                _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_NAME = ""; //kosongin bind textbox name kalo gaada
            }
        }
        
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    private async Task OnLostFocus_LookupToJrn()
    {
        var loEx = new R_Exception();

        try
        {
            if (!string.IsNullOrWhiteSpace(_PMR02100ViewModel.PrintParam.CTO_JRNGRP_CODE))
            {

                LookupGSL00400ViewModel loLookupViewModel = new LookupGSL00400ViewModel(); //use GSL's model
                var loParam = new GSL00400ParameterDTO // use match param as GSL's dto, send as type in search texbox
                {
                    CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CJRNGRP_TYPE = "10",
                    CUSER_LOGIN_ID = _clientHelper.UserId,
                    CSEARCH_TEXT =
                        _PMR02100ViewModel.PrintParam.CTO_JRNGRP_CODE, // property that bindded to search textbox
                };
                var loResult = await loLookupViewModel.GetJournalGroup(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                    _PMR02100ViewModel.PrintParam.CTO_JRNGRP_NAME = ""; //kosongin bind textbox name kalo gaada
                    //await GLAccount_TextBox.FocusAsync();
                }
                _PMR02100ViewModel.PrintParam.CTO_JRNGRP_NAME = loResult?.CJRNGRP_NAME; //assign bind textbox name kalo ada
            }
            else
            {
                _PMR02100ViewModel.PrintParam.CTO_JRNGRP_CODE = ""; //kosongin bind textbox name kalo gaada
                _PMR02100ViewModel.PrintParam.CTO_JRNGRP_NAME = ""; //kosongin bind textbox name kalo gaada
            }
            
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    #region Lookup Invoice Group
    private async void BeforeOpen_lookupInvoiceGrp(R_BeforeOpenLookupEventArgs eventArgs)
        {
            R_Exception loEx = new R_Exception();
            try
            {
                var param = new PML01200ParameterDTO()
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                    CINVGRP_CODE = "",
                    CACTIVE_TYPE = "ALL",
                    CLANGUAGE_ID = _clientHelper.Culture.Name
                };
                eventArgs.Parameter = param;
                eventArgs.TargetPageType = typeof(PML01200);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }
            loEx.ThrowExceptionIfErrors();

        }
    private void AfterOpen_lookupInvoiceGrp(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (PML01200DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _PMR02100ViewModel.PrintParam.CINV_GRP_CODE = loTempResult.CINVGRP_CODE;
        _PMR02100ViewModel.PrintParam.CINV_GRP_NAME = loTempResult.CINVGRP_NAME;
    }
    private async Task OnLostFocus_lookupInvoiceGrp()
    {
        var loEx = new R_Exception();

        try
        {
            if (!string.IsNullOrWhiteSpace(_PMR02100ViewModel.PrintParam.CINV_GRP_CODE))
            {

                LookupPML01200ViewModel loLookupViewModel = new LookupPML01200ViewModel(); //use GSL's model
                var loParam = new PML01200ParameterDTO() // use match param as GSL's dto, send as type in search texbox
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                    CINVGRP_CODE = "",
                    CACTIVE_TYPE = "ALL",
                    CLANGUAGE_ID = _clientHelper.Culture.Name,
                    CSEARCH_TEXT = _PMR02100ViewModel.PrintParam.CINV_GRP_CODE, // property that bindded to search textbox
                };
                var loResult = await loLookupViewModel.GetAgreement(loParam); //retrive single record 

                //show result & show name/related another fields
                if (loResult == null)
                {
                    loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_LMFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                    _PMR02100ViewModel.PrintParam.CINV_GRP_NAME = ""; //kosongin bind textbox name kalo gaada
                    //await GLAccount_TextBox.FocusAsync();
                }
                _PMR02100ViewModel.PrintParam.CINV_GRP_NAME = loResult.CINVGRP_NAME; //assign bind textbox name kalo ada
            }
            else
            {
                _PMR02100ViewModel.PrintParam.CINV_GRP_NAME = ""; //kosongin bind textbox name kalo gaada
            }

         
        }
        
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    private async Task Button_OnClickProcessAsync()
    {
        var loEx = new R_Exception();
        PMR02100PrintParamDTO loParam;
        try
        {
            loParam = new PMR02100PrintParamDTO()
            {
                CCOMPANY_ID = _clientHelper.CompanyId,
                CPROPERTY_ID = _PMR02100ViewModel.PropertyDefault,
                CFROM_CUSTOMER_ID = _PMR02100ViewModel.BasedOnRadioSelected == "2" ? "" : _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_ID,
                CFROM_CUSTOMER_NAME = _PMR02100ViewModel.BasedOnRadioSelected == "2" ? "" : _PMR02100ViewModel.PrintParam.CFROM_CUSTOMER_NAME,
                CTO_CUSTOMER_ID = _PMR02100ViewModel.BasedOnRadioSelected == "2" ? "" : _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_ID,
                CTO_CUSTOMER_NAME = _PMR02100ViewModel.BasedOnRadioSelected == "2" ? "" : _PMR02100ViewModel.PrintParam.CTO_CUSTOMER_NAME,
                CCUT_OFF_DATE = _PMR02100ViewModel.DCUT_OFF_DATE.ToString("yyyyMMdd"),
                DCUT_OFF_DATE = _PMR02100ViewModel.DCUT_OFF_DATE,
                LINVOICE_GROUP = _PMR02100ViewModel.PrintParam.LINVOICE_GROUP,
                CINV_GRP_CODE = _PMR02100ViewModel.PrintParam.LINVOICE_GROUP == false ? "" : _PMR02100ViewModel.PrintParam.CINV_GRP_CODE,
                CINV_GRP_NAME = _PMR02100ViewModel.PrintParam.LINVOICE_GROUP == false ? "" : _PMR02100ViewModel.PrintParam.CINV_GRP_NAME,
                LCUSTOMER_INFORMATION = _PMR02100ViewModel.PrintParam.LCUSTOMER_INFORMATION,
                LUNALLOCATED_RECEIPT = _PMR02100ViewModel.PrintParam.LUNALLOCATED_RECEIPT,
                LDESCRIPTION = _PMR02100ViewModel.PrintParam.LDESCRIPTION,
                LPENALTY = _PMR02100ViewModel.PrintParam.LPENALTY,
                CTO_JRNGRP_CODE = _PMR02100ViewModel.BasedOnRadioSelected == "1" ? "" : _PMR02100ViewModel.PrintParam.CTO_JRNGRP_CODE,
                CTO_JRNGRP_NAME = _PMR02100ViewModel.BasedOnRadioSelected == "1" ? "" : _PMR02100ViewModel.PrintParam.CTO_JRNGRP_NAME,
                CFROM_JRNGRP_CODE = _PMR02100ViewModel.BasedOnRadioSelected == "1" ? "" : _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_CODE,
                CFROM_JRNGRP_NAME = _PMR02100ViewModel.BasedOnRadioSelected == "1" ? "" : _PMR02100ViewModel.PrintParam.CFROM_JRNGRP_NAME,
                CREPORT_TYPE = _PMR02100ViewModel.ReportTypeRadioSelected == "1" ? "Summary" : "Detail",
                CLANG_ID = _clientHelper.Culture.Name,
            };
            
            //Summary and Customer
            if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "1" && loParam.LUNALLOCATED_RECEIPT == false && loParam.LPENALTY == false)
            {
                loParam.CINV_GRP_CODE = "";
                loParam.LDESCRIPTION = false;
                loParam.LINVOICE_GROUP = false;

                await _reportService.GetReport(
                    "R_DefaultServiceUrlPM",
                    "PM",
                    "rpt/PMR02101Print/DownloadResultPrintPost",
                    "rpt/PMR02101Print/ReportListGet",
                    loParam);
            }
            
            //Summary, Customer, Unallocated Receipt
            else if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "1" && loParam.LUNALLOCATED_RECEIPT == true  && loParam.LPENALTY == false)
            {
             
                loParam.CINV_GRP_CODE = "";
                loParam.LDESCRIPTION = false;
                loParam.LINVOICE_GROUP = false;

                await _reportService.GetReport(
                  "R_DefaultServiceUrlPM",
                  "PM",
                  "rpt/PMR02102Print/DownloadResultPrintPost",
                  "rpt/PMR02102Print/ReportListGet",
                  loParam);
            }
            
            //Summary, Customer, Penalty
            else if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "1" && loParam.LPENALTY == true && loParam.LUNALLOCATED_RECEIPT == false)
            {
                loParam.CINV_GRP_CODE = "";
                loParam.LDESCRIPTION = false;
                loParam.LINVOICE_GROUP = false;

                await _reportService.GetReport(
                   "R_DefaultServiceUrlPM",
                   "PM",
                   "rpt/PMR02103Print/DownloadResultPrintPost",
                   "rpt/PMR02103Print/ReportListGet",
                   loParam);
            }
            
            //Summary, Customer, Penalty, Unallocated Receipt
            else if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "1" && loParam.LPENALTY == true && loParam.LUNALLOCATED_RECEIPT == true)
            {
                loParam.CINV_GRP_CODE = "";
                loParam.LDESCRIPTION = false;
                loParam.LINVOICE_GROUP = false;

                await _reportService.GetReport(
                 "R_DefaultServiceUrlPM",
                 "PM",
                 "rpt/PMR02104Print/DownloadResultPrintPost",
                 "rpt/PMR02104Print/ReportListGet",
                 loParam);
            }
            
            //Summary and Journal Group
            else if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "2" &&  loParam.LUNALLOCATED_RECEIPT == false && loParam.LPENALTY == false)
            {

                await _reportService.GetReport(
                 "R_DefaultServiceUrlPM",
                 "PM",
                 "rpt/PMR02105Print/DownloadResultPrintPost",
                 "rpt/PMR02105Print/ReportListGet",
                 loParam);
            }

            //Summary and Journal Group, Unallocated Receipt
            else if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "2" && loParam.LUNALLOCATED_RECEIPT == true  && loParam.LPENALTY == false)
            {
                loParam.CINV_GRP_CODE = "";
                loParam.LDESCRIPTION = false;
                loParam.LINVOICE_GROUP = false;

                await _reportService.GetReport(
                 "R_DefaultServiceUrlPM",
                 "PM",
                 "rpt/PMR02106Print/DownloadResultPrintPost",
                 "rpt/PMR02106Print/ReportListGet",
                 loParam);
            }
            
            //Summary and Journal Group, Penalty
            else if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "2" && loParam.LUNALLOCATED_RECEIPT == false  && loParam.LPENALTY == true)
            {
                loParam.CINV_GRP_CODE = "";
                loParam.LDESCRIPTION = false;
                loParam.LINVOICE_GROUP = false;

                await _reportService.GetReport(
                 "R_DefaultServiceUrlPM",
                 "PM",
                 "rpt/PMR02107Print/DownloadResultPrintPost",
                 "rpt/PMR02107Print/ReportListGet",
                 loParam);
            }
            
            //Summary and Journal Group, Penalty, Unallocated Receipt
            else if (loParam.CREPORT_TYPE == "Summary" && _PMR02100ViewModel.BasedOnRadioSelected == "2" && loParam.LUNALLOCATED_RECEIPT == true  && loParam.LPENALTY == true )
            {
             
                loParam.LDESCRIPTION = false;
                await _reportService.GetReport(
                  "R_DefaultServiceUrlPM",
                  "PM",
                  "rpt/PMR02108Print/DownloadResultPrintPost",
                  "rpt/PMR02108Print/ReportListGet",
                  loParam);
            }
            
            //Detail and Customer
            else if (loParam.CREPORT_TYPE == "Detail" && _PMR02100ViewModel.BasedOnRadioSelected == "1" && loParam.LUNALLOCATED_RECEIPT == false && loParam.LPENALTY == false )
            {
                await _reportService.GetReport(
                 "R_DefaultServiceUrlPM",
                 "PM",
                 "rpt/PMR02109Print/DownloadResultPrintPost",
                 "rpt/PMR02109Print/ReportListGet",
                 loParam);
            }
            
            //Detail and Customer, Penalty
            else if (loParam.CREPORT_TYPE == "Detail" && _PMR02100ViewModel.BasedOnRadioSelected == "1" && loParam.LPENALTY == true && loParam.LUNALLOCATED_RECEIPT == false)
            {
                await _reportService.GetReport(
                 "R_DefaultServiceUrlPM",
                 "PM",
                 "rpt/PMR02110Print/DownloadResultPrintPost",
                 "rpt/PMR02110Print/ReportListGet",
                 loParam);
            }
            
            //Detail and Journal Group
            else if (loParam.CREPORT_TYPE == "Detail" && _PMR02100ViewModel.BasedOnRadioSelected == "2" && loParam.LPENALTY == false && loParam.LUNALLOCATED_RECEIPT == false)
            {

                await _reportService.GetReport(
                "R_DefaultServiceUrlPM",
                "PM",
                "rpt/PMR02111Print/DownloadResultPrintPost",
                "rpt/PMR02111Print/ReportListGet",
                loParam);
            }
            
            //Detail and Journal Group, Penalty
            else if (loParam.CREPORT_TYPE == "Detail" && _PMR02100ViewModel.BasedOnRadioSelected == "2" && loParam.LPENALTY == true && loParam.LUNALLOCATED_RECEIPT == false)
            {

                await _reportService.GetReport(
                 "R_DefaultServiceUrlPM",
                 "PM",
                 "rpt/PMR02112Print/DownloadResultPrintPost",
                 "rpt/PMR02112Print/ReportListGet",
                 loParam);
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        EndBlock:
        loEx.ThrowExceptionIfErrors();
    }
       
        
}

