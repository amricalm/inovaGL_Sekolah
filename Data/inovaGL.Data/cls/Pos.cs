using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnPos : AdnBaseClass
    {
        public string KdPos { get; set; }
        public string NmPos { get; set; }
        public string KdDept { get; set; }

        public List<AdnPosDtl> ItemDf {get; set; }
    }

    public class AdnPosDtl
    {
        public string KdPos { get; set; }
        public string KdAkun { get; set; }

        public AdnAkun Akun { get; set; }
    }

}
