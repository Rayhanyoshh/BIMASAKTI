using R_APICommonDTO;
using R_CommonFrontBackAPI;
using System;
using System.Collections.Generic;
using System.Text;
using GSM01000Common.DTOs;


namespace GSM01000Common
{
    public interface IGSM01001
    {
        IAsyncEnumerable<GSM01001ErrorValidateDTO> GetErrorProcess();
    }
}