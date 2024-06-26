﻿using Lookup_PMCOMMON.DTOs;
using Lookup_PMCOMMON.DTOs.PML01200;
using System.Collections.Generic;

namespace Lookup_PMCOMMON
{
    public interface IGetRecordLookupLM
    {
        LMLGenericRecord<LML00200DTO> LML00200UnitCharges(LML00200ParameterDTO poParam);
        LMLGenericRecord<LML00300DTO> LML00300Supervisor(LML00300ParameterDTO poParam);
        LMLGenericRecord<LML00400DTO> LML00400UtilityCharges(LML00400ParameterDTO poParam);
        LMLGenericRecord<LML00500DTO> LML00500Salesman(LML00500ParameterDTO poParam);
        LMLGenericRecord<LML00600DTO> LML00600Tenant(LML00600ParameterDTO poParam);
        LMLGenericRecord<LML00700DTO> LML00700Discount(LML00700ParameterDTO poParam);
        LMLGenericRecord<LML00800DTO> LML00800Agreement(LML00800ParameterDTO poParam);
        LMLGenericRecord<LML00900DTO> LML00900Transaction(LML00900ParameterDTO poParam);
        LMLGenericRecord<PML01200DTO> PML01200InvoiceGroup(PML01200ParameterDTO poParam);
    }
}
