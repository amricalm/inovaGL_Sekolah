using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaPOS
{
    public class AdnPemasokDao
    {
        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "mpemasok";
        private string pkey = "kd_pemasok";

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

            fld[idx] = "kd_pemasok"; nilai[idx] = o.kd_pemasok.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_ps"; nilai[idx] = o.nm_ps.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_kontak"; nilai[idx] = o.nm_kontak.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "alamat"; nilai[idx] = o.alamat.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kota"; nilai[idx] = o.kota.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "pos"; nilai[idx] = o.pos.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "telp"; nilai[idx] = o.telp.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "fax"; nilai[idx] = o.fax.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "email"; nilai[idx] = o.email.ToString(); tipe[idx] = "s"; idx++;

        }

        public void Simpan(AdnPemasok o)
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
        public void Update(AdnPemasok o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.kd_pemasok.Trim() + "'";
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
                    o.kd_pemasok = Convert.ToString(rdr["kd_pemasok"]).Trim();
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.nm_kontak = Convert.ToString(rdr["nm_kontak"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim(); ;
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();

                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnPemasok> GetAll()
        {
            List<AdnPemasok> lst = new List<AdnPemasok>();
            string sql =
            " select kd_pemasok,nm_ps "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPemasok o = new AdnPemasok();
                    o.kd_pemasok = Convert.ToString(rdr["kd_pemasok"]).Trim();
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
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



    }
}
