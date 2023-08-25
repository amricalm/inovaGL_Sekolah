using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaGL.Data
{
    public class AdnJurnalUmumDtlDao
    {
        private const short JUMLAH_KOLOM = 8;
        private const string NAMA_TABEL = "ac_tju_dtl";
        
        private string pkey = "kd_tju";
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

        public AdnJurnalUmumDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnJurnalUmumDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnJurnalUmumDtlDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
            this.trn = trn;
            this.cmd.Transaction = trn;
        }
        private void SetFldNilai(AdnJurnalUmumDtl o)
        {
            short idx = 0;

            fld[idx] = "kd_tju"; nilai[idx] = o.KdJU.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "no_urut"; nilai[idx] = o.NoUrut.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_project"; nilai[idx] = o.KdProject.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_dept"; nilai[idx] = o.KdDept.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "memo"; nilai[idx] = o.Memo.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "debet"; nilai[idx] = o.Debet.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kredit"; nilai[idx] = o.Kredit.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnJurnalUmumDtl o)
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
        public void Update(AdnJurnalUmumDtl o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdJU + "'";
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

        public List<AdnJurnalUmumDtl> Get(string kd)
        {
            List<AdnJurnalUmumDtl> lst = new List<AdnJurnalUmumDtl>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'"
            + " order by no_urut ";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnJurnalUmumDtl o = new AdnJurnalUmumDtl();
                    o.KdJU = kd;
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]);
                    o.NoUrut = AdnFungsi.CInt(rdr["no_urut"],true);
                    o.KdProject = AdnFungsi.CStr(rdr["kd_project"]);
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    o.Memo = AdnFungsi.CStr(rdr["memo"]);
                    o.Debet = AdnFungsi.CDec(rdr["debet"]);
                    o.Kredit = AdnFungsi.CDec(rdr["kredit"]);
                    lst.Add(o);
                }
                rdr.Close();
                foreach (AdnJurnalUmumDtl item in lst)
                {
                    item.Akun = new AdnAkunDao(this.cnn).Get(item.KdAkun);
                    if (item.KdDept != "")
                    {
                        item.Dept = new AdnDeptDao(this.cnn).Get(item.KdDept);
                    }
                }
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return lst;
        }
        
    }
}
