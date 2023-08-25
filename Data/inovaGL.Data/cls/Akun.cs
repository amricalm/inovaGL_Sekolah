using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnAkunRef
    {
        public struct GolonganAkun
        {
            public const string PIUTANG = "AR";
            public const string KEWAJIBAN = "AP";
            public const string PENDAPATAN = "PDT";
        }
    }

    public class AdnAkun : AdnBaseClass
    {
        public string KdAkun { get; set; }
        public string NmAkun { get; set; }
        public string DK { get; set; }
        public string KdInduk {get;set;}
        public int Turunan { get; set; }
        public string Tipe { get; set; }
        public string KdGolongan { get; set; }
        public string KdDept { get; set; }
        public bool TampilDiLoket { get; set; }
        public bool Aktif { get; set; }
        public decimal Saldo { get; set; }
        public decimal SaldoAwal { get; set; }
        public decimal Mutasi { get; set; }
        public decimal SaldoAwalDebet { get; set; }
        public decimal SaldoAwalKredit { get; set; }
        public decimal MutasiDebet { get; set; }
        public decimal MutasiKredit { get; set; }
        public decimal SaldoDebet { get; set; }
        public decimal SaldoKredit { get; set; }

        public AdnAkun()
        {
            this.KdDept = "";
            this.Aktif = true;
            this.Saldo = 0;
            this.SaldoAwal = 0;
            this.Mutasi = 0;
            this.SaldoAwalDebet = 0;
            this.SaldoAwalKredit = 0;
            this.MutasiDebet = 0;
            this.MutasiKredit = 0;
            this.SaldoDebet = 0;
            this.SaldoKredit = 0;

            this.TampilDiLoket = false;
        }
    }

}
