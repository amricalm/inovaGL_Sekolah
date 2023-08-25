using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace EDUSIS.Siswa
{
    public class AdnSiswa : AdnBaseClass
    {
        public int KdSiswa { get; set; }
        public string KdSekolah { get; set; }
        public string NmLengkap { get; set; }
        public string NIS { get; set; }
        public string NISN { get; set; }

        public string AyahNama { get; set; }
        public string NoVA { get; set; }

        public AdnSiswa()
        {
            this.NIS= "";
            this.NISN= "";
            this.AyahNama = "";
            this.NoVA = "";
        }
    }

}
