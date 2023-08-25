using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace inovaGL.Definisi
{
    public class AdnVar
    {

        public struct ModeEdit
        {
            public const int BACA = 1;
            public const int BARU = 2;
            public const int UBAH = 3;
        }

        public struct TipeJurnal
        {
            public const string KAS_MASUK = "KM";
            public const string KAS_KELUAR = "KK";
            public const string JURNAL_UMUM = "JU";
        }

        public struct SaldoNormal
        {
            public const string DEBET = "DEBET";
            public const string KREDIT = "KREDIT";
        }

        public struct JenisAkun
        {
            public const string KAS_BANK = "C/B";
        }

        public struct Klasifikasi
        {
            public const string TOTAL = "TOTAL";
            public const string DETAIL = "DTL";
        }

        public struct IdentitasPengguna
        {
            public const string REPORT_HD1 = "Badan Wakaf Al-Qur'an";
        }
       
    }

    public class AdnTreeItem
    {
        public string Kode;
        public string Nama;
        public int Tingkat;
        public AdnTreeItem(string Kode,string Nama, int Tingkat)
        {
            this.Kode = Kode;
            this.Nama = Nama; 
            this.Tingkat = Tingkat;
        }
    }
}
