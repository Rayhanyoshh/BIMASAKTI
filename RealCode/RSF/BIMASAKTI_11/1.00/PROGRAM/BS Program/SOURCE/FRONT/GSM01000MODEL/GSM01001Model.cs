using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using R_APIClient;
using R_BlazorFrontEnd.Exceptions;
using System.Data;
using R_BlazorFrontEnd;
using R_BusinessObjectFront;
using GSM01000Common;
using GSM01000Common.DTOs;

namespace GSM01000Model
{
    public class GSM01001Model : R_BusinessObjectServiceClientBase<GSM01001DTO>, IGSM01001
    {
        private const string DEFAULT_HTTP_NAME = "R_DefaultServiceUrl";
        private const string DEFAULT_SERVICEPOINT_NAME = "api/GSM01001";
        private const string DEFAULT_MODULE = "GS";
        
        public GSM01001Model(string pcHttpClientName = DEFAULT_HTTP_NAME,
            string pcRequestServiceEndPoint = DEFAULT_SERVICEPOINT_NAME,
            string pcModule = DEFAULT_MODULE,
            bool plSendWithContext = true,
            bool plSendWithToken = true) :
            base(pcHttpClientName, pcRequestServiceEndPoint, DEFAULT_MODULE, plSendWithContext, plSendWithToken)
        {
        }
        
        public IAsyncEnumerable<GSM01001ErrorValidateDTO> GetErrorProcess()
        {
            throw new NotImplementedException();
        }
    }
}