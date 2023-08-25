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
    public class AdnCateringDao
    {
        private const short JUMLAH_KOLOM = 2;  
        private const string NAMA_TABEL = "ms_catering";
        
        private string pkey = "kd_catering";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnCateringDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnCateringDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnCatering o)
        {
            short idx = 0;

            fld[idx] = "nm_catering"; nilai[idx] = o.NmCatering.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_catering"; nilai[idx] = o.KdCatering.ToString(); tipe[idx] = "s"; idx++;

            
        }

        public void Simpan(AdnCatering o)
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
        public void Update(AdnCatering o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdCatering+ "'" ;
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

        public AdnCatering Get(string kd)
        {
            AdnCatering o = null;
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
                    o = new AdnCatering();
                    o.KdCatering= AdnFungsi.CStr(rdr["kd_catering"]);
                    o.NmCatering = AdnFungsi.CStr(rdr["nm_catering"]);

                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public List<AdnCatering> GetAll()
        {
            List<AdnCatering> lst = new List<AdnCatering>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnCatering o = new AdnCatering();
                    o.KdCatering = AdnFungsi.CStr(rdr["kd_catering"]);
                    o.NmCatering = AdnFungsi.CStr(rdr["nm_catering"]);
  
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

            string KolomValue = "kd_catering";
            string KolomDisplay = "nm_catering";

            string Value = "KdCatering";
            string Display = "NmCatering";

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
                AdnFungsi.LogErr(exp.Message);
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }

    }
}
