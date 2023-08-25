using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnKasMasuk : AdnBaseClass
    {
        public string KdKM { get; set; }
        public DateTime Tgl { get; set; }
        public string Dari { get; set; }
        public string Deskripsi { get; set; }
        public string KdJurnal { get; set; }
        public string ThAjar { get; set; }

        public List<AdnKasMasukDtl> ItemDf { get; set; }
    }

    public class AdnKasMasukDtl : AdnBaseClass
    {
        public string KdKM { get; set; }
        public string KdAkun { get; set; }
        public int NoUrut { get; set; }
        public string KdProject { get; set; }
        public string KdDept { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit { get; set; }
        public string Memo { get; set; }
        
        public AdnAkun Akun {get;set;}
        public AdnDept Dept { get; set; }

        public AdnKasMasukDtl()
        {
            this.KdProject="";
            this.KdDept="";
            this.Debet=0;
            this.Kredit=0;
            this.Memo="";
            this.Dept = new AdnDept();
        }
    }
}
