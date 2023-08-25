using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.VirtualAccount
{
    public class AdnVacDao
    {
        private const short JUMLAH_KOLOM = 3;
        private const string NAMA_TABEL = "siswa_ext";

        private string pkey = "kd_siswa";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnVacDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnVacDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnSiswaExt o)
        {
            short idx = 0;

            fld[idx] = "kd_siswa"; nilai[idx] = o.KdSiswa.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_ext"; nilai[idx] = o.KdExt.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "flag"; nilai[idx] = o.Flag.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnSiswaExt o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
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

        public int Update(AdnSiswaExt o)
        {
            int Hasil = 0;
            this.SetFldNilai(o);
            sWhere = this.pkey + "=" + o.KdSiswa ;
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere);

            try
            {
                cmd.CommandText = sql;
                Hasil = cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }
            return Hasil;
        }

        public DataTable GetSiswaExt(string NoVac, string Flag)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoVac", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(Int32));
            
            string sql =
            " select kd_ext, kd_siswa "
            + " from siswa_ext " 
            + " where kd_ext = '" + NoVac + "' AND flag ='" + Flag + "'";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoVac"] = AdnFungsi.CStr(rdr["kd_ext"]);
                baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"],true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetSiswaExt(string ThAjar, string KdSekolah, string Kelas, string Flag)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoVac", typeof(String));
            tbl.Columns.Add("KdSiswa", typeof(Int32));
            tbl.Columns.Add("NmSiswa", typeof(String));
            tbl.Columns.Add("Nis", typeof(String));

            string sql =
            " select kd_ext, sis.kd_siswa , sis.nis, sis.nama_lengkap "
            + " from ms_siswa sis "
            + " INNER JOIN kelas_siswa ks "
            + "     ON sis.kd_sekolah = ks.kd_sekolah "
            + "     AND sis.nis = ks.nis "
            + " LEFT OUTER JOIN siswa_ext ext "
            + "     ON sis.kd_siswa = ext.kd_siswa "
            + "     AND ext.flag ='" + Flag + "'"
            + " where ks.kd_sekolah = '" + KdSekolah + "' "
            + "     AND ks.th_ajar = '" + ThAjar + "' "
            + "     AND ks.kelas = '" + Kelas + "'";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["NoVac"] = AdnFungsi.CStr(rdr["kd_ext"]);
                baris["KdSiswa"] = AdnFungsi.CInt(rdr["kd_siswa"], true);
                baris["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                baris["NmSiswa"] = AdnFungsi.CStr(rdr["nama_lengkap"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

    }
}
