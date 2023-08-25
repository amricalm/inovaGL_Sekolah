using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaPayroll.Core
{
    public class AdnKomponen : AdnBaseClass
    {
        public int Kd  { get; set; }
        public string KdKomponen { get; set; }
        public string NmKomponen { get; set; }
        public bool Flag  { get; set; }
        public string Rumus  { get; set; }
        public string KdAKun { get; set; }
        public int NoUrut  { get; set; }
        public AdnKomponenJenis Jenis { get; set; }

        public AdnKomponen()
        {
            this.Flag = false;
            this.Rumus = "";
            this.KdAKun = "";
            this.NoUrut = 0;
            this.Jenis = new AdnKomponenJenis();
        }
    }

    public class AdnKomponenKaryawan : AdnBaseClass
    {
        public string nip { get; set; }
        public int Kd { get; set; }
        public decimal Jmh  { get; set; }

        public AdnKomponenKaryawan()
        {
            this.Jmh = 0;
        }
    }

    public class AdnGaji : AdnBaseClass
    {
        public Int64 KdGaji  { get; set; }
        public string Nip  { get; set; }
        public string Periode { get; set; }
        public string ThAjar { get; set; }
        public decimal Jmh { get; set; }
        public int Posting  { get; set; }

        public List<AdnKomponenKaryawan> DfKomponen { get; set; }

        public AdnGaji()
        {
            this.ThAjar = "";
            this.Jmh = 0;
            this.Posting = 0;
            this.DfKomponen = new List<AdnKomponenKaryawan>();
        }
    }

    public class AdnGajiVar
    {
        public string Periode  { get; set; }
        public string Nip { get; set; }
        public string KdJenis { get; set; }
        public decimal Jmh  { get; set; }

        public AdnGajiVar()
        {
            this.Jmh = 0;
        }
    }

    public class AdnKomponenJenis
    {
        public string KdJenis { get; set; }
        public string NmJenis  { get; set; }
        public int NoUrut   { get; set; }

        public AdnKomponenJenis()
        {
            this.NoUrut = 0;
        }
    }
    public class AdnAbsenTrMesin
    {
        public string NoKaryawan { get; set; }
        public string NmKaryawan { get; set; }
        public string Tgl { get; set; }
        public string WaktuKerja { get; set; }
        public string WaktuLembur { get; set; }

        public string Nip { get; set; }
    }


}
