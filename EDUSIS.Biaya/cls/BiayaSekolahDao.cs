using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.Biaya
{
    public class AdnBiayaSekolahDao
    {
        private const short JUMLAH_KOLOM = 4;
        private const string NAMA_TABEL = "biaya_sekolah";
        
        //private string pkey = "kd_biaya";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnBiayaSekolahDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnBiayaSekolahDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnBiayaSekolah o)
        {
            short idx = 0;

            fld[idx] = "kd_biaya"; nilai[idx] = o.KdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tingkat"; nilai[idx] = o.Tingkat.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "jmh"; nilai[idx] = o.Jmh.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnBiayaSekolah o)
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
        //public void Update(AdnBiayaSekolah o)
        //{
        //    this.SetFldNilai(o);
        //    sWhere = this.pkey + "='" + o.KdBiaya + "' AND kd_sekolah = '" + o.KdSekolah + "'" ;
        //    sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

        //    try
        //    {
        //        cmd.CommandText = sql;
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (DbException exp)
        //    {
        //        AdnFungsi.LogErr(exp.Message);
        //    }
        //}
        public void Hapus(string KdSekolah, string ThAjar, string Tingkat, string KdBiaya)
        {

            sWhere = " kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
                + "     AND th_ajar ='" + ThAjar.ToString().Trim() + "'"
                + "     AND tingkat = '" + Tingkat.ToString().Trim() + "'"
                + "     AND kd_biaya = '" + KdBiaya.ToString().Trim() + "'";

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
        public AdnBiayaSekolah Get(string KdSekolah, string ThAjar, string KdBiaya, string Tingkat)
        {
            AdnBiayaSekolah o = null;

            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND kd_biaya = '" + KdBiaya.ToString().Trim() + "'"
            + "     AND tingkat  = '" + Tingkat.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnBiayaSekolah();
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.Tingkat = AdnFungsi.CStr(rdr["tingkat"]);
                    o.Jmh = AdnFungsi.CDec(rdr["rupiah"]);

                }
                rdr.Close();

                if (o != null)
                {
                    o.oBiaya = new AdnBiayaDao(this.cnn).Get(o.KdBiaya);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o;
        }
        public List<AdnBiayaSekolah> Get(string KdSekolah, string ThAjar, string Tingkat)
        {
            List<AdnBiayaSekolah> lst = new List<AdnBiayaSekolah>();
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND tingkat = '" + Tingkat.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnBiayaSekolah o = new AdnBiayaSekolah();
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]) ;
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.Tingkat = AdnFungsi.CStr(rdr["tingkat"]);
                    o.Jmh = AdnFungsi.CDec(rdr["rupiah"]);

                    lst.Add(o);
                }
                rdr.Close();

                foreach (AdnBiayaSekolah item in lst)
                {
                    item.oBiaya = new AdnBiayaDao(this.cnn).Get(item.KdBiaya);
                }

            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst; 
        }
        public List<AdnBiayaSekolah> GetByJenis(string KdSekolah, string ThAjar, string Tingkat, string KdJenis)
        {
            List<AdnBiayaSekolah> lst = new List<AdnBiayaSekolah>();
            
            sql =
            " select * "
            + " from " + NAMA_TABEL + " bs"
            + " INNER JOIN ms_biaya bya "
            + "     ON bya.kd_biaya = bs.kd_biaya "
            + "     AND bya.kd_jenis = '" + KdJenis.Trim() + "'"
            + " WHERE kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND tingkat = '" + Tingkat.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnBiayaSekolah o = new AdnBiayaSekolah();
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.Tingkat = AdnFungsi.CStr(rdr["tingkat"]);
                    o.Jmh = AdnFungsi.CDec(rdr["rupiah"]);

                    lst.Add(o);
                }
                rdr.Close();

                foreach (AdnBiayaSekolah item in lst)
                {
                    item.oBiaya = new AdnBiayaDao(this.cnn).Get(item.KdBiaya);
                }

            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst;
        }

        public DataTable GetDf(string KdSekolah, string ThAjar, string Tingkat)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("ThAjar", typeof(String));
            tbl.Columns.Add("Tingkat", typeof(String));
            tbl.Columns.Add("Jmh", typeof(Decimal));

            sql =
            " select ms.kd_biaya, bs.kd_sekolah, bs.th_ajar, bs.tingkat, bs.rupiah, ms.nm_biaya "
            + " from " + NAMA_TABEL + " bs"
            + " right outer join ms_biaya ms "
            + "     ON bs.kd_biaya = ms.kd_biaya "
            + "     AND bs.kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND bs.th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND bs.tingkat = '" + Tingkat.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                    baris["NmBiaya"] = AdnFungsi.CStr(rdr["nm_biaya"]);
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["ThAjar"] = AdnFungsi.CStr(rdr["th_ajar"]);
                    baris["Tingkat"] = AdnFungsi.CStr(rdr["tingkat"]);
                    baris["Jmh"] = AdnFungsi.CDec(rdr["rupiah"]);

                    //switch (Tingkat)
                    //{
                    //    case "0":
                    //        baris["Jmh1"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh2"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh3"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh4"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        break;
                    //    case "1":
                    //        baris["Jmh1"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh2"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh3"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh4"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh5"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh6"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        break;
                    //    case "2":
                    //    case "3":
                    //        baris["Jmh1"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh2"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        baris["Jmh3"] = AdnFungsi.CDec(rdr["rupiah"]);
                    //        break;
                    //}

                    

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return tbl;
        }
        public DataTable GetDfUtama(string KdSekolah, string ThAjar, string Tingkat)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("ThAjar", typeof(String));
            tbl.Columns.Add("Tingkat", typeof(String));
            tbl.Columns.Add("Jmh", typeof(Decimal));

            sql =
            " select ms.kd_biaya, bs.kd_sekolah, bs.th_ajar, bs.tingkat, bs.rupiah, ms.nm_biaya "
            + " from " + NAMA_TABEL + " bs"
            + " inner join ms_biaya ms "
            + "     ON bs.kd_biaya = ms.kd_biaya "
            + " WHERE bs.kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND bs.th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND bs.tingkat = '" + Tingkat.ToString().Trim() + "'"
            +"     AND (ms.kd_jenis = '" + EDUSIS.Shared.EdusisVar.JenisBiaya.KEGIATAN + "' OR ms.kd_jenis ='"
                    + EDUSIS.Shared.EdusisVar.JenisBiaya.PSB + "' OR ms.kd_jenis ='" + EDUSIS.Shared.EdusisVar.JenisBiaya.SPP + "')";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                    baris["NmBiaya"] = AdnFungsi.CStr(rdr["nm_biaya"]);
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["ThAjar"] = AdnFungsi.CStr(rdr["th_ajar"]);
                    baris["Tingkat"] = AdnFungsi.CStr(rdr["tingkat"]);
                    baris["Jmh"] = AdnFungsi.CDec(rdr["rupiah"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return tbl;
        }
        public decimal GetPotongan(string KdSekolah, string ThAjar, string KdBiaya, string Nis)
        {
            decimal Hasil = 0;
            
            sql =
            " select jmh "
            + " from biaya_potongan bs "
            + "     WHERE bs.kd_biaya = '" + KdBiaya.Trim() + "'"
            + "     AND bs.kd_sekolah ='" + KdSekolah.ToString().Trim() + "' "
            + "     AND bs.th_ajar ='" + ThAjar.ToString().Trim() + "'"
            + "     AND bs.nis = '" + Nis.ToString().Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                Object  o =  cmd.ExecuteScalar();
                if (o != null)
                {
                    Hasil = (decimal)o;
                }
            }
        catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return Hasil;
        }
        public DataTable GetRincian(string KdSekolah, string ThAjar, string Tingkat, string Nis)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("KdAkunPiutang", typeof(String));
            tbl.Columns.Add("KdAkunKewajiban", typeof(String));
            tbl.Columns.Add("KdAkunPendapatan", typeof(String));
            tbl.Columns.Add("ItemBulan", typeof(String));
            tbl.Columns.Add("Keterangan", typeof(String));
            tbl.Columns.Add("Jmh", typeof(decimal));
            tbl.Columns.Add("JmhBiaya", typeof(decimal));
            tbl.Columns.Add("JmhPotongan", typeof(decimal));

            try
            {
                string sql =
                " select ms.kd_biaya, nm_biaya, isnull(bs.rupiah,0) jmh_biaya, isnull(pot.jmh,0) jmh_potongan"
                + "     , (isnull(bs.rupiah,0) - isnull(pot.jmh,0)) jmh, '' as item_bulan, '' as ket"
                + "     , kd_akun_piutang, kd_akun_kewajiban, kd_akun_pendapatan "
                + " from  ms_biaya ms "
                + " LEFT OUTER JOIN biaya_sekolah bs"
                + "     ON ms.kd_biaya = bs.kd_biaya "
                + "     AND bs.th_ajar = '" + ThAjar.Trim() + "'"
                + "     AND bs.kd_sekolah = '" + KdSekolah.Trim() + "'"
                + "     AND bs.tingkat = '" + Tingkat.Trim() + "'"
                + " LEFT OUTER JOIN biaya_potongan pot "
                + "     ON ms.kd_biaya = pot.kd_biaya "
                + "     AND pot.th_ajar = '" + ThAjar.Trim() + "'"
                + "     AND pot.nis = '" + Nis.Trim() + "'"
                + "     AND pot.kd_sekolah = '" + KdSekolah.Trim() + "'";

                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                    baris["KdAkunPiutang"] = AdnFungsi.CStr(rdr["kd_akun_piutang"]);
                    baris["KdAkunKewajiban"] = AdnFungsi.CStr(rdr["kd_akun_kewajiban"]);
                    baris["KdAkunPendapatan"] = AdnFungsi.CStr(rdr["kd_akun_pendapatan"]);
                    baris["NmBiaya"] = AdnFungsi.CStr(rdr["nm_biaya"]);
                    baris["ItemBulan"] = AdnFungsi.CStr(rdr["item_bulan"]);
                    baris["Keterangan"] = AdnFungsi.CStr(rdr["ket"]);
                    baris["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);
                    baris["JmhBiaya"] = AdnFungsi.CDec(rdr["jmh_biaya"]);
                    baris["JmhPotongan"] = AdnFungsi.CDec(rdr["jmh_potongan"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return tbl;

        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo, DataTable lst)
        {
            //DataTable lst = new DataTable();
            //DataRow row;

            //string KolomValue = "kd_biaya";
            //string KolomDisplay = "nm_biaya";

            //string Value = "KdBiaya";
            //string Display = "NmBiaya";

            //lst.Columns.Add(Value, typeof(string));
            //lst.Columns.Add(Display, typeof(string));

            //string sql =
            //"SELECT " + KolomValue
            //+ " FROM  " + NAMA_TABEL
            //+ " ORDER BY " + KolomDisplay;

            //try
            //{
            //    foreach(DataRow item in lst.Rows)
            //    {
            //        row = lst.NewRow();
            //        row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
            //        row[Display] = AdnFungsi.CStr(rdr[KolomDisplay]);
            //        lst.Rows.Add(row);
            //    }
            //}
            //catch (DbException exp)
            //{
            //    AdnFungsi.LogErr(exp.Message);
            //}

            //cbo.DisplayMember = Display;
            //cbo.ValueMember = Value;
            //cbo.DataSource = lst;

            return true;
        }
    }
}
