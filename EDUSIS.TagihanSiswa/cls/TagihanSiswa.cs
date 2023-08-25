using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace EDUSIS.KeuanganTagihan
{
    public class AdnTagihanSiswa : AdnBaseClass
    {
        public string KdTagihan { get; set; }
        public string Periode { get; set; }
        public string ThAjar { get; set; }
        public DateTime TglTerbit {get;set;}
        public string KdSekolah { get; set; }
        public int KdSiswa { get; set; }
        public decimal Total { get; set; }
        public string Keterangan { get; set; }
        public List<AdnTagihanSiswaDtl> DfItem { get; set; }

        public AdnTagihanSiswa()
        {
            this.TglTerbit = DateTime.Now;
            this.Total  = 0;
            this.Keterangan = "";
            this.DfItem = new List<AdnTagihanSiswaDtl>();
        }
    }

    public class AdnTagihanSiswaDtl 
    {
        public int KdDtl { get; set; }
        public string KdTagihan { get; set; }
        public string KdBiaya { get; set; }
        public decimal Jmh { get; set; }
        public string ItemBulan { get; set; } //Pisahkan dengan tanda ';' contoh (2012-08;2012-09 -> untuk pembayaran bulan agustus dan september)
        public string Keterangan { get; set; }
        public List<AdnTagihanSiswaDtlPeriode> DfPeriode { get; set; }

        public AdnTagihanSiswaDtl()
        {
            this.Jmh = 0;
            this.ItemBulan = "";
            this.Keterangan = "";
            this.DfPeriode = new List<AdnTagihanSiswaDtlPeriode>();
        }
    }

    public class AdnTagihanSiswaDtlPeriode 
    {
        public int KdDtl { get; set; }
        public string Periode { get; set; } //format YYYY-MM
    }

  

}
