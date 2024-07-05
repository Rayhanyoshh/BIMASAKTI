using System.Collections.Generic;
using PMR02100Common.DTOs;
using PMR02200Common.DTOs;

namespace PMR02100Common;

public interface IPMR02100
{
    IAsyncEnumerable<PropertyListDTO> GetPropertyList();
}