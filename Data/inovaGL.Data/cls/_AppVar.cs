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
        public const string Organisasi = "Badan Wakaf Al-Qur'an";
        public static SqlConnection AppConn = null;
        public static AdnScPengguna AppPengguna = null;
        public const string AppName = "inovaGL-Keuangan";
        public const string CaptionDialogBox = "inovaGL-Keuangan";

        public const string ReportExt = "rdlc";
        public const string ReportPath = "rpt";

        public static DateTime PeriodeMulai;
        public static decimal DiskonPelanggan;

        //Struk
        public static string StrukHeader2;
        public static string StrukHeader3;
    }
}
