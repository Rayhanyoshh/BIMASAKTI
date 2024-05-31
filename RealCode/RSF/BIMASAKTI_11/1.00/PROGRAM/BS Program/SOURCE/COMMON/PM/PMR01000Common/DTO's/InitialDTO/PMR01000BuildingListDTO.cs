using System;
using System.Collections.Generic;
using R_APICommonDTO;

namespace PMR01000Common.DTO_s;

public class PMR01000BuildingListDTO
{
    public string CCOMPANY_ID { get; set; }
    public string CPROPERTY_ID { get; set; }
    public string CBUILDING_ID { get; set; }
    public string CBUILDING_NAME { get; set; }
    public bool LACTIVE { get; set; }
    public string CCREATE_BY { get; set; }
    public DateTime DCREATE_DATE { get; set; }
    public string CUPDATE_BY { get; set; }
    public DateTime DUPDATE_DATE { get; set; }

}

public class PMR01000BuildingDataDTO :R_APIResultBaseDTO

{
    public List<PMR01000BuildingListDTO> Data { get; set; }
}