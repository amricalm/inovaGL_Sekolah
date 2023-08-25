using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL
{
    public class AdnBukuBank : AdnBaseClass
    {
        public string Kd { get; set; }
        public string KdKas { get; set; }
        public DateTime Tgl { get; set; }
        public string Deskripsi { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit {get;set;}
        public string NoKwitansiDonasi { get; set; }

        public AdnBukuBank()
        {
            this.KdKas = "";
            this.NoKwitansiDonasi = "";
            this.Deskripsi = "";
            this.Debet = 0;
            this.Kredit = 0;

        }
    }

}
