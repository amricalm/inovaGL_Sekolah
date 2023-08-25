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
    public class AdnTReturBeliDao
    {
        private const short JUMLAH_KOLOM = 4;
        private const string NAMA_TABEL = "ac_tretur_beli";
        private string pkey = "no_faktur";

        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnTReturBeliDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        private void SetFldNilai(AdnTReturBeli o)
        {
            short idx = 0;

            fld[idx] = "no_faktur"; nilai[idx] = o.no_faktur.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "kd_ps"; nilai[idx] = o.kd_ps.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "ket"; nilai[idx] = o.ket.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnTReturBeli o)
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
        public void Update(AdnTReturBeli o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.no_faktur.Trim() + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere, o.uid);

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
        public AdnTReturBeli Get(string kd)
        {
            AdnTReturBeli o = null;
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
                    o = new AdnTReturBeli();
                    o.no_faktur = Convert.ToString(rdr["no_faktur"]).Trim();
                    o.tgl = Convert.ToDateTime(rdr["tgl"]);
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();

                    o.uid = Convert.ToString(rdr["uid"]).Trim();
                    o.tgl_tambah = Convert.ToDateTime(rdr["tgl_tambah"]);
                    o.uid_edit = Convert.ToString(rdr["uid_edit"]).Trim();
                    o.tgl_edit = Convert.ToDateTime(rdr["tgl_edit"]);                    
                }
                rdr.Close();

                if (o != null)
                {
                    o.item_df = new AdnTReturBeliDtlDao(this.cnn).GetByNoFaktur(o.no_faktur);
                }
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public DataTable GetByArgs(DateTime TglDr, DateTime TglSd, string KdPemasok)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("tgl", typeof(DateTime));
            tbl.Columns.Add("no_faktur", typeof(String));
            tbl.Columns.Add("nm_ps", typeof(String));
            tbl.Columns.Add("ket", typeof(String));
            tbl.Columns.Add("total", typeof(Decimal));

            string sql =
            " select hdr.no_faktur, tgl, nm_ps, ket"
            + " , sum(dtl.qty) total"
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tretur_beli_dtl dtl "
            + "     on hdr.no_faktur = dtl.no_faktur "
            + " left outer join im_mpemasok mps "
            + "     on hdr.kd_ps = mps.kd_ps "
            + " where tgl >= '" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
            + "     AND tgl < '" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            if (KdPemasok.Trim() != "")
            {
                sql = sql + " AND hdr.kd_ps = '" + KdPemasok.Trim() + "'";
            }

            sql = sql + " group by  hdr.no_faktur, tgl, nm_ps, ket ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = tbl.NewRow();
                    row["tgl"] = Convert.ToDateTime(rdr["tgl"]);
                    row["no_faktur"] = Convert.ToString(rdr["no_faktur"]).Trim();
                    row["nm_ps"] = Convert.ToString(rdr["nm_ps"]).Trim();
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
        public List<AdnTReturBeli> GetAll()
        {
            List<AdnTReturBeli> lst = new List<AdnTReturBeli>();
            string sql =
            " select no_faktur, tgl,kd_ps, ket, uid, tgl_tambah, uid_edit, tgl_edit "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnTReturBeli o = new AdnTReturBeli();
                    o.no_faktur = Convert.ToString(rdr["no_faktur"]).Trim();
                    o.tgl = Convert.ToDateTime(rdr["tgl"]);
                    o.kd_ps = Convert.ToString(rdr["kd_ps"]).Trim();
                    o.ket = Convert.ToString(rdr["ket"]).Trim();

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
    }
}
