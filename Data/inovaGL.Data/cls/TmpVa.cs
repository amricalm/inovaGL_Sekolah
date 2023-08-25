using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnTmpVa:AdnBaseClass
    {
        public Int64 Kd { get; set; }
        public string NmFile { get; set; }
        public List<AdnTmpVaDtl> ItemDf { get; set; }

        public AdnTmpVa()
        {
            this.NmFile = "";
            this.ItemDf = new List<AdnTmpVaDtl>();
        }
    }

    public class AdnTmpVaDtl : AdnBaseClass
    {
        public Int64 Kd { get; set; }
        public string Baris { get; set; }
    }
}
