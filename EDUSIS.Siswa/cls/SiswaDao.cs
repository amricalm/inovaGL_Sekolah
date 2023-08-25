using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.Siswa
{
    public class AdnSiswaDao
    {
        private const short JUMLAH_KOLOM = 6;
        private const string NAMA_TABEL = "ms_siswa";
        
        private string pkey = "kd_siswa";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnSiswaDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnSiswaDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnSiswa o)
        {
            short idx = 0;

            fld[idx] = "kd_siswa"; nilai[idx] = o.KdSiswa.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nama_lengkap"; nilai[idx] = o.NmLengkap.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nis"; nilai[idx] = o.NIS.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nisn"; nilai[idx] = o.NISN.ToString(); tipe[idx] = "s"; idx++;

            fld[idx] = "ayah_nama"; nilai[idx] = o.AyahNama.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnSiswa o)
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
        public void Update(AdnSiswa o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "=" + o.KdSiswa ;
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

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
        public void Hapus(Int64 kd)
        {

            sWhere = this.pkey + "=" + kd ;
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

        public AdnSiswa Get(Int64 kd)
        {
            AdnSiswa o = null;
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + "=" + kd ;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSiswa();
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NmLengkap = AdnFungsi.CStr(rdr["nama_lengkap"]);
                    o.NIS = AdnFungsi.CStr(rdr["nis"]);
                    o.NISN = AdnFungsi.CStr(rdr["nisn"]);
                    o.AyahNama = AdnFungsi.CStr(rdr["ayah_nama"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public AdnSiswa Get(string Nis, string KdSekolah)
        {
            AdnSiswa o = null;
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where nis ='" + Nis + "' and kd_sekolah ='" + KdSekolah + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSiswa();
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NmLengkap = AdnFungsi.CStr(rdr["nama_lengkap"]);
                    o.NIS = AdnFungsi.CStr(rdr["nis"]);
                    o.NISN = AdnFungsi.CStr(rdr["nisn"]);
                    o.AyahNama = AdnFungsi.CStr(rdr["ayah_nama"]);

                    //o.NoVA = AdnFungsi.CStr(rdr["NoVA"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public AdnSiswa GetByNoVA(string NoVA)
        {
            AdnSiswa o = null;
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where no_va ='" + NoVA.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSiswa();
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NmLengkap = AdnFungsi.CStr(rdr["nama_lengkap"]);
                    o.NIS = AdnFungsi.CStr(rdr["nis"]);
                    o.NISN = AdnFungsi.CStr(rdr["nisn"]);
                    o.AyahNama = AdnFungsi.CStr(rdr["ayah_nama"]);

                    o.NoVA = AdnFungsi.CStr(rdr["NoVA"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }

        public List<AdnSiswa> GetAll(string KdSekolah)
        {
            List<AdnSiswa> lst = new List<AdnSiswa>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            if (KdSekolah.ToString().Trim() != "")
            {
                sql += " WHERE kd_sekolah = '" + KdSekolah.ToString().Trim() + "'";
            }

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSiswa o = new AdnSiswa();
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NmLengkap = AdnFungsi.CStr(rdr["nama_lengkap"]);
                    o.NIS = AdnFungsi.CStr(rdr["nis"]);
                    o.NISN = AdnFungsi.CStr(rdr["nisn"]);

                    o.AyahNama = AdnFungsi.CStr(rdr["ayah_nama"]);
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

        public bool SetCombo(System.Windows.Forms.ComboBox cbo, string KdSekolah)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_siswa";
            string KolomDisplay = "nama_lengkap";

            string Value = "KdSiswa";
            string Display = "NmLengkap";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue  + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "'"
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
        public bool SetComboDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo,string KdSekolah, bool TambahBarisKosong)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_siswa";
            string KolomDisplay = "nama_lengkap";

            string Value = "KdSiswa";
            string Display = "NmLengkap";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue 
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "'"
            + " ORDER BY " + KolomDisplay;

            if (TambahBarisKosong)
            {
                row = lst.NewRow();
                row[Value] = "";
                row[Display] = "";
                lst.Rows.Add(row);
            }

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

            cbo.DataPropertyName = "KdSiswa";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

        public int GetKdSiswa(string Nis, string KdSekolah)
        {
            int Kd = 0;

            sql =
            " select kd_siswa "
            + " from " + NAMA_TABEL
            + " where nis ='" + Nis + "' AND kd_sekolah = '" + KdSekolah + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    Kd = AdnFungsi.CInt(rdr["kd_siswa"], true);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return Kd;
        } 
    }
}
