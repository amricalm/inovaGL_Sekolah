using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaPOS
{
    class AdnTReturBeliDtlDao
    {
        private const short JUMLAH_KOLOM = 3;
        private const string NAMA_TABEL = "ac_tretur_beli_dtl";
        private string pkey = "no_faktur";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnTReturBeliDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnTReturBeliDtl o)
        {
            short idx = 0;

            fld[idx] = "no_faktur"; nilai[idx] = o.no_faktur.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_barang"; nilai[idx] = o.kd_barang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "qty"; nilai[idx] = o.qty.ToString(); tipe[idx] = "n"; idx++;
 
        }

        public void Simpan(AdnTReturBeliDtl o)
        {
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
        }
        public void Update(AdnTReturBeliDtl o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.no_faktur.Trim() + "'";
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
        public List<AdnTReturBeliDtl> GetByNoFaktur(string kd)
        {
            List<AdnTReturBeliDtl> lst = new List<AdnTReturBeliDtl>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL + " dtl"
            + " inner join im_mbarang brg "
            + "     on dtl.kd_barang = brg.kd_barang "
            + " where " + this.pkey + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {   
                    AdnTReturBeliDtl o = new AdnTReturBeliDtl();
                    o.no_faktur = Convert.ToString(rdr["no_faktur"]).Trim();
                    o.kd_barang = Convert.ToString(rdr["kd_barang"]).Trim();
                    o.barang.barcode = Convert.ToString(rdr["barcode"]).Trim();
                    o.barang.nm_barang = Convert.ToString(rdr["nm_barang"]).Trim();

                    o.qty = Convert.ToInt32(rdr["qty"]);
                    o.kd_satuan = Convert.ToString(rdr["kd_satuan"]).Trim();
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
        //public List<AdnTReturBeliDtl> GetAll()
        //{
        //    List<AdnTReturBeliDtl> lst = new List<AdnTReturBeliDtl>();
        //    string sql =
        //    " select no_faktur, kd_barang, qty, kd_satuan, harga, diskon, uid, tgl_tambah, uid_edit, tgl_edit "
        //    + " from " + NAMA_TABEL;

        //    try
        //    {
        //        cmd.CommandText = sql;
        //        rdr = cmd.ExecuteReader();

        //        while (rdr.Read())
        //        {
        //            AdnTReturBeliDtl o = new AdnTReturBeliDtl();
        //            o.no_faktur = Convert.ToString(rdr["no_faktur"]).Trim();
        //            o.no_faktur_beli = Convert.ToString(rdr["no_faktur_beli"]).Trim();
        //            o.kd_barang = Convert.ToString(rdr["kd_barang"]).Trim();
        //            o.qty = Convert.ToInt32(rdr["qty"]);
        //            o.uid = Convert.ToString(rdr["uid"]).Trim();
        //            o.tgl_tambah = Convert.ToDateTime(rdr["tgl_tambah"]);
        //            o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
        //            o.tgl_edit = Convert.ToDateTime(rdr["tgl_edit"]);   
        //            lst.Add(o);
        //        }
        //        rdr.Close();
        //    }
        //    catch (DbException exp)
        //    {
        //        throw new Exception(exp.Message.ToString());
        //    }
        //    return lst; 
        //}
    }
}
