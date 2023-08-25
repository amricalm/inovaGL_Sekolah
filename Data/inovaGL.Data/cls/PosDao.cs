using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL.Data
{
    public class AdnPosDao
    {
        private const short JUMLAH_KOLOM = 3;
        private const string NAMA_TABEL = "ac_mpos";
        private string pkey = "kd_pos";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private SqlTransaction trn;
        private AdnScPengguna pengguna;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnPosDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnPosDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnPosDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;

        }

        private void SetFldNilai(AdnPos o)
        {
            short idx = 0;
            fld[idx] = "kd_pos"; nilai[idx] = o.KdPos.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_pos"; nilai[idx] = o.NmPos.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_dept"; nilai[idx] = o.KdDept.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnPos o)
        {
            //o.KdPos = AdnFungsi.GetKodeByPola(this.cnn, NAMA_TABEL, pkey, "000");

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            //try
            //{
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                foreach (AdnPosDtl item in o.ItemDf)
                {
                    new AdnPosDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
                }

            //}
            //catch (DbException exp)
            //{
            //    throw new Exception(exp.Message.ToString());
            //}
        }
        public void Update(AdnPos o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdPos.ToString() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            //try
            //{
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                new AdnPosDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.KdPos);

                foreach (AdnPosDtl item in o.ItemDf)
                {
                    new AdnPosDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
                }
            //}
        }
        public void Hapus(string kd)
        {
            sWhere = this.pkey + "='" + kd.Trim() + "'";
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            try
            {
                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
        public AdnPos Get(string kd)
        {
            AdnPos o = null;
            string sql =
            " select kd_pos,nm_pos, kd_dept"
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnPos();
                    o.KdPos = Convert.ToString(rdr["kd_pos"]).Trim();
                    o.NmPos = Convert.ToString(rdr["nm_pos"]).Trim();
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    o.ItemDf = new List<AdnPosDtl>();
                }
                rdr.Close();
                if (o != null)
                {
                    o.ItemDf = new AdnPosDtlDao(this.cnn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnPos> GetAll()
        {
            List<AdnPos> lst = new List<AdnPos>();
            string sql =
            " select kd_pos, nm_pos,kd_dept "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPos o = new AdnPos();
                    o.KdPos = Convert.ToString(rdr["kd_pos"]).Trim();
                    o.NmPos = Convert.ToString(rdr["nm_pos"]).Trim();
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return lst;
        }
        public DataTable GetByDept(string KdDept)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdPos", typeof(String));
            tbl.Columns.Add("NmPos", typeof(String));
            tbl.Columns.Add("KdDept", typeof(String));

            string sql =
            " select kd_pos, nm_pos, kd_dept "
            + " from " + NAMA_TABEL + "";

            if (KdDept.Trim() != "")
            {
                sql = sql + " WHERE kd_dept ='" + KdDept.Trim() + "'";
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdPos"] = Convert.ToString(rdr["kd_pos"]).Trim();
                baris["NmPos"] = Convert.ToString(rdr["nm_pos"]).Trim();
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }
        public DataTable GetByArgs(string kd_pos)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("kd_pos", typeof(String));
            tbl.Columns.Add("nm_pos", typeof(String));
            tbl.Columns.Add("kd_dept", typeof(String));

            string sql =
            " select kd_pos, nm_pos, kd_dept "
            + " from " + NAMA_TABEL + "";

            if (kd_pos.Trim() != "")
            {
                sql = sql + " WHERE kd_pos ='" + kd_pos.Trim() + "'";
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["kd_pos"] = Convert.ToString(rdr["kd_pos"]).Trim();
                baris["nm_pos"] = Convert.ToString(rdr["nm_pos"]).Trim();
                baris["kd_dept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_pos";
            string KolomDisplay = "nm_pos";

            string Value = "KdPos";
            string Display = "NmPos";

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

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetCombo(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_pos";
            string KolomDisplay = "nm_pos";

            string Value = "KdPos";
            string Display = "NmPos";

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

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
    }
}
