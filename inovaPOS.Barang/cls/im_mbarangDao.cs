using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaPOS
{
    public class AdnBarangDao
    {

        private const int DataAktif = 0;
        private static string[] TABEL_BARANG={"im_mbarang","ms_barang"};
        private static string[] KODE_KUNCI = { "kd_barang", "kode_barang" };

        private static string[] FldKdBarang = { "kd_barang", "kode_barang" };
        private static string[] FldBarcode = { "barcode", "barcode" };
        private static string[] FldKdGroup = { "kd_group", "kode_group" };
        private static string[] FldNmBarang = { "nm_barang", "nama_barang" };
        private static string[] FldKdSatuan = { "kd_satuan", "satuan" };
        private static string[] FldHargaJual = { "harga_jual", "harga_jual" };
        private static string[] FldHPP = { "hpp", "harga_pokok" };
        private static string[] FldKdPemasok = { "kd_pemasok", "kode_supplier" };
        private static string[] FldStockMin = { "stock_min", "stock_min" };


        
        private const short JUMLAH_KOLOM =9;
        
        private string NAMA_TABEL =  TABEL_BARANG[DataAktif];
        private string kd = KODE_KUNCI[DataAktif];
        
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnBarangDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnBarang o)
        {
            

            short idx = 0;
            fld[idx] = FldKdBarang[DataAktif]; nilai[idx] = o.kd_barang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = FldBarcode[DataAktif]; nilai[idx] = o.barcode.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = FldKdGroup[DataAktif]; nilai[idx] = o.kd_group.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = FldNmBarang[DataAktif]; nilai[idx] = o.nm_barang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = FldKdSatuan[DataAktif]; nilai[idx] = o.kd_satuan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = FldHargaJual[DataAktif]; nilai[idx] = o.harga_jual.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = FldHPP[DataAktif]; nilai[idx] = o.hpp.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = FldKdPemasok[DataAktif]; nilai[idx] = o.kd_pemasok.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = FldStockMin[DataAktif]; nilai[idx] = o.stock_min.ToString(); tipe[idx] = "n"; idx++;
            //fld[idx] = "kd_rak"; nilai[idx] = o.kd_rak.ToString(); tipe[idx] = "s"; idx++;
     
        }

        public void Simpan(AdnBarang o)
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
        public void Update(AdnBarang o)
        {
            this.SetFldNilai(o);
            sWhere = this.kd + "='" + o.kd_barang.ToString() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,o.uid_edit);

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
        public void UpdateHarga(AdnBarang o)
        {
            this.SetFldNilai(o);
            sWhere = this.kd + "='" + o.kd_barang.ToString() + "'";
            sql = "UPDATE " + NAMA_TABEL
            + " SET " + FldHargaJual[DataAktif] + " =" + o.harga_jual.ToString("#")
            + " WHERE " + sWhere;

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
            sWhere = this.kd + "='" + kd.Trim() + "'";
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
        
        public AdnBarang Get(string kd)
        {
            AdnBarang o = null;
            string sql =
            " select " 
            + FldKdBarang[DataAktif] + ","
            + FldBarcode[DataAktif] + ","
            + FldKdGroup[DataAktif] + ","
            + FldNmBarang[DataAktif] + ","
            + FldKdSatuan[DataAktif] + ","
            + FldHargaJual[DataAktif] + ","
            + FldHPP[DataAktif] + ","
            + FldKdPemasok[DataAktif] + ","
            + FldStockMin[DataAktif] + ","
            + " kd_rak, "
            + " uid, tgl_tambah,uid_edit, tgl_edit "
            + " from " + NAMA_TABEL
            + " where " + this.kd + " ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnBarang();
                    o.kd_barang = Convert.ToString(rdr["kd_barang"]).Trim();
                    o.barcode = Convert.ToString(rdr["barcode"]).Trim();
                    o.kd_group = Convert.ToString(rdr["kd_group"]).Trim();
                    o.nm_barang = Convert.ToString(rdr["nm_barang"]).Trim();
                    o.kd_satuan = Convert.ToString(rdr["kd_satuan"]).Trim();
                    o.harga_jual = Convert.ToDecimal(rdr["harga_jual"]);
                    o.hpp = AdnFungsi.CDec(rdr["hpp"]);
                    o.kd_pemasok = Convert.ToString(rdr["kd_pemasok"]).Trim();
                    o.stock_min = AdnFungsi.CInt(rdr["stock_min"],true);
                    o.kd_rak = Convert.ToString(rdr["kd_rak"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = AdnFungsi.CDate(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit =AdnFungsi.CDate(rdr["tgl_edit"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return o;
        }
        public AdnBarang GetByBarcode(string kd)
        {
            AdnBarang o = null;
            string sql =
            " select "
            + FldKdBarang[DataAktif] + ","
            + FldBarcode[DataAktif] + ","
            + FldKdGroup[DataAktif] + ","
            + FldNmBarang[DataAktif] + ","
            + FldKdSatuan[DataAktif] + ","
            + FldHargaJual[DataAktif] + ","
            + FldHPP[DataAktif] + ","
            + FldKdPemasok[DataAktif] + ","
            + FldStockMin[DataAktif] + ","
            + " kd_rak, "
            + " uid, tgl_tambah,uid_edit, tgl_edit "
            + " from " + NAMA_TABEL
            + " where barcode ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnBarang();
                    o.kd_barang = Convert.ToString(rdr["kd_barang"]).Trim();
                    o.barcode = Convert.ToString(rdr["barcode"]).Trim();
                    o.kd_group = Convert.ToString(rdr["kd_group"]).Trim();
                    o.nm_barang = Convert.ToString(rdr["nm_barang"]).Trim();
                    o.kd_satuan = Convert.ToString(rdr["kd_satuan"]).Trim();
                    o.harga_jual = Convert.ToDecimal(rdr["harga_jual"]);
                    o.hpp = AdnFungsi.CDec(rdr["hpp"]);
                    o.kd_pemasok = Convert.ToString(rdr["kd_pemasok"]).Trim();
                    o.stock_min = AdnFungsi.CInt(rdr["stock_min"], true);
                    o.kd_rak = Convert.ToString(rdr["kd_rak"]).Trim();
                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = Convert.ToDateTime(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = Convert.ToDateTime(rdr["tgl_edit"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }

        public List<AdnBarang> GetAll()
        {
            List<AdnBarang> lst = new List<AdnBarang>();
            string sql =
            " select kd_barang, barcode, kd_group, nm_barang, hdr.kd_satuan, "
            + " nm_satuan, harga_jual, hpp, kd_pemasok, stock_min, kd_rak"
            + " , hdr.uid, hdr.tgl_tambah, hdr.uid_edit, hdr.tgl_edit "
            + " from " + NAMA_TABEL + " hdr"
            + " LEFT OUTER JOIN im_msatuan stn "
            + "     ON  hdr.kd_satuan = stn.kd_satuan ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnBarang o = new AdnBarang();
                    o.kd_barang = Convert.ToString(rdr["kd_barang"]).Trim();
                    o.barcode = Convert.ToString(rdr["barcode"]).Trim();
                    o.kd_group = Convert.ToString(rdr["kd_group"]).Trim();
                    o.nm_barang = Convert.ToString(rdr["nm_barang"]).Trim();
                    o.kd_satuan = Convert.ToString(rdr["kd_satuan"]).Trim();
                    o.satuan.kd_satuan = o.kd_satuan;
                    o.satuan.nm_satuan = Convert.ToString(rdr["nm_satuan"]).Trim();
                    o.harga_jual = Convert.ToDecimal(rdr["harga_jual"]);
                    o.hpp = Convert.ToDecimal(rdr["hpp"]);
                    o.kd_pemasok = Convert.ToString(rdr["kd_pemasok"]).Trim();
                    o.stock_min = Convert.ToInt32(rdr["stock_min"]);
                    o.kd_rak = Convert.ToString(rdr["kd_rak"]).Trim();
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
        public DataTable GetByArgs(string kd_group , string kd_pemasok)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("barcode", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));
            tbl.Columns.Add("nm_satuan", typeof(String));
            tbl.Columns.Add("nm_group", typeof(String));
            tbl.Columns.Add("nm_ps", typeof(String));
            tbl.Columns.Add("harga_jual", typeof(Decimal));
            tbl.Columns.Add("hpp", typeof(Decimal));

            string sql =
            " select brg.kd_barang,barcode, nm_barang, nm_satuan,nm_group, nm_ps "
            + " , isnull(tmp.hpp,0) hpp, harga_jual "
            + " from " + NAMA_TABEL + " brg "
            + " left outer join im_msatuan sat "
            + "     on brg.kd_satuan = sat.kd_satuan "
            + " left outer join im_mpemasok pmk "
            + "     on brg.kd_pemasok = pmk.kd_ps "
            + " left outer join im_mgroup_barang grp"
            + "     on brg.kd_group = grp.kd_group"
            + " LEFT OUTER JOIN "
            + "     ( "
            + "         SELECT kd_barang, hpp = case sum(qty) when 0 then 0 else (sum(qty*harga))/sum(qty) end "
            + "         FROM ac_tbeli_dtl "
            + "         GROUP BY kd_barang "
            + "     ) tmp "
            + "     ON brg.kd_barang  = tmp.kd_barang ";

            if (kd_group.Trim() != "")
            {
                sql = sql + " WHERE brg.kd_group ='" + kd_group.Trim() + "'";
            }

            if (kd_pemasok.Trim() != "")
            {
                if (kd_group.Trim() == "")
                {
                    sql = sql + " WHERE ";
                }
                else
                {
                    sql = sql + " AND ";
                }
                sql = sql + " brg.kd_pemasok ='" + kd_pemasok.Trim() + "'";
            }


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                baris["barcode"] = Convert.ToString(rdr["kd_barang"]).Trim();
                baris["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                baris["nm_satuan"] = Convert.ToString(rdr["nm_satuan"]).Trim();
                baris["nm_group"] = Convert.ToString(rdr["nm_group"]).Trim();
                baris["nm_ps"] = Convert.ToString(rdr["nm_ps"]).Trim();
                baris["harga_jual"] = AdnFungsi.CDec(rdr["harga_jual"]);
                baris["hpp"] = AdnFungsi.CDec(rdr["hpp"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; 
        }
        public DataTable GetByArgs(string kd_pemasok,string KdBarang, string NmBarang)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("barcode", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));
            tbl.Columns.Add("nm_satuan", typeof(String));
            tbl.Columns.Add("nm_group", typeof(String));
            tbl.Columns.Add("kd_ps", typeof(String));

            string Where = "";
            string sql =
            " select brg.kd_barang,barcode, nm_barang, nm_satuan,nm_group, pmk.kd_ps "
            + " from " + NAMA_TABEL + " brg "
            + " left outer join im_msatuan sat "
            + "     on brg.kd_satuan = sat.kd_satuan "
            + " left outer join im_mbarang_pemasok pmk "
            + "     on brg.kd_barang = pmk.kd_barang ";

            if (kd_pemasok.Trim() != "")
            {
                sql = sql + "  AND pmk.kd_ps = '" + kd_pemasok.Trim() + "'";
            }

            sql= sql  + " left outer join im_mgroup_barang grp"
             + "     on brg.kd_group = grp.kd_group";



            if (KdBarang != "")
            {
                if (Where == "")
                {
                    sql = sql + " WHERE ";
                }
                else
                {
                    sql = sql + " AND ";
                }
                sql = sql + " brg.kd_barang ='" + KdBarang.ToString().Trim() + "'";
            }

            if (NmBarang != "")
            {
                if (Where == "")
                {
                    sql = sql + " WHERE ";
                }
                else
                {
                    sql = sql + " AND ";
                }
                sql = sql + " nm_barang ='" + KdBarang.ToString().Trim() + "'";
            }


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                baris["barcode"] = Convert.ToString(rdr["kd_barang"]).Trim();
                baris["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                baris["nm_satuan"] = Convert.ToString(rdr["nm_satuan"]).Trim();
                baris["nm_group"] = Convert.ToString(rdr["nm_group"]).Trim();
                baris["kd_ps"] = Convert.ToString(rdr["kd_ps"]).Trim();
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl;
        }
        public DataTable GetLapDaftarBarang(String KdBrDr, String KdBrSd, String KdGroup)
        {
            DataTable tbl = new DataTable("im_mbarang");
            DataRow row;
            
            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));
            tbl.Columns.Add("nm_satuan", typeof(String));
            tbl.Columns.Add("harga_jual", typeof(Decimal));
            tbl.Columns.Add("hpp", typeof(Decimal));
            tbl.Columns.Add("stock_min", typeof(Int32));

            String where = "WHERE brg.kd_barang BETWEEN '" + KdBrDr + "' AND '"+ KdBrSd + "'";
            String sql;
            if (KdGroup != "")
            {
                where = where + " AND brg.kd_group = '" + KdGroup + "'";
            }
            sql =
                " SELECT  brg.kd_barang, brg.nm_barang,   " +
                "   brg.kd_group, nm_group, " +
                "   nm_satuan, harga_jual,  " +
                "   hpp, stock_min " +
                " FROM im_mbarang brg " +
                " LEFT JOIN im_mgroup_barang grp " +
                "   ON brg.kd_group = grp.kd_group " +
                " INNER JOIN im_msatuan " +
                "   ON brg.kd_satuan = im_msatuan.kd_satuan ";

            
            sql = sql + where;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                    row["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                    row["nm_satuan"] = Convert.ToString(rdr["nm_satuan"]).Trim();
                    row["harga_jual"] = Convert.ToDecimal(rdr["harga_jual"]);
                    row["hpp"] = Convert.ToDecimal(rdr["hpp"]);
                    row["stock_min"] = Convert.ToInt32(rdr["stock_min"]);
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
        public DataTable GetLapDaftarBarang(String KdBrDr, String KdBrSd, String KdGroup, DateTime PeriodeAwal)
        {
            DataTable tbl = new DataTable("im_mbarang");
            DataRow row;
            
            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));
            tbl.Columns.Add("nm_satuan", typeof(String));
            tbl.Columns.Add("harga_jual", typeof(Decimal));
            tbl.Columns.Add("hpp", typeof(Decimal));
            tbl.Columns.Add("stock_min", typeof(Int32));

            String where = "WHERE brg.kd_barang BETWEEN '" + KdBrDr + "' AND '"+ KdBrSd + "'";
            String sql;
            if (KdGroup != "")
            {
                where = where + " AND brg.kd_group = '" + KdGroup + "'";
            }
            sql =
                " SELECT  brg.kd_barang, brg.nm_barang,   " +
                "   brg.kd_group, nm_group, " +
                "   nm_satuan, harga_jual,  " +
                "   thpp.hpp, stock_min " +
                " FROM im_mbarang brg " +
                " LEFT JOIN im_mgroup_barang grp " +
                "   ON brg.kd_group = grp.kd_group " +
                " INNER JOIN im_msatuan " +
                "   ON brg.kd_satuan = im_msatuan.kd_satuan "
                + " LEFT OUTER JOIN "
                + " ( "
                + "     SELECT kd_barang, hpp = case sum(qty) when 0 then 0 else (sum(qty*hpp))/sum(qty) end "
                + "     FROM "
                + "     ( "
                + "         SELECT kd_barang,sum(qty)qty, hpp = case sum(qty) when 0 then 0 else (sum(qty*harga))/sum(qty) end "
                + "         FROM ac_tbeli_dtl "
                + "         GROUP BY kd_barang "
                + "         UNION "
                + "         SELECT kd_barang, qty,hpp "
                + "         FROM im_tstock "
                + "         WHERE tgl = '" + AdnFungsi.SetSqlTglEN(PeriodeAwal) + "' "
                + "     ) TBL_HPP "
                + "     GROUP BY kd_barang "
                + " ) thpp " 
                + "     ON brg.kd_barang = thpp.kd_barang " ;

            
            sql = sql + where;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                    row["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                    row["nm_satuan"] = Convert.ToString(rdr["nm_satuan"]).Trim();
                    row["harga_jual"] = Convert.ToDecimal(rdr["harga_jual"]);
                    row["hpp"] =AdnFungsi.CDec(rdr["hpp"]);
                    row["stock_min"] = Convert.ToInt32(rdr["stock_min"]);
                    tbl.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            return tbl;
        }
        public DataTable GetAll(bool IsSort)
        {
            DataTable tbl = new DataTable("im_mbarang");
            DataRow row;

            tbl.Columns.Add("kd_barang", typeof(String));
            tbl.Columns.Add("barcode", typeof(String));
            tbl.Columns.Add("nm_barang", typeof(String));
            tbl.Columns.Add("nm_satuan", typeof(String));
            tbl.Columns.Add("harga_jual", typeof(Decimal));
            tbl.Columns.Add("hpp", typeof(Decimal));

            string sql =
            " select hdr.kd_barang, barcode, kd_group, nm_barang, hdr.kd_satuan, nm_satuan, harga_jual, kd_pemasok, stock_min, kd_rak"
            + " , hdr.uid, hdr.tgl_tambah, hdr.uid_edit, hdr.tgl_edit "
            + " , isnull(tmp.hpp,0) hpp "
            + " from " + NAMA_TABEL + " hdr"
            + " LEFT OUTER JOIN im_msatuan stn "
            + "     ON  hdr.kd_satuan = stn.kd_satuan "
            + " LEFT OUTER JOIN "
            + "     ( "
            + "         SELECT kd_barang, hpp = case sum(qty) when 0 then 0 else (sum(qty*harga))/sum(qty) end "
            + "         FROM ac_tbeli_dtl "
            + "         GROUP BY kd_barang "
            + "     ) tmp "
            + "     ON hdr.kd_barang  = tmp.kd_barang ";


            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["kd_barang"] = Convert.ToString(rdr["kd_barang"]).Trim();
                    row["barcode"] = Convert.ToString(rdr["barcode"]).Trim();
                    row["nm_barang"] = Convert.ToString(rdr["nm_barang"]).Trim();
                    row["nm_satuan"] = Convert.ToString(rdr["nm_satuan"]).Trim();
                    row["harga_jual"] = Convert.ToDecimal(rdr["harga_jual"]);
                    row["hpp"] = Convert.ToDecimal(rdr["hpp"]);
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
