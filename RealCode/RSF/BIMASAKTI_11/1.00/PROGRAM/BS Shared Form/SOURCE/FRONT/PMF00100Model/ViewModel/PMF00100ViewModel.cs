﻿using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_BlazorFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using PMF00100COMMON.DTOs.PMF00100;
using PMF00100COMMON.DTOs;
using System.ComponentModel;

namespace PMF00100Model.ViewModel
{
    public class PMF00100ViewModel : R_ViewModel<PMF00100ListDTO>
    {
        private PMF00100Model loModel = new PMF00100Model();

        public PMF00100ListDTO loAllocation = new PMF00100ListDTO();

        public ObservableCollection<PMF00100ListDTO> loAllocationList = new ObservableCollection<PMF00100ListDTO>();

        public PMF00100HeaderResultDTO loHeaderRtn = null;

        public PMF00100HeaderDTO loHeader = new PMF00100HeaderDTO();

        public PMF00100ListResultDTO loListRtn = null;

        //public GetAPSystemParamResultDTO loAPSystemParamRtn = null;

        //public GetPeriodYearRangeResultDTO loPeriodYearRangeRtn = null;

        //public GetCompanyInfoResultDTO loCompanyInfoRtn = null;

        //public GetGLSystemParamResultDTO loGLSystemParamRtn = null;

        //public GetTransCodeInfoResultDTO loTransCodeInfoRtn = null;

        public GetCompanyInfoDTO loCompanyInfo = new GetCompanyInfoDTO();

        public GetGLSystemParamDTO loGLSystemParam = null;

        public GetCallerTrxInfoDTO loCallerTrxInfo = null;

        public GetPeriodDTO loSoftPeriod = null;

        public GetPeriodDTO loCurrentPeriod = null;

        public GetTransactionFlagDTO loGetTransactionFlag = null;


        public OpenAllocationParameterDTO loAllocationParameter = new OpenAllocationParameterDTO();

        public async Task GetAllocationListStreamAsync()
        {
            R_Exception loException = new R_Exception();
            PMF00100ListParameterDTO loParam = null;
            List<PMF00100ListDTO> loTemp = null;
            int a = 0;
            try
            {
                loParam = new PMF00100ListParameterDTO()
                {
                    CPROPERTY_ID = loAllocationParameter.CPROPERTY_ID,
                    CDEPT_CODE = loAllocationParameter.CDEPT_CODE,
                    CREF_NO = loAllocationParameter.CREF_NO,
                    CTRANS_CODE = loAllocationParameter.CTRANS_CODE
                };
                R_FrontContext.R_SetStreamingContext(ContextConstant.PMF00100_GET_ALLOCATION_LIST_STREAMING_CONTEXT, loParam);
                loListRtn = await loModel.GetAllocationListStreamAsync();
                loListRtn.Data.ForEach(x =>
                {
                    x.INO = a + 1;
                    x.DALLOC_DATE = DateTime.ParseExact(x.CALLOC_DATE, "yyyyMMdd", null);
                    a++;
                });
                loAllocationList = new ObservableCollection<PMF00100ListDTO>(loListRtn.Data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }

        public async Task GetHeaderAsync()
        {
            R_Exception loEx = new R_Exception();
            PMF00100HeaderResultDTO loResult = null;
            try
            {
                loResult = await loModel.GetHeaderAsync(new PMF00100HeaderParameterDTO()
                {
                    CREC_ID = loAllocationParameter.CREC_ID
                });
                loHeader = loResult.Data;
                //loHeader.DDOC_DATE = DateTime.ParseExact(loHeader.CDOC_DATE, "yyyyMMdd", null);
                loHeader.DREF_DATE = DateTime.ParseExact(loHeader.CREF_DATE, "yyyyMMdd", null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task InitialProcess()
        {
            R_Exception loEx = new R_Exception();
            try
            {
                await GetCompanyInfoAsync();
                await GetCallerTrxInfoAsync();
                await GetGLSystemParamAsync();
                await GetSoftPeriodAsync();
                await GetCurrentPeriodAsync();
                await GetTransactionFlagAsync();
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task GetCompanyInfoAsync()
        {
            R_Exception loEx = new R_Exception();
            GetCompanyInfoResultDTO loResult = null;
            try
            {
                loResult = await loModel.GetCompanyInfoAsync();
                loCompanyInfo = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetGLSystemParamAsync()
        {
            R_Exception loEx = new R_Exception();
            GetGLSystemParamResultDTO loResult = null;
            try
            {
                loResult = await loModel.GetGLSystemParamAsync();
                loGLSystemParam = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetCallerTrxInfoAsync()
        {
            R_Exception loEx = new R_Exception();
            GetCallerTrxInfoResultDTO loResult = null;
            try
            {
                loResult = await loModel.GetCallerTrxInfoAsync(new GetCallerTrxInfoParameterDTO()
                {
                    CREC_ID = loAllocationParameter.CREC_ID
                });
                loCallerTrxInfo = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        public async Task GetSoftPeriodAsync()
        {
            R_Exception loEx = new R_Exception();
            GetPeriodResultDTO loResult = null;
            try
            {
                loResult = await loModel.GetPeriodAsync(new GetPeriodParameterDTO()
                {
                    CPERIOD_YY = loGLSystemParam.CSOFT_PERIOD_YY,
                    CPERIOD_MM = loGLSystemParam.CSOFT_PERIOD_MM
                });
                loSoftPeriod = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetCurrentPeriodAsync()
        {
            R_Exception loEx = new R_Exception();
            GetPeriodResultDTO loResult = null;
            try
            {
                loResult = await loModel.GetPeriodAsync(new GetPeriodParameterDTO()
                {
                    CPERIOD_YY = loGLSystemParam.CCURRENT_PERIOD_YY,
                    CPERIOD_MM = loGLSystemParam.CCURRENT_PERIOD_MM
                });
                loCurrentPeriod = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task GetTransactionFlagAsync()
        {
            R_Exception loEx = new R_Exception();
            GetTransactionFlagResultDTO loResult = null;
            try
            {
                loResult = await loModel.GetTransactionFlagAsync(new GetTransactionFlagParameterDTO()
                {
                    CTRANS_CODE = loAllocationParameter.CTRANS_CODE
                });
                loGetTransactionFlag = loResult.Data;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        public void RefreshAllocationListValidation()
        {
            bool llCancel = false;
            R_Exception loEx = new R_Exception();

            try
            {
                //llCancel = string.IsNullOrWhiteSpace(loAllocation.CDEPARTMENT_CODE);
                //if (llCancel)
                //{
                //    loEx.Add(R_FrontUtility.R_GetError(
                //        typeof(Resources_Dummy_Class),
                //        "V001"));
                //}

                //llCancel = string.IsNullOrWhiteSpace(loAllocation.CSUPPLIER_ID) && loAllocation.CSUPPLIER_OPTIONS == "S";
                //if (llCancel)
                //{
                //    loEx.Add(R_FrontUtility.R_GetError(
                //        typeof(Resources_Dummy_Class),
                //        "V002"));
                //}
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}