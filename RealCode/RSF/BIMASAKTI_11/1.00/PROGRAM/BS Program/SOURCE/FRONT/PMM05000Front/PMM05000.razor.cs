﻿

using System.Globalization;
using BlazorClientHelper;
using GFF00900COMMON.DTOs;
using PMM05000Common.DTOs;
using PMM05000Model;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Popup;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_LockingFront;

namespace PMM05000Front;

public partial class PMM05000 : R_Page
{
    private PMM05010ViewModel _PMM05010ViewModel = new();
    private R_Grid<PMM05010DTO> _gridUnitTypeRef;
    private R_ConductorGrid _condUnitTypeRef;
    //
    private PMM05000ViewModel _PMM05000ViewModel = new();
    private R_Grid<PMM05000DTO> _gridUnitTypePriceRef;
    private R_ConductorGrid _conductorUnitPriceRef;

    private string loLabel = "Activate";
    [Inject] private R_PopupService PopupService { get; set; }
    [Inject] private IClientHelper _clientHelper { get; set; }




    protected override async Task R_Init_From_Master(object poParam)
    {
        var loEx = new R_Exception();

        try
        {

            await GetParamListData(null);

            _PMM05000ViewModel.SqmTotalList = new List<DropDownSqmTotalDTO>()
            {
                new DropDownSqmTotalDTO { Id = "01", Text = "By Sqm"},
                new DropDownSqmTotalDTO { Id = "02", Text = "Total" }
            };


            _PMM05010ViewModel.propertyId = _PMM05000ViewModel.PropertyValue;
            await _gridUnitTypeRef.R_RefreshGrid(null);

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }

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
            var loData = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(eventArgs.Data);

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
                    Program_Id = "PMM05000",
                    Table_Name = "PMM_UNIT_TYPE_PRICE",
                    Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CUNIT_TYPE_ID)
                };

                loLockResult = await loCls.R_Lock(loLockPar);
            }
            else
            {
                var loUnlockPar = new R_ServiceLockingUnLockParameterDTO
                {
                    Company_Id = _clientHelper.CompanyId,
                    User_Id = _clientHelper.UserId,
                    Program_Id = "PMM05000",
                    Table_Name = "PMM_UNIT_TYPE_PRICE",
                    Key_Value = string.Join("|", _clientHelper.CompanyId, loData.CCOMPANY_ID, loData.CUNIT_TYPE_ID)
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
    private async Task GetParamListData(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            await _PMM05000ViewModel.GetPropertyList();
            // await _PMM05010ViewModel.GetUnitTypeGridList();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    #region UNIT TYPE (TABEL KIRI)
    private async Task UnitTypeGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            _PMM05000ViewModel.Data.CPROPERTY_ID = _PMM05000ViewModel.PropertyValue;
            await _PMM05010ViewModel.GetUnitTypeGridList();
            eventArgs.ListEntityResult = _PMM05010ViewModel.UnitTypeList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }

    private async Task UnitTypeGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<PMM05010DTO>(eventArgs.Data);
            _PMM05000ViewModel.UnitTypeValue = loParam.CUNIT_TYPE_ID;

            await OnChanged(loParam);

            eventArgs.Result = _PMM05010ViewModel.UnitType;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    private async Task UnitTypeGrid_Display(R_DisplayEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<PMM05010DTO>(eventArgs.Data);
            _PMM05010ViewModel.UnitTypeId = loParam.CUNIT_TYPE_ID;
            _PMM05000ViewModel.UnitTypeValue = loParam.CUNIT_TYPE_ID;

            await _gridUnitTypePriceRef.R_RefreshGrid(loParam);
            // await OnChanged(loParam);

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    #endregion

    #region UNIT TYPE PRICE (TABEL ANAK)

    private async Task GridUnitPrice_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            await _PMM05000ViewModel.GetUnitTypePriceList();
            eventArgs.ListEntityResult = _PMM05000ViewModel.UnitPriceList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private async Task ConductorUnitPrice_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(eventArgs.Data);
            // loParam.LACTIVE = _PMM05000ViewModel.SelectedActiveInactive;
            await _PMM05000ViewModel.GetUnitPriceById(loParam);
            eventArgs.Result = _PMM05000ViewModel.loTmp;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private async Task Grid_Display(R_DisplayEventArgs eventArgs)
    {
        if (eventArgs.ConductorMode == R_eConductorMode.Normal)
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(eventArgs.Data);
            _PMM05000ViewModel.loTmp.CVALID_INTERNAL_ID = loParam.CVALID_INTERNAL_ID;
            _PMM05000ViewModel.loTmp.LACTIVE = loParam.LACTIVE;
            //_PMM05000ViewModel.Data.DVALID_DATE = DateTime.ParseExact(loParam.CVALID_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);
            if (loParam.LACTIVE)
            {
                loLabel = "Inactive";
                _PMM05000ViewModel.SelectedActiveInactive = false;
            }
            else
            {
                loLabel = "Active";
                _PMM05000ViewModel.SelectedActiveInactive = true;
            }
        }
    }

    private async Task Conductor_ServiceSave(R_ServiceSaveEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(eventArgs.Data);
            await _PMM05000ViewModel.SaveUnitPrice(loParam, (eCRUDMode)eventArgs.ConductorMode);
            eventArgs.Result = _PMM05000ViewModel.loTmp;

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }

    public async Task Conductor_Saving(R_SavingEventArgs eventArgs)
    {
        int sequenceNumber = (GetNextSequenceNumber() - 1);
        var loParam = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(eventArgs.Data);
        // (loParam).CVALID_DATE = loParam.DVALID_DATE.ToString("yyyyMMdd");
        var loEx = new R_Exception();
        try
        {

            if (eventArgs.ConductorMode == R_eConductorMode.Edit)
            {
                loParam.CVALID_INTERNAL_ID = loParam.CVALID_INTERNAL_ID;
                //loParam.CVALID_DATE = loParam.DVALID_DATE.ToString("yyyyMMdd");
                loParam.DVALID_DATE = DateTime.ParseExact(loParam.CVALID_DATE, "yyyyMMdd", CultureInfo.InvariantCulture);

                if (loParam.NBOTTOM_PRICE_SQM > loParam.NNORMAL_PRICE_SQM)
                {
                    loEx.Add("002", "NBOTTOM_PRICE_SQM cannot be greater than NNORMAL_PRICE_SQM");
                }

                // Validasi nilai tidak boleh negatif
                if (loParam.NBOTTOM_PRICE_SQM < 0 || loParam.NNORMAL_PRICE_SQM < 0)
                {
                    loEx.Add("003", "Prices cannot be negative");
                }

            }
            if (eventArgs.ConductorMode == R_eConductorMode.Add)
            {
                loParam.CVALID_DATE = loParam.DVALID_DATE.ToString("yyyyMMdd");
                loParam.CVALID_INTERNAL_ID = loParam.DVALID_DATE.ToString("yyyyMMdd") + sequenceNumber.ToString("D3");


                bool isInvalidDate = false;
                foreach (var validDate in _PMM05000ViewModel.UnitPriceList)
                {
                    if (loParam.DVALID_DATE < validDate.DVALID_DATE)
                    {
                        isInvalidDate = true;
                    }
                }
                if (isInvalidDate)
                {
                    loEx.Add("001", "Valid Date cannot be earlier than last Valid Date");
                }
                if (loParam.NBOTTOM_PRICE_SQM > loParam.NNORMAL_PRICE_SQM)
                {
                    loEx.Add("002", "NBOTTOM_PRICE_SQM cannot be greater than NNORMAL_PRICE_SQM");
                }

                // Validasi nilai tidak boleh negatif
                if (loParam.NBOTTOM_PRICE_SQM < 0 || loParam.NNORMAL_PRICE_SQM < 0)
                {
                    loEx.Add("003", "Prices cannot be negative");
                }


            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();


    }
    public void Conductor_AfterAdd(R_AfterAddEventArgs eventArgs)
    {
        var loEx = new R_Exception();

        try
        {
            int sequenceNumber = GetNextSequenceNumber();
            var loEntity = (PMM05000DTO)eventArgs.Data;
            loEntity.DVALID_DATE = DateTime.Now;
            loEntity.CVALID_INTERNAL_ID = loEntity.DVALID_DATE.ToString("yyyyMMdd") + sequenceNumber.ToString("D3");
            loEntity.LACTIVE = true;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }

    private async Task Connductor_AfterSave(R_AfterSaveEventArgs eventArgs)
    {
        await _gridUnitTypePriceRef.R_RefreshGrid(null);
    }

    private async Task Conductor_ServiceDelete(R_ServiceDeleteEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<PMM05000DTO>(eventArgs.Data);
            await _PMM05000ViewModel.DeleteUnitPrice(loParam);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    #endregion

    #region Active InActive
    private async Task R_Before_Open_Popup_ActivateInactive(R_BeforeOpenPopupEventArgs eventArgs)
    {
        R_Exception loException = new R_Exception();
        try
        {
            var loValidateViewModel = new GFF00900Model.ViewModel.GFF00900ViewModel();
            loValidateViewModel.ACTIVATE_INACTIVE_ACTIVITY_CODE =
                "PMM05001"; //Uabh Approval Code sesuai Spec masing masing
            await loValidateViewModel
                .RSP_ACTIVITY_VALIDITYMethodAsync(); //Jika IAPPROVAL_CODE == 3, maka akan keluar RSP_ERROR disini

            //Jika Approval User ALL dan Approval Code 1, maka akan langsung menjalankan ActiveInactive
            if (loValidateViewModel.loRspActivityValidityList.FirstOrDefault().CAPPROVAL_USER == "ALL" &&
                loValidateViewModel.loRspActivityValidityResult.Data.FirstOrDefault().IAPPROVAL_MODE == 1)
            {
                //var loParam = R_FrontUtility.ConvertObjectToObject<PMM05010DTO>(_PMM05000ViewModel.UnitPriceList);
                await _PMM05000ViewModel
                    .ActiveInactiveProcessAsync(); //Ganti jadi method ActiveInactive masing masing
                await _gridUnitTypePriceRef.R_RefreshGrid(null);
                return;
            }
            else //Disini Approval Code yang didapat adalah 2, yang berarti Active Inactive akan dijalankan jika User yang diinput ada di RSP_ACTIVITY_VALIDITY
            {
                eventArgs.Parameter = new GFF00900ParameterDTO()
                {
                    Data = loValidateViewModel.loRspActivityValidityList,
                    IAPPROVAL_CODE = "PMM05001" //Uabh Approval Code sesuai Spec masing masing
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
            PMM05010DTO param = new PMM05010DTO()
            {
                CPROPERTY_ID = _PMM05000ViewModel.PropertyValue,
                CUNIT_TYPE_ID = _PMM05000ViewModel.UnitTypeValue,
                LACTIVE = _PMM05000ViewModel.SelectedActiveInactive
            };


            if (eventArgs.Success == false)
            {
                return;
            }

            bool result = (bool)eventArgs.Result;
            if (result == true)
            {
                await _PMM05000ViewModel.ActiveInactiveProcessAsync();
                await _gridUnitTypePriceRef.R_RefreshGrid(null);

            }
        }
        catch (Exception ex)
        {
            loException.Add(ex);
        }
        loException.ThrowExceptionIfErrors();
        await _gridUnitTypePriceRef.R_RefreshGrid(null);
    }
    #endregion


    private async Task OnChanged(object poParam)
    {
        var loEx = new R_Exception();
        try
        {
            _PMM05010ViewModel.propertyId = _PMM05000ViewModel.PropertyValue;
            await _gridUnitTypeRef.R_RefreshGrid(null);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }

    private int NextId = 001;
    int GetNextSequenceNumber()
    {
        return Interlocked.Increment(ref NextId);
    }

    #region BeforeAdd



    private async Task GridAddCOA_Validation(R_ValidationEventArgs eventArgs)
    {
        R_Exception loException = new R_Exception();
        R_PopupResult loResult = null;
        GFF00900ParameterDTO loParam = null;
        PMM05000DTO loData = null;
        try
        {
            loData = (PMM05000DTO)eventArgs.Data;
            if (loData.LACTIVE == true && _conductorUnitPriceRef.R_ConductorMode == R_eConductorMode.Add)
            {
                var loValidateViewModel = new GFF00900Model.ViewModel.GFF00900ViewModel();
                loValidateViewModel.ACTIVATE_INACTIVE_ACTIVITY_CODE = "PMM05001";
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
                        IAPPROVAL_CODE = "PMM05001"
                    };
                    loResult = await PopupService.Show(typeof(GFF00900FRONT.GFF00900), loParam);
                    if (loResult.Success == false || (bool)loResult.Result == false)
                    {
                        eventArgs.Cancel = true;
                    }
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