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
    public class AdnGudangDao
    {
        private const short JUMLAH_KOLOM = 2;
        private const string NAMA_TABEL = "im_mgudang";
        private string pkey = "kd_gudang";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnGudangDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnGudang o)
        {
            short idx = 0;
            fld[idx] = "kd_gudang"; nilai[idx] = o.kd_gudang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_gudang"; nilai[idx] = o.nm_gudang.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "alamat"; nilai[idx] = o.alamat.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "kota"; nilai[idx] = o.kota.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "pos"; nilai[idx] = o.pos.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "propinsi"; nilai[idx] = o.propinsi.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "telp"; nilai[idx] = o.telp.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "fax"; nilai[idx] = o.fax.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "email"; nilai[idx] = o.email.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "hp"; nilai[idx] = o.hp.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "cp"; nilai[idx] = o.cp.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnGudang o)
        {
            o.kd_gudang = AdnFungsi.GetKodeByPola(this.cnn, NAMA_TABEL, pkey, "000");

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe, o.uid);
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
        public void Update(AdnGudang o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.kd_gudang.ToString() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere, o.uid_edit);

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
        public AdnGudang Get(string kd)
        {
            AdnGudang o = null;
            string sql =
            " select kd_gudang,nm_gudang,alamat,kota,pos,propinsi,telp,fax,email,hp,cp, uid, tgl_tambah,uid_edit, tgl_edit "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnGudang();
                    o.kd_gudang = Convert.ToString(rdr["kd_gudang"]).Trim();
                    o.nm_gudang = Convert.ToString(rdr["nm_gudang"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim();
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.hp = Convert.ToString(rdr["hp"]).Trim();
                    o.cp = Convert.ToString(rdr["cp"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = AdnFungsi.CDate(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = AdnFungsi.CDate(rdr["tgl_edit"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnGudang> GetAll()
        {
            List<AdnGudang> lst = new List<AdnGudang>();
            string sql =
            " select kd_gudang, nm_gudang, uid, tgl_tambah,uid_edit, tgl_edit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnGudang o = new AdnGudang();
                    o.kd_gudang = Convert.ToString(rdr["kd_gudang"]).Trim();
                    o.nm_gudang = Convert.ToString(rdr["nm_gudang"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = AdnFungsi.CDate(rdr["tgl_tambah"]);
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
        public DataTable GetByArgs(string kd_gudang)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("kd_gudang", typeof(String));
            tbl.Columns.Add("nm_gudang", typeof(String));

            string sql =
            " select kd_gudang, nm_gudang "
            + " from " + NAMA_TABEL + "";

            if (kd_gudang.Trim() != "")
            {
                sql = sql + " WHERE kd_gudang ='" + kd_gudang.Trim() + "'";
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["kd_gudang"] = Convert.ToString(rdr["kd_gudang"]).Trim();
                baris["nm_gudang"] = Convert.ToString(rdr["nm_gudang"]).Trim();
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_gudang";
            string KolomDisplay = "nm_gudang";

            string Value = "kd_gudang";
            string Display = "nm_gudang";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
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
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }


    }
}
