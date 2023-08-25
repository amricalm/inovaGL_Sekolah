using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace inovaPOS
{
    public class BarangDao
    {
        private const short JUMLAH_KOLOM = 3;
        private const string NAMA_TABEL = "im_mbarang";
        private SqlConnection cnn;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public BarangDao(SqlConnection cnn)
        {
            this.cnn = cnn;
        }
        private void SetFldNilai(Barang o)
        {
            short idx = 0;

            fld[idx] = "kd_barang"; nilai[idx] = o.kd_barang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_barang"; nilai[idx] = o.nm_barang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "harga_jual"; nilai[idx] = o.harga_jual.ToString(); tipe[idx] = "n"; idx++;
        }
        public Barang Get(string kd_barang)
        {
            Barang o = null;
            string sql =
            " select nm_barang, harga_jual "
            + " from " + NAMA_TABEL
            + " where kd_barang ='" + kd_barang.Trim() + "'" ;

            SqlCommand cmd = new SqlCommand(sql,this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            if (rdr.Read())
            {
                o = new Barang();
                o.nm_barang = Convert.ToString(rdr["nm_barang"]).Trim();
                o.harga_jual = Convert.ToDecimal(rdr["harga_jual"]);
            }
            rdr.Close();
            return o;
        }
        public List<Barang> GetBy(string filter)
        {
            List<Barang> lst = new List<Barang>();
            string sql =
            " select kd_barang,nm_barang "
            + " from " + NAMA_TABEL
            + " where nm_barang LIKE '" + filter.Trim() + "%'";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Barang o = new Barang();
                o.kd_barang = Convert.ToString(rdr["kd_barang"]).Trim();
                o.nm_barang = Convert.ToString(rdr["nm_barang"]).Trim();
                lst.Add(o);
            }
            rdr.Close();
            return lst; 
        }
        public List<Barang>GetALL()
        {
            List<Barang> lst = new List<Barang>();
            string sql =
            " select kd_barang,nm_barang "
            + " from " + NAMA_TABEL;

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Barang o = new Barang();
                o.kd_barang = Convert.ToString(rdr["kd_barang"]).Trim();
                o.nm_barang = Convert.ToString(rdr["nm_barang"]).Trim();
                lst.Add(o);
            }
            rdr.Close();
            return lst;
        }
        public DataTable GetDf()
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));

            string sql =
            " select kd_barang,nm_barang "
            + " from " + NAMA_TABEL;

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                baris["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

    }
}
