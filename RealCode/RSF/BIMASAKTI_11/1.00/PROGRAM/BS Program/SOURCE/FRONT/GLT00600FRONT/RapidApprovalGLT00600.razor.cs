using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using GLT00600Common.DTOs;
using GLT00600Model;
using GLT00600Model.ViewModel;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;

namespace GLT00600Front
{
    public partial class RapidApprovalGLT00600 : R_Page
    {
        private GLT00600ViewModel _JournalListViewModel = new();
        private R_Conductor _conductorRef;

        private R_ConductorGrid _conductorGridRef;
        private R_Grid<GLT00600JournalGridDTO> _gridRef;

        private R_Grid<GLT00600JournalGridDetailDTO> _gridDetailRef;
        private R_ConductorGrid _conductorGridDetailRef;
        [Inject] IClientHelper clientHelper { get; set; }

        #region Invoke
        private void StateChangeInvoke()
        {
            StateHasChanged();
        }
        public async Task ShowSuccessInvoke()
        {
            var loValidate = await R_MessageBox.Show("", "Selected Journal Approved Successfully!", R_eMessageBoxButtonType.OK);
            if (loValidate == R_eMessageBoxResult.OK)
            {
                await _gridRef.R_RefreshGrid(null);
            }
        }
        #endregion

        protected override async Task R_Init_From_Master(object poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                _JournalListViewModel.StateChangeAction = StateChangeInvoke;
                _JournalListViewModel.ShowSuccessAction = async () =>
                {
                    await ShowSuccessInvoke();
                };
                GLT00600DTO data = (GLT00600DTO)poParameter;

                await _JournalListViewModel.GetDepartmentList();

                _JournalListViewModel.Data.ISOFT_PERIOD_YY = data.ISOFT_PERIOD_YY;
                _JournalListViewModel.Data.CSOFT_PERIOD_MM = "88";
                _JournalListViewModel.Data.CSEARCH_TEXT = data.CSEARCH_TEXT;
                _JournalListViewModel.Data.CSTATUS = data.CSTATUS;
                _JournalListViewModel.Data.CSTATUS_NAME = data.CSTATUS_NAME;
                _JournalListViewModel.Data.CDEPT_CODE = data.CDEPT_CODE;
                var selectedDept = (from dept in _JournalListViewModel.AllDeptData
                                    where dept.CDEPT_CODE == data.CDEPT_CODE
                                    select dept).FirstOrDefault();
                if (selectedDept != null)
                {
                    _JournalListViewModel.Data.CDEPT_NAME = selectedDept.CDEPT_NAME;
                }
                _JournalListViewModel.COMPANYID = clientHelper.CompanyId;
                _JournalListViewModel.USERID = clientHelper.UserId;

                await _gridRef.R_RefreshGrid(null);

                _JournalListViewModel.buttonRapidApprove = _JournalListViewModel.JournalList.Count < 1 ? false : true;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

      private async Task ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
      {
          var loEx = new R_Exception();

          try
          {
              await _JournalListViewModel.ShowAllJournals();
              eventArgs.ListEntityResult = _JournalListViewModel.JournalList;
          }
          catch (Exception ex)
          {
              loEx.Add(ex);
          }
          loEx.ThrowExceptionIfErrors();
      }


      private async Task R_display(R_DisplayEventArgs eventArgs)
      {
      //if (eventArgs.ConductorMode == R_BlazorFrontEnd.Enums.R_eConductorMode.Normal)
      //{
      //    _JournalListViewModel.buttonRapidApprove = _JournalListViewModel.JournalList.Count < 1 ? false : true;
      //}
      }
      private async Task OnClose()
      {
          await Close(false, null);
      }
      #region SaveBatchApprove
      private void R_BeforeSaveBatchApprove(R_BeforeSaveBatchEventArgs events)
      {
          var loEx = new R_Exception();
          try
          {
              var loData = (List<GLT00600JournalGridDTO>)events.Data;
              if (loData.Count == 0)
              {
                  R_MessageBox.Show("", @_localizer["_validationNoDataFound"], R_eMessageBoxButtonType.OK);
                  events.Cancel = true;
              }
          }
          catch (Exception ex)
          {
              loEx.Add(ex);
          }
          loEx.ThrowExceptionIfErrors();
      }
      private async Task R_AfterSaveBatchApprove(R_AfterSaveBatchEventArgs eventArgs)
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
      private async Task ServiceSaveBatchApprove(R_ServiceSaveBatchEventArgs eventArgs)
      {
          var loEx = new R_Exception();
          try
          {
              List<GLT00600JournalGridDTO> dataList = ((IEnumerable<GLT00600JournalGridDTO>)eventArgs.Data).ToList();
              // Mengambil dataList yang dipilih
              List<GLT00600JournalGridDTO> selectedData = dataList.Where(dto => dto.LSELECTED).ToList();

              // Mengambil nilai CREF_NO dari dataList yang dipilih
              List<string> crefNumbers = selectedData.Select(dto => dto.CREC_ID).ToList();

              // Menggabungkan nilai CREF_NO dengan koma sebagai separator
              string lcCombinedCREF_NOWithCommaSeparator = string.Join(",", crefNumbers);

              await _JournalListViewModel.RapidApprove(lcCombinedCREF_NOWithCommaSeparator);
              await _JournalListViewModel.GetJournal(new GLT00600DTO() { CREC_ID = crefNumbers.FirstOrDefault() });

              if (_JournalListViewModel.Journal.CSTATUS == "20")
              {
                  R_MessageBox.Show("", "Selected Journal Approved Successfully!", R_eMessageBoxButtonType.OK);
                  Close(true, true);

              }

          }
          catch (Exception ex)
          {
              loEx.Add(ex);
          }
          loEx.ThrowExceptionIfErrors();
      }
      private async Task OnChangeRapidApprove()
      {
          var loEx = new R_Exception();
          try
          {
              var res = await R_MessageBox.Show("", "Are you sure want to process selected Journal(s)?", R_eMessageBoxButtonType.YesNo);
              if (res == R_eMessageBoxResult.Yes)
              {
                  await _conductorGridRef.R_SaveBatch();
              }
          }
          catch (Exception ex)
          {
              loEx.Add(ex);
          }
          loEx.ThrowExceptionIfErrors();
      }
      #endregion
    }
}
