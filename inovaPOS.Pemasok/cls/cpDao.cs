using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaPOS
{
    public class AdnContactPersonDao
    {

        private const short JUMLAH_KOLOM = 7;
        private const string NAMA_TABEL = "im_mpemasok_cp";
        private string pkey = "kd_cp";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnContactPersonDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnContactPerson o)
        {
            short idx = 0;
            fld[idx] = "kd_ps"; nilai[idx] = o.kd_ps.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_lengkap"; nilai[idx] = o.nm_lengkap.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jabatan"; nilai[idx] = o.jabatan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "telp"; nilai[idx] = o.telp.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "hp"; nilai[idx] = o.hp.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "email"; nilai[idx] = o.email.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.ket.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnContactPerson o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
            try
            {
                cmd.CommandText = sql + "; SELECT CAST(scope_identity() AS int);";
                o.kd_cp = (Int32)cmd.ExecuteScalar();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.ErrorCode.ToString() + "; " + exp.Message.ToString());
            }
        }
        public void Update(AdnContactPerson o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.kd_cp + "'";
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
        public void Hapus(int kd)
        {
            sWhere = this.pkey + "=" + kd ;
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
        public AdnContactPerson Get(int kd)
        {
            AdnContactPerson o = null;
            string sql =
            " select kd_ps,kd_cp,nm_lengkap,jabatan,telp,hp,email,ket,uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " =" + kd;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnContactPerson();
                    o.kd_cp = Convert.ToInt32(rdr["kd_cp"]);
                    o.nm_lengkap = Convert.ToString(rdr["nm_lengkap"]).Trim();
                    o.jabatan = Convert.ToString(rdr["jabatan"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.hp = Convert.ToString(rdr["hp"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = Convert.ToDateTime(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = Convert.ToDateTime(rdr["tgl_edit"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnContactPerson> GetAll()
        {
            List<AdnContactPerson> lst = new List<AdnContactPerson>();
            string sql =
            " select kd_ps,nm_lengkap,jabatan,telp,hp,email,ket,uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnContactPerson o = new AdnContactPerson();
                    o.kd_cp = Convert.ToInt32(rdr["kd_cp"]);
                    o.nm_lengkap = Convert.ToString(rdr["nm_lengkap"]).Trim();
                    o.jabatan = Convert.ToString(rdr["jabatan"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.hp = Convert.ToString(rdr["hp"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
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
        public List<AdnContactPerson> GetByPS(string kdPs)
        {
            List<AdnContactPerson> lst = new List<AdnContactPerson>();
            string sql =
            " select kd_ps,kd_cp,nm_lengkap,jabatan,telp,hp,email,ket,uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL
            + " where kd_ps ='" + kdPs.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnContactPerson o = new AdnContactPerson();
                    o.kd_cp = Convert.ToInt32(rdr["kd_cp"]);
                    o.nm_lengkap = Convert.ToString(rdr["nm_lengkap"]).Trim();
                    o.jabatan = Convert.ToString(rdr["jabatan"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.hp = Convert.ToString(rdr["hp"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
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
                AdnFungsi.LogErr(exp.ErrorCode.ToString() + "; " + exp.Message.ToString());
            }
            return lst;
        }
    }
}
