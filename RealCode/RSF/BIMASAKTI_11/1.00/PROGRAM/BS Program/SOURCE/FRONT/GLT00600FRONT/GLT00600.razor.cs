using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorClientHelper;
using Microsoft.AspNetCore.Components;
using R_BlazorFrontEnd.Controls;
using R_BlazorFrontEnd.Controls.DataControls;
using R_BlazorFrontEnd.Controls.Events;
using R_BlazorFrontEnd.Controls.Grid.Columns;
using R_BlazorFrontEnd.Enums;
using R_BlazorFrontEnd.Exceptions;
using R_BlazorFrontEnd.Helpers;
using R_CommonFrontBackAPI;
using R_ContextFrontEnd;
using R_BlazorFrontEnd.Helpers;
using Lookup_GSFRONT;
using GLT00600Model.ViewModel;
using GLT00600Common.DTOs;
using Lookup_GSCOMMON.DTOs;
using R_BlazorFrontEnd.Controls.Enums;
using R_BlazorFrontEnd.Controls.MessageBox;
using R_LockingFront;
using Lookup_GSModel.ViewModel;

namespace GLT00600Front;

public partial class GLT00600 : R_Page
{
    private GLT00600ViewModel _JournalListViewModel = new();
    private R_Conductor _conductorRef;

    private R_ConductorGrid _conductorGridRef;
    private R_Grid<GLT00600JournalGridDTO> _gridRef;

    private R_Grid<GLT00600JournalGridDetailDTO> _gridDetailRef;
    private R_ConductorGrid _conductorGridDetailRef;
    [Inject] IClientHelper _clientHelper { get; set; }

