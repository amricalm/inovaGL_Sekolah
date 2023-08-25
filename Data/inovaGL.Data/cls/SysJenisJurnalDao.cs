using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;
using Andhana;

namespace inovaGL.Data
{
    public class AdnSysJenisJurnalDao
    {
        private const short JUMLAH_KOLOM = 2;
        private const string NAMA_TABEL = "ac_sys_jn_jurnal";
        
        private string pkey = "jn_jurnal";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnSysJenisJurnalDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnSysJenisJurnalDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnSysJenisJurnal o)
        {
            short idx = 0;

            fld[idx] = "jn_jurnal"; nilai[idx] = o.JenisJurnal.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;

        }

        public void Simpan(AdnSysJenisJurnal o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();               
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
        }
        public void Update(AdnSysJenisJurnal o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.JenisJurnal+ "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
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
                AdnFungsi.LogErr(exp.Message.ToString());
            }
        }

        public AdnSysJenisJurnal Get(string kd)
        {
            AdnSysJenisJurnal o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSysJenisJurnal();
                    o.JenisJurnal = kd;
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnSysJenisJurnal> GetAll()
        {
            List<AdnSysJenisJurnal> lst = new List<AdnSysJenisJurnal>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSysJenisJurnal o = new AdnSysJenisJurnal();
                    o.JenisJurnal= AdnFungsi.CStr(rdr["jn_jurnal"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return lst; 
        }

        public DataTable GetDf()
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("JenisJurnal", typeof(String));
            tbl.Columns.Add("Keterangan", typeof(String));

            string sql =
            " select jn_jurnal, ket "
            + " from " + NAMA_TABEL;

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["JenisJurnal"] = AdnFungsi.CStr(rdr["jn_jurnal"]);
                baris["Keterangan"] = AdnFungsi.CStr(rdr["ket"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "jn_jurnal";
            string KolomDisplay = "ket";

            string Value = "JenisJurnal";
            string Display = "Keterangan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY jn_jurnal ";

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
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo, bool TambahBarisSemua)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "jn_jurnal";
            string KolomDisplay = "ket";

            string Value = "JenisJurnal";
            string Display = "Keterangan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY jn_jurnal";

            if (TambahBarisSemua)
            {
                row = lst.NewRow();
                row[Value] = "";
                row[Display] = "-- SEMUA --";
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
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboJurnalUmum(System.Windows.Forms.ComboBox cbo, bool TambahBarisSemua)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "jn_jurnal";
            string KolomDisplay = "ket";

            string Value = "JenisJurnal";
            string Display = "Keterangan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE jurnal_umum = 1 "
            + " ORDER BY jn_jurnal";

            if (TambahBarisSemua)
            {
                row = lst.NewRow();
                row[Value] = "";
                row[Display] = "-- SEMUA --";
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
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboJurnalSiswa(System.Windows.Forms.ComboBox cbo, bool TambahBarisSemua)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "jn_jurnal";
            string KolomDisplay = "ket";

            string Value = "JenisJurnal";
            string Display = "Keterangan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE jurnal_siswa = 1 "
            + " ORDER BY jn_jurnal";

            if (TambahBarisSemua)
            {
                row = lst.NewRow();
                row[Value] = "";
                row[Display] = "-- SEMUA --";
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
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }      
    }
}
