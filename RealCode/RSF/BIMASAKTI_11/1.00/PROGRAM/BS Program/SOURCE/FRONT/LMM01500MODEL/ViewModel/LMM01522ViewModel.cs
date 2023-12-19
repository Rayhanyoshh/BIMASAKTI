using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LMM01500Common.DTOs;
using LMM01500MODEL;
using R_BlazorFrontEnd.Exceptions;

namespace LMM01500Model.ViewModel
{
    public class LMM01522ViewModel
    {
        private LMM01520Model _LMM01520Model = new LMM01520Model();

        public ObservableCollection<LMM01522DTO> LookupAdditional { get; set; } = new ObservableCollection<LMM01522DTO>();

        public async Task GetLookupAdditionalId()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _LMM01520Model.GetAdditionalIdLookupAsync();

                LookupAdditional = new ObservableCollection<LMM01522DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
    }
}