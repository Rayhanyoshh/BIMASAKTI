using GSM01000Common.DTOs;
using GSM01000Model;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Exceptions;

namespace GSM01000Front
{
    public partial class CopyFromModal : R_Page
    {
        private GSM01000ViewModel _GSM01000ViewModel = new();

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
                await _GSM01000ViewModel.GetCompanyListStreamAsync();
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
                _GSM01000ViewModel.SelectedCopyFromCompanyId = loData.CCOMPANY_ID;
                await _GSM01000ViewModel.CopyFromProcessAsync();
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
