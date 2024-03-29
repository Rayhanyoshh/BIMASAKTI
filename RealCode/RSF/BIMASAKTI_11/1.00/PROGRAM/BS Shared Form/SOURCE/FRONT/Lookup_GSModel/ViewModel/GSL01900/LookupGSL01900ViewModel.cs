﻿using Lookup_GSCOMMON.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Lookup_GSModel.ViewModel
{
    public class LookupGSL01900ViewModel : R_ViewModel<GSL01900DTO>
    {
        private PublicLookupModel _model = new PublicLookupModel();
        private PublicLookupRecordModel _modelRecord = new PublicLookupRecordModel();

        public ObservableCollection<GSL01900DTO> LOBGrid = new ObservableCollection<GSL01900DTO>();

        public async Task GetLOBList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.GSL01900GetLOBListAsync();

                LOBGrid = new ObservableCollection<GSL01900DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task<GSL01900DTO> GetLOB(GSL01900ParameterDTO poEntity)
        {
            var loEx = new R_Exception();
            GSL01900DTO loRtn = null;
            try
            {
                var loResult = await _modelRecord.GSL01900GetLOBAsync(poEntity);
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
