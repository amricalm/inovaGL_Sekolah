using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Andhana;

namespace inovaGL
{
    class AppVar
    {
        public static string Organisasi ="";// "Madina Islamic School";//Yayasan Amal Mulia";
        public static SqlConnection AppConn = null;
        public static AdnScPengguna AppPengguna = null;
        public const string AppName = "inovaGL-Keuangan";
        public const string CaptionDialogBox = "inovaGL-Keuangan";

        public const string ReportExt = "rdlc";
        public const string ReportPath = "rpt";

        public const string LapNeraca = "NRC";
        public const string LapLR = "LR";

        public static DateTime PeriodeMulai;
        public static string KdSekolah;
        public static string ThAjar;

        public static int ModeTunai = 1;// Cash Basis (default) or Acrual Basis
        public static string KdAkunGaji;

        public static string KdAkunPenyeimbangSAW = "";
        public static string KdAkunLabaDitahan = "";
        public static string KdAkunLabaTahunBerjalan = "";
        public static string KdAkunIkhtisarLabaRugi = "";
        public static string KdAkunUangMukaSiswa = "";

        public static int BKKNoBuktiAuto = 1;
        public static int BKMNoBuktiAuto = 1;
        public static int BJUNoBuktiAuto = 1;

        //public static decimal DiskonPelanggan;

        //Struk
        //public static string StrukHeader2;
        //public static string StrukHeader3;
    }


}
