using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using Andhana;

namespace inovaGL.Data
{
    public class AdnJurnalDtlDao
    {
        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "ac_tjurnal_dtl";
        
        private string pkey = "kd_jurnal";
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

        public AdnJurnalDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnJurnalDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }

        public AdnJurnalDtlDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }



        private void SetFldNilai(AdnJurnalDtl o)
        {
            short idx = 0;

            fld[idx] = "kd_jurnal"; nilai[idx] = o.KdJurnal.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "no_urut"; nilai[idx] = o.NoUrut.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_project"; nilai[idx] = o.KdProject.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "sumber_dana"; nilai[idx] = o.SumberDana.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_dept"; nilai[idx] = o.KdDept.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "memo"; nilai[idx] = o.Memo.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "debet"; nilai[idx] = o.Debet.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kredit"; nilai[idx] = o.Kredit.ToString(); tipe[idx] = "n"; idx++;
        }

        public void Simpan(AdnJurnalDtl o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();               
        }
        public void Update(AdnJurnalDtl o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdJurnal + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }
        public void Hapus(string kd)
        {

            sWhere = this.pkey + "='" + kd + "'";
            sql = AdnFungsi.SetStringDeleteQry(NAMA_TABEL, sWhere);
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
        }

        public List<AdnJurnalDtl> Get(string kd)
        {
            List<AdnJurnalDtl> lst = new List<AdnJurnalDtl>();
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnJurnalDtl o = new AdnJurnalDtl();
                    o.KdJurnal = kd;
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]);
                    o.NoUrut = AdnFungsi.CInt(rdr["no_urut"],true);
                    o.KdProject = AdnFungsi.CStr(rdr["kd_project"]);
                    o.KdDept= AdnFungsi.CStr(rdr["kd_dept"]);
                    o.SumberDana = AdnFungsi.CStr(rdr["sumber_dana"]);
                    o.Memo= AdnFungsi.CStr(rdr["memo"]);
                    o.Debet = AdnFungsi.CDec(rdr["debet"]);
                    o.Kredit = AdnFungsi.CDec(rdr["kredit"]);
                    lst.Add(o);
                }
                rdr.Close();

                foreach (AdnJurnalDtl item in lst)
                {
                    item.Akun = new AdnAkunDao(this.cnn).Get(item.KdAkun);
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