    protected override async Task R_Init_From_Master(object poParameter)
    {
        var loEx = new R_Exception();

        try
        {

            await _JournalListViewModel.GetSystemParam();
            await _JournalListViewModel.GetDepartmentList();
            VAR_GL_SYSTEM_PARAMDTO systemparamData = new VAR_GL_SYSTEM_PARAMDTO()
            {
                CCLOSE_DEPT_CODE = _JournalListViewModel.Data.CDEPT_CODE,
                CSTART_PERIOD = _JournalListViewModel.CurrentPeriodStartCollection.CSTART_DATE,
                CCURRENT_PERIOD_MM = _JournalListViewModel.SystemParamCollection.CSTART_PERIOD_MM,
                CCURRENT_PERIOD_YY = _JournalListViewModel.SystemParamCollection.CSTART_PERIOD_YY,
                CSOFT_PERIOD_MM = _JournalListViewModel.SystemParamCollection.CSOFT_PERIOD_MM,
                CSOFT_PERIOD_YY = _JournalListViewModel.SystemParamCollection.CSOFT_PERIOD_YY
            };
            //await _JournalListViewModel.GetCurrentPeriodStart(systemparamData);
            await _JournalListViewModel.GetSoftPeriodStart(systemparamData);
            await _JournalListViewModel.GetGsmCompany();
            await _JournalListViewModel.GetGSMPeriod();
            await _JournalListViewModel.GetTransactionCode();
            await _JournalListViewModel.GetIundo();
            await _JournalListViewModel.GetStatusList();
            //await GetMonth();
            _JournalListViewModel.COMPANYID = _clientHelper.CompanyId;
            _JournalListViewModel.USERID = _clientHelper.UserId;

            //await _JournalListViewModel.GetAllData();
            //await _gridRef.R_RefreshGrid(null);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        loEx.ThrowExceptionIfErrors();
    }
    
   

    public async Task GetMonth()
    {
        _JournalListViewModel.GetMonthList = new List<GetMonthDTO>();

        for (int i = 0; i < 12; i++)
        {
            string monthId = "88";
            GetMonthDTO month = new GetMonthDTO { Id = monthId };
            _JournalListViewModel.GetMonthList.Add(month);
        }
    }
    
    

    public async Task OnclickSearch(object poParam)
    {
        var loEx = new R_Exception();
        try
        {
            loEx.ThrowExceptionIfErrors();
            if (string.IsNullOrEmpty((_JournalListViewModel).Data.CSEARCH_TEXT))
            {
                loEx.Add(new Exception("Please input keyword to search!"));
                goto EndBlock;
            }
            if (!string.IsNullOrEmpty(_JournalListViewModel.Data.CSEARCH_TEXT)
                && _JournalListViewModel.Data.CSEARCH_TEXT.Length < 3)
            {
                loEx.Add(new Exception("Minimum search keyword is 3 characters!"));
                goto EndBlock;
            }

            await _JournalListViewModel.ShowAllJournals();
            if (_JournalListViewModel.JournalList.Count() < 1)
            {
                _JournalListViewModel.JournaDetailList.Clear();
                loEx.Add(new Exception("Data Not Found!"));
                _JournalListViewModel.EnableButton = false;
            }
        EndBlock:;

            GLT00600JournalGridDTO param = new GLT00600JournalGridDTO()
            {
                CPERIOD = _JournalListViewModel.Data.CPERIOD,
                CSEARCH_TEXT = _JournalListViewModel.Data.CSEARCH_TEXT,
                CSTATUS = _JournalListViewModel.Data.CSTATUS,
                CDEPT_CODE = _JournalListViewModel.Data.CDEPT_CODE
            };
            await _gridRef.R_RefreshGrid(param);
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();

    }

    public async Task OnClickShowAll(object poParam)
    {
        var loEx = new R_Exception();
        try
        {
            _JournalListViewModel.Data.CSEARCH_TEXT = "";
            await _JournalListViewModel.ShowAllJournals();
            if (_JournalListViewModel.JournalList.Count() < 1)
            {
                _JournalListViewModel.JournaDetailList.Clear();
                loEx.Add(new Exception("Data Not Found!"));
                _JournalListViewModel.EnableButton = false;
            }

            _JournalListViewModel.lcDeptCode = _JournalListViewModel.Data.CDEPT_CODE;
            GLT00600JournalGridDTO param = new GLT00600JournalGridDTO()
            {
                CPERIOD = _JournalListViewModel.Data.CPERIOD,
                CSEARCH_TEXT = "",
                CSTATUS = _JournalListViewModel.Data.CSTATUS,
                CDEPT_CODE = _JournalListViewModel.Data.CDEPT_CODE
            };

            await _gridRef.R_RefreshGrid(param);

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();

    }

   

    #region JournalGrid
    private async Task JournalGrid_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            await _JournalListViewModel.ShowAllJournals();
            eventArgs.ListEntityResult = _JournalListViewModel.JournalList;

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }
    
    private bool _EnableApprove = false;
    private bool _EnableSubmit = false;
    private async Task JournalGrid_ServiceGetRecord(R_ServiceGetRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(eventArgs.Data);
            await _JournalListViewModel.GetJournal(loParam);

          
            eventArgs.Result = _JournalListViewModel.Journal;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }
    
    private string lcCommitLabel = "Commit";
    private async Task JournalGrid_Display(R_DisplayEventArgs eventArgs)
    {
        R_Exception loEx = new R_Exception();
        try
        {

            _JournalListViewModel.JournalEntity = (GLT00600JournalGridDTO)eventArgs.Data;
            _JournalListViewModel.LcCrecID = _JournalListViewModel.JournalEntity.CREC_ID;

            await _JournalListViewModel.GetJournalDetailList();
            _JournalListViewModel.EnableDept = false;

            if (eventArgs.ConductorMode == R_eConductorMode.Normal)
            {
                var loData = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(eventArgs.Data);
                bool llApprove = _JournalListViewModel.JournalEntity.LALLOW_APPROVE;
                _EnableApprove = loData.CSTATUS == "10" &&
                                 llApprove == true;

                if (loData.CSTATUS == "10" && llApprove == true)
                {
                    _EnableApprove = true;
                }
                else
                {
                    _EnableApprove = false;
                }
                _EnableSubmit = (loData.CSTATUS == "20" || (loData.CSTATUS == "10" && !_JournalListViewModel.TransactionCodeCollection.LAPPROVAL_FLAG)) ||
                                (loData.CSTATUS == "80" && _JournalListViewModel.IundoCollection.IOPTION != 1) &&
                                int.Parse(loData.CREF_PRD) >= int.Parse(_JournalListViewModel.SystemParamCollection.CSOFT_PERIOD);
                _JournalListViewModel.CommitLabel = (_JournalListViewModel.JournalEntity.CSTATUS == "80") ? @_localizer["_btnUndoCommit"] : @_localizer["_btnCommit"];
                ;
                if (!string.IsNullOrWhiteSpace(loData.CREC_ID))
                {
                    await _gridDetailRef.R_RefreshGrid(eventArgs.Data);
                }
                
                if (!(_JournalListViewModel.JournalList == null) && _JournalListViewModel.JournalEntity.LALLOW_APPROVE == true)
                {
                    _JournalListViewModel.EnableButton = true;
                }
                else
                {
                    _JournalListViewModel.EnableButton = false;
                }
            }
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }

    private async Task ApproveJournalProcess()
    {
        var loEx = new R_Exception();
        try
        {
            if (_JournalListViewModel.JournalEntity.LALLOW_APPROVE == false)
            {
                R_MessageBox.Show("", "You don’t have right to approve this journal!", R_eMessageBoxButtonType.OK);
                goto EndBlock;
            }

            GLT00600JournalGridDTO data = new GLT00600JournalGridDTO()
            {
                CSTATUS = _JournalListViewModel.JournalEntity.CSTATUS,
                LCOMMIT_APRJRN = _JournalListViewModel.JournalEntity.LCOMMIT_APRJRN,
                LUNDO_COMMIT = _JournalListViewModel.JournalEntity.LUNDO_COMMIT,
                CREC_ID = _JournalListViewModel.JournalEntity.CREC_ID
            };
            await _JournalListViewModel.ApproveJournal(data);
            await _JournalListViewModel.GetJournal(new GLT00600DTO() { CREC_ID = _JournalListViewModel.JournalEntity.CREC_ID });
            if (_JournalListViewModel.Journal.CSTATUS == "20")
            {
                R_MessageBox.Show("", "Selected Journal Approved Successfully!", R_eMessageBoxButtonType.OK);

            }
            await _JournalListViewModel.ShowAllJournals();

        EndBlock:;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }
    private async Task CommitJournalProcess()
    {
        var loEx = new R_Exception();
        try
        {
            GLT00600JournalGridDTO lcdata = new GLT00600JournalGridDTO()
            {
                CDEPT_CODE = _JournalListViewModel.JournalEntity.CDEPT_CODE,
                CREF_NO = _JournalListViewModel.JournalEntity.CREF_NO,
                CREC_ID = _JournalListViewModel.JournalEntity.CREC_ID
            };
            if (_JournalListViewModel.JournalEntity.CSTATUS == "80")
            {
                var result = await R_MessageBox.Show("", "Are you sure want to undo commit this journal? [Yes/No]", R_eMessageBoxButtonType.YesNo);
                await _JournalListViewModel.UndoCommitJournal(lcdata);
                if (result == R_eMessageBoxResult.Yes)
                {
                    goto commit;
                }
                goto EndBlock;
            }
            else
            {
                var result = await R_MessageBox.Show("", "Are you sure want to commit this journal? [Yes/No]", R_eMessageBoxButtonType.YesNo);
                if (result == R_eMessageBoxResult.Yes)
                {
                    if (_JournalListViewModel.SystemParamCollection.IREVERSE_JRN_POST == 1)
                    {

                        await _JournalListViewModel.CommitJournal(lcdata);
                    }
                }
                else
                {
                    goto EndBlock;
                }
            }
        commit:;
            GLT00600JournalGridDTO data = new GLT00600JournalGridDTO()
            {
                CSTATUS = _JournalListViewModel.JournalEntity.CSTATUS,
                LCOMMIT_APRJRN = _JournalListViewModel.JournalEntity.LCOMMIT_APRJRN,
                LUNDO_COMMIT = _JournalListViewModel.JournalEntity.LUNDO_COMMIT
            };
            await _JournalListViewModel.CommitJournal(data);
            await _JournalListViewModel.GetJournal(new GLT00600DTO()
            {
                CREC_ID = _JournalListViewModel.JournalEntity.CREC_ID,
            });
            if (_JournalListViewModel.CSTATUS_TEMP == "80")
            {
                R_MessageBox.Show("", "Selected Journal Undo Commit Successfully!", R_eMessageBoxButtonType.OK);
            }
            else
            {
                R_MessageBox.Show("", "Selected Journal Committed Successfully!", R_eMessageBoxButtonType.OK);
            }
            await _JournalListViewModel.ShowAllJournals();
        EndBlock:;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }
    #endregion

    #region JournalGridDetail
    private async Task JournalGridDetail_ServiceGetListRecord(R_ServiceGetListRecordEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            await _JournalListViewModel.GetJournalDetailList();
            eventArgs.ListEntityResult = _JournalListViewModel.JournaDetailList;
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        R_DisplayException(loEx);
    }
    #endregion

    #region Predefine Journal Entry
    private void Predef_JournalEntry(R_InstantiateDockEventArgs eventArgs)
    {

        eventArgs.TargetPageType = typeof(GLT00600JournalEntry);
        eventArgs.Parameter = _JournalListViewModel.JournalEntity;
    }
    private async Task AfterPredef_JournalEntry(R_AfterOpenPredefinedDockEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            await _JournalListViewModel.ShowAllJournals();
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        loEx.ThrowExceptionIfErrors();
    }

    #endregion

    #region lookupDept
    private R_Lookup R_LookupBtnDept;
    private async Task Before_Open_lookupDept(R_BeforeOpenLookupEventArgs eventArgs)
    {
        var param = new GSL00700ParameterDTO
        {
            CUSER_ID = _clientHelper.UserId,
            CCOMPANY_ID = _clientHelper.CompanyId
        };
        eventArgs.Parameter = param;
        eventArgs.TargetPageType = typeof(GSL00700);


    }
    private void After_Open_lookupDept(R_AfterOpenLookupEventArgs eventArgs)
    {
        var loTempResult = (GSL00700DTO)eventArgs.Result;
        if (loTempResult == null)
        {
            return;
        }

        _JournalListViewModel.Data.CDEPT_CODE = loTempResult.CDEPT_CODE;
        _JournalListViewModel.Data.CDEPT_NAME = loTempResult.CDEPT_NAME;
    }

    private async Task OnLostFocus_LookupDept()
    {
        var loEx = new R_Exception();
        try
        {
            LookupGSL00700ViewModel loLookupViewModel = new LookupGSL00700ViewModel(); //use GSL's model
            var loParam = new GSL00700ParameterDTO // use match param as GSL's dto, send as type in search texbox
            {
                CSEARCH_TEXT = _JournalListViewModel.Data.CDEPT_CODE, // property that bindded to search textbox
            };


            var loResult = await loLookupViewModel.GetDepartment(loParam); //retrive single record 

            //show result & show name/related another fields
            if (loResult == null)
            {
                loEx.Add(R_FrontUtility.R_GetError(
                    typeof(Lookup_GSFrontResources.Resources_Dummy_Class),
                    "_ErrLookup01"));
                _JournalListViewModel.Data.CDEPT_CODE = ""; //kosongin bind textbox name kalo gaada
                //await GLAccount_TextBox.FocusAsync();
            }
            else
                _JournalListViewModel.Data.CDEPT_NAME = loResult.CDEPT_NAME; //assign bind textbox name kalo ada
        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }

        R_DisplayException(loEx);
    }
    #endregion

    #region RapidApprove
    private async Task R_Before_Open_PopupRapidApprove(R_BeforeOpenPopupEventArgs eventArgs)
    {
        var loEx = new R_Exception();
        try
        {
            var loParam = R_FrontUtility.ConvertObjectToObject<GLT00600DTO>(eventArgs.Parameter);
            if (_JournalListViewModel.JournalEntity.LALLOW_APPROVE == false)
            {
                R_MessageBox.Show("", @_localizer["_validationAllowApprove"], R_eMessageBoxButtonType.OK);
                goto EndBlock;
            }
            if (string.IsNullOrWhiteSpace(_JournalListViewModel.Data.CDEPT_CODE))
            {
                R_MessageBox.Show("", @_localizer["_validationDepartment"], R_eMessageBoxButtonType.OK);
                goto EndBlock;
            }

            GLT00600DTO param = new GLT00600DTO
            {
                ISOFT_PERIOD_YY = _JournalListViewModel.Data.ISOFT_PERIOD_YY,
                CSOFT_PERIOD_MM = _JournalListViewModel.Data.CSOFT_PERIOD_MM,
                CSEARCH_TEXT = "",
                CDEPT_CODE = _JournalListViewModel.lcDeptCode,
                CSTATUS = "10",
                CSTATUS_NAME = _localizer["_submitted"]
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(RapidApprovalGLT00600);

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        EndBlock:
        loEx.ThrowExceptionIfErrors();
    }
    private async Task R_After_Open_PopupRapidApprove(R_AfterOpenPopupEventArgs eventArgs)
    {
        GLT00600ParameterDTO param = new GLT00600ParameterDTO()
        {
            CPERIOD = _JournalListViewModel.JournalEntity.CPERIOD,
            CSEARCH_TEXT = _JournalListViewModel.JournalEntity.CSEARCH_TEXT,
            CSTATUS = "20",
            CDEPT_CODE = _JournalListViewModel.JournalEntity.CDEPT_CODE
        };

        await _gridRef.R_RefreshGrid(param);
        var firstJournal = _JournalListViewModel.JournalList.FirstOrDefault();
        if (firstJournal != null)
        {
            await _gridDetailRef.R_RefreshGrid(firstJournal);
        }
        else
        {
            _JournalListViewModel.JournaDetailList.Clear();
        }
        await Close(true, true);

    }
    #endregion

    #region RapidCommit
    private async Task R_Before_Open_PopupRapidCommit(R_BeforeOpenPopupEventArgs eventArgs)
    {

        var loEx = new R_Exception();

        try
        {
            if (_JournalListViewModel.JournalEntity.LALLOW_APPROVE == false)
            {
                R_MessageBox.Show("", @_localizer["_validationAllowApprove"], R_eMessageBoxButtonType.OK);
                goto EndBlock;
            }
            if (string.IsNullOrWhiteSpace(_JournalListViewModel.Data.CDEPT_CODE))
            {
                R_MessageBox.Show("", @_localizer["_validationDepartment"], R_eMessageBoxButtonType.OK);
                goto EndBlock;
            }

            GLT00600DTO param = new GLT00600DTO
            {
                ISOFT_PERIOD_YY = _JournalListViewModel.Data.ISOFT_PERIOD_YY,
                CSOFT_PERIOD_MM = _JournalListViewModel.Data.CSOFT_PERIOD_MM,
                CSEARCH_TEXT = "",
                CDEPT_CODE = _JournalListViewModel.lcDeptCode,
                CSTATUS = "20",
                CSTATUS_NAME = _localizer["_approved"]
            };
            eventArgs.Parameter = param;
            eventArgs.TargetPageType = typeof(RapidCommitGLT00600);

        }
        catch (Exception ex)
        {
            loEx.Add(ex);
        }
        EndBlock:
        loEx.ThrowExceptionIfErrors();
    }
    private async Task R_After_Open_PopupRapidCommit(R_AfterOpenPopupEventArgs eventArgs)
    {
        // GLT00600JournalGridDTO param;
        // param = _JournalListViewModel.JournalEntity;
        // param.CSTATUS = "80";

        GLT00600ParameterDTO param = new GLT00600ParameterDTO()
        {
            CPERIOD = _JournalListViewModel.JournalEntity.CPERIOD,
            CSEARCH_TEXT = _JournalListViewModel.JournalEntity.CSEARCH_TEXT,
            CSTATUS = "80",
            CDEPT_CODE = _JournalListViewModel.JournalEntity.CDEPT_CODE
        };

        await _gridRef.R_RefreshGrid(param);
        var firstJournal = _JournalListViewModel.JournalList.FirstOrDefault();
        if (firstJournal != null)
        {
            await _gridDetailRef.R_RefreshGrid(firstJournal);
        }
        else
        {
            _JournalListViewModel.JournaDetailList.Clear();
        }

        await Close(true,true);
    }
    #endregion






}