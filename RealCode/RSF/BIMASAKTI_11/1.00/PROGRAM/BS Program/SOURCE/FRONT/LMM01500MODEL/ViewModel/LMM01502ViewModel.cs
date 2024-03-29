using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using LMM01500Common.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;

namespace LMM01500Model.ViewModel
{
    public class LMM01502ViewModel: R_ViewModel<LMM01500DTO>
    {
        private LMM01500Model _LMM01500Model = new LMM01500Model();

        public ObservableCollection<LMM01502DTO> LookupBank { get; set; } = new ObservableCollection<LMM01502DTO>();

        public async Task GetLookupBankGrid()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _LMM01500Model.LMM01500LookupBankAsync();

                LookupBank = new ObservableCollection<LMM01502DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

    }
}