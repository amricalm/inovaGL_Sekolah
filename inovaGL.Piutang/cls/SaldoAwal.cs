using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Piutang
{
    public class AdnSaldoAwal : AdnBaseClass
    {
        public string KdSaldoAwal { get; set; }
        public DateTime Tgl { get; set; }
        public string Periode { get; set; }
        public string ThAjar { get; set; }
        public string KdSekolah { get; set; }
        public int KdSiswa { get; set; }
        public decimal Total { get; set; }
        public List<AdnSaldoAwalDtl> DfItem { get; set; }
        
        public AdnSaldoAwal()
        {
            this.Total = 0;
            this.DfItem = new List<AdnSaldoAwalDtl>();
        }
    }

    public class AdnSaldoAwalDtl 
    {
        public int KdDtl { get; set; }
        public string KdSaldoAwal { get; set; }
        public string KdBiaya { get; set; }
        public decimal Jmh { get; set; }
        public string ItemBulan { get; set; } //Pisahkan dengan tanda ';' contoh (2012-08;2012-09 -> untuk pembayaran bulan agustus dan september)
        public string Keterangan { get; set; } //Bisa digunakan untuk mengisi Bulan (Biaya Bulanan, contoh SPP Bulan Jan sd Jun ...)
        public List<AdnSaldoAwalDtlPeriode> DfPeriode { get; set; }
        
        public AdnSaldoAwalDtl()
        {
            this.Jmh = 0;
            this.Keterangan = "";
            this.DfPeriode = new List<AdnSaldoAwalDtlPeriode>();
        }
    }

    public class AdnSaldoAwalDtlPeriode 
    {
        public int KdDtl { get; set; }
        public string Periode { get; set; } //format YYYY-MM
    }
}
