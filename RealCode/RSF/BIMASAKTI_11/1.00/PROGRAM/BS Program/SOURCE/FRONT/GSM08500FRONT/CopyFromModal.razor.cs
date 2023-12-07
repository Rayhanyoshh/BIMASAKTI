using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;
using GSM08500Common.DTOs;
using GSM08500Model.ViewModel;

namespace GSM08500Front
{
    public partial class CopyFromModal  : R_Page
    {
        private GSM08500ViewModel _GSM08500ViewModel = new();

        private R_ConductorGrid _conGridCompanyRef;

        private R_Grid<CopyFromProcessCompanyDTO> _gridRef;
        
        
        protected override async Task R_Init_From_Master(object poParam)
        {
            var loEx = new R_Exception();

            try
            {
                await _gridRef.R_RefreshGrid(null);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            R_DisplayException(loEx);
        }
        
        private async Task Grid_R_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
        {
            var loEx = new R_Exception();

            try
            {
                await _GSM08500ViewModel.GetCompanyListStreamAsync();
                eventArgs.ListEntityResult = _GSM08500ViewModel.loCompanyList;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        private async Task OnProcess()
        {
            R_Exception loException = new R_Exception();
            try
            {

                var loData = (CopyFromProcessCompanyDTO)_gridRef.GetCurrentData();
                _GSM08500ViewModel.SelectedCopyFromCompanyId = loData.CCOMPANY_ID;
                await _GSM08500ViewModel.CopyFromProcessAsync();
                await this.Close(true, null);
            }
            catch (Exception ex)
            {

                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
        private async Task OnClose()
        {
            await this.Close(true, null);
        }
    }
}

