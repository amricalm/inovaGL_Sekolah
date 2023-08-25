using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL
{
    public class AdnKasMasukDao
    {
        private const short JUMLAH_KOLOM = 5;
        private const string NAMA_TABEL = "ac_tkm";
        
        private string pkey = "kd_tkm";
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

        public AdnKasMasukDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnKasMasukDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }

        public AdnKasMasukDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;

        }
        private void SetFldNilai(AdnKasMasuk o)
        {
            short idx = 0;

            fld[idx] = "kd_tkm"; nilai[idx] = o.KdKM.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl"; nilai[idx] = o.Tgl.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "dr"; nilai[idx] = o.Dari.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "deskripsi"; nilai[idx] = o.Deskripsi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_jurnal"; nilai[idx] = o.KdJurnal.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnKasMasuk o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();               

                foreach(AdnKasMasukDtl item in o.ItemDf)
                {
                    new AdnKasMasukDtlDao(this.cnn,pengguna,this.trn).Simpan(item);
                }
            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Update(AdnKasMasuk o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdKM + "'";
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();

                new AdnKasMasukDtlDao(this.cnn,this.pengguna,this.trn).Hapus(o.KdKM);

                foreach (AdnKasMasukDtl item in o.ItemDf)
                {
                    new AdnKasMasukDtlDao(this.cnn,this.pengguna,this.trn).Simpan(item);
                }

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

        public AdnKasMasuk Get(string kd)
        {
            AdnKasMasuk o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnKasMasuk();
                    o.KdKM = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Dari = AdnFungsi.CStr(rdr["dr"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]);
                }
                rdr.Close();
                if (o != null)
                {
                    o.ItemDf = new AdnKasMasukDtlDao(this.cnn, this.pengguna, this.trn).Get(kd);
                }
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnKasMasuk> GetList(string kd)
        {
            List<AdnKasMasuk> lst = new List<AdnKasMasuk>();

            AdnKasMasuk o = null;
            string sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnKasMasuk();
                    o.KdKM = kd;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Dari = AdnFungsi.CStr(rdr["dr"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]);
                }
                rdr.Close();

                o.ItemDf = new AdnKasMasukDtlDao(this.cnn).Get(kd);
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            lst.Add(o);
            return lst;
        }
        public List<AdnKasMasuk> GetAll()
        {
            List<AdnKasMasuk> lst = new List<AdnKasMasuk>();
            sql =
            " select kd_tkm, tgl, dari, deskripsi "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnKasMasuk o = new AdnKasMasuk();
                    o.KdKM = AdnFungsi.CStr(rdr["kd_tkm"]) ;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Dari = AdnFungsi.CStr(rdr["dari"]);
                    o.Deskripsi = AdnFungsi.CStr(rdr["deskripsi"]);
                    o.KdJurnal = AdnFungsi.CStr(rdr["kd_jurnal"]);
                    lst.Add(o);
                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            return lst; 
        }

        public DataTable GetByArgs(string sFilter)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKM", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Dari", typeof(String));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdJurnal", typeof(String));

            string sql =
            " select kd_tkm, tgl, dr, deskripsi, kd_jurnal "
            + " from " + NAMA_TABEL;
            
            if (sFilter !="")
            {
                sql = sql + " WHERE " + sFilter;
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdKM"] = AdnFungsi.CStr(rdr["kd_tkm"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Dari"] = AdnFungsi.CStr(rdr["dr"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["KdJurnal"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

        public DataTable GetDonasiGroupByProgramByPeriode(DateTime Tgl, string KdKas)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("KdProgram", typeof(String));
            tbl.Columns.Add("NmProgram", typeof(String));
            tbl.Columns.Add("Jmh", typeof(Decimal));


            sql =
            " SELECT dtl.kd_program, nm_program, sum(jmh) jmh "
            + " FROM tdonasi hdr "
            + " INNER JOIN tdonasi_dtl dtl "
            + " 	ON hdr.no_kwitansi = dtl.no_kwitansi"
            + " INNER JOIN mprogram prg "
            + " 	ON dtl.kd_program = prg.kd_program "
            + " WHERE tgl ='" + AdnFungsi.SetSqlTglEN(Tgl) + "'";

            if(KdKas!="")
            {
                sql = sql + " AND hdr.kd_kas = '" + KdKas.ToString().Trim() + "'";
            }


            sql = sql +  " GROUP BY dtl.kd_program, nm_program ";


            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                row = tbl.NewRow();
                row["KdProgram"] = Convert.ToString(rdr["kd_program"]).Trim();
                row["NmProgram"] = Convert.ToString(rdr["nm_program"]).Trim();
                row["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);

                tbl.Rows.Add(row);

            }
            rdr.Close();
            //    //}
            //    //catch (DbException exp)
            //    //{
            //    //    throw new Exception(exp.Message.ToString());
            //    //}

            return tbl;

        }

        public bool BatchBkmDonasi(DateTime Tgl, AdnScPengguna pengguna)
        {
            bool Sukses = false;
            this.pengguna = pengguna;
            //-------------- Menentukan No Bukti
                string NoBukti = "";
                string sPrefix = "RV" + Tgl.ToString("yy") + Tgl.ToString("MM");
                string sql =
                "SELECT isnull(max(right(rtrim(" + this.pkey + "),3)),0) as kd  "
                + " FROM " + NAMA_TABEL
                + " WHERE left(" + this.pkey + ", 4)= '" + sPrefix + "'";

                cmd.CommandText = sql;
                int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                NoBukti= sPrefix + iMax.ToString().PadLeft(3,'0');
            //=============== END 


            sql =
            "select mkas.kd_akun as akun_kas, hdr.tgl, dtl.kd_program,mpg.kd_akun,debet, sum(jmh) jmh "
            + " from tdonasi hdr "
            + " inner join tdonasi_dtl dtl "
            + " 	on hdr.no_kwitansi = dtl.no_kwitansi "
            + " inner  join mprogram mpg "
            + " 	on dtl.kd_program = mpg.kd_program "
            + " inner join mkas "
            + "     on mkas.kd_kas = hdr.kd_kas "
            + " inner join "
            + "     ( "
            + "         select kd_kas, tgl, sum(jmh) debet "
            +   "       from tdonasi hdr "
            + "         inner join tdonasi_dtl dtl "
            + "             on hdr.no_kwitansi = dtl.no_kwitansi "
            + "         where tgl ='" + AdnFungsi.SetSqlTglEN(Tgl) + "'"
            + "         group by kd_kas, tgl "
            + "     ) tot "
            + "     on hdr.kd_kas = tot.kd_kas "
            + "     and hdr.tgl = tot.tgl "
            + " where hdr.tgl ='" + AdnFungsi.SetSqlTglEN(Tgl) + "'"
            + " group by mkas.kd_akun, hdr.tgl, dtl.kd_program, mpg.kd_akun,debet ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            List<AdnKasMasuk> lstKasMasuk = new List<AdnKasMasuk>();

            int cacah = 0;
            string LastKdKas = "";

            AdnKasMasuk o=null;

            List<AdnKasMasukDtl> lstItem=null;
            ArrayList ListKM = new ArrayList();
            while (rdr.Read())
            {
                if (LastKdKas != AdnFungsi.CStr(rdr["akun_kas"]))
                {
                    if (LastKdKas != "")//Bukan Pertama Kali
                    {
                        o.ItemDf = lstItem;
                        lstKasMasuk.Add(o);

                        iMax++;
                        NoBukti = sPrefix + iMax.ToString().PadLeft(3, '0');
                    }

                    LastKdKas = AdnFungsi.CStr(rdr["akun_kas"]);

                    o = new AdnKasMasuk();
                    lstItem = new List<AdnKasMasukDtl>();

                    o.KdKM = NoBukti;
                    o.Tgl = AdnFungsi.CDate(rdr["tgl"]);
                    o.Dari = "Wakif";
                    o.Deskripsi = " Donasi Rek. " + LastKdKas + ", #" + o.Tgl.ToString("yyyyMMdd") + "#";
                    o.KdJurnal = "";

                    //Debet
                    AdnKasMasukDtl itemDebet = new AdnKasMasukDtl();

                    itemDebet.KdKM = o.KdKM;
                    itemDebet.KdAkun = AdnFungsi.CStr(rdr["akun_kas"]);
                    itemDebet.NoUrut = cacah; cacah++;
                    itemDebet.Debet = AdnFungsi.CDec(rdr["debet"]);
                    itemDebet.Kredit = 0;

                    lstItem.Add(itemDebet);

                }

                AdnKasMasukDtl itemKredit = new AdnKasMasukDtl();

                itemKredit.KdKM = NoBukti;
                itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]);
                itemKredit.NoUrut = cacah; cacah++;
                itemKredit.Debet = 0;
                itemKredit.Kredit = AdnFungsi.CDec(rdr["jmh"]);

                lstItem.Add(itemKredit);
            }

            if (o != null)
            {
                o.ItemDf = lstItem;
                lstKasMasuk.Add(o);
            }

            rdr.Close();


            foreach (AdnKasMasuk item in lstKasMasuk)
            {
                this.Simpan(item);
                sql = "UPDATE tdonasi  "
                + " SET kd_tkm='" + item.KdKM + "'"
                + " FROM tdonasi hdr "
                + " INNER JOIN mkas "
                + "     ON hdr.kd_kas = mkas.kd_kas "
                + " WHERE kd_akun ='" + item.ItemDf[0].KdAkun.ToString().Trim() + "'"
                + "     AND tgl ='" + AdnFungsi.SetSqlTglEN(item.Tgl) + "'";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            Sukses = true;

            return Sukses;

        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_tkm";
            string KolomDisplay = "deskripsi";

            string Value = "KdKM";
            string Display = "Deskripsi";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }

        public DataTable GetLapBKM(string NoBKM)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdKM", typeof(String));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Dari", typeof(String));
            tbl.Columns.Add("Deskripsi", typeof(String));
            tbl.Columns.Add("KdJurnal", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));

            tbl.Columns.Add("KdProject", typeof(String));

            string sql =
            " select hdr.kd_tkm, tgl, dr, deskripsi, kd_jurnal, "
            + "     dtl.kd_akun, nm_akun, dtl.kd_project, debet, kredit "
            + " from " + NAMA_TABEL + " hdr"
            + " inner join ac_tkm_dtl dtl "
            + "     on hdr.kd_tkm = dtl.kd_tkm "
            + " inner join ac_makun mak "
            + "     on dtl.kd_akun = mak.kd_akun "
            + " where hdr.kd_tkm = '" + NoBKM.ToString().Trim() + "'"
            + " order by dtl.no_urut ";

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdKM"] = AdnFungsi.CStr(rdr["kd_tkm"]);
                baris["Tgl"] = AdnFungsi.CDate(rdr["tgl"]);
                baris["Dari"] = AdnFungsi.CStr(rdr["dr"]);
                baris["Deskripsi"] = AdnFungsi.CStr(rdr["deskripsi"]);
                baris["KdJurnal"] = AdnFungsi.CStr(rdr["kd_jurnal"]);
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);
                baris["KdProject"] = AdnFungsi.CStr(rdr["kd_project"]);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }

    }
}
