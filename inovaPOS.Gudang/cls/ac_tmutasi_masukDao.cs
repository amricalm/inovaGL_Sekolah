using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaPOS
{
    public class AdnMutasiMasukDao
    {
        private const short JUMLAH_KOLOM = 7;
        private const string NAMA_TABEL = "ac_tmutasi_masuk";
        private string pkey = "no_faktur";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnMutasiMasukDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnMutasiMasuk o)
        {
            short idx = 0;

            fld[idx] = "no_faktur"; nilai[idx] = o.no_faktur.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "kd_gudang"; nilai[idx] = o.kd_gudang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.ket.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_term"; nilai[idx] = o.kd_term.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "diskon"; nilai[idx] = o.diskon.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "biaya_kirim"; nilai[idx] = o.biaya_kirim.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnMutasiMasuk o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,o.uid);
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
        public void Update(AdnMutasiMasuk o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.no_faktur.Trim() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,o.uid);

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
        public AdnMutasiMasuk Get(string kd)
        {
            AdnMutasiMasuk o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {   
                    o = new AdnMutasiMasuk();
                    o.no_faktur = Convert.ToString(rdr["no_faktur"]).Trim();
                    o.tgl = Convert.ToDateTime(rdr["tgl"]);
                    o.kd_gudang = Convert.ToString(rdr["kd_gudang"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
                    o.kd_term = Convert.ToString(rdr["kd_term"]).Trim();
                    o.diskon = AdnFungsi.CDec(rdr["diskon"]);
                    o.biaya_kirim = AdnFungsi.CDec(rdr["biaya_kirim"]);

                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = AdnFungsi.CDate(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = AdnFungsi.CDate(rdr["tgl_edit"]);
                }
                rdr.Close();

                if (o != null)
                {
                    o.item_df = new AdnMutasiMasukDtlDao(this.cnn).GetByNoFaktur(o.no_faktur);
                }
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnMutasiMasuk> GetAll()
        {
            List<AdnMutasiMasuk> lst = new List<AdnMutasiMasuk>();
            string sql =
            " select no_faktur, tgl, kd_gudang, ket, kd_term, diskon, biaya_kirim"
            + ", uid, tgl_tambah, uid_edit, tgl_edit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnMutasiMasuk o = new AdnMutasiMasuk();
                    o.no_faktur = Convert.ToString(rdr["no_faktur"]).Trim();
                    o.tgl = Convert.ToDateTime(rdr["tgl"]);
                    o.kd_gudang = Convert.ToString(rdr["kd_gudang"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
                    o.kd_term = Convert.ToString(rdr["kd_term"]).Trim();
                    o.diskon = AdnFungsi.CDec(rdr["diskon"]);
                    o.biaya_kirim = AdnFungsi.CDec(rdr["biaya_kirim"]);

                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = Convert.ToDateTime(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = Convert.ToDateTime(rdr["tgl_edit"]);   
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

        public List<AdnMutasiMasuk> GetByPeriode(DateTime TglDr, DateTime TglSd)
        {
            List<AdnMutasiMasuk> lst = new List<AdnMutasiMasuk>();
            string sql =
            " select no_faktur, tgl, kd_gudang, ket, kd_term, diskon, biaya_kirim "
            + " ,uid, tgl_tambah, uid_edit, tgl_edit "
            + " from " + NAMA_TABEL
            + " where tgl >=  '" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
            + "     AND tgl < '" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnMutasiMasuk o = new AdnMutasiMasuk();
                    o.no_faktur = Convert.ToString(rdr["no_faktur"]).Trim();
                    o.tgl = Convert.ToDateTime(rdr["tgl"]);
                    o.kd_gudang = Convert.ToString(rdr["kd_gudang"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();
                    o.kd_term = Convert.ToString(rdr["kd_term"]).Trim();
                    o.diskon = AdnFungsi.CDec(rdr["diskon"]);
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = Convert.ToDateTime(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = Convert.ToDateTime(rdr["tgl_edit"]);
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
        public DataTable GetByArgs (DateTime TglDr, DateTime TglSd,string KdPemasok)
        {
            DataTable tbl = new DataTable("ac_tmutasi_masuk");
            DataRow row;

            tbl.Columns.Add("tgl", typeof(DateTime));
            tbl.Columns.Add("no_faktur", typeof(String));
            tbl.Columns.Add("nm_gudang", typeof(String));
            tbl.Columns.Add("ket", typeof(String));
            tbl.Columns.Add("total", typeof(Decimal));

            string sql =
            " select hdr.no_faktur, tgl, nm_gudang, ket"
            + " , sum(dtl.qty * dtl.harga - (dtl.qty*dtl.harga *dtl.diskon/100)) total"
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tmutasi_masuk_dtl dtl "
            + "     on hdr.no_faktur = dtl.no_faktur "
            + " left outer join im_mgudang mgd "
            + "     on hdr.kd_gudang = mgd.kd_gudang "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
            + "     AND tgl < '" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdPemasok.Trim() != "")
            {
                sql = sql + " AND hdr.kd_gudang = '" + KdPemasok.Trim() + "'";
            }

            sql = sql + " group by  hdr.no_faktur, tgl, nm_gudang, ket ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["tgl"] = Convert.ToDateTime(rdr["tgl"]);
                    row["no_faktur"] = Convert.ToString(rdr["no_faktur"]).Trim();
                    row["nm_gudang"] = Convert.ToString(rdr["nm_gudang"]).Trim();
                    row["ket"] = Convert.ToString(rdr["ket"]).Trim();
                    row["total"] = Convert.ToDecimal(rdr["total"]);
                    tbl.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return tbl;
        }
        public DataTable GetLapPembelianByPeriode(DateTime TglDr, DateTime TglSd)
        {
            DataTable tbl = new DataTable("ac_tmutasi_masuk");
            DataRow row;

            tbl.Columns.Add("tgl", typeof(DateTime));
            tbl.Columns.Add("no_faktur", typeof(String));
            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));
            tbl.Columns.Add("qty", typeof(Int32));
            tbl.Columns.Add("harga", typeof(decimal));
            tbl.Columns.Add("diskon", typeof(decimal));
            tbl.Columns.Add("nm_satuan", typeof(String));

            String where = " WHERE  hdr.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "'  AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
            String sql =
                " SELECT hdr.tgl, hdr.no_faktur, " +
                " rtrim(dtl.kd_barang) as kd_barang,nm_barang,qty,harga,dtl.diskon,nm_satuan " +
                " FROM " + NAMA_TABEL + " hdr" +
                " INNER JOIN ac_tbeli_dtl dtl" +
                "   ON hdr.no_faktur = dtl.no_faktur " +
                " INNER JOIN im_mbarang " +
                "   ON dtl.kd_barang = im_mbarang.kd_barang  " +
                " LEFT OUTER JOIN im_msatuan stn" +
                "   ON im_mbarang.kd_satuan = stn.kd_satuan "+
                where;
            
            sql = sql + " ORDER BY hdr.tgl,hdr.no_faktur, nm_barang ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["tgl"] = Convert.ToDateTime(rdr["tgl"]);
                    row["no_faktur"] = Convert.ToString(rdr["no_faktur"]).Trim();
                    row["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                    row["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                    row["nm_satuan"] = Convert.ToString(rdr["nm_satuan"]).Trim();
                    row["qty"] = Convert.ToInt32(rdr["qty"]);
                    row["harga"] = Convert.ToDecimal(rdr["harga"]);
                    row["diskon"] = Convert.ToDecimal(rdr["diskon"]);
                    tbl.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return tbl;
        }
        public DataTable GetLapRekapPembelian(DateTime TglDr, DateTime TglSd, String kdBrDr,String KdBrSd)
        {
            DataTable tbl = new DataTable("ac_tmutasi_masuk");
            DataRow row;

            tbl.Columns.Add("nm_satuan", typeof(String));
            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));
            tbl.Columns.Add("qty", typeof(Int32));

            String where = " WHERE  ac_tbeli.tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND ac_tbeli.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";
            if (kdBrDr != "" && KdBrSd != "")
            {
                where = where + " AND im_mbarang.kd_barang BETWEEN '" + kdBrDr.Trim() + "' AND '" + KdBrSd.Trim() + "'";
            }
            string sql =
                " SELECT rtrim(ac_tbeli_dtl.kd_barang) as kd_barang, " +
                " nm_barang,sum(qty) as qty , nm_satuan " +
                " FROM " +
                " ( " +
                NAMA_TABEL + 
                " INNER JOIN ac_tbeli_dtl " +
                " ON ac_tbeli.no_faktur= ac_tbeli_dtl.no_faktur " +
                " )  "+
                " INNER JOIN im_mbarang " +
                " ON ac_tbeli_dtl.kd_barang = im_mbarang.kd_barang  " +
                " LEFT OUTER JOIN im_msatuan " +
                " ON ac_tbeli_dtl.kd_satuan = im_msatuan.kd_satuan  " 
                + where;
            sql = sql +
                " GROUP BY rtrim(ac_tbeli_dtl.kd_barang),nm_barang, im_msatuan.nm_satuan " +
                " ORDER BY nm_barang ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["nm_satuan"] = Convert.ToString(rdr["nm_satuan"]).Trim();
                    row["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                    row["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                    row["qty"] = Convert.ToInt32(rdr["qty"]);
                    tbl.Rows.Add(row);
                }

                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            return tbl;
        }
    }
}
