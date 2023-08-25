using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnKasKeluar : AdnBaseClass
    {
        public string KdKK { get; set; }
        public DateTime Tgl { get; set; }
        public string Kepada { get; set; }
        public string Deskripsi { get; set; }
        public string KdJurnal { get; set; }
        public string ThAjar { get; set; }

        public List<AdnKasKeluarDtl> ItemDf { get; set; }

    }
    public class AdnKasKeluarDtl : AdnBaseClass
    {
        public string KdKK { get; set; }
        public string KdAkun { get; set; }
        public int NoUrut { get; set; }
        public string KdProject { get; set; }
        public string SumberDana { get; set; }
        public string KdDept { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit { get; set; }
        public string Memo { get; set; }

        public AdnAkun Akun { get; set; }
        public AdnDept Dept { get; set; }

        public AdnKasKeluarDtl()
        {
            this.KdDept = "";
            this.KdProject = "";
            this.SumberDana = "";
            this.Debet = 0;
            this.Kredit = 0;
            this.Memo = "";
            this.Dept = new AdnDept();
        }
    }

}
