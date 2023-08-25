using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace EDUSIS.Kelas
{
    public class AdnKelas : AdnBaseClass
    {
        public string KdSekolah { get; set; }
        public string Kelas { get; set; }
        public string KdJurusan { get; set; }
        public string Tingkat { get; set; }

        public AdnKelas()
        {
            this.Kelas= "";
            this.KdJurusan = "";
            this.Tingkat = "";
        }
    }
}
