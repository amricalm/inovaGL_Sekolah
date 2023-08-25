using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace EDUSIS.VirtualAccount
{
    public class AdnBni : AdnBaseClass
    {
        public DateTime Tgl { get; set; }
        public string Cabang { get; set; }
        public string NoJurnal { get; set; }
        public string Deskripsi { get; set; }
        public decimal Jmh { get; set; }
        public string DK { get; set; }
        public decimal Saldo { get; set; }

        public AdnBni()
        {
            this.Cabang = "";
            this.NoJurnal = "";
            this.Deskripsi = "";
            this.Jmh = 0;
            this.DK = "";
            this.Saldo = 0;
        }
    }

    public class AdnBniTransaksi : AdnBaseClass
    {
        public string Tgl { get; set; }
        public string Cabang { get; set; }
        public string NoJurnal { get; set; }
        public string Deskripsi { get; set; }
        public string Jmh { get; set; }
        public string DK { get; set; }
        public string Saldo { get; set; }

        public string NoVac { get; set; }

        public AdnBniTransaksi()
        {
            this.Cabang = "";
            this.NoJurnal = "";
            this.Deskripsi = "";
            this.Jmh = "";
            this.DK = "";
            this.Saldo = "";

            this.NoVac = "";
        }
    }

    public class AdnSiswaExt 
    {
        public int KdSiswa { get; set; }
        public string KdExt { get; set; }
        public string Flag { get; set; }

        public AdnSiswaExt()
        {
            this.KdExt = "";
            this.Flag = "";
        }
    }
}
