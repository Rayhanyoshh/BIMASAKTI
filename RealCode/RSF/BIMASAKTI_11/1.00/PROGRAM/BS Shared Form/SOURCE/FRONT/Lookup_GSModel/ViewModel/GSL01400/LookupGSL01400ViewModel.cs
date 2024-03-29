﻿using Lookup_GSCOMMON.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Lookup_GSModel.ViewModel
{
    public class LookupGSL01400ViewModel : R_ViewModel<GSL01400DTO>
    {
        private PublicLookupModel _model = new PublicLookupModel();
        private PublicLookupRecordModel _modelRecord = new PublicLookupRecordModel();

        public ObservableCollection<GSL01400DTO> OtherChargesGrid = new ObservableCollection<GSL01400DTO>();

        public async Task GetOtherChargesList(GSL01400ParameterDTO poParameter)
        {
            var loEx = new R_Exception();

            try
            {

                var loResult = await _model.GSL01400GetOtherChargesListAsync(poParameter);

                OtherChargesGrid = new ObservableCollection<GSL01400DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task<GSL01400DTO> GetOtherCharges(GSL01400ParameterDTO poParameter)
        {
            var loEx = new R_Exception();
            GSL01400DTO loRtn = null;
            try
            {

                var loResult = await _modelRecord.GSL01400GetOtherChargesAsync(poParameter);
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
