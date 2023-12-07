using GSM01500COMMON.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace GSM01500MODEL.ViewModel
{
    public class GSM01510ViewModel : R_ViewModel<GSM01510DepartmentDTO>
    {

        private GSM01510Model loModel = new GSM01510Model();

        public GSM01510DepartmentDTO loDept = new GSM01510DepartmentDTO();

        public ObservableCollection<GSM01510DepartmentDTO> loDeptList = new ObservableCollection<GSM01510DepartmentDTO>();

        public GSM01510DepartmentListDTO loRtn = new GSM01510DepartmentListDTO();

        //public R_ContextHeader loContextHeader = new R_ContextHeader();

        public string SelectedCenterCode = "HRD";

        public async Task GetDepartmentListStreamAsync()
        {
            R_Exception loException = new R_Exception();
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CENTER_CODE_STREAM_CONTEXT, SelectedCenterCode);

                loRtn = await loModel.GetCenterDepartmentListStreamAsync();
                loDeptList = new ObservableCollection<GSM01510DepartmentDTO>(loRtn.Data);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }
            loException.ThrowExceptionIfErrors();
        }
    }
}
