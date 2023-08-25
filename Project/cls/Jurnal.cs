using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL
{
    public class AdnJurnal : AdnBaseClass
    {
        public string KdJurnal { get; set; }
        public DateTime Tgl { get; set; }
        public string Deskripsi { get; set; }
        public bool StatusPosting { get; set; }

        public List<AdnJurnalDtl> ItemDf { get; set; }

        public AdnJurnal()
        {
            this.StatusPosting =false;
            this.Deskripsi = "";
        }

    }


    public class AdnJurnalDtl : AdnBaseClass
    {
        public string KdJurnal { get; set; }
        public string KdAkun { get; set; }
        public string KdProject { get; set; }
        public string KdDept { get; set; }
        public string Memo { get; set; }
        public int NoUrut { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit { get; set; }

        public AdnAkun Akun { get; set; }

        public AdnJurnalDtl()
        {
            this.KdJurnal = "";
            this.KdAkun = "";
            this.KdProject = "";
            this.KdDept = "";
            this.Memo = "";
            this.Debet = 0;
            this.Kredit=0;
        }
    }
}
