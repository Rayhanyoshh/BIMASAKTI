using System.Collections.Generic;
using R_APICommonDTO;

namespace TXR00200Common.DTOs;

public class TXR00200DTO
{
    public string FK { get; set; }
    public string KD_JENIS_TRANSAKSI { get; set; }
    public string FG_PENGGANTI { get; set; }
    public string NOMOR_FAKTUR { get; set; }
    public string MASA_PAJAK { get; set; }
    public string TAHUN_PAJAK { get; set; }
    public string TANGGAL_FAKTUR { get; set; }
    public string NPWP { get; set; }
    public string NAMA { get; set; }
    public string ALAMAT_LENGKAP { get; set; }
    public string JUMLAH_DPP { get; set; }
    public string JUMLAH_PPN { get; set; }
    public string JUMLAH_PPNBM { get; set; }
    public string ID_KETERANGAN_TAMBAHAN { get; set; }
    public string FG_UANG_MUKA { get; set; }
    public string UANG_MUKA_DPP { get; set; }
    public string UANG_MUKA_PPN { get; set; }
    public string UANG_MUKA_PPNBM { get; set; }
    public string REFERENSI { get; set; }
    public string KODE_DOKUMEN_PENDUKUNG { get; set; }
}

public class TXR00200DataDTO : R_APIResultBaseDTO
{
    public List<TXR00200DTO> Data { get; set; }
}