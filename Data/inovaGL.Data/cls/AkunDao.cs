using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using Andhana;
using inovaGL.Definisi;

namespace inovaGL.Data
{
    public class AdnAkunDao
    {
        private const short JUMLAH_KOLOM = 10;
        private const string NAMA_TABEL = "ac_makun";
        
        private string pkey = "kd_akun";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;
        private SqlTransaction trn;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnAkunDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnAkunDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnAkunDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnAkun o)
        {
            short idx = 0;

            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_akun"; nilai[idx] = o.NmAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_induk"; nilai[idx] = o.KdInduk.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "turunan"; nilai[idx] = o.Turunan.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "dk"; nilai[idx] = o.DK.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tipe"; nilai[idx] = o.Tipe.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_gol"; nilai[idx] = o.KdGolongan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "aktif"; nilai[idx] = o.Aktif.ToString(); tipe[idx] = "b"; idx++;
            fld[idx] = "kd_dept"; nilai[idx] = o.KdDept.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tampil_loket"; nilai[idx] = o.TampilDiLoket.ToString(); tipe[idx] = "b"; idx++;
        }

        public void Simpan(AdnAkun o)
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
                MessageBox.Show(exp.Message, "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Update(AdnAkun o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdAkun + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

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

        public AdnAkun Get(string kd)
        {
            AdnAkun o = null;
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
                    o = new AdnAkun();
                    o.KdAkun = kd;
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    o.KdGolongan = AdnFungsi.CStr(rdr["kd_gol"]);
                    o.KdInduk = AdnFungsi.CStr(rdr["kd_induk"]);
                    o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                    o.Turunan = AdnFungsi.CShort(rdr["turunan"]);
                    o.DK = AdnFungsi.CStr(rdr["dk"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"], true);
                    o.TampilDiLoket = AdnFungsi.CBool(rdr["tampil_loket"],true);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public AdnAkun Get(string kd, DateTime TglDr, DateTime TglSd, DateTime PeriodeMulai)
        {
            AdnAkun o = null;
            //string sql = "select mak.kd_akun,nm_akun, kd_dept, kd_gol, kd_induk, tipe, turunan, dk, aktif, debet,kredit "
            //    + " from ac_makun mak "
            //    + " left outer join ac_tsaldo_awal saw"
            //    + "     on mak.kd_akun = saw.kd_akun "
            //    + " where mak.kd_akun='" + kd.ToString().Trim() + "' "
            //    + " and saw.tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'";


            string sql =
            " select mak.kd_akun, mak.nm_akun, mak.kd_dept, mak.kd_gol, mak.kd_induk, mak.tipe, mak.turunan,  mak.dk, mak.aktif, sum(tr.debet) debet, sum(tr.kredit) kredit "
            + " from ac_makun mak"
            + " INNER JOIN ac_sys_gol_akun sys "
            + "     ON mak.kd_gol = sys.kd_gol "
            + "     AND sys.laporan ='NRC'"

            + " LEFT OUTER JOIN "
            + " ("
            + "     SELECT kd_akun, debet, kredit  "
            + "     FROM ac_tsaldo_awal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
            + "         AND kd_akun = '" + kd.ToString().Trim() + "' "

            + "     UNION "

            + "     SELECT kd_akun, sum(debet) debet, sum(kredit) kredit"
            + "     FROM ac_tjurnal hdr"
            + "     INNER JOIN ac_tjurnal_dtl dtl"
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
            + "         AND jn_jurnal = '" + AdnJurnalVar.JenisJurnal.JSAW_PIUTANG + "'"
            + "         AND dtl.kd_akun = '" + kd.ToString().Trim() + "' "
            + "     GROUP BY kd_akun "

            + "     UNION "

            + "     SELECT kd_akun, sum(debet) debet, sum(kredit) kredit  "
            + "     FROM ac_tjurnal hdr"
            + "     INNER JOIN ac_tjurnal_dtl dtl"
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
            + "         AND jn_jurnal = '" + AdnJurnalVar.JenisJurnal.JSAW_UTANG + "'"
            + "         AND dtl.kd_akun = '" + kd.ToString().Trim() + "' "
            + "     GROUP BY kd_akun "


            + " ) tr"
            + " ON mak.kd_akun = tr.kd_akun";

            sql += " WHERE mak.tipe = 'DTL'";
            if (kd != "")
            {
                sql += " AND mak.kd_akun = '" + kd + "'";
            }

            sql += " GROUP BY mak.kd_akun, mak.kd_dept, mak.tipe, mak.kd_induk,mak.turunan,mak.aktif, mak.nm_akun, mak.dk,mak.kd_gol ";
            sql += " order by mak.kd_akun ";









            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnAkun();
                    o.KdAkun = kd;
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    o.KdGolongan = AdnFungsi.CStr(rdr["kd_gol"]);
                    o.KdInduk = AdnFungsi.CStr(rdr["kd_induk"]);
                    o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                    o.Turunan = AdnFungsi.CShort(rdr["turunan"]);
                    o.DK = AdnFungsi.CStr(rdr["dk"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"], true);

                    switch (o.DK)
                    {
                        case "DEBET":
                            o.Saldo = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                            break;

                        case "KREDIT":
                            o.Saldo = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                            break;
                    }
                }
                rdr.Close();


                if (o != null)
                {
                    //Hitung Mutasi Antara Tanggal Saldo Awal sampai dengan Awal Periode (TglSaldoAwal --> TglDr)
                    sql = "SELECT kd_akun,SUM(dtl.debet)debet,SUM(dtl.kredit)kredit"
                    + " FROM ac_tjurnal hdr"
                    + " INNER JOIN ac_tjurnal_dtl dtl"
                    + " 	ON hdr.kd_jurnal = dtl.kd_jurnal"
                    + " WHERE hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' AND hdr.tgl<'" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
                    + " 	AND dtl.kd_akun='" + o.KdAkun.ToString().Trim() + "'"
                    + " GROUP BY kd_akun";

                    cmd.CommandText = sql;
                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        switch (o.DK.ToUpper())
                        {
                            case "DEBET":
                                o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                                break;

                            case "KREDIT":
                                o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                                break;
                        }
                    }
                    rdr.Close();

                    //Hitung Mutasi Akun dalam Periode Yang Diinginkan (TglDr --> TglSd)
                    sql = "SELECT kd_akun,SUM(dtl.debet)debet,SUM(dtl.kredit)kredit"
                    + " FROM ac_tjurnal hdr"
                    + " INNER JOIN ac_tjurnal_dtl dtl"
                    + " 	ON hdr.kd_jurnal = dtl.kd_jurnal"
                    + " WHERE hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl<'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                    + " 	AND dtl.kd_akun='" + o.KdAkun.ToString().Trim() + "'"
                    + " GROUP BY kd_akun";

                    cmd.CommandText = sql;
                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        switch (o.DK.ToUpper())
                        {
                            case "DEBET":
                                o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                                break;

                            case "KREDIT":
                                o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                                break;
                        }
                    }
                    rdr.Close();
                }

            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }


            return o;
        }
        public AdnAkun GetPengeluaranPerSumberDana(string kd, DateTime TglDr, DateTime TglSd, DateTime PeriodeMulai)
        {
            AdnAkun o = null;

            string sql =
            " select mak.kd_akun, mak.nm_akun, mak.kd_dept, mak.kd_gol, mak.kd_induk, mak.tipe, mak.turunan,  mak.dk, mak.aktif, sum(tr.debet) debet, sum(tr.kredit) kredit "
            + " from ac_makun mak"
            + " INNER JOIN ac_sys_gol_akun sys "
            + "     ON mak.kd_gol = sys.kd_gol "
            //+ "     AND sys.laporan ='NRC'"

            + " LEFT OUTER JOIN "
            + " ("
            + "     SELECT kd_akun, debet, kredit  "
            + "     FROM ac_tsaldo_awal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
            + "         AND kd_akun = '" + kd.ToString().Trim() + "' "

            + "     UNION "

            + "     SELECT kd_akun, sum(debet) debet, sum(kredit) kredit"
            + "     FROM ac_tjurnal hdr"
            + "     INNER JOIN ac_tjurnal_dtl dtl"
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
            + "         AND jn_jurnal = '" + AdnJurnalVar.JenisJurnal.JSAW_PIUTANG + "'"
            + "         AND dtl.kd_akun = '" + kd.ToString().Trim() + "' "
            + "     GROUP BY kd_akun "

            + "     UNION "

            + "     SELECT kd_akun, sum(debet) debet, sum(kredit) kredit  "
            + "     FROM ac_tjurnal hdr"
            + "     INNER JOIN ac_tjurnal_dtl dtl"
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
            + "         AND jn_jurnal = '" + AdnJurnalVar.JenisJurnal.JSAW_UTANG + "'"
            + "         AND dtl.kd_akun = '" + kd.ToString().Trim() + "' "
            + "     GROUP BY kd_akun "

            + "     UNION "

            + "     SELECT kd_akun, sum(debet) debet, sum(kredit) kredit  "
            + "     FROM ac_tjurnal hdr"
            + "     INNER JOIN ac_tjurnal_dtl dtl"
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
            + "     GROUP BY kd_akun "


            + " ) tr"
            + " ON mak.kd_akun = tr.kd_akun";

            sql += " WHERE mak.tipe = 'DTL'";
            if (kd != "")
            {
                sql += " AND mak.kd_akun = '" + kd + "'";
            }

            sql += " GROUP BY mak.kd_akun, mak.kd_dept, mak.tipe, mak.kd_induk,mak.turunan,mak.aktif, mak.nm_akun, mak.dk,mak.kd_gol ";
            sql += " order by mak.kd_akun ";



            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnAkun();
                    o.KdAkun = kd;
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    o.KdGolongan = AdnFungsi.CStr(rdr["kd_gol"]);
                    o.KdInduk = AdnFungsi.CStr(rdr["kd_induk"]);
                    o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                    o.Turunan = AdnFungsi.CShort(rdr["turunan"]);
                    o.DK = AdnFungsi.CStr(rdr["dk"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"], true);

                    switch (o.DK)
                    {
                        case "DEBET":
                            o.Saldo = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                            break;

                        case "KREDIT":
                            o.Saldo = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                            break;
                    }
                }
                rdr.Close();


                if (o != null)
                {
                    //Hitung Mutasi Pengeluaran Antara Tanggal Saldo Awal sampai dengan Awal Periode (TglSaldoAwal --> TglDr)
                    sql = "SELECT SUM(dtl.debet)debet"
                    + " FROM ac_tjurnal hdr"
                    + " INNER JOIN ac_tjurnal_dtl dtl"
                    + " 	ON hdr.kd_jurnal = dtl.kd_jurnal"
                    + " WHERE hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' AND hdr.tgl<'" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
                    + " 	AND dtl.sumber_dana='" + o.KdAkun.ToString().Trim() + "'";

                    cmd.CommandText = sql;
                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        o.Saldo = o.Saldo - AdnFungsi.CDec(rdr["debet"]);
                        //switch (o.DK.ToUpper())
                        //{
                        //    case "DEBET":
                        //        o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        //        break;

                        //    case "KREDIT":
                        //        o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        //        break;
                        //}
                    }
                    rdr.Close();

                    //Hitung Mutasi Akun dalam Periode Yang Diinginkan (TglDr --> TglSd)
                    sql = "SELECT kd_akun,SUM(dtl.debet)debet,SUM(dtl.kredit)kredit"
                    + " FROM ac_tjurnal hdr"
                    + " INNER JOIN ac_tjurnal_dtl dtl"
                    + " 	ON hdr.kd_jurnal = dtl.kd_jurnal"
                    + " WHERE hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl<'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                    + " 	AND dtl.sumber_dana='" + o.KdAkun.ToString().Trim() + "'"
                    + " GROUP BY kd_akun";

                    cmd.CommandText = sql;
                    rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        switch (o.DK.ToUpper())
                        {
                            case "DEBET":
                                o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                                break;

                            case "KREDIT":
                                o.Saldo = o.Saldo + AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                                break;
                        }
                    }
                    rdr.Close();
                }

            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }


            return o;
        }
        public List<AdnAkun> GetAll()
        {
            List<AdnAkun> lst = new List<AdnAkun>();
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " order by kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAkun o = new AdnAkun();
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]) ;
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
                    o.Turunan = AdnFungsi.CInt(rdr["turunan"],true);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            return lst; 
        }

        public List<AdnAkun> GetAll(DateTime TglDr, DateTime TglSd, DateTime PeriodeMulai, string ThAjar)
        {
            List<AdnAkun> lst = new List<AdnAkun>();
            string sql = "select mak.kd_akun,nm_akun, kd_dept, kd_gol, kd_induk, tipe, turunan, dk, aktif,"
                + " (isnull(saw.debet,0)+isnull(sau.debet,0)) debet,(isnull(saw.kredit,0)+isnull(sau.kredit,0)) kredit "
                + " from ac_makun mak "
                + " left outer join ac_tsaldo_awal saw"
                + "     on mak.kd_akun = saw.kd_akun "
                + "     and saw.tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"

                + "     left outer join "
                + "     ("
                + "     select dtl.kd_akun, sum(dtl.debet) debet, sum(dtl.kredit) kredit"
                + "     from ac_tjurnal hdr"
                + "     inner join ac_tjurnal_dtl dtl"
                + "     	on hdr.kd_jurnal = dtl.kd_jurnal"
                
                + "     inner join ms_siswa sis "
                + "     on hdr.kd_siswa = sis.kd_siswa "
                + "     inner join kelas_siswa ks "
                + "         on ks.kd_sekolah = sis.kd_sekolah "
                + "         and ks.nis  = sis.nis "
                + "         and ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"


                + "     where hdr.tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
                + "     and (jn_jurnal = 'JSU' OR jn_jurnal = 'JSP') "
                + "     group by dtl.kd_akun"
                + "     ) sau"
                + "     on mak.kd_akun = sau.kd_akun"

                //+ "     left outer join "
                //+ "     ("
                //+ "     select dtl.kd_akun, sum(dtl.debet) debet, sum(dtl.kredit) kredit"
                //+ "     from ac_tjurnal hdr"
                //+ "     inner join ac_tjurnal_dtl dtl"
                //+ "     	on hdr.kd_jurnal = dtl.kd_jurnal"
                //+ "     where jn_jurnal = 'JSP'"
                //+ "     and hdr.tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "'"
                //+ "     group by dtl.kd_akun"
                //+ "     ) sap"
                //+ "     on mak.kd_akun = sap.kd_akun"

                + " where tipe ='DTL'"
                + " order by mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAkun o = new AdnAkun();
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]);
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    o.KdGolongan = AdnFungsi.CStr(rdr["kd_gol"]);
                    o.KdInduk = AdnFungsi.CStr(rdr["kd_induk"]);
                    o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                    o.Turunan = AdnFungsi.CShort(rdr["turunan"]);
                    o.DK = AdnFungsi.CStr(rdr["dk"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"], true);

                    switch (o.DK)
                    {
                        case "DEBET":
                            o.SaldoAwal = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                            break;

                        case "KREDIT":
                            o.SaldoAwal = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                            break;
                    }
                    lst.Add(o);
                }
                rdr.Close();

                if (lst.Count > 0)
                {
                    //Hitung Mutasi Antara Tanggal Saldo Awal sampai dengan Awal Periode (TglSaldoAwal --> TglDr)
                    sql = "SELECT mak.kd_akun,SUM(isnull(debet,0))debet,SUM(isnull(kredit,0))kredit"
                    + " FROM ac_makun mak "
                    + " LEFT OUTER JOIN "
                    + " ( ";

                    sql += "SELECT kd_akun,SUM(dtl.debet)debet,SUM(dtl.kredit)kredit"
                    + " FROM ac_tjurnal hdr"
                    + " INNER JOIN ac_tjurnal_dtl dtl"
                    + " 	ON hdr.kd_jurnal = dtl.kd_jurnal"
                    + " WHERE hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' AND hdr.tgl<'" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
                    + "     AND ((jn_jurnal <> 'JSU' AND jn_jurnal <>'JSP')  OR hdr.jn_jurnal IS NULL ) "
                    + " GROUP BY kd_akun";

                    sql +=
                    " ) TR "
                    + " ON mak.kd_akun = TR.kd_akun "
                    + " where tipe ='DTL'"
                    + " GROUP BY mak.kd_akun "
                    + " ORDER BY mak.kd_akun ";

                    cmd.CommandText = sql;
                    rdr = cmd.ExecuteReader();
                    int idx = 0;
                    while (rdr.Read())
                    {

                        switch (lst[idx].DK.ToUpper())
                        {
                            case "DEBET":
                                lst[idx].SaldoAwal = lst[idx].SaldoAwal + AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                                if (lst[idx].SaldoAwal > 0)
                                {
                                    lst[idx].SaldoAwalDebet = lst[idx].SaldoAwal;
                                    lst[idx].SaldoAwalKredit = 0;
                                }
                                else
                                {
                                    lst[idx].SaldoAwalDebet = 0;
                                    lst[idx].SaldoAwalKredit = -1 * lst[idx].SaldoAwal;
                                }
                                break;

                            case "KREDIT":
                                lst[idx].SaldoAwal = lst[idx].SaldoAwal + AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                                if (lst[idx].SaldoAwal > 0)
                                {
                                    lst[idx].SaldoAwalKredit = lst[idx].SaldoAwal;
                                    lst[idx].SaldoAwalDebet = 0;
                                }
                                else
                                {
                                    lst[idx].SaldoAwalKredit = 0;
                                    lst[idx].SaldoAwalDebet = -1 * lst[idx].SaldoAwal;
                                }
                                break;
                        }
                        idx++;
                    }
                    rdr.Close();

                    //Hitung Mutasi Akun dalam Periode Yang Diinginkan (TglDr --> TglSd)
                    sql = "SELECT mak.kd_akun,SUM(isnull(debet,0))debet,SUM(isnull(kredit,0))kredit"
                    + " FROM ac_makun mak "
                    + " LEFT OUTER JOIN "
                    + " ( ";

                    sql += "SELECT kd_akun,SUM(dtl.debet)debet,SUM(dtl.kredit)kredit"
                    + " FROM ac_tjurnal hdr"
                    + " INNER JOIN ac_tjurnal_dtl dtl"
                    + " 	ON hdr.kd_jurnal = dtl.kd_jurnal"
                    + " WHERE hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl<'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                    + "     AND  ((hdr.jn_jurnal <> 'JSU' AND  hdr.jn_jurnal <> 'JSP') OR hdr.jn_jurnal IS NULL ) "
                    + " GROUP BY kd_akun";

                    sql +=
                    " ) TR "
                    + " ON mak.kd_akun = TR.kd_akun "
                    + " where tipe ='DTL'"
                    + " GROUP BY mak.kd_akun "
                    + " ORDER BY mak.kd_akun";

                    cmd.CommandText = sql;
                    rdr = cmd.ExecuteReader();

                    idx = 0;
                    while (rdr.Read())
                    {
                        //switch (lst[idx].DK.ToUpper())
                        //{
                        //    case "DEBET":
                        //        lst[idx].Mutasi = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        //        if (lst[idx].Mutasi > 0)
                        //        {
                        //            lst[idx].MutasiDebet = lst[idx].Mutasi;
                        //            lst[idx].MutasiKredit = 0;
                        //        }
                        //        else
                        //        {
                        //            lst[idx].MutasiDebet = 0;
                        //            lst[idx].MutasiKredit = -1 * lst[idx].Mutasi;
                        //        }
                        //        break;

                        //    case "KREDIT":
                        //        lst[idx].Mutasi = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        //        if (lst[idx].Mutasi > 0)
                        //        {
                        //            lst[idx].MutasiDebet = 0;
                        //            lst[idx].MutasiKredit = lst[idx].Mutasi;
                        //        }
                        //        else
                        //        {
                        //            lst[idx].MutasiDebet = -1 * lst[idx].Mutasi;
                        //            lst[idx].MutasiKredit = 0;
                        //        }
                        //        break;

                        lst[idx].MutasiDebet = AdnFungsi.CDec(rdr["debet"]);
                        lst[idx].MutasiKredit = AdnFungsi.CDec(rdr["kredit"]);

                        switch (lst[idx].DK.ToUpper())
                        {
                            case "DEBET":
                                lst[idx].Mutasi = lst[idx].MutasiDebet - lst[idx].MutasiKredit;// AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                                //if (lst[idx].Mutasi > 0)
                                //{
                                //    lst[idx].MutasiDebet = lst[idx].Mutasi;
                                //    lst[idx].MutasiKredit = 0;
                                //}
                                //else
                                //{
                                //    lst[idx].MutasiDebet = 0;
                                //    lst[idx].MutasiKredit = -1 * lst[idx].Mutasi;
                                //}
                                break;

                            case "KREDIT":
                                lst[idx].Mutasi = lst[idx].MutasiKredit - lst[idx].MutasiDebet;//AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                                //if (lst[idx].Mutasi > 0)
                                //{
                                //    lst[idx].MutasiDebet = 0;
                                //    lst[idx].MutasiKredit = lst[idx].Mutasi;
                                //}
                                //else
                                //{
                                //    lst[idx].MutasiDebet = -1 * lst[idx].Mutasi;
                                //    lst[idx].MutasiKredit = 0;
                                //}
                                break;

                        }
                        
                        //lst[idx].Saldo = lst[idx].SaldoAwal + lst[idx].Mutasi;
                        lst[idx].Saldo = lst[idx].SaldoAwal + lst[idx].Mutasi;
                        lst[idx].SaldoDebet = lst[idx].SaldoAwalDebet + lst[idx].MutasiDebet;
                        lst[idx].SaldoKredit = lst[idx].SaldoAwalKredit + lst[idx].MutasiKredit;
                        //switch (lst[idx].DK.ToUpper())
                        //{
                        //    case "DEBET":
                        //        if (lst[idx].Saldo> 0)
                        //        {
                        //            lst[idx].SaldoDebet = lst[idx].Saldo;
                        //            lst[idx].SaldoKredit = 0;
                        //        }
                        //        else
                        //        {
                        //            lst[idx].SaldoDebet = 0;
                        //            lst[idx].SaldoKredit = -1 * lst[idx].Saldo;
                        //        }
                        //        break;

                        //    case "KREDIT":
                        //        if (lst[idx].Saldo > 0)
                        //        {
                        //            lst[idx].SaldoDebet = 0;
                        //            lst[idx].SaldoKredit = lst[idx].Saldo;
                        //        }
                        //        else
                        //        {
                        //            lst[idx].SaldoDebet= -1 * lst[idx].Saldo;
                        //            lst[idx].SaldoKredit = 0;
                        //        }
                        //        break;
                        //}
                        if (lst[idx].DK.ToUpper() == "DEBET")
                        {
                            lst[idx].SaldoDebet = lst[idx].SaldoDebet - lst[idx].SaldoKredit;
                            lst[idx].SaldoKredit = 0;
                            if (lst[idx].SaldoDebet < 0)
                            {
                                lst[idx].SaldoKredit = lst[idx].SaldoDebet * -1;
                                lst[idx].SaldoDebet = 0;
                            }
                        }
                        else
                        {
                            lst[idx].SaldoKredit = lst[idx].SaldoKredit - lst[idx].SaldoDebet;
                            lst[idx].SaldoDebet = 0;
                            if (lst[idx].SaldoKredit < 0)
                            {
                                lst[idx].SaldoDebet = lst[idx].SaldoKredit * -1;
                                lst[idx].SaldoKredit = 0;
                            }
                        }

                        //lst[idx].SaldoDebet = lst[idx].SaldoAwalDebet + lst[idx].MutasiDebet;
                        //lst[idx].SaldoKredit = lst[idx].SaldoAwalKredit + lst[idx].MutasiKredit;
                        idx++;
                    }
                    rdr.Close();
                }


            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }


            return lst;
        }

        public List<AdnAkun> GetPiutangBiaya()
        {
            List<AdnAkun> lst = new List<AdnAkun>();
            sql =
            " select distinct mak.kd_akun, mak.nm_akun "
            + " from ac_makun mak "
            + " inner join ms_biaya bya "
            + "     on mak.kd_akun = bya.kd_akun_piutang "
            + " order by mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAkun o = new AdnAkun();
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]);
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
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
        public List<AdnAkun> GetUtangBiaya()
        {
            List<AdnAkun> lst = new List<AdnAkun>();
            sql =
            " select distinct mak.kd_akun, mak.nm_akun "
            + " from ac_makun mak "
            + " inner join ms_biaya bya "
            + "     on mak.kd_akun = bya.kd_akun_kewajiban "
            + " order by mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAkun o = new AdnAkun();
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]);
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
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
        public DataTable GetByTingkat(int Tingkat)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));

            string sql =
            " select kd_akun, nm_akun "
            + " from " + NAMA_TABEL
            + " where turunan= " + Tingkat;

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf()
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));

            string sql =
            " select kd_akun, nm_akun, tipe "
            + " from " + NAMA_TABEL
            + " ORDER BY kd_akun ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf(string Tipe, bool BarisKosong)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));

            string sql =
            " select kd_akun, nm_akun "
            + " from " + NAMA_TABEL
            + " where tipe = '" + Tipe.ToString().Trim() + "'"
            + " order by kd_akun ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf(string KdGolongan)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));
            tbl.Columns.Add("KdInduk", typeof(String));
            tbl.Columns.Add("KdGolongan", typeof(String));
            tbl.Columns.Add("NmGolongan", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Turunan", typeof(short));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("Aktif", typeof(bool));

            string sql =
            " select * "
            + " from " + NAMA_TABEL + " ms"
            + " LEFT OUTER JOIN ac_sys_gol_akun sys "
            + "     ON ms.kd_gol = sys.kd_gol ";


            if (KdGolongan != "")
            {
                sql = sql + " Where ms.kd_gol = '" + KdGolongan.Trim() + "'";
            }

            sql = sql + " ORDER BY kd_akun ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Tipe"] = AdnFungsi.CStr(rdr["tipe"]);
                baris["KdInduk"] = AdnFungsi.CStr(rdr["kd_induk"]);
                baris["KdGolongan"] = AdnFungsi.CStr(rdr["kd_gol"]);
                baris["NmGolongan"] = AdnFungsi.CStr(rdr["nm_gol"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Turunan"] = AdnFungsi.CShort(rdr["turunan"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["Aktif"] = AdnFungsi.CBool(rdr["aktif"], true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDfByGolDetail(string KdGolongan)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));
            tbl.Columns.Add("KdInduk", typeof(String));
            tbl.Columns.Add("KdGolongan", typeof(String));
            tbl.Columns.Add("NmGolongan", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Turunan", typeof(short));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("Aktif", typeof(bool));

            string sql =
            " select * "
            + " from " + NAMA_TABEL + " ms"
            + " LEFT OUTER JOIN ac_sys_gol_akun sys "
            + "     ON ms.kd_gol = sys.kd_gol "
            + " WHERE ms.tipe ='DTL' ";


            if (KdGolongan != "")
            {
                sql = sql + " AND ms.kd_gol = '" + KdGolongan.Trim() + "'";
            }

            sql = sql + " ORDER BY kd_akun ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Tipe"] = AdnFungsi.CStr(rdr["tipe"]);
                baris["KdInduk"] = AdnFungsi.CStr(rdr["kd_induk"]);
                baris["KdGolongan"] = AdnFungsi.CStr(rdr["kd_gol"]);
                baris["NmGolongan"] = AdnFungsi.CStr(rdr["nm_gol"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Turunan"] = AdnFungsi.CShort(rdr["turunan"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["Aktif"] = AdnFungsi.CBool(rdr["aktif"], true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDfByJenis(string KdJenis)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));
            tbl.Columns.Add("KdInduk", typeof(String));
            tbl.Columns.Add("KdGolongan", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Turunan", typeof(short));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("Aktif", typeof(bool));

            string sql =
            " select * "
            + " from " + NAMA_TABEL;

            if (KdJenis != "")
            {
                sql = sql + " Where kd_jenis = '" + KdJenis.Trim() + "'";
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Tipe"] = AdnFungsi.CStr(rdr["tipe"]);
                baris["KdInduk"] = AdnFungsi.CStr(rdr["kd_induk"]);
                baris["KdGolongan"] = AdnFungsi.CStr(rdr["kd_gol"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Turunan"] = AdnFungsi.CShort(rdr["turunan"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["Aktif"] = AdnFungsi.CBool(rdr["aktif"], true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

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
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetCombo2(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

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
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]) + " -" + AdnFungsi.CStr(rdr[KolomDisplay]); ;
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

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
                throw new Exception(exp.Message.ToString());
            }

            cbo.DataPropertyName = "KdAkun";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboAkunDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

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
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DataPropertyName = "KdAkun";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboAkunPendapatanDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_gol ='PDT' "
            + "     AND tipe = '" + AdnVar.TipeAkun.DTL + "'"
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
                throw new Exception(exp.Message.ToString());
            }

            cbo.DataPropertyName = "KdAkun";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

        public bool SetComboByGol(System.Windows.Forms.ComboBox cbo, string KdGol)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_gol ='" + KdGol.Trim() + "'"
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]) + " - " + AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboByGolDetail(System.Windows.Forms.ComboBox cbo, string KdGol)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE  tipe = '" + AdnVar.TipeAkun.DTL + "'";

            if (KdGol!="")
            {
                sql = sql +  "     AND kd_gol ='" + KdGol.Trim() + "'";
            }
            
            sql = sql +  " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]) + " - " + AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }

        public bool SetComboKas(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_gol = '" + AdnVar.JenisAkun.KAS_BANK + "'"
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]) + " - " + AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboKasDetail(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_gol = '" + AdnVar.JenisAkun.KAS_BANK + "'"
            + "     AND tipe = '" + AdnVar.TipeAkun.DTL + "'"
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]) + " - " + AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboBiayaDetail(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_gol = '" + AdnVar.JenisAkun.BEBAN + "'"
            + "     AND tipe = '" + AdnVar.TipeAkun.DTL + "'"
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]) + " - " + AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool ImportDbAccess()
        {
            bool Sukses = false;
            
            string sKoneksi = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DATA_BWA.mdb";// E:\\DOKUMEN\\TEst.mdb";
            OleDbConnection oCon = new OleDbConnection(sKoneksi);
            oCon.Open();

            string sql =
            " select *  "
            + " from ms_perkiraan";

            OleDbCommand cmd = new OleDbCommand(sql, oCon);
            OleDbDataReader rdr = cmd.ExecuteReader();

            List<AdnAkun> lst = new List<AdnAkun>();

            while (rdr.Read())
            {
                AdnAkun o = new AdnAkun();
                o.KdAkun = AdnFungsi.CStr(rdr["kode_perkiraan"]);
                o.NmAkun = AdnFungsi.CStr(rdr["nama_perkiraan"]);
                o.KdInduk = AdnFungsi.CStr(rdr["kode_induk"]);
                o.Turunan = AdnFungsi.CInt(rdr["turunan"],true);
                o.DK = AdnFungsi.CStr(rdr["kategori"]);
                o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                o.KdGolongan = AdnFungsi.CStr(rdr["kode_gol"]);
                o.KdDept = AdnFungsi.CStr(rdr["kode_dept"]);

                lst.Add(o);
            }
            rdr.Close();

            foreach (AdnAkun item in lst)
            {
                this.Simpan(item);
            }

            return Sukses; ;
        }
        public bool ImportDbAccessMsGolongan()
        {
            bool Sukses = false;

            string sKoneksi = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DATA_BWA.mdb";// E:\\DOKUMEN\\TEst.mdb";
            OleDbConnection oCon = new OleDbConnection(sKoneksi);
            oCon.Open();

            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdGol", typeof(String));
            tbl.Columns.Add("NmGol", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));
            tbl.Columns.Add("NoUrut", typeof(Int32));

            string sql =
            " select *  "
            + " from ms_gol_perkiraan";

            OleDbCommand cmd = new OleDbCommand(sql, oCon);
            OleDbDataReader rdr = cmd.ExecuteReader();

            List<AdnAkun> lst = new List<AdnAkun>();

            while (rdr.Read())
            {
                DataRow row = tbl.NewRow();
                row["KdGol"] = AdnFungsi.CStr(rdr["kode_gol"]);
                row["NmGol"] = AdnFungsi.CStr(rdr["nama_gol"]);
                row["Tipe"] = AdnFungsi.CStr(rdr["kategori"]);
                row["NoUrut"] = AdnFungsi.CInt(rdr["urutan"], true);

                tbl.Rows.Add(row);
            }
            rdr.Close();

            foreach (DataRow row in tbl.Rows)
            {
                sql = "INSERT INTO ac_sys_gol_akun(kd_gol,nm_gol,tipe,no_urut)Values('" + row["KdGol"] + "','" + row["NmGol"] + "','" + row["Tipe"] + "'," + row["NoUrut"] + ")";

                this.cmd.CommandText = sql;
                this.cmd.ExecuteNonQuery();
            }

            return Sukses; ;
        }
        public short GetTurunan(string KdAkunInduk)
        {
            short hasil = 0;

            string sql =
            " select turunan "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + KdAkunInduk.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    hasil= AdnFungsi.CShort(rdr["turunan"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return hasil;
        }
        public Int32 GetTingkatAkhir()
        {
            Int32 hasil = 0;

            string sql =
            " select max(turunan)  "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                object o = cmd.ExecuteScalar();
                hasil = Convert.ToInt32(o);
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return hasil;
        }
        
    }
}
