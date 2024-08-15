using System.Collections.Generic;
using R_APICommonDTO;

namespace TXR00200Common.DTOs;

public class TXR00200DTO
{
    public string OF { get; set; }
    public string KODE_OBJEK { get; set; }
    public string NAMA { get; set; }
    public decimal HARGA_SATUAN { get; set; }
    public decimal JUMLAH_BARANG { get; set; }
    public decimal HARGA_TOTAL { get; set; }
    public decimal DISKON { get; set; }
    public decimal DPP { get; set; }
    public decimal PPN { get; set; }
    public decimal TARIF_PPNBM { get; set; }
    public decimal PPNBM { get; set; }
}

public class TXR00200DataDTO :R_APIResultBaseDTO
{
    public List<TXR00200DTO> Data { get; set; }
}