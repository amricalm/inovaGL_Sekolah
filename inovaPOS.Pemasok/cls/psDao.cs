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
    public class AdnPerusahaanDao
    {
        private const short JUMLAH_KOLOM = 17;
        private const string NAMA_TABEL = "mps";
        private string pkey = "kd_ps";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnPerusahaanDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnPerusahaan o)
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
            fld[idx] = "ket"; nilai[idx] = o.ket.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "bidang_usaha"; nilai[idx] = o.bidang_usaha.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "web"; nilai[idx] = o.web.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "aset"; nilai[idx] = o.aset.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "omset"; nilai[idx] = o.omset.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "jmh_karyawan"; nilai[idx] = o.jmh_karyawan.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "modal"; nilai[idx] = o.modal.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "sumber"; nilai[idx] = o.sumber.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "marketing_fee"; nilai[idx] = o.marketing_fee.ToString(); tipe[idx] = "n"; idx++;
            //fld[idx] = "uid"; nilai[idx] = o.uid.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "tgl_tambah"; nilai[idx] = o.tgl_tambah.ToString(); tipe[idx] = "d"; idx++;
            //fld[idx] = "uid_edit"; nilai[idx] = o.uid_edit.ToString(); tipe[idx] = "s"; idx++;
            //fld[idx] = "tgl_edit"; nilai[idx] = o.tgl_edit.ToString(); tipe[idx] = "d"; idx++;
        }

        public string Simpan(AdnPerusahaan o)
        {
            string kode = "";
            kode = this.GetKd();
            o.kd_ps = kode.Trim();
            o.uid = "adn";
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
            return kode;
        }
        public void Update(AdnPerusahaan o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.kd_ps.ToString() + "'";
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
        public AdnPerusahaan Get(string kd)
        {
            AdnPerusahaan o = null;
            string sql =
            " select nm_ps,alamat,kota,pos,propinsi,telp,fax,email,ket,"
            + " bidang_usaha,web, aset,omset,jmh_karyawan, modal,sumber "
            + " ,uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnPerusahaan();
                    o.kd_ps = kd.Trim();
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim();
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
                    o.bidang_usaha = Convert.ToString(rdr["bidang_usaha"]).Trim();
                    o.web = Convert.ToString(rdr["web"]).Trim();
                    o.aset = Convert.ToDecimal(rdr["aset"]);
                    o.omset = Convert.ToDecimal(rdr["omset"]);
                    o.jmh_karyawan = Convert.ToInt32(rdr["jmh_karyawan"]);
                    o.modal = Convert.ToDecimal(rdr["modal"]);
                    o.sumber= Convert.ToString(rdr["sumber"]).Trim();
                    //o.marketing_fee = Convert.ToDecimal(rdr["marketing_fee"]);
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
        public List<AdnPerusahaan> GetAll()
        {
            List<AdnPerusahaan> lst = new List<AdnPerusahaan>();
            string sql =
            " select kd_ps,nm_ps,alamat,kota,pos,propinsi,telp,fax,email,ket,sumber,uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPerusahaan o = new AdnPerusahaan();
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]);
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim();
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
                    o.sumber = Convert.ToString(rdr["sumber"]).Trim();
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
        public List<AdnPerusahaan> GetPelanggan()
        {
            List<AdnPerusahaan> lst = new List<AdnPerusahaan>();
            string sql =
            " select ps.kd_ps,nm_ps,alamat,kota,pos,propinsi,telp,fax,email,ket,uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL + " ps"
            + " inner join "
            + " ( "
            + " select kd_ps, min(tgl_closing) tgl_pelanggan "
            + " from tprospek "
            + " inner join rf_st_prospek rf "
            + "     on tprospek.kd_status = rf.kd_status "
            + " where mulai_pelanggan = 1"
            + " group by kd_ps "
            + " ) prs "
            + "     on  ps.kd_ps = prs.kd_ps ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPerusahaan o = new AdnPerusahaan();
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]);
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim();
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
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
        public List<AdnPerusahaan> GetBukanPelanggan()
        {
            List<AdnPerusahaan> lst = new List<AdnPerusahaan>();
            string sql =
            " select ps.kd_ps,nm_ps,alamat,kota,pos,propinsi,telp,fax,email,ket,uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL + " ps"
            + " where kd_ps NOT IN "
            + " ( "
            + " select DISTINCT kd_ps "
            + " from tprospek "
            + " inner join rf_st_prospek rf "
            + "     on tprospek.kd_status = rf.kd_status "
            + " where mulai_pelanggan = 1"
            + " ) ";
            //+ "     on  ps.kd_ps <> prs.kd_ps ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPerusahaan o = new AdnPerusahaan();
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]);
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim();
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
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
        public List<AdnPerusahaan> LapPsByKd(string kd)
        {
            List<AdnPerusahaan> lst = new List<AdnPerusahaan>();
            string sql =
            " select kd_ps,nm_ps,alamat,kota,pos,propinsi,telp,fax,email,ket, "
            + " bidang_usaha,web, aset,omset,jmh_karyawan, modal, "
            + " uid,tgl_tambah,uid_edit,tgl_edit "
            + " from " + NAMA_TABEL
            + " where kd_ps ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPerusahaan o = new AdnPerusahaan();
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]);
                    o.nm_ps = Convert.ToString(rdr["nm_ps"]).Trim();
                    o.alamat = Convert.ToString(rdr["alamat"]).Trim();
                    o.kota = Convert.ToString(rdr["kota"]).Trim();
                    o.pos = Convert.ToString(rdr["pos"]).Trim();
                    o.propinsi = Convert.ToString(rdr["propinsi"]).Trim();
                    o.telp = Convert.ToString(rdr["telp"]).Trim();
                    o.fax = Convert.ToString(rdr["fax"]).Trim();
                    o.email = Convert.ToString(rdr["email"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
                    o.bidang_usaha = Convert.ToString(rdr["bidang_usaha"]).Trim();
                    o.web = Convert.ToString(rdr["web"]).Trim();
                    o.aset = Convert.ToDecimal(rdr["aset"]);
                    o.omset = Convert.ToDecimal(rdr["omset"]);
                    o.jmh_karyawan = Convert.ToInt32(rdr["jmh_karyawan"]);
                    o.modal = Convert.ToDecimal(rdr["modal"]);
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
        public ArrayList LapDaftarPerusahaan()
        {

            ArrayList lst = new ArrayList();

            string sql =
            " select mps.kd_ps, nm_ps,alamat, telp,fax, email, nm_lengkap "
            + " from " + NAMA_TABEL 
            + " inner join "
            + " ( select kd_ps, min(nm_lengkap) as nm_lengkap from mcp group by kd_ps ) mcp "
            + " on mps.kd_ps = mcp.kd_ps";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                for (int i = 0; i < rdr.FieldCount; ++i)
                {
                    fld[i] = rdr[i];
                }
                lst.Add(fld);
            }
            rdr.Close();
            return lst; 
        }
        
        private string GetKd()
        {
            string kode = "";
            sql = "SELECT isnull(max([" + pkey + "]),0) as kd "
            + " FROM " + NAMA_TABEL;

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            kode = iMax.ToString("000000");
            return kode;
        }
    }
}
