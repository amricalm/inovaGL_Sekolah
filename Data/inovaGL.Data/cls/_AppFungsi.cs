using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using Andhana;

namespace inovaGL
{
    class AppFungsi
    {
        internal static void SetKoneksiApp()
        {
            string sDriver = ConfigurationManager.ConnectionStrings["KoneksiServer"].ToString();
            SqlConnection oCon = new SqlConnection(sDriver);
            oCon.Open();
            AppVar.AppConn = oCon;
        }

        internal static SqlConnection GetKoneksiSql()
        {
            return AppVar.AppConn;
        }

        internal static void SetPengguna(AdnScPengguna o)
        {
            AppVar.AppPengguna = o;
        }
        internal static void LoadSysVar()
        {
            //AppVar.PeriodeMulai =Convert.ToDateTime(AdnFungsi.GetSysVar(AppVar.AppConn,"periode_mulai"));
            //AppVar.DiskonPelanggan = AdnFungsi.CDec(AdnFungsi.GetSysVar(AppVar.AppConn, "diskon_pelanggan"));
            //AppVar.StrukHeader2 = AdnFungsi.GetSysVar(AppVar.AppConn, "struk_hdr2");
            //AppVar.StrukHeader3 = AdnFungsi.GetSysVar(AppVar.AppConn, "struk_hdr3");
        }

        internal static int KonversiNamaBulanToAngka(string NamaBulan)
        {
            int AngkaBulan = 0;
            switch (NamaBulan)
            {
                case "Jan":
                    AngkaBulan = 1;
                    break;
                case "Feb":
                    AngkaBulan = 2;
                    break;
                case "Mar":
                    AngkaBulan = 3;
                    break;
                case "Apr":
                    AngkaBulan = 4;
                    break;
                case "Mei":
                    AngkaBulan = 5;
                    break;
                case "Jun":
                    AngkaBulan = 6;
                    break;
                case "Jul":
                    AngkaBulan = 7;
                    break;
                case "Agt":
                    AngkaBulan = 8;
                    break;
                case "Sep":
                    AngkaBulan = 9;
                    break;
                case "Okt":
                    AngkaBulan = 10;
                    break;
                case "Nov":
                    AngkaBulan = 11;
                    break;
                case "Des":
                    AngkaBulan = 12;
                    break;
            }
            return AngkaBulan;
        }
    }
}
