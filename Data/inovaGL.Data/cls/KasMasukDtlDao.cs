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
    public class AdnKasMasukDtlDao
    {
        private const short JUMLAH_KOLOM = 8;
        private const string NAMA_TABEL = "ac_tkm_dtl";
        
        private string pkey = "kd_tkm";
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

        public AdnKasMasukDtlDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnKasMasukDtlDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnKasMasukDtlDao(SqlConnection cnn, AdnScPengguna pengguna,SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.trn = trn;
            this.pengguna = pengguna;
        }



        private void SetFldNilai(AdnKasMasukDtl o)
        {
            short idx = 0;

            fld[idx] = "kd_tkm"; nilai[idx] = o.KdKM.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "no_urut"; nilai[idx] = o.NoUrut.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kd_project"; nilai[idx] = o.KdProject.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_dept"; nilai[idx] = o.KdDept.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "debet"; nilai[idx] = o.Debet.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "kredit"; nilai[idx] = o.Kredit.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "memo"; nilai[idx] = o.Memo.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnKasMasukDtl o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe);
            //try
            //{
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();               
            //}
            //catch(DbException exp)
            //{
            //    throw new Exception(exp.Message.ToString());
            //}
        }
        public void Update(AdnKasMasukDtl o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdKM + "'";
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

        public List<AdnKasMasukDtl> Get(string kd)
        {
            List<AdnKasMasukDtl> lst = new List<AdnKasMasukDtl>();
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
                    AdnKasMasukDtl o = new AdnKasMasukDtl();
                    o.KdKM = kd;
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
                foreach (AdnKasMasukDtl item in lst)
                {
                    item.Akun = new AdnAkunDao(this.cnn,this.pengguna,this.trn).Get(item.KdAkun);
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
        public DataTable Get2(string kd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKM", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(string));
            tbl.Columns.Add("KdProject", typeof(string));
            tbl.Columns.Add("KdDept", typeof(string));
            tbl.Columns.Add("Memo", typeof(string));
            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(String));
            tbl.Columns.Add("Kredit", typeof(String));

            string sql =
            " select kd_tkm, dtl.kd_akun, nm_akun, no_urut, debet,kredit, kd_project, kd_dept, memo "
            + " from " + NAMA_TABEL + " dtl "
            + " inner join ac_makun mak"
            + "     on dtl.kd_akun = mak.kd_akun "
            + " where " + this.pkey + " = '" + kd + "'";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdKM"] = AdnFungsi.CStr(rdr["kd_tkm"]);
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["Memo"] = AdnFungsi.CStr(rdr["memo"]);
                baris["Debet"] = AdnFungsi.CStr(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CStr(rdr["kredit"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
    }
}
