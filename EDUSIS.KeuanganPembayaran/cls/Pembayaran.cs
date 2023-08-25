using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace EDUSIS.KeuanganPembayaran
{
    public class AdnPembayaran : AdnBaseClass
    {
        public string KdKwitansi { get; set; }
        public string ThAjar { get; set; }
        public DateTime Tgl {get;set;}
        public string KdSekolah { get; set; }
        public int KdSiswa { get; set; }
        public string Nis { get; set; }
        public decimal Total { get; set; }
        public string Keterangan { get; set; }
        public List<AdnPembayaranDtl> DfItem { get; set; }

        public AdnPembayaran()
        {
            this.Nis = "";
            this.Tgl = DateTime.Now;
            this.Total  = 0;
            this.Keterangan = "";
            this.DfItem = new List<AdnPembayaranDtl>();
        }
    }

    public class AdnPembayaranDtl 
    {
        public int KdDtl { get; set; }
        public string KdKwitansi { get; set; }
        public string KdTagihan { get; set; }
        public string KdBiaya { get; set; }
        public decimal Jmh { get; set; }
        public string ItemBulan { get; set; } //Pisahkan dengan tanda ';' contoh (2012-08;2012-09 -> untuk pembayaran bulan agustus dan september)
        public string Keterangan { get; set; }
        public List<AdnPembayaranDtlPeriode> DfPeriode { get; set; }

        public AdnPembayaranDtl()
        {
            this.KdTagihan = "";
            this.Jmh = 0;
            this.ItemBulan = "";
            this.Keterangan = "";
            this.DfPeriode = new List<AdnPembayaranDtlPeriode>();
        }
    }

    public class AdnPembayaranDtlPeriode 
    {
        public int KdDtl { get; set; }
        public string Periode { get; set; } //format YYYY-MM
    }

    public class AdnLoket 
    {
        public string KdKwitansi { get; set; }
        public string ThAjar { get; set; }
        public DateTime Tgl { get; set; }
        public string KdSekolah { get; set; }
        public string Nis { get; set; }
        public Int64 NoBayar { get; set; }
        public int KdSiswa { get; set; }
        public decimal Total { get; set; }
        public string KasPerkiraan { get; set; }
        public bool Posting { get; set; }
        public string ThAjarTagihan { get; set; }
        public string Flag { get; set; }
        public string Sumber { get; set; }
        public string Uoe { get; set; }
        public List<AdnLoketDtl> DfItem { get; set; }

        public AdnLoket()
        {
            this.Tgl = DateTime.Now;
            this.Total = 0;
            this.Sumber = "";
            this.Uoe = "";
            this.DfItem = new List<AdnLoketDtl>();
        }
    }

    public class AdnLoketDtl
    {
        public string ThAjar { get; set; }
        public string KdSekolah { get; set; }
        public string Nis { get; set; }
        public Int64 NoBayar {get;set;}
        public string KdBiaya { get; set; }
        public DateTime Tgl {get;set;}
        public int? Tag {get;set;}
        public decimal Jmh { get; set; }
        public decimal Diskon { get; set; }
        public string ItemBulan { get; set; } //Pisahkan dengan tanda ';' contoh (2012-08;2012-09 -> untuk pembayaran bulan agustus dan september)
        public int JmhBulan { get; set; }
        public decimal Potongan {get;set;}
        public List<AdnLoketDtlPeriode> DfPeriode { get; set; }

        public Int64 KdDtl { get; set; }

        public decimal JmhSatuan { get; set; }
        public int Qty { get; set; }
        public string Ket { get; set; }
        public string KetKdBiaya { get; set; }

        public AdnLoketDtl()
        {
            this.KdDtl = 0;
            this.Jmh = 0;
            this.ItemBulan = "";
            this.DfPeriode = new List<AdnLoketDtlPeriode>();
        }
    }

    public class AdnLoketDtlPeriode
    {
        public Int64 KdDtl { get; set; }
        public string ThAjar { get; set; }
        public string KdSekolah { get; set; }
        public string Nis { get; set; }
        public Int64 NoBayar { get; set; }
        public string KdBiaya { get; set; }
        public int Bulan { get; set; } 
    }

  

}
