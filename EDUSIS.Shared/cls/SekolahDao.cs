using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.Shared
{
    public class AdnSekolahDao
    {
        private const short JUMLAH_KOLOM = 15;
        private const string NAMA_TABEL = "ms_sekolah";
        
        private string pkey = "kd_sekolah";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnSekolahDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnSekolahDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnSekolah o)
        {
            short idx = 0;

            fld[idx] = "nama_sekolah"; nilai[idx] = o.NmSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nss"; nilai[idx] = o.NSS.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tingkat"; nilai[idx] = o.Tingkat.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "alamat"; nilai[idx] = o.Alamat.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kelurahan"; nilai[idx] = o.Kelurahan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "pos"; nilai[idx] = o.KdPos.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kecamatan"; nilai[idx] = o.Kecamatan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kabupaten"; nilai[idx] = o.Kabupaten.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "propinsi"; nilai[idx] = o.Propinsi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "telp"; nilai[idx] = o.Telp.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "fax"; nilai[idx] = o.Fax.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "email"; nilai[idx] = o.Email.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "sms"; nilai[idx] = o.SMS.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "web"; nilai[idx] = o.Web.ToString(); tipe[idx] = "s"; idx++;
            
        }

        public void Simpan(AdnSekolah o)
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
        public void Update(AdnSekolah o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdSekolah+ "' AND kd_sekolah = '" + o.KdSekolah + "'" ;
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

        public AdnSekolah Get(string kd)
        {
            AdnSekolah o = null;
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + "='" + kd.ToString().Trim() +"'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSekolah();
                    o.KdSekolah= AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NmSekolah = AdnFungsi.CStr(rdr["nama_sekolah"]);
                    o.NSS = AdnFungsi.CStr(rdr["nss"]);
                    o.Tingkat = AdnFungsi.CInt(rdr["tingkat"],true);
                    o.Alamat= AdnFungsi.CStr(rdr["alamat_sekolah"]);
                    o.Kelurahan = AdnFungsi.CStr(rdr["kelurahan"]);
                    o.KdPos = AdnFungsi.CStr(rdr["pos"]);
                    o.Kecamatan = AdnFungsi.CStr(rdr["kecamatan"]);
                    o.Kabupaten = AdnFungsi.CStr(rdr["kabupaten"]);
                    o.Propinsi = AdnFungsi.CStr(rdr["propinsi"]);
                    o.Telp = AdnFungsi.CStr(rdr["telp"]);
                    o.Fax = AdnFungsi.CStr(rdr["fax"]);
                    o.Email = AdnFungsi.CStr(rdr["email"]);
                    o.SMS = AdnFungsi.CStr(rdr["sms"]);
                    o.Web = AdnFungsi.CStr(rdr["web"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public List<AdnSekolah> GetAll()
        {
            List<AdnSekolah> lst = new List<AdnSekolah>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSekolah o = new AdnSekolah();
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NmSekolah = AdnFungsi.CStr(rdr["nama_sekolah"]);
                    o.NSS = AdnFungsi.CStr(rdr["nss"]);
                    o.Tingkat = AdnFungsi.CInt(rdr["tingkat"], true);
                    o.Alamat = AdnFungsi.CStr(rdr["alamat_sekolah"]);
                    o.Kelurahan = AdnFungsi.CStr(rdr["kelurahan"]);
                    o.KdPos = AdnFungsi.CStr(rdr["pos"]);
                    o.Kecamatan = AdnFungsi.CStr(rdr["kecamatan"]);
                    o.Kabupaten = AdnFungsi.CStr(rdr["kabupaten"]);
                    o.Propinsi = AdnFungsi.CStr(rdr["propinsi"]);
                    o.Telp = AdnFungsi.CStr(rdr["telp"]);
                    o.Fax = AdnFungsi.CStr(rdr["fax"]);
                    o.Email = AdnFungsi.CStr(rdr["email"]);
                    o.SMS = AdnFungsi.CStr(rdr["sms"]);
                    o.Web = AdnFungsi.CStr(rdr["web"]);

                    lst.Add(o);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_sekolah";
            string KolomDisplay = "nama_sekolah";

            string Value = "KdSekolah";
            string Display = "NmSekolah";

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
                AdnFungsi.LogErr(exp.Message);
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }

    }
}
