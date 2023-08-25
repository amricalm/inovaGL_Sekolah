using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Security.Cryptography;
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
            AppVar.PeriodeMulai = Convert.ToDateTime(AdnFungsi.GetSysVar(AppVar.AppConn, "periode_mulai"));
            AppVar.KdSekolah = Convert.ToString(AdnFungsi.GetSysVar(AppVar.AppConn, "sid"));
            AppVar.ThAjar = Convert.ToString(AdnFungsi.GetSysVar(AppVar.AppConn, "sth"));
            AppVar.ModeTunai = Convert.ToInt16(AdnFungsi.GetSysVar(AppVar.AppConn, "mode_tunai"));
            AppVar.KdAkunGaji = Convert.ToString(AdnFungsi.GetSysVar(AppVar.AppConn, "kd_akun_gaji"));
            AppVar.KdAkunPenyeimbangSAW = AdnFungsi.CStr(AdnFungsi.GetSysVar(AppVar.AppConn, "akun_penyeimbang_saw"));
            
            AppVar.KdAkunLabaDitahan = AdnFungsi.CStr(AdnFungsi.GetSysVar(AppVar.AppConn, "LabaDitahan"));
            AppVar.KdAkunLabaTahunBerjalan = AdnFungsi.CStr(AdnFungsi.GetSysVar(AppVar.AppConn, "LabaThBerjalan"));
            AppVar.KdAkunIkhtisarLabaRugi = AdnFungsi.CStr(AdnFungsi.GetSysVar(AppVar.AppConn, "IkhtisarLR"));
            AppVar.KdAkunUangMukaSiswa = AdnFungsi.CStr(AdnFungsi.GetSysVar(AppVar.AppConn, "um_siswa"));
            
            //AppVar.DiskonPelanggan = AdnFungsi.CDec(AdnFungsi.GetSysVar(AppVar.AppConn, "diskon_pelanggan"));
            //AppVar.StrukHeader2 = AdnFungsi.GetSysVar(AppVar.AppConn, "struk_hdr2");
            //AppVar.StrukHeader3 = AdnFungsi.GetSysVar(AppVar.AppConn, "struk_hdr3");

            string Pelanggan = ConfigurationManager.AppSettings["EdusisPelanggan"].ToString();
            string EdusisModel = ConfigurationManager.AppSettings["EdusisModel"].ToString();
            //string acak = AppFungsi.Encrypt("YPI Mutiara Bangsa", "#Cibubur0104#123");
            AppVar.Organisasi = AppFungsi.Decrypt(Pelanggan, "#Cibubur0104#123");

            inovaGL.Data.AdnLaporanModel.EdusisModel = AppFungsi.Decrypt(EdusisModel, "#Cibubur0104#123");

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
        internal static string KonversiAngkaToNamaBulan(int Bulan)
        {
            string NamaBulan = "";
            switch (Bulan)
            {
                case 1:
                    NamaBulan = "Jan";
                    break;
                case 2:
                    NamaBulan = "Feb";
                    break;
                case 3:
                    NamaBulan = "Mar";
                    break;
                case 4:
                    NamaBulan = "Apr";
                    break;
                case 5:
                    NamaBulan = "Mei";
                    break;
                case 6:
                    NamaBulan = "Jun";
                    break;
                case 7:
                    NamaBulan = "Jul";
                    break;
                case 8:
                    NamaBulan = "Agt";
                    break;
                case 9:
                    NamaBulan = "Sep";
                    break;
                case 10:
                    NamaBulan = "Okt";
                    break;
                case 11:
                    NamaBulan = "Nov";
                    break;
                case 12:
                    NamaBulan = "Des";
                    break;
            }
            return NamaBulan;
        }
        public static string Encrypt(string input, string key)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        public static string Decrypt(string input, string key)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(key);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
    }
}
