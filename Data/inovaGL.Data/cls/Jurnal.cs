using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnJurnal : AdnBaseClass
    {
        public string KdJurnal { get; set; }
        public DateTime Tgl { get; set; }
        public string Deskripsi { get; set; }
        public bool StatusPosting { get; set; }
        public string Sumber { get; set; }
        public string JenisJurnal { get; set; }
        
        public int KdSiswa { get; set; }
        public string Nis { get; set; }
        public string ThAjar { get; set; } //Periode Akuntansi 
        public string ThAjarTagihan { get; set; }
        public string KdSekolah { get; set; }
        public string NoKwitansi { get; set; }

        public string Tag { get; set; }
        public string Periode { get; set; }

        public List<AdnJurnalDtl> ItemDf { get; set; }

        public AdnJurnal()
        {
            this.StatusPosting =false;
            this.Deskripsi = "";
            this.Sumber="";
            this.JenisJurnal = "";

            this.Nis = "";
            this.KdSekolah = "";
            this.ThAjarTagihan = "";
            this.ThAjar = "";
            this.KdSiswa = 0;
            this.NoKwitansi = "";

            this.Tag = "";
            this.Periode = "";
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
        public string SumberDana { get; set; }

        public AdnAkun Akun { get; set; }

        public AdnJurnalDtl()
        {
            this.KdJurnal = "";
            this.KdAkun = "";
            this.KdProject = "";
            this.KdDept = "";
            this.SumberDana = "";
            this.Memo = "";
            this.Debet = 0;
            this.Kredit=0;
            
        }
    }
}
