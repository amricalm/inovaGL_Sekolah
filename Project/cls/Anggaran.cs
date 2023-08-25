using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL
{
    public class AdnAnggaran : AdnBaseClass
    {
        public string KdAkun { get; set; }
        public string ThAjar { get; set; }
        public int Bulan { get; set; }
        public decimal Nilai{ get; set; }

        public AdnAnggaran()
        {
        }

    }

}
