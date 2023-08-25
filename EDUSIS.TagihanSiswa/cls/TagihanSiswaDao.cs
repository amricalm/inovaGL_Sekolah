using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.KeuanganTagihan
{
    public class AdnTagihanSiswaDao
    {
        private const short JUMLAH_KOLOM = 8 ;
        private const string NAMA_TABEL = "ku_tagihan_siswa";
        
        private string pkey = "kd_tagihan";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private SqlTransaction trn;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnTagihanSiswaDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnTagihanSiswaDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnTagihanSiswaDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }

        private void SetFldNilai(AdnTagihanSiswa o)
        {
            short idx = 0;

            fld[idx] = "kd_tagihan"; nilai[idx] = o.KdTagihan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl_terbit"; nilai[idx] = o.TglTerbit.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "periode"; nilai[idx] = o.Periode.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "th_ajar"; nilai[idx] = o.ThAjar.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_sekolah"; nilai[idx] = o.KdSekolah.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_siswa"; nilai[idx] = o.KdSiswa.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "total"; nilai[idx] = o.Total.ToString("G", new CultureInfo("en-US")); ; tipe[idx] = "n"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;
        
        }

        public void Simpan(AdnTagihanSiswa o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            foreach (AdnTagihanSiswaDtl item in o.DfItem)
            {
                new AdnTagihanSiswaDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
            }
        }
        public void Update(AdnTagihanSiswa o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdTagihan + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();


                new AdnTagihanSiswaDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.KdTagihan);

                foreach (AdnTagihanSiswaDtl item in o.DfItem)
                {
                    new AdnTagihanSiswaDtlDao(this.cnn, this.pengguna, this.trn).Simpan(item);
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

        public AdnTagihanSiswa Get(string kd)
        {
            AdnTagihanSiswa o = null;
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
                    o = new AdnTagihanSiswa();
                    o.KdTagihan = kd;
                    o.TglTerbit = AdnFungsi.CDate(rdr["tgl_terbit"]);
                    o.Periode = AdnFungsi.CStr(rdr["periode"]);
                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["ket"]);
                }
                rdr.Close();

                if (o != null)
                {
                    o.DfItem = new AdnTagihanSiswaDtlDao(this.cnn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return o;
        }
        public List<AdnTagihanSiswa> GetAll()
        {
            List<AdnTagihanSiswa> lst = new List<AdnTagihanSiswa>();
            sql =
            " select kd_tagihan, tgl_terbit, Periode, th_ajar, kd_sekolah, kd_siswa, total "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnTagihanSiswa o = new AdnTagihanSiswa();
                    o.KdTagihan = AdnFungsi.CStr(rdr["kd_tagihan"]) ;
                    o.TglTerbit = AdnFungsi.CDate(rdr["tgl_terbit"]);
                    o.Periode = AdnFungsi.CStr(rdr["Periode"]);
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
        public DataTable GetByArgs(string sFilter)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdSaldoAwal", typeof(String));
            tbl.Columns.Add("TglTerbit", typeof(DateTime));
            tbl.Columns.Add("Periode", typeof(String));
            tbl.Columns.Add("ThAjar", typeof(String));
            tbl.Columns.Add("KdSekolah", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Total", typeof(Decimal));
            tbl.Columns.Add("Keterangan", typeof(String));

            try
            {
                string sql =
                " select kd_tagihan, tgl_terbit, periode, th_ajar, kd_sekolah, kd_siswa, total,ket "
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
                    baris["KdSaldoAwal"] = AdnFungsi.CStr(rdr["kd_tagihan"]);
                    baris["tgl_terbit"] = AdnFungsi.CDate(rdr["tgl_terbit"]);
                    baris["Periode"] = AdnFungsi.CStr(rdr["periode"]);
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

        public DataTable Get(int KdSiswa, string ThAjar, int Tahun, int Bulan)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("ItemBulan", typeof(String));
            tbl.Columns.Add("Keterangan", typeof(String));
            tbl.Columns.Add("Jmh", typeof(decimal));
            tbl.Columns.Add("KdAkunPiutang", typeof(String));
            tbl.Columns.Add("KdAkunKewajiban", typeof(String));
            tbl.Columns.Add("KdAkunPendapatan", typeof(String));

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
                + "     , kd_akun_piutang, kd_akun_kewajiban, kd_akun_pendapatan "
                + " from  ms_biaya bs "
                + " LEFT OUTER JOIN "
                + " ( "
                + "     SELECT kd_biaya, jmh, item_bulan, dtl.ket "
                + "     FROM ku_tagihan_siswa hdr "
                + "     INNER JOIN ku_tagihan_siswa_dtl dtl "
                + "         ON hdr.kd_tagihan = dtl.kd_tagihan "
                + "     WHERE hdr.th_ajar = '" + ThAjar + "'"
                + "         AND hdr.periode = '" + Tahun.ToString() + Bulan.ToString().PadLeft(2, '0') + "'"
                + "         AND hdr.kd_siswa = " + KdSiswa
                + " ) tgh"
                + " ON bs.kd_biaya = tgh.kd_biaya ";

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
        public DataTable GetRingkasan(string Kelas, string ThAjar,string KdSekolah, int Tahun, int Bulan)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdTagihan", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(int));
            tbl.Columns.Add("Nis", typeof(String));
            tbl.Columns.Add("NmLengkap", typeof(String));
            tbl.Columns.Add("Total", typeof(decimal));

            try
            {
                string sql =
                " Select ms.kd_siswa, ms.nis, ms.nama_lengkap, isnull(trn.kd_tagihan,'') kd_tagihan, isnull(trn.total,0) total "
                + " FROM ms_siswa ms "
                + " INNER JOIN kelas_siswa ks "
                + "     ON ks.kd_sekolah = ms.kd_sekolah"
                + "     AND ks.nis = ms.nis "
                + " LEFT OUTER JOIN "
                + " ( "
                + "     SELECT hdr.kd_tagihan, hdr.kd_siswa, hdr.total "
                + "     FROM ku_tagihan_siswa hdr "
                + "     WHERE hdr.periode= '" + Tahun.ToString() + Bulan.ToString().PadLeft(2,'0') + "'"
                + " ) trn"
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
                    baris["KdTagihan"] = AdnFungsi.CStr(rdr["kd_tagihan"]);
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
        public DataTable GetRincian(string KdTagihan)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("KdJenis", typeof(String));
            tbl.Columns.Add("ItemBulan", typeof(String));
            tbl.Columns.Add("Keterangan", typeof(String));
            tbl.Columns.Add("Jmh", typeof(decimal));
            tbl.Columns.Add("KdAkunPiutang", typeof(String));
            tbl.Columns.Add("KdAkunKewajiban", typeof(String));
            tbl.Columns.Add("KdAkunPendapatan", typeof(String));

            try
            {
                string sql =
                " select bs.kd_biaya, nm_biaya, isnull(jmh,0) jmh, item_bulan, ket, kd_jenis "
                + "     , kd_akun_piutang, kd_akun_kewajiban, kd_akun_pendapatan "
                + " from  ms_biaya bs "
                + " LEFT OUTER JOIN "
                + " ( "
                + "     SELECT kd_biaya, jmh, item_bulan, dtl.ket "
                + "     FROM ku_tagihan_siswa hdr "
                + "     INNER JOIN ku_tagihan_siswa_dtl dtl "
                + "         ON hdr.kd_tagihan = dtl.kd_tagihan "
                + "     WHERE hdr.kd_tagihan = '" + KdTagihan + "'"
                + " ) tgh"
                + " ON bs.kd_biaya = tgh.kd_biaya ";

                SqlCommand cmd = new SqlCommand(sql, this.cnn);
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                    baris["KdJenis"] = AdnFungsi.CStr(rdr["kd_jenis"]);
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

        public string GetPrefixTagihan(int TingkatSekolah)
        {
            string Hasil = "";

            switch (TingkatSekolah)
            {
                case 0:
                    Hasil = "TK";
                    break;
                case 1:
                    Hasil = "SD";
                    break;
                case 2:
                    Hasil = "SP";
                    break;
                case 3:
                    Hasil = "SA";
                    break;
            }

            return Hasil;
        }
        public string GetKode(int TkSekolah, string Periode)
        {
            string kode = "";
            string kolom = "kd_tagihan";
            int PanjangNoUrut = 4;
            string Prefix = "";
            switch (TkSekolah)
            {
                case 0:
                    Prefix = "TK";
                    break;
                case 1:
                    Prefix = "SD";
                    break;
                case 2:
                    Prefix = "SP";
                    break;
                case 3:
                    Prefix = "SA";
                    break;
            }

            Prefix = Prefix + Periode;

            sql =
            "SELECT isnull(max(right(rtrim(" + kolom + ")," + PanjangNoUrut +")),0) as kd  "
            + " FROM " + NAMA_TABEL
            + " where left(" + kolom + "," + Prefix.Length +") ='" + Prefix + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            kode = Prefix + iMax.ToString().PadLeft(PanjangNoUrut, '0');

            return kode;
        }

    }
}
