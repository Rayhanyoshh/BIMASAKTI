using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GSM01000Common.DTOs;
using GSM01000Model;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;

namespace GSM01000Front
{
    public partial class PrintPopUp : R_Page
    {
        private GSM01000ViewModel _GSM01000ViewModel = new();
        
        private R_Conductor _conductorRef;
        [Inject] private R_IReport _reportService { get; set; }
        public R_Lookup COALookUp { get; set; }
        
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await _GSM01000ViewModel.GetGridList();
                var FirstCOA = _GSM01000ViewModel.loGridList.FirstOrDefault();
                var LastCOA = _GSM01000ViewModel.loGridList.LastOrDefault();
                _GSM01000ViewModel.loPrint.CGL_ACCOUNT_FROM = FirstCOA.CGLACCOUNT_NO;
                _GSM01000ViewModel.loPrint.CGL_ACCOUNT_NAME_FROM = FirstCOA.CGLACCOUNT_NAME;
                _GSM01000ViewModel.loPrint.CGL_ACCOUNT_TO = LastCOA.CGLACCOUNT_NO;
                _GSM01000ViewModel.loPrint.CGL_ACCOUNT_NAME_TO = LastCOA.CGLACCOUNT_NO;

                _GSM01000ViewModel.loPrint.LBALANCE_SHEET = true;

                _GSM01000ViewModel.loPrint.CSHORT_BY = _GSM01000ViewModel.SortBy.FirstOrDefault().CSHORT_BY;

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        private Task R_BeforeOpenLookUpFrom(R_BeforeOpenLookupEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                eventArgs.Parameter = new GSL00500ParameterDTO()

                {

                };
                eventArgs.TargetPageType = typeof(GSL00500);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return Task.CompletedTask;
        }

        private Task R_AfterOpenLookUpFrom(R_AfterOpenLookupEventArgs eventArgs)
        {
            if (eventArgs.Result == null)
            {
                return Task.CompletedTask;
            }


            var loData = (GSL00500DTO)eventArgs.Result;
            _GSM01000ViewModel.loPrint.CGL_ACCOUNT_FROM = loData.CGLACCOUNT_NO;
            _GSM01000ViewModel.loPrint.CGL_ACCOUNT_NAME_FROM = loData.CGLACCOUNT_NAME;


            return Task.CompletedTask;
        }


        private Task R_BeforeOpenLookUpTo(R_BeforeOpenLookupEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                eventArgs.Parameter = new GSL00500ParameterDTO()

                {

                   

                };
                eventArgs.TargetPageType = typeof(GSL00500);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return Task.CompletedTask;
        }

        private Task R_AfterOpenLookUpTo(R_AfterOpenLookupEventArgs eventArgs)
        {
            if (eventArgs.Result == null)
            {
                return Task.CompletedTask;
            }


            var loData = (GSL00500DTO)eventArgs.Result;
            _GSM01000ViewModel.loPrint.CGL_ACCOUNT_TO = loData.CGLACCOUNT_NO;
            _GSM01000ViewModel.loPrint.CGL_ACCOUNT_NAME_TO = loData.CGLACCOUNT_NAME;

            return Task.CompletedTask;
        }

        public async Task OnChanged()
        {
            var loEx = new R_Exception();

            try
            {
                // var loData = _GSM00720ViewModel.RadioButtonCopyFrom;
                // if (_GSM00720ViewModel.loCopyFromEntity.CFROM_CASH_FLOW_FLAG == "01")
                // {
                //     CashFlowGroup.Enabled = true;
                //
                //
                // }
                //
                // else
                // {
                //     CashFlowGroup.Enabled = false;
                //     //_GSM00720ViewModel.CashFlowPlanCode = _GSM00720ViewModel.loCopyFromEntity.CFROM_CASH_FLOW_CODE;
                // }

            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        [Inject] private IClientHelper _clientHelper { get; set; }

        private async Task Button_OnClickProcessAsync()
        {
            var loEx = new R_Exception();
            GSM01000PrintParamCOADTO loParam;

            try
            {
                loParam = new GSM01000PrintParamCOADTO()
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CGL_ACCOUNT_FROM = _GSM01000ViewModel.loPrint.CGL_ACCOUNT_FROM,
                    CGL_ACCOUNT_TO = _GSM01000ViewModel.loPrint.CGL_ACCOUNT_TO,
                    LBALANCE_SHEET = _GSM01000ViewModel.loPrint.LBALANCE_SHEET,
                    LINCOME_STATEMENT = _GSM01000ViewModel.loPrint.LINCOME_STATEMENT,
                    CSHORT_BY = _GSM01000ViewModel.loPrint.CSHORT_BY,
                    LPRINT_INACTIVE = _GSM01000ViewModel.loPrint.LPRINT_INACTIVE,
                    LPRINT_USER_RESTRICT= _GSM01000ViewModel.loPrint.LPRINT_USER_RESTRICT,
                    LPRINT_CENTER_RESTRICT = _GSM01000ViewModel.loPrint.LPRINT_CENTER_RESTRICT,
                    CUSER_LOGIN_ID = _clientHelper.UserId,
                };

                if (loParam.CGL_ACCOUNT_TO == loParam.CGL_ACCOUNT_FROM)
                {
                    await  R_MessageBox.Show("", "CGL_ACCOUNT_FROM cannot be same with CGL_ACCOUNT_TO", R_BlazorFrontEnd.Controls.MessageBox.R_eMessageBoxButtonType.OK);
                }
                
                
                await _reportService.GetReport(
                                  "R_DefaultServiceUrl",
                                  "GS",
                                  "rpt/GSM01000Print/AllCOAPost",
                                  "rpt/GSM01000Print/ChartOfAccountReport",
                                  loParam);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }


        public async Task Button_OnClickCloseAsync()
        {
            await this.Close(true, null);
        }
    }

}
