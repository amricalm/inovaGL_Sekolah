using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL.Piutang
{
    public class AdnSaldoAwalDao
    {
        private const short JUMLAH_KOLOM =7 ;
        private const string NAMA_TABEL = "ac_saldo_piutang_siswa";
        
        private string pkey = "kd_saw";
        private string sql;
        private string sWhere;

        private int  PanjangNoUrutKolomKey= 4;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private SqlTransaction trn;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnSaldoAwalDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnSaldoAwalDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnSaldoAwalDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }

        private void SetFldNilai(AdnSaldoAwal o)
        {
            short idx = 0;

            fld[idx] = "kd_saw"; nilai[idx] = o.KdSaldoAwal.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "periode"; nilai[idx] = o.Periode.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_siswa"; nilai[idx] = o.KdSiswa.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "total"; nilai[idx] = o.Total.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnSaldoAwal o)
        {
            
            if (o.KdSaldoAwal == "")
            {
                o.KdSaldoAwal = AdnFungsi.GetKode(this.cnn,this.trn, this.GetTabel(), this.GetKolomKey(), this.GetPrefix(), this.GetPanjangNoUrutKolomKey());
            }
            this.SetFldNilai(o);

            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            foreach (AdnSaldoAwalDtl item in o.DfItem)
            {
                item.KdSaldoAwal = o.KdSaldoAwal;
                new AdnSaldoAwalDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
            }
        }
        public void Update(AdnSaldoAwal o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdSaldoAwal + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();


                new AdnSaldoAwalDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.KdSaldoAwal);

                foreach (AdnSaldoAwalDtl item in o.DfItem)
                {
                    new AdnSaldoAwalDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
                }

            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
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
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Hapus(int KdSiswa, string ThAjar, DateTime PerTgl)
        {

            sWhere = " th_ajar ='" + ThAjar + "'  AND kd_siswa =" + KdSiswa + " AND tgl ='" + AdnFungsi.SetSqlTglEN(PerTgl) + "'";
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();


        }


        public AdnSaldoAwal Get(string kd)
        {
            AdnSaldoAwal o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSaldoAwal();
                    o.KdSaldoAwal = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Periode = AdnFungsi.CStr(rdr["periode"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                }
                rdr.Close();

                if (o != null)
                {
                    o.DfItem = new AdnSaldoAwalDtlDao(this.cnn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnSaldoAwal> GetAll()
        {
            List<AdnSaldoAwal> lst = new List<AdnSaldoAwal>();
            sql =
            " select kd_saw, tgl, Periode, th_ajar, kd_sekolah, kd_siswa, total "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSaldoAwal o = new AdnSaldoAwal();
                    o.KdSaldoAwal = AdnFungsi.CStr(rdr["kd_saw"]) ;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Periode = AdnFungsi.CStr(rdr["Periode"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst; 
        }
        public DataTable GetByArgs(string sFilter)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdSaldoAwal", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Periode", typeof(String));
            tbl.Columns.Add("ThAjar", typeof(String));
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Total", typeof(Decimal));

            try
            {
                string sql =
                " select kd_saw, tgl, periode, th_ajar, kd_sekolah, kd_siswa, total "
                + " from " + NAMA_TABEL;

                if (sFilter != "")
                {
                    sql = sql + " WHERE " + sFilter;
                }

                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdSaldoAwal"] = AdnFungsi.CStr(rdr["kd_saw"]);
                    baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                    baris["Periode"] = AdnFungsi.CStr(rdr["periode"]);
                    baris["ThAjar"] = AdnFungsi.CStr(rdr["th_ajar"]);
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    baris["Total"] = AdnFungsi.CDec(rdr["total"]);

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

        public DataTable Get(int KdSiswa, string ThAjar, DateTime PerTgl)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("ItemBulan", typeof(String));
            tbl.Columns.Add("Keterangan", typeof(String));
            tbl.Columns.Add("Jmh", typeof(decimal));

            try
            {
                string Nis = "";
                EDUSIS.Siswa.AdnSiswa oSiswa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).Get(KdSiswa);
                if (oSiswa != null)
                {
                    Nis = oSiswa.NIS;
                }

                string sql =
                " select bs.kd_biaya, nm_biaya, isnull(jmh,0) jmh, item_bulan, ket "
                + " from  ms_biaya bs "
                + " LEFT OUTER JOIN "
                + " ( "
                + "     SELECT kd_biaya, jmh, item_bulan,ket "
                + "     FROM ac_saldo_piutang_siswa hdr "
                + "     INNER JOIN ac_saldo_piutang_siswa_dtl dtl "
                + "         ON hdr.kd_saw = dtl.kd_saw "
                + "     WHERE hdr.th_ajar = '" + ThAjar + "'"
                + "         AND hdr.tgl ='" + AdnFungsi.SetSqlTglEN(PerTgl) + "'"
                + "         AND hdr.kd_siswa = " + KdSiswa
                + " ) saw "
                + " ON bs.kd_biaya = saw.kd_biaya ";

                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                    baris["NmBiaya"] = AdnFungsi.CStr(rdr["nm_biaya"]);
                    baris["ItemBulan"] = AdnFungsi.CStr(rdr["item_bulan"]);
                    baris["Keterangan"] = AdnFungsi.CStr(rdr["ket"]);
                    baris["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);

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
        public bool Get(int KdSiswa, string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");
            tbl.Columns.Add("KdSaw", typeof(String));

            try
            {
                string sql =
                "     SELECT kd_saw "
                + "     FROM ac_saldo_piutang_siswa hdr "
                + "     WHERE hdr.th_ajar = '" + ThAjar + "'"
                + "         AND hdr.kd_siswa = " + KdSiswa;

                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdSaw"] = AdnFungsi.CStr(rdr["kd_saw"]);
                    tbl.Rows.Add(baris);
                }
                rdr.Close();
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            if (tbl.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public DataTable GetRingkasan(string Kelas, string ThAjar, string KdSekolah,DateTime PerTgl)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdSaw", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("NmLengkap", typeof(String));
            tbl.Columns.Add("Total", typeof(decimal));

            try
            {
                string sql =
                " Select ms.kd_siswa, ms.nis, ms.nama_lengkap, isnull(trn.kd_saw,'') kd_saw, isnull(trn.total,0) total "
                + " FROM ms_siswa ms "
                + " INNER JOIN kelas_siswa ks "
                + "     ON ks.kd_sekolah = ms.kd_sekolah"
                + "     AND ks.nis = ms.nis "
                + " LEFT OUTER JOIN "
                + " ( "
                + "     SELECT hdr.kd_saw, hdr.kd_siswa, hdr.total "
                + "     FROM ac_saldo_piutang_siswa hdr "
                + "     WHERE hdr.th_ajar = '" + ThAjar + "'"
                + "         AND hdr.kd_sekolah = '" + KdSekolah + "'"
                + "         AND hdr.tgl ='" + AdnFungsi.SetSqlTglEN(PerTgl) + "'"
                + " ) trn "
                + "     ON ms.kd_siswa = trn.kd_siswa "
                + " WHERE  ks.kelas = '" + Kelas + "'"
                + "         AND ks.kd_sekolah = '" + KdSekolah + "'"
                + "         AND ks.th_ajar = '" + ThAjar + "'"
                + " ORDER BY ms.nama_lengkap ";


                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdSaw"] = AdnFungsi.CStr(rdr["kd_saw"]);
                    baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    baris["NmLengkap"] = AdnFungsi.CStr(rdr["nama_lengkap"]);
                    baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                    baris["Total"] = AdnFungsi.CDec(rdr["Total"]);

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
        public DataTable GetRincian(string KdSaw)
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

            try
            {
                string sql =
                " select bs.kd_biaya, nm_biaya, isnull(jmh,0) jmh, item_bulan, ket"
                + "     , kd_akun_piutang, kd_akun_kewajiban, kd_akun_pendapatan "
                + " from  ms_biaya bs "
                + " LEFT OUTER JOIN "
                + " ( "
                + "     SELECT kd_biaya, jmh, item_bulan,ket "
                + "     FROM ac_saldo_piutang_siswa hdr "
                + "     INNER JOIN ac_saldo_piutang_siswa_dtl dtl "
                + "         ON hdr.kd_saw = dtl.kd_saw "
                + "     WHERE hdr.kd_saw = '" + KdSaw.ToString().Trim() + "'"
                + " ) saw "
                + " ON bs.kd_biaya = saw.kd_biaya ";

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

        public string GetPrefix()
        {
            string Hasil = "SAW";

            return Hasil;
        }

        public string GetTabel()
        {
            return NAMA_TABEL;
        }

        public string GetKolomKey()
        {
            return this.pkey;
        }

        public int GetPanjangNoUrutKolomKey()
        {
            return this.PanjangNoUrutKolomKey;
        }
        

    }
}
