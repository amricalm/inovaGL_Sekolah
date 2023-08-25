using System;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.KeuanganPembayaran
{
    public class AdnPembayaranDao
    {
        private const short JUMLAH_KOLOM = 7 ;
        private const string NAMA_TABEL = "ku_pembayaran";
        
        private string pkey = "kd_kwitansi";
        private string sql;
        private string sWhere;

        private int PanjangNoUrutKolomKey = 6 ;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private SqlTransaction trn;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnPembayaranDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnPembayaranDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnPembayaranDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }

        private void SetFldNilai(AdnPembayaran o)
        {
            short idx = 0;

            fld[idx] = "kd_kwitansi"; nilai[idx] = o.KdKwitansi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_siswa"; nilai[idx] = o.KdSiswa.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "total"; nilai[idx] = o.Total.ToString("G", new CultureInfo("en-US")); ; tipe[idx] = "n"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;
        
        }

        public void Simpan(AdnPembayaran o)
        {
            if (o.KdKwitansi == "")
            {
                o.KdKwitansi = AdnFungsi.GetKode(this.cnn, this.trn, this.GetTabel(), this.GetKolomKey(), this.GetPrefix(o.Tgl.Year), this.GetPanjangNoUrutKolomKey());
            }

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            foreach (AdnPembayaranDtl item in o.DfItem)
            {
                item.KdKwitansi = o.KdKwitansi;
                new AdnPembayaranDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
            }
        }
        public void Update(AdnPembayaran o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdKwitansi + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            new AdnPembayaranDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.KdKwitansi);

            foreach (AdnPembayaranDtl item in o.DfItem)
            {
                new AdnPembayaranDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
            }
        }
        public void Hapus(string kd)
        {

            sWhere = this.pkey + "='" + kd + "'";
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public AdnPembayaran Get(string kd)
        {
            AdnPembayaran o = null;
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
                    o = new AdnPembayaran();
                    o.KdKwitansi = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
                }
                rdr.Close();

                if (o != null)
                {
                    o.DfItem = new AdnPembayaranDtlDao(this.cnn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnPembayaran> GetAll()
        {
            List<AdnPembayaran> lst = new List<AdnPembayaran>();
            sql =
            " select kd_kwitansi, tgl,  th_ajar, kd_sekolah, kd_siswa, total "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnPembayaran o = new AdnPembayaran();
                    o.KdKwitansi = AdnFungsi.CStr(rdr["kd_kwitansi"]) ;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
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
        public DataTable GetRingkasan(int KdSiswa)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKwitansi", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("ThAjar", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("NmLengkap", typeof(String));
            tbl.Columns.Add("Total", typeof(decimal));

            try
            {
                string sql =
                "   SELECT TOP 20 hdr.kd_kwitansi,hdr.th_ajar, hdr.tgl, hdr.kd_siswa, hdr.total, sis.nama_lengkap, sis.nis "
                + " FROM ku_pembayaran hdr "
                + " INNER JOIN ms_siswa sis "
                + "     ON hdr.kd_siswa = sis.kd_siswa "
                + "     WHERE hdr.kd_siswa = " + KdSiswa
                + " ORDER BY hdr.kd_kwitansi desc";

                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdKwitansi"] = AdnFungsi.CStr(rdr["kd_kwitansi"]);
                    baris["ThAjar"] = AdnFungsi.CStr(rdr["th_ajar"]);
                    baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                    baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"], true);
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
        public DataTable GetLengkap(string KdKwitansi)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKwitansi", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("NmLengkap", typeof(String));
            tbl.Columns.Add("Kelas", typeof(String));
            tbl.Columns.Add("Total", typeof(decimal));

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("ItemBulan", typeof(String));
            tbl.Columns.Add("Jmh", typeof(decimal));

            try
            {
                string sql =
                "   SELECT hdr.kd_kwitansi, hdr.tgl, hdr.kd_siswa, hdr.total"
                + " , sis.nama_lengkap, sis.nis, ks.kelas"
                + " , dtl.kd_biaya, nm_biaya, dtl.jmh, dtl.item_bulan "
                + " FROM ku_pembayaran hdr "
                + " INNER JOIN ku_pembayaran_dtl dtl "
                + "     ON hdr.kd_kwitansi = dtl.kd_kwitansi "
                + " INNER JOIN ms_biaya bya "
                + "     ON dtl.kd_biaya = bya.kd_biaya "
                + " INNER JOIN ms_siswa sis "
                + "     ON hdr.kd_siswa = sis.kd_siswa "
                + " INNER JOIN kelas_siswa ks"
                + "     ON hdr.th_ajar = ks.th_ajar "
                + "     AND hdr.kd_sekolah = ks.kd_sekolah "
                + "     AND sis.nis = ks.nis "
                + "     WHERE hdr.kd_kwitansi = " + KdKwitansi
                + " ORDER BY hdr.tgl ";

                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdKwitansi"] = AdnFungsi.CStr(rdr["kd_kwitansi"]);
                    baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                    baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    baris["NmLengkap"] = AdnFungsi.CStr(rdr["nama_lengkap"]);
                    baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                    baris["Total"] = AdnFungsi.CDec(rdr["Total"]);
                    baris["Kelas"] = AdnFungsi.CStr(rdr["Kelas"]);

                    baris["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                    baris["NmBiaya"] = AdnFungsi.CStr(rdr["nm_biaya"]);
                    baris["ItemBulan"] = AdnFungsi.CStr(rdr["item_bulan"]);
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
        public DataTable GetByArgs(string sFilter)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKwitansi", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("ThAjar", typeof(String));
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Total", typeof(Decimal));
            tbl.Columns.Add("Keterangan", typeof(String));

            try
            {
                string sql =
                " select kd_kwitansi, tgl,  th_ajar, kd_sekolah, kd_siswa, total,ket "
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
                    baris["KdKwitansi"] = AdnFungsi.CStr(rdr["kd_kwitansi"]);
                    baris["tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                    baris["ThAjar"] = AdnFungsi.CStr(rdr["th_ajar"]);
                    baris["KdSekolah"] = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    baris["Total"] = AdnFungsi.CDec(rdr["total"]);
                    baris["Keterangan"] = AdnFungsi.CStr(rdr["ket"]);

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

        public string GetPrefix(int Tahun)
        {
            string Hasil = DateTime.Now.Year.ToString().Trim().Substring(2,2);

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

        public DataTable LapHarian(DateTime Tgl, string KdSekolah, string Kasir)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKwitansi", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("NmLengkap", typeof(String));
            tbl.Columns.Add("Kelas", typeof(String));
            tbl.Columns.Add("Total", typeof(decimal));

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("ItemBulan", typeof(String));
            tbl.Columns.Add("Jmh", typeof(decimal));

            //Cetak Kolom Biaya
            string sKolomBiaya = ""; string sSumKolomBiaya = "";
            string sql =
                "select kd_biaya,nm_biaya "
                + "from ms_biaya ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int barisBiaya = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomBiaya != "")
                {
                    sKolomBiaya = sKolomBiaya + ",";
                    sSumKolomBiaya = sSumKolomBiaya + ",";
                }

                sKolomBiaya = sKolomBiaya + "'jmh" + barisBiaya.ToString() + "' = case when dtl.kd_biaya = '" + fld[0].ToString().Trim() + "' then (dtl.jmh) end";
                sSumKolomBiaya = sSumKolomBiaya + "isnull(sum(jmh" + barisBiaya.ToString() + "),0)jmh" + barisBiaya.ToString();

                //Tambah Kolom
                tbl.Columns.Add("C" + barisBiaya.ToString(), typeof(Decimal));

                barisBiaya++;
            }
            rdr.Close();

            //== END  Cetak Kolom Biaya



            try
            {
                sql =
                "   SELECT trn.kd_kwitansi, trn.tgl, trn.kd_siswa, trn.total"
                + " , sis.nama_lengkap, sis.nis, ks.kelas";

                if (sSumKolomBiaya != "")
                {
                    sql = sql + "," + sSumKolomBiaya;
                }

                //+ " isnull(jmh,0)jmh "
                sql = sql
                      + " from "
                      + " ( "
                      + "       SELECT hdr.kd_kwitansi, hdr.tgl, hdr.kd_siswa, hdr.total"
                                + " , dtl.kd_biaya,  hdr.kd_sekolah, hdr.th_ajar ";
            
                                if (sKolomBiaya != "")
                                {
                                    sql = sql + "," + sKolomBiaya;
                                }

                                sql = sql
                                + " FROM ku_pembayaran hdr "
                                + " INNER JOIN ku_pembayaran_dtl dtl "
                                + "     ON hdr.kd_kwitansi = dtl.kd_kwitansi "
                                + "     WHERE hdr.tgl = '" + AdnFungsi.SetSqlTglEN(Tgl) + "'"

                        + " ) trn"
                + " INNER JOIN ms_biaya bya "
                + "     ON trn.kd_biaya = bya.kd_biaya "
                + " INNER JOIN ms_siswa sis "
                + "     ON trn.kd_siswa = sis.kd_siswa "
                + " INNER JOIN kelas_siswa ks"
                + "     ON trn.th_ajar = ks.th_ajar "
                + "     AND trn.kd_sekolah = ks.kd_sekolah "
                + "     AND sis.nis = ks.nis "
                + " GROUP BY trn.kd_kwitansi, trn.tgl, trn.kd_siswa, trn.total"
                + " , sis.nama_lengkap, sis.nis, ks.kelas"
                + " ORDER BY trn.kd_kwitansi ";

                cmd = new SqlCommand(sql, this.cnn);
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdKwitansi"] = AdnFungsi.CStr(rdr["kd_kwitansi"]);
                    baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                    baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    baris["NmLengkap"] = AdnFungsi.CStr(rdr["nama_lengkap"]);
                    baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                    baris["Total"] = AdnFungsi.CDec(rdr["Total"]);
                    baris["Kelas"] = AdnFungsi.CStr(rdr["Kelas"]);

                    for (int i = 1; i < barisBiaya; i++)
                    {
                        baris["C" + i.ToString()] = Convert.ToDecimal(rdr["jmh" + i.ToString()]);
                    }

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

    }

    public class AdnLoketDao
    {
        private const short JUMLAH_KOLOM = 13;
        private const string NAMA_TABEL = "tbtr_loket_header";

        private string pkey = "no_kwitansi";
        private string sql;
        private string sWhere;

        private int PanjangNoUrutKolomKey = 6;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private SqlTransaction trn;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnLoketDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnLoketDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnLoketDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }

        private void SetFldNilai(AdnLoket o)
        {
            short idx = 0;
           
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nis"; nilai[idx] =o.Nis.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "no_bayar"; nilai[idx] = o.NoBayar.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "no_kwitansi"; nilai[idx] = o.KdKwitansi.ToString(); ; tipe[idx] = "s"; idx++;
            fld[idx] = "jumlah"; nilai[idx] = o.Total.ToString("G", new CultureInfo("en-US")); tipe[idx] = "n"; idx++;
            fld[idx] = "tanggal"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "kas_perkiraan"; nilai[idx] = o.KasPerkiraan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "posting"; nilai[idx] = o.Posting.ToString(); tipe[idx] = "b"; idx++;
            fld[idx] = "th_ajar_tagihan"; nilai[idx] = o.ThAjarTagihan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "flag"; nilai[idx] = o.Flag.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_siswa"; nilai[idx] = o.KdSiswa.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "uoe"; nilai[idx] = o.Uoe.ToString(); tipe[idx] = "s"; idx++;


        }

        public void Simpan(AdnLoket o)
        {
            if (o.KdKwitansi == "")
            {
                o.KdKwitansi = AdnFungsi.GetKode(this.cnn, this.trn, this.GetTabel(), this.GetKolomKey(), this.GetPrefix(o.Tgl.Year), this.GetPanjangNoUrutKolomKey());
            }

            if (o.NoBayar == 0)
            {
                o.NoBayar = AdnFungsi.GetMaxNo(NAMA_TABEL, "no_bayar", "th_ajar = '" + o.ThAjar + "' AND kd_sekolah = '" + o.KdSekolah + "' AND nis = '" + o.Nis + "'",this.cnn,this.trn);
                o.NoBayar = o.NoBayar + 1;
            }

            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            foreach (AdnLoketDtl item in o.DfItem)
            {
                item.NoBayar = o.NoBayar;
                new AdnLoketDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
            }
        }
        public void Update(AdnLoket o, string ThAjar, string KdSekolah, string Nis, int NoBayar)
        {
            this.SetFldNilai(o);
            sWhere = "th_ajar  = '" + ThAjar + "'"
            + " and kd_sekolah = '" + KdSekolah + "'"
            + " and nis = '" + Nis + "'"
            + " and no_bayar = " + NoBayar;

            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere, pengguna.nm_login);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            new AdnLoketDtlDao(this.cnn, this.pengguna, this.trn).Hapus(ThAjar, KdSekolah, Nis, NoBayar);

            foreach (AdnLoketDtl item in o.DfItem)
            {
                new AdnLoketDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
            }
        }
        public void Hapus(string ThAjar, string KdSekolah, string Nis, int NoBayar)
        {

            sWhere = "th_ajar  = '" + ThAjar + "'"
            + " and kd_sekolah = '" + KdSekolah + "'"
            + " and nis = '" + Nis + "'"
            + " and no_bayar = " + NoBayar;

            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public AdnLoket Get(string ThAjar, string KdSekolah, string Nis, int NoBayar)
        {
            AdnLoket o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where th_ajar  = '" + ThAjar + "'"
            + " and kd_sekolah = '" + KdSekolah + "'"
            + " and nis = '" + Nis + "'"
            + " and no_bayar = " + NoBayar;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnLoket();
                    o.ThAjar = ThAjar;
                    o.KdSekolah = KdSekolah;
                    o.Nis = Nis;
                    o.NoBayar = NoBayar;
                    o.KdKwitansi = AdnFungsi.CStr(rdr["no_kwitansi"]);
                    o.Total = AdnFungsi.CDec(rdr["jumlah"]);
                    o.Tgl = AdnFungsi.CDate(rdr["tanggal"]);
                    o.KasPerkiraan = AdnFungsi.CStr(rdr["kas_perkiraan"]);
                    o.Posting = AdnFungsi.CBool(rdr["posting"],true);
                    o.ThAjarTagihan = AdnFungsi.CStr(rdr["th_ajar_tagihan"]);

                    o.Flag = AdnFungsi.CStr(rdr["flag"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    
                    
                }
                rdr.Close();

                if (o != null)
                {
                    o.DfItem = new AdnLoketDtlDao(this.cnn).Get(o.ThAjar, o.KdSekolah,  o.Nis, o.NoBayar);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnLoket> GetAll()
        {
            List<AdnLoket> lst = new List<AdnLoket>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnLoket o = new AdnLoket();
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.Nis = AdnFungsi.CStr(rdr["nis"]);
                    o.NoBayar = AdnFungsi.CInt(rdr["no_bayar"],true);
                    o.KdKwitansi = AdnFungsi.CStr(rdr["no_kwitansi"]);
                    o.Total = AdnFungsi.CDec(rdr["jumlah"]);
                    o.Tgl = AdnFungsi.CDate(rdr["tanggal"]);
                    o.KasPerkiraan = AdnFungsi.CStr(rdr["kas_perkiraan"]);
                    o.Posting = AdnFungsi.CBool(rdr["posting"], true);
                    o.ThAjarTagihan = AdnFungsi.CStr(rdr["th_ajar_tagihan"]);

                    o.Flag = AdnFungsi.CStr(rdr["flag"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"], true);
                    lst.Add(o);
                }
                rdr.Close();

                if (lst.Count > 0)
                {
                    foreach (AdnLoket item in lst)
                    {
                        item.DfItem = new AdnLoketDtlDao(this.cnn).Get(item.ThAjar, item.KdSekolah,item.Nis,item.NoBayar);
                    }
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst;
        }

        public string GetPrefix(int Tahun)
        {
            string Hasil = DateTime.Now.Year.ToString().Trim().Substring(2, 2);

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
