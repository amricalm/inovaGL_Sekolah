using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnSysJenisJurnal : AdnBaseClass
    {
        public string JenisJurnal{ get; set; }
        public string Keterangan { get; set; }


        public AdnSysJenisJurnal()
        {
            this.Keterangan = "";
        }
    }
}
