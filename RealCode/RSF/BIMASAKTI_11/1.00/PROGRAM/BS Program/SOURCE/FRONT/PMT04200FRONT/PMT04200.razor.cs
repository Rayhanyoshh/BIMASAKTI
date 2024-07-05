using BlazorClientHelper;
using PMT04200Common;
using PMT04200Common.DTOs;
using PMT04200MODEL;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Lookup_GSModel.ViewModel;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lookup_PMCOMMON.DTOs;
using Lookup_PMFRONT;
using Lookup_PMModel.ViewModel.LML00600;

namespace PMT04200FRONT;

public partial class PMT04200 : R_Page
{
    private PMT04200ViewModel _TransactionListViewModel = new();
    private R_Conductor _conductorRef;
    private R_Grid<PMT04200DTO> _gridRef;
    
    [Inject] IClientHelper clientHelper { get; set; }
    
    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {
            await _TransactionListViewModel.GetAllUniversalData();
            await _TransactionListViewModel.GetPropertyList();
            //Set Dept Code
            if (_TransactionListViewModel.VAR_PROPERTY_LIST.Count > 0)
            {
                var lcPropertyId = _TransactionListViewModel.VAR_PROPERTY_LIST.FirstOrDefault().CPROPERTY_ID;
                OnChangedProperty(lcPropertyId);
            }
            
            //Set Journal Period
            if (!string.IsNullOrWhiteSpace(_TransactionListViewModel.VAR_PM_SYSTEM_PARAM.CSOFT_PERIOD_YY))
                _TransactionListViewModel.ParamPeriodYear = int.Parse(_TransactionListViewModel.VAR_PM_SYSTEM_PARAM.CSOFT_PERIOD_YY);

            _TransactionListViewModel.ParamPeriodMonth = _TransactionListViewModel.VAR_PM_SYSTEM_PARAM.CSOFT_PERIOD_MM;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    
    #region Search
    public async Task OnclickSearch()
    {
        var loEx = new R_Exception();
        try
        {
            if (string.IsNullOrEmpty(_TransactionListViewModel.Param.CSEARCH_TEXT))
            {
                loEx.Add(new Exception("Please input keyword to search!"));
                goto EndBlock;
            }
            if (!string.IsNullOrEmpty(_TransactionListViewModel.Param.CSEARCH_TEXT)
                && _TransactionListViewModel.Param.CSEARCH_TEXT.Length < 3)
            {
                loEx.Add(new Exception("Minimum search keyword is 3 characters!"));
                goto EndBlock;
            }
            await _gridRef.R_RefreshGrid(null);
            if (_TransactionListViewModel.TransactionGrid.Count <= 0)
            {
                loEx.Add("", "No Data Found!");
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

    EndBlock:
        loEx.ThrowExceptionIfErrors();
    }
    public async Task OnClickShowAll()
    {
        var loEx = new R_Exception();
        try
        {
            //reset detail
            _TransactionListViewModel.Param.CSEARCH_TEXT = "";
            await _gridRef.R_RefreshGrid(null);
            if (_TransactionListViewModel.TransactionGrid.Count <= 0)
            {
                loEx.Add("", "No Data Found!");
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();

    }
    #endregion
    #region TransactionGrid
    private async Task JournalGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            await _TransactionListViewModel.GetJournalList();
            eventArgs.ListEntityResult = _TransactionListViewModel.TransactionGrid;
  
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        R_DisplayException(loEx);
    }
    private async Task JournalGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
    {
        R_Exception loEx = new R_Exception();

        try
        {
            eventArgs.Result = (PMT04200DTO)eventArgs.Data;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        R_DisplayException(loEx);
    }
    
    private async Task JournalGrid_Display(R_DisplayEventArgs eventArgs)
    {
        R_Exception loEx = new R_Exception();
        try
        {
            var loData = (PMT04200DTO)eventArgs.Data;
            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                await Task.CompletedTask;
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        R_DisplayException(loEx);
    }
    #endregion
    #region Predefine Transaction Entry
    private void Predef_TransactionEntry(R_InstantiateDockEventArgs eventArgs)
    {
        /*var loParam = (PMT04200DTO)_conductorRef.R_GetCurrentData();*/
        eventArgs.TargetPageType = typeof(PMT04210);
        eventArgs.Parameter = _TransactionListViewModel.Param;
    }
    private async Task AfterPredef_TransactionEntry(R_AfterOpenPredefinedDockEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            /*await _gridRef.R_RefreshGrid(null);*/
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }

    #endregion
    #region lookupDept
    private async void BeforeOpen_lookupDept(R_BeforeOpenLookupEventArgs eventArgs)
    {
        R_Exception loEx = new R_Exception();
        try
        {
            var param = new GSL00710ParameterDTO
            {
                CUSER_LOGIN_ID = clientHelper.UserId,
                CCOMPANY_ID = clientHelper.CompanyId,
                CPROPERTY_ID = _TransactionListViewModel.PropertyDefault
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(GSL00710);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();

    }
    private void AfterOpen_lookupDept(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (GSL00710DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _TransactionListViewModel.Param.CDEPT_CODE = loTempResult.CDEPT_CODE;
        _TransactionListViewModel.Param.CDEPT_NAME = loTempResult.CDEPT_NAME;
    }
    private async Task OnLostFocus_LookupDept()
    {
        var loEx = new R_Exception();

        try
        {
            LookupGSL00710ViewModel loLookupViewModel = new LookupGSL00710ViewModel(); //use GSL's model
            var loParam = new GSL00710ParameterDTO // use match param as GSL's dto, send as type in search texbox
            {
                CSEARCH_TEXT = _TransactionListViewModel.Param.CDEPT_CODE, // property that bindded to search textbox
            };


            var loResult = await loLookupViewModel.GetDepartmentProperty(loParam); //retrive single record 

            //show result & show name/related another fields
            if (loResult == null)
            {
                loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                _TransactionListViewModel.Param.CDEPT_NAME = ""; //kosongin bind textbox name kalo gaada
                //await GLAccount_TextBox.FocusAsync();
            }
            else
                _TransactionListViewModel.Param.CDEPT_NAME = loResult.CDEPT_NAME; //assign bind textbox name kalo ada
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    #region lookupCust
    private async void BeforeOpen_lookupCust(R_BeforeOpenLookupEventArgs eventArgs)
    {
        R_Exception loEx = new R_Exception();
        try
        {
            var param = new LML00600ParameterDTO
            {
                CUSER_ID = clientHelper.UserId,
                CPROPERTY_ID = _TransactionListViewModel.PropertyDefault,
                CSEARCH_TEXT = "",
                CCOMPANY_ID = clientHelper.CompanyId
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
    private void AfterOpen_lookupCust(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (LML00600DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _TransactionListViewModel.Param.CCUSTOMER_ID = loTempResult.CTENANT_ID;
        _TransactionListViewModel.Param.CCUSTOMER_NAME = loTempResult.CTENANT_NAME;
        _TransactionListViewModel.Param.CCUSTOMER_TYPE = loTempResult.CTENANT_TYPE_NAME;
    }
    private async Task OnLostFocus_LookupCust()
    {
        var loEx = new R_Exception();

        try
        {
            LookupLML00600ViewModel loLookupViewModel = new LookupLML00600ViewModel(); //use GSL's model
            var loParam = new LML00600ParameterDTO // use match param as GSL's dto, send as type in search texbox
            {
                CSEARCH_TEXT = _TransactionListViewModel.Param.CDEPT_CODE, // property that bindded to search textbox
            };


            var loResult = await loLookupViewModel.GetTenant(loParam); //retrive single record 

            //show result & show name/related another fields
            if (loResult == null)
            {
                loEx.Add(R_FrontUtility.R_GetError(
                        typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                        "_ErrLookup01"));
                _TransactionListViewModel.Param.CCUSTOMER_NAME = ""; //kosongin bind textbox name kalo gaada
                _TransactionListViewModel.Param.CCUSTOMER_TYPE = ""; //kosongin bind textbox name kalo gaada
                //await GLAccount_TextBox.FocusAsync();
            }
            else
                _TransactionListViewModel.Param.CCUSTOMER_NAME = loResult.CTENANT_NAME; //assign bind textbox name kalo ada
                _TransactionListViewModel.Param.CCUSTOMER_TYPE = loResult.CTENANT_TYPE_NAME; //assign bind textbox name kalo ada
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion
    
    
    private async Task OnChangedProperty(object poParam)
    {
        var loEx = new R_Exception();
        try
        {
            _TransactionListViewModel.Param.CDEPT_CODE = "";
            _TransactionListViewModel.Param.CDEPT_NAME = "";
            _TransactionListViewModel.Param.CCUSTOMER_ID = "";
            _TransactionListViewModel.Param.CCUSTOMER_NAME = "";
            _TransactionListViewModel.Param.CCUSTOMER_TYPE = "";
            
            if (_gridRef.DataSource.Count > 0)
            {
                _gridRef.DataSource.Clear();
            }
            
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }


}