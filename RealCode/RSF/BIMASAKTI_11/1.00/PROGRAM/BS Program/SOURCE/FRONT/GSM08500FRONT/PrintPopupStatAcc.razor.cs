using GSM08500Model.ViewModel;
using Lookup_GSCOMMON.DTOs;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Interfaces;
using BlazorClientHelper;
using GSM08500Common.DTOs;
using Lookup_GSCOMMON.DTOs;
using Lookup_GSFRONT;
using GSM008500Common.DTOs.PrintDTO;
using R_BlazorFrontEnd.Controls.MessageBox;

namespace GSM08500Front
{
    public partial class PrintPopupStatAcc : R_Page
    {
        private GSM08500ViewModel _GSM08500ViewModel = new();

        private R_Conductor _conductorRef;
        [Inject] private R_IReport _reportService { get; set; }
        public R_Lookup StatAccLookUp { get; set; }


        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await _GSM08500ViewModel.GetGridList();
                var FirstCOA = _GSM08500ViewModel.loGridList.FirstOrDefault();
                var LastCOA = _GSM08500ViewModel.loGridList.LastOrDefault();
                _GSM08500ViewModel.loPrint.CGL_ACCOUNT_FROM = FirstCOA.CGLACCOUNT_NO;
                _GSM08500ViewModel.loPrint.CGL_ACCOUNT_NAME_FROM = FirstCOA.CGLACCOUNT_NAME;
                _GSM08500ViewModel.loPrint.CGL_ACCOUNT_TO = LastCOA.CGLACCOUNT_NO;
                _GSM08500ViewModel.loPrint.CGL_ACCOUNT_NAME_TO = LastCOA.CGLACCOUNT_NO;

                _GSM08500ViewModel.loPrint.LBALANCE_SHEET = true;
                _GSM08500ViewModel.loPrint.LINCOME_STATEMENT = true;


                _GSM08500ViewModel.loPrint.CSHORT_BY = _GSM08500ViewModel.SortBy.FirstOrDefault().CSHORT_BY;

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
                eventArgs.Parameter = new GSL00510ParameterDTO()

                {
                    CGLACCOUNT_TYPE = "S"
                };
                eventArgs.TargetPageType = typeof(GSL00510);
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


            var loData = (GSL00510DTO)eventArgs.Result;
            _GSM08500ViewModel.loPrint.CGL_ACCOUNT_FROM = loData.CGLACCOUNT_NO;
            _GSM08500ViewModel.loPrint.CGL_ACCOUNT_NAME_FROM = loData.CGLACCOUNT_NAME;


            return Task.CompletedTask;
        }


        private Task R_BeforeOpenLookUpTo(R_BeforeOpenLookupEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                eventArgs.Parameter = new GSL00510ParameterDTO()

                {
                    CGLACCOUNT_TYPE = "S"
                };
                eventArgs.TargetPageType = typeof(GSL00510);
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


            var loData = (GSL00510DTO)eventArgs.Result;
            _GSM08500ViewModel.loPrint.CGL_ACCOUNT_TO = loData.CGLACCOUNT_NO;
            _GSM08500ViewModel.loPrint.CGL_ACCOUNT_NAME_TO = loData.CGLACCOUNT_NAME;

            return Task.CompletedTask;
        }

        public async Task OnChanged()
        {
            var loEx = new R_Exception();

            try
            {


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
            GSM08500PrintParamStatAccDTO loParam;

            try
            {
                loParam = new GSM08500PrintParamStatAccDTO()
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CGL_ACCOUNT_FROM = _GSM08500ViewModel.loPrint.CGL_ACCOUNT_FROM,
                    CGL_ACCOUNT_TO = _GSM08500ViewModel.loPrint.CGL_ACCOUNT_TO,
                    LBALANCE_SHEET = _GSM08500ViewModel.loPrint.LBALANCE_SHEET,
                    LINCOME_STATEMENT = _GSM08500ViewModel.loPrint.LINCOME_STATEMENT,
                    CSHORT_BY = _GSM08500ViewModel.loPrint.CSHORT_BY,
                    LPRINT_INACTIVE = _GSM08500ViewModel.loPrint.LPRINT_INACTIVE,
                    CUSER_LOGIN_ID = _clientHelper.UserId
                };

                if (loParam.CGL_ACCOUNT_TO == loParam.CGL_ACCOUNT_FROM)
                {
                    await R_MessageBox.Show("", "CGL_ACCOUNT_FROM cannot be same with CGL_ACCOUNT_TO", R_BlazorFrontEnd.Controls.MessageBox.R_eMessageBoxButtonType.OK);
                }


                await _reportService.GetReport(
                                  "R_DefaultServiceUrl",
                                  "GS",
                                  "rpt/GSM08500Print/StatAccountPost",
                                  "rpt/GSM08500Print/StatisticAccountReport",
                                  loParam);

                await R_MessageBox.Show("", "Print berhasil", R_eMessageBoxButtonType.OK);
                await this.Close(true, null);
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
