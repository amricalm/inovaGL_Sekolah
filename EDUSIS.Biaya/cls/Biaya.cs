using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace EDUSIS.Biaya
{
    //public static class AdnVar 
    //{
        
    //}

    public class AdnBiaya : AdnBaseClass
    {
        public string KdBiaya { get; set; }
        public string NmBiaya { get; set; }
        public string KdJenis { get; set; }
        public string Keterangan { get; set; }
        public bool Gabungan { get; set; }
        public bool LaporanRutin { get; set; }
        public bool LaporanPSB { get; set; }
        public bool TidakDijurnal { get; set; }
        public string KdAkunPiutang { get; set; }
        public string KdAkunPendapatan{ get; set; }
        public string KdAkunKewajiban { get; set; }
        public string KdAkunDeposit { get; set; }
        
        public struct JenisBiaya
        {
            public const string ASRAMA = "ASR";
            public const string BULANAN = "BL";
            public const string JEMPUTAN = "JPT";
            public const string KEGIATAN = "KBM";
            public const string PSB = "PSB";
            public const string SPP = "SPP";
            public const string TAHUNAN = "THN";
            public const string PENGEMBANGAN = "PGB";
            public const string CATERING = "CTG";
            public const string OPERASIONAL = "OPR";

            //BIAYA BISA DICICIL/ TIDAK TETAP

            public const string ASRAMA_CICIL = "ASR_CICIL";
            public const string BULANAN_CICIL = "BL_CICIL";
            public const string JEMPUTAN_CICIL = "JPT_CICIL";
            public const string KEGIATAN_CICIL = "KGT_CICIL";
            public const string PSB_CICIL = "PSB_CICIL";
            public const string SPP_CICIL = "SPP_CICIL";
            public const string TAHUNAN_CICIL = "THN_CICIL";
            public const string PENGEMBANGAN_CICIL = "PGB_CICIL";
            public const string CATERING_CICIL = "CTG_CICIL";
            public const string OPERASIONAL_CICIL = "OPR_CICIL";
        }
        public AdnBiaya()
        {
            this.KdJenis = "";
            this.Keterangan = "";
            this.Gabungan = false;
            this.LaporanPSB = false;
            this.LaporanPSB = true;

            this.TidakDijurnal = false;

            this.KdAkunPiutang = "";
            this.KdAkunPendapatan = "";
            this.KdAkunKewajiban = "";
            this.KdAkunDeposit = "";
        }



    }
    public class AdnBiayaSekolah : AdnBaseClass
    {
        public string KdBiaya { get; set; }
        public string KdSekolah { get; set; }
        public string ThAjar { get; set; }
        public string Tingkat { get; set; }
        public decimal Jmh { get; set; }

        public AdnBiaya oBiaya { get; set; }
    }
}
