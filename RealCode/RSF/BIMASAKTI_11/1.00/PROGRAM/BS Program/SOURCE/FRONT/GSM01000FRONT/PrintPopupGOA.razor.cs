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
using System;

namespace GSM01000Front
{
    public partial class PrintPopupGOA : R_Page
    {
        private GSM01300ViewModel _GSM01300ViewModel = new ();
        
        private R_Conductor _conductorRef;
        [Inject] private R_IReport _reportService { get; set; }
        public R_Lookup GOALookUp { get; set; }
        
        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();
            try
            {
                await _GSM01300ViewModel.GetGridGoAList();
                var LastGOA = _GSM01300ViewModel.loGridGoAList.LastOrDefault();
                var FirstGOA = _GSM01300ViewModel.loGridGoAList.FirstOrDefault();
                _GSM01300ViewModel.loPrint.CGOA_CODE_FROM = FirstGOA.CGOA_CODE;
                _GSM01300ViewModel.loPrint.CGOA_CODE_NAME_FROM = FirstGOA.CGOA_NAME;
                _GSM01300ViewModel.loPrint.CGOA_CODE_TO = LastGOA.CGOA_CODE;
                _GSM01300ViewModel.loPrint.CGOA_CODE_NAME_TO = LastGOA.CGOA_NAME;

                _GSM01300ViewModel.loPrint.LBALANCE_SHEET = true;
                _GSM01300ViewModel.loPrint.LINCOME_STATEMENT = true;
                _GSM01300ViewModel.loPrint.LPRINT_GL_ACCOUNT = true;
                _GSM01300ViewModel.loPrint.LPRINT_INACTIVE = true;




                _GSM01300ViewModel.loPrint.CSHORT_BY = _GSM01300ViewModel.SortBy.FirstOrDefault().CSHORT_BY;



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
                eventArgs.Parameter = new GSL00550ParameterDTO()

                {
                   
                };
                eventArgs.TargetPageType = typeof(GSL00550);
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
        
        
            var loData = (GSL00550DTO)eventArgs.Result;
            _GSM01300ViewModel.loPrint.CGOA_CODE_FROM = loData.CGOA_CODE;
            _GSM01300ViewModel.loPrint.CGOA_CODE_NAME_FROM = loData.CGOA_NAME;
        
        
            return Task.CompletedTask;
        }
        
        
        private Task R_BeforeOpenLookUpTo(R_BeforeOpenLookupEventArgs eventArgs)
        {
            var loEx = new R_Exception();
        
            try
            {
                eventArgs.Parameter = new GSL00550ParameterDTO()
        
                {
        
                   
        
                };
                eventArgs.TargetPageType = typeof(GSL00550);
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
        
        
            var loData = (GSL00550DTO)eventArgs.Result;
            _GSM01300ViewModel.loPrint.CGOA_CODE_TO = loData.CGOA_CODE;
            _GSM01300ViewModel.loPrint.CGOA_CODE_NAME_TO = loData.CGOA_NAME;
        
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
            GSM01000PrintParamGOADTO loParam;
        
            try
            {
                loParam = new GSM01000PrintParamGOADTO()
                {
                    CCOMPANY_ID = _clientHelper.CompanyId,
                    CGOA_CODE_FROM = _GSM01300ViewModel.loPrint.CGOA_CODE_FROM,
                    CGOA_CODE_TO = _GSM01300ViewModel.loPrint.CGOA_CODE_TO,
                    LBALANCE_SHEET = _GSM01300ViewModel.loPrint.LBALANCE_SHEET,
                    LINCOME_STATEMENT = _GSM01300ViewModel.loPrint.LINCOME_STATEMENT,
                    CSHORT_BY = _GSM01300ViewModel.loPrint.CSHORT_BY,
                    LPRINT_GL_ACCOUNT= _GSM01300ViewModel.loPrint.LPRINT_GL_ACCOUNT,
                    LPRINT_INACTIVE = _GSM01300ViewModel.loPrint.LPRINT_INACTIVE,
                    CUSER_LOGIN_ID = _clientHelper.UserId,
                };
        
                if (loParam.CGOA_CODE_TO == loParam.CGOA_CODE_FROM)
                {
                    await  R_MessageBox.Show("", "CGOA_CODE_FROM cannot be same with CGOA_CODE_TO", R_BlazorFrontEnd.Controls.MessageBox.R_eMessageBoxButtonType.OK);
                }
                
                
                await _reportService.GetReport(
                                  "R_DefaultServiceUrl",
                                  "GS",
                                  "rpt/GSM01000GOAPrint/AllGOAPost",
                                  "rpt/GSM01000GOAPrint/GroupOfAccountReport",
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





