﻿using Lookup_GSCOMMON.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Lookup_GSModel.ViewModel
{
    public class LookupGSL01200ViewModel : R_ViewModel<GSL01200DTO>
    {
        private PublicLookupModel _model = new PublicLookupModel();
        private PublicLookupRecordModel _modelRecord = new PublicLookupRecordModel();

        public ObservableCollection<GSL01200DTO> BankGrid = new ObservableCollection<GSL01200DTO>();

        public async Task GetBankList(GSL01200ParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.GSL01200GetBankListAsync(poParameter);

                BankGrid = new ObservableCollection<GSL01200DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task<GSL01200DTO> GetBank(GSL01200ParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            GSL01200DTO loRtn = null;
            try
            {
                var loResult = await _modelRecord.GSL01200GetBankAsync(poParameter);
                loRtn = loResult;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
            return loRtn;
        }
    }
}
