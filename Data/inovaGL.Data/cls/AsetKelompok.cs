using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnAsetKelompok : AdnBaseClass
    {
        public string KdKelompokAset { get; set; }
        public string NmKelompokAset{ get; set; }
        public string Keterangan { get; set; }
        public string CoaAkumulasiPenyusutan { get; set; }
        public string CoaBebanPenyusutan { get; set; }

        public AdnAsetKelompok()
        {
            this.KdKelompokAset = "";
            this.NmKelompokAset = "";
            this.Keterangan = "";
            this.CoaAkumulasiPenyusutan = "";
            this.CoaBebanPenyusutan = "";
        }
    }
}
