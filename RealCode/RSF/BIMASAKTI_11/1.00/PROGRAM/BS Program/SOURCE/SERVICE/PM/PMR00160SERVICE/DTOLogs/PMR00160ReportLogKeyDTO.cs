using R_CommonFrontBackAPI.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMR00160SERVICE.DTOLogs
{
    public class PMR00160ReportLogKeyDTO <T>
    {
        public T? poParam { get; set; }
        public R_NetCoreLogKeyDTO? poLogKey { get; set; }
    }
}
