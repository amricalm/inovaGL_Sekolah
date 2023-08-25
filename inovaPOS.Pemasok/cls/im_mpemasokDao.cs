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
    public class AdnPemasokDao
    {
        private const short JUMLAH_KOLOM = 10;
        private const string NAMA_TABEL = "im_mpemasok";
        private string pkey = "kd_ps";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnPemasokDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnPemasok o)
        {
            short idx = 0;

            fld[idx] = "kd_ps"; nilai[idx] = o.kd_ps.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_ps"; nilai[idx] = o.nm_ps.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "alamat"; nilai[idx] = o.alamat.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kota"; nilai[idx] = o.kota.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "pos"; nilai[idx] = o.pos.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "propinsi"; nilai[idx] = o.propinsi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "telp"; nilai[idx] = o.telp.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "fax"; nilai[idx] = o.fax.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "email"; nilai[idx] = o.email.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "web"; nilai[idx] = o.web.ToString(); tipe[idx] = "s"; idx++;
 
        }

        public void Simpan(AdnPemasok o)
        {
            o.kd_ps = AdnFungsi.GetKodeByPola(this.cnn,NAMA_TABEL,pkey,"000000");
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,o.uid);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.ErrorCode.ToString() + "; " + exp.Message);
            }
        }
        public void Update(AdnPemasok o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.kd_ps.Trim() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere);

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
        public AdnPemasok Get(string kd)
        {
            AdnPemasok o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {   
                    o = new AdnPemasok();
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]).Trim();
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim(); ;
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.web = Convert.ToString(rdr["web"]).Trim();
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
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnPemasok> GetAll()
        {
            List<AdnPemasok> lst = new List<AdnPemasok>();
            string sql =
            " select kd_ps,nm_ps, alamat, kota, pos, propinsi, telp, fax, email, web, cp, uid, tgl_tambah, uid_edit, tgl_edit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPemasok o = new AdnPemasok();
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]).Trim();
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim(); ;
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.web = Convert.ToString(rdr["web"]).Trim();
                    o.cp = Convert.ToString(rdr["cp"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = AdnFungsi.CDate(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit =AdnFungsi.CDate(rdr["tgl_edit"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return lst;
        }
        public List<AdnPemasok> GetByPs()
        {
            this.cnn.Open();
            string sql;
            List<AdnPemasok> lst = new List<AdnPemasok>();
            //if (kd != "")
            //{
            //    sql =
            //    " select kd_ps,nm_ps, alamat, cp ,uid,tgl_tambah,uid_edit, tgl_edit "
            //    + " from " + NAMA_TABEL + " where kd_ps = " + kd;
            //}
            //else
            //{
                sql =
                " select kd_ps,nm_ps, telp, cp ,uid,tgl_tambah,uid_edit, tgl_edit "
                + " from " + NAMA_TABEL;
            //}
            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPemasok o = new AdnPemasok();
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]).Trim();
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.cp = Convert.ToString(rdr["cp"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = Convert.ToDateTime(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = Convert.ToDateTime(rdr["tgl_edit"]);
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

        public DataTable GetDfBarangPembelian(string KdPemasok)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("barcode", typeof(String));
            tbl.Columns.Add("KdBarangPemasok", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));


            string sql =
            " select hdr.kd_barang, barcode, nm_barang, kd_barang_pemasok"
            + " from im_mbarang hdr"
            + " INNER JOIN im_mbarang_pemasok pms "
            + "     ON  hdr.kd_barang = pms.kd_barang ";

            if (KdPemasok != "")
            {
                sql = sql + " WHERE pms.kd_ps = '" + KdPemasok.ToString().Trim() + "'";
            }

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                    row["barcode"] = Convert.ToString(rdr["barcode"]).Trim();
                    row["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                    row["KdBarangPemasok"] = Convert.ToString(rdr["kd_barang_pemasok"]).Trim();
                    tbl.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return tbl;
        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_ps";
            string KolomDisplay = "nm_ps";

            string Value = "kd_ps";
            string Display = "nm_ps";

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
