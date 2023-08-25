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
    public class AdnSysAkunGolonganDao
    {
        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "ac_sys_gol_akun";
        
        private string pkey = "kd_gol";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnSysAkunGolonganDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnSysAkunGolonganDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnSysAkunGolongan o)
        {
            short idx = 0;

            fld[idx] = "kd_gol"; nilai[idx] = o.KdGolongan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_gol"; nilai[idx] = o.NmGolongan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tipe"; nilai[idx] = o.Tipe.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnSysAkunGolongan o)
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
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Update(AdnSysAkunGolongan o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdGolongan+ "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
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
                throw new Exception(exp.Message.ToString());
            }
        }

        public AdnSysAkunGolongan Get(string kd)
        {
            AdnSysAkunGolongan o = null;
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
                    o = new AdnSysAkunGolongan();
                    o.KdGolongan = kd;
                    o.NmGolongan = AdnFungsi.CStr(rdr["nm_gol"]);
                    o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnSysAkunGolongan> GetAll()
        {
            List<AdnSysAkunGolongan> lst = new List<AdnSysAkunGolongan>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSysAkunGolongan o = new AdnSysAkunGolongan();
                    o.KdGolongan= AdnFungsi.CStr(rdr["kd_gol"]);
                    o.NmGolongan = AdnFungsi.CStr(rdr["nm_gol"]);
                    o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            return lst; 
        }

        public DataTable GetDf()
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdGolongan", typeof(String));
            tbl.Columns.Add("NmGolongan", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));

            string sql =
            " select kd_gol, nm_gol, tipe "
            + " from " + NAMA_TABEL;

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdGolongan"] = AdnFungsi.CStr(rdr["kd_gol"]);
                baris["NmGolongan"] = AdnFungsi.CStr(rdr["nm_gol"]);
                baris["Tipe"] = AdnFungsi.CStr(rdr["tipe"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_gol";
            string KolomDisplay = "nm_gol";

            string Value = "KdGolongan";
            string Display = "NmGolongan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY no_urut";

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

            string KolomValue = "kd_gol";
            string KolomDisplay = "nm_gol";

            string Value = "KdGolongan";
            string Display = "NmGolongan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY no_urut";

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
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboByJenis(System.Windows.Forms.ComboBox cbo, string KdJenis)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_gol";
            string KolomDisplay = "nm_gol";

            string Value = "KdGolongan";
            string Display = "NmGolongan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_jenis ='" + KdJenis.ToString().Trim() + "'"
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
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_gol";
            string KolomDisplay = "nm_gol";

            string Value = "KdGolongan";
            string Display = "NmGolongan";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY " + KolomValue;

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

            cbo.DataPropertyName = "KdGolongan";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }
        
    
    }
}
