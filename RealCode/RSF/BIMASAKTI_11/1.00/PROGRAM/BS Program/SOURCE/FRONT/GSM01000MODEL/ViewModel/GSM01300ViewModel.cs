using R_BlazorFrontEnd;
using R_BlazorFrontEnd.Exceptions;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using GSM01000Common.DTOs;
using GSM01000Common;
using R_BlazorFrontEnd.Interfaces;
using System.Net.NetworkInformation;

using GSM01000FrontResources;
using R_BlazorFrontEnd.Interfaces;


namespace GSM01000Model
{
    public class GSM01300ViewModel : R_ViewModel<GSM01300DTO>
    {
        private GSM01300Model _GSM01300Model = new GSM01300Model();


        public List<AssignCoADTO> loGetCOAAssignList = new List<AssignCoADTO>();
        public ObservableCollection<AssignCoADTO> CoAToAssignList { get; set; } = new ObservableCollection<AssignCoADTO>();

        public ObservableCollection<GSM01300DTO> loGridGoAList { get; set; } = new ObservableCollection<GSM01300DTO>();
        public GSM01300DTO loEntity = new GSM01300DTO();
        public string loGOA { get; set; } = "";
        public string GOAcode { get; set; }
        public string GOAname { get; set; }

        public R_ContextHeader _ContextHeader { get; set; }
        
        #region Print

        public GSM01000PrintParamGOADTO loPrint = new GSM01000PrintParamGOADTO();

        public List<GSM01000PrintParamGOADTO> SortBy { get; set; } =
            new List<GSM01000PrintParamGOADTO>()
            {
                new GSM01000PrintParamGOADTO() { CSHORT_BY = "01", SortBy = "Group Of Account Code"},
                new GSM01000PrintParamGOADTO() { CSHORT_BY = "02", SortBy = "Group Of Account Name" },
            };
        #endregion

        public async Task GetGridGoAList()
        {
            R_Exception loEx = new R_Exception();
            GSM01300ListDTO loResult = null;
            try
            {
                R_FrontContext.R_SetStreamingContext(ContextConstant.CGOA_CODE, loGOA);
                loResult = await _GSM01300Model.GetAllGOAAsync();
                loGridGoAList = new ObservableCollection<GSM01300DTO>(loResult.Data);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetGoASingle(GSM01300DTO poParam)
        {
            var loEx = new R_Exception();

            try
            {
                GSM01300DTO loParam = new GSM01300DTO();
                loParam = poParam;
                var loResult = await _GSM01300Model.R_ServiceGetRecordAsync(loParam);
                loEntity = loResult;
                loEntity.CGOA_CODE = GOAcode;
                loEntity.CGOA_NAME = GOAname;
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }
        
        public async Task GetEntity(GSM01300DTO poEntity)
        {
            var loEx = new R_Exception();

            try
            {
                loEntity = await _GSM01300Model.R_ServiceGetRecordAsync(poEntity);
            }
            catch (Exception ex)
            {
                loEx.Add(ex);
            }

            loEx.ThrowExceptionIfErrors();
        }

        public async Task SaveCOAAssign(COAtoAssignParam loParam)
        {
            var loException = new R_Exception();
            R_FrontContext.R_SetStreamingContext(ContextConstant.CGOA_CODE, loGOA);

            try
            {
                COAtoAssignParam loContent = new COAtoAssignParam()
                {
                    CCOMPANY_ID = loEntity.CCOMPANY_ID,
                    CGOA_CODE = loGOA,
                    CUSER_ID = loEntity.CUSER_ID,
                    CCOA_LIST = loParam.CCOA_LIST
                };

                await _GSM01300Model.AssignCenterActionAsync(poParameter: loContent);
            }
            catch (Exception ex)
            {
                loException.Add(ex);
            }

            loException.ThrowExceptionIfErrors();
        }
    }
}