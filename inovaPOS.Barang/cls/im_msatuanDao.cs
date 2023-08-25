using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaPOS
{
    public class AdnSatuanDao
    {
        private const short JUMLAH_KOLOM = 2;
        private const string NAMA_TABEL = "im_msatuan";
        private string pkey = "kd_satuan";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnSatuanDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnSatuan o)
        {
            short idx = 0;
            fld[idx] = "kd_satuan"; nilai[idx] = o.kd_satuan.ToString(); tipe[idx] = "s"; idx++; 
            fld[idx] = "nm_satuan"; nilai[idx] = o.nm_satuan.ToString(); tipe[idx] = "s"; idx++;     
        }

        public void Simpan(AdnSatuan o)
        {
            o.kd_satuan = AdnFungsi.GetKodeByPola(this.cnn, NAMA_TABEL, pkey,"000");
  
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,o.uid);
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
        public void Update(AdnSatuan o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.kd_satuan.ToString() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,o.uid_edit);

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
        public void Hapus(string kd)
        {
            sWhere = this.pkey + "='" + kd.Trim() + "'";
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL,sWhere);
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
        public AdnSatuan Get(string kd)
        {
            AdnSatuan o = null;
            string sql =
            " select kd_satuan,nm_satuan, uid, tgl_tambah,uid_edit, tgl_edit "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSatuan();
                    o.kd_satuan = Convert.ToString(rdr["kd_satuan"]).Trim();
                    o.nm_satuan = Convert.ToString(rdr["nm_satuan"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = AdnFungsi.CDate(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = AdnFungsi.CDate(rdr["tgl_edit"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnSatuan> GetAll()
        {
            List<AdnSatuan> lst = new List<AdnSatuan>();
            string sql =
            " select kd_satuan, nm_satuan, uid, tgl_tambah,uid_edit, tgl_edit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSatuan o = new AdnSatuan();
                    o.kd_satuan = Convert.ToString(rdr["kd_satuan"]).Trim();
                    o.nm_satuan = Convert.ToString(rdr["nm_satuan"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah =AdnFungsi.CDate(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = AdnFungsi.CDate(rdr["tgl_edit"]);
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

        public bool SetComboDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_satuan";
            string KolomDisplay = "nm_satuan";

            string Value = "kd_satuan";
            string Display = "nm_satuan";

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

            cbo.DataPropertyName = "kd_satuan";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }
    }
}
