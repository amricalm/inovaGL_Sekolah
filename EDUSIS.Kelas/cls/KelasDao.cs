using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.Kelas
{
    public class AdnKelasDao
    {
        private const short JUMLAH_KOLOM = 4;
        private const string NAMA_TABEL = "ms_kelas";
        
        private string pkey = "kelas";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnKelasDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnKelasDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnKelas o)
        {
            short idx = 0;

            fld[idx] = "kelas"; nilai[idx] = o.Kelas.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_jurusan"; nilai[idx] = o.KdJurusan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tingkat"; nilai[idx] = o.Tingkat.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnKelas o)
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
        public void Update(AdnKelas o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.Kelas + "' AND kd_sekolah = '" + o.KdSekolah + "'" ;
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
        public void Hapus(string kd)
        {

            sWhere = this.pkey + "='" + kd + "'";
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

        public AdnKelas Get(string Kd)
        {
            AdnKelas o = null;

            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + "='" + Kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnKelas();
                    o.Kelas = AdnFungsi.CStr(rdr["kelas"]);
                    o.KdJurusan = AdnFungsi.CStr(rdr["kd_jurusan"]);
                    o.Tingkat = AdnFungsi.CStr(rdr["tingkat"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public List<AdnKelas> GetBySekolah(string KdSekolah)
        {
            List<AdnKelas> lst = new List<AdnKelas>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            if (KdSekolah != "")
            {
                sql +=" where kd_sekolah ='" + KdSekolah.ToString().Trim() +"'";
            }

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnKelas o = new AdnKelas();
                    o.Kelas = AdnFungsi.CStr(rdr["kelas"]) ;
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.KdJurusan = AdnFungsi.CStr(rdr["kd_jurusan"]);
                    o.Tingkat = AdnFungsi.CStr(rdr["tingkat"]);

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

            string KolomValue = "kelas";
            //string KolomDisplay = "kelas";

            string Value = "Kelas";
            //string Display = "Kelas";

            lst.Columns.Add(Value, typeof(string));
            //lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue 
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "'"
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    //row[Display] = AdnFungsi.CStr(rdr[KolomValue]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            cbo.DisplayMember = Value;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo,string KdSekolah, bool TambahBarisKosong)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kelas";
            //string KolomDisplay = "kd_sekolah";

            string Value = "Kelas";
            //string Display = "Kelas";

            lst.Columns.Add(Value, typeof(string));
            //lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue 
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "'"
            + " ORDER BY " + KolomValue;

            if (TambahBarisKosong)
            {
                row = lst.NewRow();
                row[Value] = "";
                //row[Display] = "";
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
                    //row[Display] = AdnFungsi.CStr(rdr[KolomValue]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            cbo.DataPropertyName = "Kelas";
            cbo.DisplayMember = Value;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

    }
}
