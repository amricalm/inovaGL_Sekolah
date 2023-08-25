using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.KelasSiswa
{
    public class AdnKelasSiswaDao
    {
        private const short JUMLAH_KOLOM = 4;
        private const string NAMA_TABEL = "kelas_siswa";
        
        //private string pkey = "kelas";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnKelasSiswaDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnKelasSiswaDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnKelasSiswa o)
        {
            short idx = 0;

            fld[idx] = "kelas"; nilai[idx] = o.Kelas.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nis"; nilai[idx] = o.NIS.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnKelasSiswa o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
        }
        //public void Update(AdnKelasSiswa o)
        //{
        //    this.SetFldNilai(o);
        //    sWhere = this.pkey + "='" + o.Kelas + "' AND kd_sekolah = '" + o.KdSekolah + "'" ;
        //    sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

        //    try
        //    {
        //        cmd.CommandText = sql;
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (DbException exp)
        //    {
        //        AdnFungsi.LogErr(exp.Message);
        //    }
        //}
        public void Hapus(string KdSekolah, string ThAjar, string NIS, string Kelas)
        {

            sWhere = " kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
                + "     AND th_ajar ='" + ThAjar.ToString().Trim() + "'"
                + "     AND nis = '" + NIS.ToString().Trim() + "'"
                + "     AND kelas = '" + Kelas.ToString().Trim() + "'";

            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
        }

        public AdnKelasSiswa Get(string KdSekolah, string ThAjar, string Nis)
        {
            AdnKelasSiswa o = null;

            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND nis = '" + Nis.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if(rdr.Read())
                {
                    o = new AdnKelasSiswa();
                    o.Kelas= AdnFungsi.CStr(rdr["kelas"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.NIS = Nis;

                }
                rdr.Close();

                if (o != null)
                {
                    if (o.Kelas.Trim() != "")
                    {
                        o.oKelas = new Kelas.AdnKelasDao(this.cnn).Get(o.Kelas);
                    }
                }

            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public List<AdnKelasSiswa> GetList(string KdSekolah, string ThAjar, string Kelas)
        {
            List<AdnKelasSiswa> lst = new List<AdnKelasSiswa>();
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND kelas = '" + Kelas.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnKelasSiswa o = new AdnKelasSiswa();
                    o.Kelas = AdnFungsi.CStr(rdr["kelas"]) ;
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.NIS = AdnFungsi.CStr(rdr["nis"]);

                    lst.Add(o);
                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst; 
        }

        public List<int> GetKdSiswa(string KdSekolah, string ThAjar, string Kelas)
        {
            List<int> lst = new List<int>();
            sql =
            " select sis.kd_siswa "
            + " from " + NAMA_TABEL + " ks "
            + " INNER JOIN ms_siswa sis "
            + "     ON ks.kd_sekolah = sis.kd_sekolah "
            + "     AND ks.nis = sis.nis "
            + " WHERE ks.kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND ks.kelas = '" + Kelas.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    int kd = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    lst.Add(kd);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst;
        }
        

        public bool SetListBox(System.Windows.Forms.ListBox cbo, string KdSekolah, string ThAjar, string Kelas)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "nis";
            string KolomDisplay = "nama_lengkap";

            string Value = "NIS";
            string Display = "NmLengkap";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT ks." + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL + " ks "
            + " INNER JOIN ms_siswa ms "
            + "     ON ks.nis = ms.nis "
            + "     AND ks.kd_sekolah = ms.kd_sekolah "
            + " WHERE ks.kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND ks.kelas = '" + Kelas.ToString().Trim() + "'"
            + " ORDER BY " + KolomDisplay;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo, string KdSekolah, string ThAjar, string Kelas)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "nis";
            string KolomDisplay = "nama_lengkap";

            string Value = "NIS";
            string Display = "NmLengkap";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT ks." + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL + " ks "
            + " INNER JOIN ms_siswa ms "
            + "     ON ks.nis = ms.nis "
            + "     AND ks.kd_sekolah = ms.kd_sekolah "
            + " WHERE ks.kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND ks.kelas = '" + Kelas.ToString().Trim() + "'"
            + " ORDER BY " + KolomDisplay;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }

    }
}
