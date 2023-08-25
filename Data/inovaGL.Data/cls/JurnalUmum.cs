using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnJurnalUmum : AdnBaseClass
    {
        public string KdJU { get; set; }
        public DateTime Tgl { get; set; }
        public string Deskripsi { get; set; }
        public string KdJurnal { get; set; }
        public string ThAjar { get; set; }

        public string Tag { get; set; }
        public string Sumber { get; set; }
        public string Periode { get; set; }
        public string JenisJurnal { get; set; }

        public List<AdnJurnalUmumDtl> ItemDf { get; set; }

        public AdnJurnalUmum()
        {
            this.Tag = "";
            this.Sumber = "";
            this.Periode = "";
            this.JenisJurnal = "";
            this.ThAjar = "";
        }
    
    }

    public class AdnJurnalUmumDtl : AdnBaseClass
    {
        public string KdJU { get; set; }
        public string KdAkun { get; set; }
        public int NoUrut { get; set; }
        public string KdProject { get; set; }
        public string KdDept { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit { get; set; }
        public string Memo { get; set; }

        public AdnAkun Akun { get; set; }
        public AdnDept Dept { get; set; }

        public AdnJurnalUmumDtl()
        {
            this.KdProject = "";
            this.KdDept = "";
            this.Debet = 0;
            this.Kredit = 0;
            this.Memo = "";
            this.Dept = new AdnDept();
        }
    }
}
