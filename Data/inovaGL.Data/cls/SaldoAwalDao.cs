using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL.Data
{
    public class AdnSaldoAwalDao
    {
        private const short JUMLAH_KOLOM = 4;
        private const string NAMA_TABEL = "ac_tsaldo_awal";
        
        private string pkey = "kd_akun";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
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
        private void SetFldNilai(AdnSaldoAwal o)
        {
            short idx = 0;

            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "debet"; nilai[idx] = o.Debet.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kredit"; nilai[idx] = o.Kredit.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnSaldoAwal o)
        {
            this.SetFldNilai(o);
            //cmd.ResetCommandTimeout();
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,"");
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
        public bool Update(AdnSaldoAwal o)
        {
            bool sukses = false;
            this.SetFldNilai(o);
            sWhere = " tgl ='" + AdnFungsi.SetSqlTglEN(o.Tgl) + "' AND kd_akun ='" + o.KdAkun.ToString().Trim() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere, pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                if (cmd.ExecuteNonQuery() > 0)
                {
                    sukses = true;
                }
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            return sukses;
        }
        public void Hapus(string kd,DateTime tgl)
        {

            sWhere = this.pkey + "='" + kd + "' AND tgl = '" + AdnFungsi.SetSqlTglEN(tgl) + "'";
            sql = "";
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
        public void Hapus(DateTime tgl)
        {

            sWhere = " AND tgl = '" + AdnFungsi.SetSqlTglEN(tgl) + "'";
            sql = "";
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

        public bool UpdatePiutang(DateTime Tgl)
        {
            bool hasil = false;
            //string sql = " SELECT kd_akun, sum(debet-kredit) jmh "
            //+ " FROM ac_tjurnal "
            //+ " WHERE tgl = '" + AdnFungsi.SetSqlTglEN(Tgl) + "'"
            //+ "     AND jn_jurnal = '" + AdnJurnalVar.JenisJurnal.JSAW_PIUTANG + "'"
            //+ " GROUP BY kd_akun ";


            return hasil;
        }

        public AdnSaldoAwal Get(string kd,DateTime tgl)
        {
            AdnSaldoAwal o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "' AND tgl = '" + tgl + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnSaldoAwal();
                    o.KdAkun = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Debet = AdnFungsi.CDec(rdr["debet"]);
                    o.Kredit = AdnFungsi.CDec(rdr["kredit"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnSaldoAwal> GetAll()
        {
            List<AdnSaldoAwal> lst = new List<AdnSaldoAwal>();
            sql =
            " select kd_akun, tgl, debet, kredit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnSaldoAwal o = new AdnSaldoAwal();
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]) ;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Debet = AdnFungsi.CDec(rdr["debet"]);
                    o.Kredit = AdnFungsi.CDec(rdr["kredit"]);
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
        public DataTable GetByArgs(string sFilter)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            string sql =
            " select kd_akun, tgl, debet, kredit "
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
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetLapBB(string KdAkun,string TglDari,string TglSampai,string KdProject, string KdProgram)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            string sql =
            " select dtl.kd_akun, nm_akun, dtl.kd_project, debet, kredit "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tkk_dtl dtl "
            + "     on hdr.kd_tkk = dtl.kd_tkk "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " where hdr.kd_tkk = '" + KdAkun.ToString().Trim() + "'"
            + " order by dtl.no_urut ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf(DateTime tgl)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            string sql =
            " select ac_makun.kd_akun, ac_makun.nm_akun, ac_tsaldo_awal.debet, ac_tsaldo_awal.kredit "
            + " from ac_makun "
            + " LEFT OUTER JOIN ac_tsaldo_awal "
            + "     ON ac_makun.kd_akun = ac_tsaldo_awal.kd_akun "
            + "     AND tgl = '" + AdnFungsi.SetSqlTglEN(tgl) + "'"
            + " WHERE tipe ='DTL' "
            + "     AND (kd_gol = 'H' OR kd_gol = 'K' OR kd_gol = 'M' ) "
            + " order by ac_makun.kd_akun ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf(string Kd, string kdGol, DateTime Tgl, string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdGol", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Lampiran", typeof(int));

            string sql =
            " select mak.kd_akun, mak.nm_akun, mak.dk,mak.kd_gol, sum(tr.debet) debet, sum(tr.kredit) kredit, sum(lampiran) lampiran "
            + " from ac_makun mak"
            + " INNER JOIN ac_sys_gol_akun sys "
            + "     ON mak.kd_gol = sys.kd_gol "
            + "     AND sys.laporan ='NRC'"

            + " LEFT OUTER JOIN "
            + " ("
            + "     SELECT kd_akun, debet, kredit, 0 as lampiran "
            + "     FROM ac_tsaldo_awal "
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(Tgl) + "'"

            + "     UNION "

            + "     SELECT kd_akun, sum(debet) debet, sum(kredit) kredit, 1 as lampiran "
            + "     FROM ac_tjurnal hdr"
            + "     INNER JOIN ac_tjurnal_dtl dtl"
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "


            + "     inner join ms_siswa sis "
            + "     on hdr.kd_siswa = sis.kd_siswa "
            + "     inner join kelas_siswa ks "
            + "         on ks.kd_sekolah = sis.kd_sekolah "
            + "         and ks.nis  = sis.nis "
            + "         and ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"

   
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(Tgl) + "'"
            + "         AND jn_jurnal = '" + AdnJurnalVar.JenisJurnal.JSAW_PIUTANG + "'"
            + "     GROUP BY kd_akun "

            + "     UNION "

            + "     SELECT kd_akun, sum(debet) debet, sum(kredit) kredit, 1 as lampiran "
            + "     FROM ac_tjurnal hdr"
            + "     INNER JOIN ac_tjurnal_dtl dtl"
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "

            + "     inner join ms_siswa sis "
            + "     on hdr.kd_siswa = sis.kd_siswa "
            + "     inner join kelas_siswa ks "
            + "         on ks.kd_sekolah = sis.kd_sekolah "
            + "         and ks.nis  = sis.nis "
            + "         and ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"
            
            
            + "     WHERE tgl = '" + AdnFungsi.SetSqlTglEN(Tgl) + "'"
            
            + "         AND jn_jurnal = '" + AdnJurnalVar.JenisJurnal.JSAW_UTANG + "'"
            + "     GROUP BY kd_akun "


            + " ) tr"
            + " ON mak.kd_akun = tr.kd_akun";

            sql += " WHERE mak.tipe = 'DTL'";
            if (Kd != "")
            {
                sql += " AND mak.kd_akun = '" + Kd + "'";
            }
            if (kdGol != "")
            {
                sql += " AND mak.kd_gol = '" + kdGol + "'";
            }

            sql += " GROUP BY mak.kd_akun, mak.nm_akun, mak.dk,mak.kd_gol ";
            sql += " order by mak.kd_akun ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();
            decimal Nilai = 0;

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["KdGol"] = AdnFungsi.CStr(rdr["kd_gol"]);
                if (AdnFungsi.CStr(rdr["dk"]) == AdnVar.SaldoNormal.DEBET)
                {
                    Nilai = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                    if (Nilai < 0)
                    {
                        baris["Debet"] = 0;
                        baris["Kredit"] = Nilai*-1;
                    }
                    else
                    {
                        baris["Debet"] = Nilai;
                        baris["Kredit"] = 0;
                    }
                }
                else
                {
                    Nilai = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                    if (Nilai < 0)
                    {
                        baris["Debet"] = Nilai*-1;
                        baris["Kredit"] = 0;
                    }
                    else
                    {
                        baris["Debet"] = 0;
                        baris["Kredit"] =Nilai;
                    }
                }
                //baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                //baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);
                baris["Lampiran"] = AdnFungsi.CInt(rdr["lampiran"],true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_tkk";
            string KolomDisplay = "deskripsi";

            string Value = "KdKK";
            string Display = "Deskripsi";

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
    }
}
