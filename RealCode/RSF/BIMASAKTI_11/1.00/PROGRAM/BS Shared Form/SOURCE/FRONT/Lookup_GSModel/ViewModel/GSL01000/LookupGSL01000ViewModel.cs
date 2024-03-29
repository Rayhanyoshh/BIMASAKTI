﻿using Lookup_GSCOMMON.DTOs;
using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Lookup_GSModel.ViewModel
{
    public class LookupGSL01000ViewModel : R_ViewModel<GSL01000DTO>
    {
        private PublicLookupModel _model = new PublicLookupModel();
        private PublicLookupRecordModel _modelRecord = new PublicLookupRecordModel();

        public ObservableCollection<GSL01000DTO> UserGrid = new ObservableCollection<GSL01000DTO>();

        public async Task GetUserList()
        {
            var loEx = new R_Exception();

            try
            {
                var loResult = await _model.GSL01000GetUserListAsync();

                UserGrid = new ObservableCollection<GSL01000DTO>(loResult);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        public async Task<GSL01000DTO> GetUser(GSL01000ParameterDTO poEntity)
        {
            var loEx = new R_Exception();
            GSL01000DTO loRtn = null;
            try
            {
                var loResult = await _modelRecord.GSL01000GetUserAsync(poEntity);
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
