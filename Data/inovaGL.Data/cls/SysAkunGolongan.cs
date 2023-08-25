using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnSysAkunGolongan : AdnBaseClass
    {
        public string KdGolongan { get; set; }
        public string NmGolongan { get; set; }
        public string Tipe { get; set; }
        public string KdJenis { get; set; }

        public AdnSysAkunGolongan()
        {
            this.Tipe = "";
            this.KdJenis = "";
        }
    }
}
