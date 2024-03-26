using CBT01200Common.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace CBT01200Common
{
    public interface ICBT01200
    {
        #region INIT
        VAR_GSM_COMPANYDTO GetCompanyDTO();
        VAR_GL_SYSTEM_PARAMDTO GetGLSystemParam();
        VAR_CB_SYSTEM_PARAMDTO GetCBSystemParam();
        VAR_CSOFT_PERIOD_START_DATEDTO GetSoftPeriodStartDate(VAR_GL_SYSTEM_PARAMDTO poData);
        VAR_USER_DEPARTMENT_LISTDTO GetDeptList();
        VAR_GSM_TRANSACTION_CODEDTO GetLincementLapproval();
        VAR_GSM_PERIODDTO GetPeriod();
        #endregion
    }
}
