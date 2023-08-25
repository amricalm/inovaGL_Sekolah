using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL
{
    public class AdnAkun : AdnBaseClass
    {
        public string KdAkun { get; set; }
        public string NmAkun { get; set; }
        public string DK { get; set; }
        public string KdInduk {get;set;}
        public int Turunan { get; set; }
        public string Tipe { get; set; }
        public string KdGolongan { get; set; }
        public string KdDept { get; set; }
        public bool Aktif { get; set; }

        public AdnAkun()
        {
            this.KdDept = "";
            this.Aktif = true;
        }
    }

}
