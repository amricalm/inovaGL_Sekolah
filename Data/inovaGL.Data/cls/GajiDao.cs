using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL.Data
{
    public class AdnGajiDao
    {
        private const short JUMLAH_KOLOM = 5;
        private const string NAMA_TABEL = "";
        
        private string pkey = "";
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

        public AdnGajiDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnGajiDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }

        public AdnGajiDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;

        }
        public DataTable GetByPeriode(int Tahun, int Bulan)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("NmDept", typeof(String));
            tbl.Columns.Add("Jmh", typeof(Decimal));
            tbl.Columns.Add("Status", typeof(string));

            string Periode = Tahun.ToString() + Bulan.ToString().PadLeft(2, '0');

            sql =
            " SELECT dep.kd_dept,nm_dept, sum(jmh) jmh "
            + " FROM gaji_karyawan gaj "
            + " INNER JOIN ms_karyawan kar "
            + "     ON gaj.nip = kar.nip "
            + " INNER JOIN ac_mdept dep "
            + "     ON dep.kd_dept = kar.unit_kerja "
            + " WHERE periode = '" + Periode + "'"
            + " AND gaj.posting = 0 "
            + " GROUP BY dep.kd_dept, nm_dept ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                row = tbl.NewRow();
                row["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                row["NmDept"] = AdnFungsi.CStr(rdr["nm_dept"]);
                row["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);
                row["Status"] = "BELUM DIPOSTING";

                tbl.Rows.Add(row);

            }
            rdr.Close();
            //}
            //catch (DbException exp)
            //{
            //    throw new Exception(exp.Message.ToString());
            //}

            return tbl;

        }

      
        private string SimpanJurnalHeader()
        {
            string Pesan="";

            AdnJurnalDao dao = new AdnJurnalDao(this.cnn);
            AdnJurnal Jurnal = new AdnJurnal();
            Jurnal.KdJurnal = "";
            

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            return Pesan;
        }
    }
}
