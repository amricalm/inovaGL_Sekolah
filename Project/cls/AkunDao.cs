using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using Andhana;

namespace inovaGL
{
    public class AdnAkunDao
    {
        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "ac_makun";
        
        private string pkey = "kd_akun";
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

        public AdnAkunDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnAkunDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnAkunDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = trn;
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnAkun o)
        {
            short idx = 0;

            fld[idx] = "kd_akun"; nilai[idx] = o.KdAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_akun"; nilai[idx] = o.NmAkun.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_induk"; nilai[idx] = o.KdInduk.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "turunan"; nilai[idx] = o.Turunan.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "dk"; nilai[idx] = o.DK.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tipe"; nilai[idx] = o.Tipe.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_gol"; nilai[idx] = o.KdGolongan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "aktif"; nilai[idx] = o.Aktif.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_dept"; nilai[idx] = o.KdDept.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnAkun o)
        {
            this.SetFldNilai(o);
            sql = AdnFungsi.SetStringInsertQry(NAMA_TABEL, fld, nilai, tipe,pengguna.nm_login);
            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();               
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
                MessageBox.Show(exp.Message, "Terjadi Kesalahan", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Update(AdnAkun o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdAkun + "'";
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

        public AdnAkun Get(string kd)
        {
            AdnAkun o = null;
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
                    o = new AdnAkun();
                    o.KdAkun = kd;
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]);
                    o.KdGolongan = AdnFungsi.CStr(rdr["kd_gol"]);
                    o.KdInduk = AdnFungsi.CStr(rdr["kd_induk"]);
                    o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                    o.Turunan = AdnFungsi.CShort(rdr["turunan"]);
                    o.DK = AdnFungsi.CStr(rdr["dk"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"], true);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return o;
        }
        public List<AdnAkun> GetAll()
        {
            List<AdnAkun> lst = new List<AdnAkun>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAkun o = new AdnAkun();
                    o.KdAkun = AdnFungsi.CStr(rdr["kd_akun"]) ;
                    o.NmAkun = AdnFungsi.CStr(rdr["nm_akun"]);
                    o.Turunan = AdnFungsi.CInt(rdr["turunan"],true);
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

        public DataTable GetByTingkat(int Tingkat)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));

            string sql =
            " select kd_akun, nm_akun "
            + " from " + NAMA_TABEL
            + " where turunan= " + Tingkat;

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf()
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));

            string sql =
            " select kd_akun, nm_akun, tipe "
            + " from " + NAMA_TABEL;

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf(string Tipe, bool BarisKosong)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));

            string sql =
            " select kd_akun, nm_akun "
            + " from " + NAMA_TABEL
            + " where tipe = '" + Tipe.ToString().Trim() + "'"
            + " order by kd_akun ";


            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDf(string KdGolongan)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));
            tbl.Columns.Add("KdInduk", typeof(String));
            tbl.Columns.Add("KdGolongan", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Turunan", typeof(short));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("Aktif", typeof(bool));

            string sql =
            " select * "
            + " from " + NAMA_TABEL;

            if (KdGolongan != "")
            {
                sql = sql + " Where kd_gol = '" + KdGolongan.Trim() + "'";
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Tipe"] = AdnFungsi.CStr(rdr["tipe"]);
                baris["KdInduk"] = AdnFungsi.CStr(rdr["kd_induk"]);
                baris["KdGolongan"] = AdnFungsi.CStr(rdr["kd_gol"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Turunan"] = AdnFungsi.CShort(rdr["turunan"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["Aktif"] = AdnFungsi.CBool(rdr["aktif"],true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public DataTable GetDfByJenis(string KdJenis)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));
            tbl.Columns.Add("KdInduk", typeof(String));
            tbl.Columns.Add("KdGolongan", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("Turunan", typeof(short));
            tbl.Columns.Add("KdDept", typeof(String));
            tbl.Columns.Add("Aktif", typeof(bool));

            string sql =
            " select * "
            + " from " + NAMA_TABEL;

            if (KdJenis != "")
            {
                sql = sql + " Where kd_jenis = '" + KdJenis.Trim() + "'";
            }

            SqlCommand cmd = new SqlCommand(sql, this.cnn);
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                DataRow baris = tbl.NewRow();
                baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                baris["Tipe"] = AdnFungsi.CStr(rdr["tipe"]);
                baris["KdInduk"] = AdnFungsi.CStr(rdr["kd_induk"]);
                baris["KdGolongan"] = AdnFungsi.CStr(rdr["kd_gol"]);
                baris["DK"] = AdnFungsi.CStr(rdr["dk"]);
                baris["Turunan"] = AdnFungsi.CShort(rdr["turunan"]);
                baris["KdDept"] = AdnFungsi.CStr(rdr["kd_dept"]);
                baris["Aktif"] = AdnFungsi.CBool(rdr["aktif"], true);

                tbl.Rows.Add(baris);
            }
            rdr.Close();
            return tbl; ;
        }
        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

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
        public bool SetComboDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

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
                    row[Display] = AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DataPropertyName = "KdAkun";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboAkunDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

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

            cbo.DataPropertyName = "KdAkun";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboAkunPendapatanDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_gol ='P'"
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomDisplay]);
                    lst.Rows.Add(row);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            cbo.DataPropertyName = "KdAkun";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

        public bool SetComboKas(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_akun";
            string KolomDisplay = "nm_akun";

            string Value = "KdAkun";
            string Display = "NmAkun";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " WHERE kd_jenis = '" + AdnVar.JenisAkun.KAS_BANK + "'"
            + " ORDER BY " + KolomValue;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    row = lst.NewRow();
                    row[Value] = AdnFungsi.CStr(rdr[KolomValue]);
                    row[Display] = AdnFungsi.CStr(rdr[KolomValue]) + " - " + AdnFungsi.CStr(rdr[KolomDisplay]);
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

        public bool ImportDbAccess()
        {
            bool Sukses = false;
            
            string sKoneksi = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DATA_BWA.mdb";// E:\\DOKUMEN\\TEst.mdb";
            OleDbConnection oCon = new OleDbConnection(sKoneksi);
            oCon.Open();

            string sql =
            " select *  "
            + " from ms_perkiraan";

            OleDbCommand cmd = new OleDbCommand(sql, oCon);
            OleDbDataReader rdr = cmd.ExecuteReader();

            List<AdnAkun> lst = new List<AdnAkun>();

            while (rdr.Read())
            {
                AdnAkun o = new AdnAkun();
                o.KdAkun = AdnFungsi.CStr(rdr["kode_perkiraan"]);
                o.NmAkun = AdnFungsi.CStr(rdr["nama_perkiraan"]);
                o.KdInduk = AdnFungsi.CStr(rdr["kode_induk"]);
                o.Turunan = AdnFungsi.CInt(rdr["turunan"],true);
                o.DK = AdnFungsi.CStr(rdr["kategori"]);
                o.Tipe = AdnFungsi.CStr(rdr["tipe"]);
                o.KdGolongan = AdnFungsi.CStr(rdr["kode_gol"]);
                o.KdDept = AdnFungsi.CStr(rdr["kode_dept"]);

                lst.Add(o);
            }
            rdr.Close();

            foreach (AdnAkun item in lst)
            {
                this.Simpan(item);
            }

            return Sukses; ;
        }
        public bool ImportDbAccessMsGolongan()
        {
            bool Sukses = false;

            string sKoneksi = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=DATA_BWA.mdb";// E:\\DOKUMEN\\TEst.mdb";
            OleDbConnection oCon = new OleDbConnection(sKoneksi);
            oCon.Open();

            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("KdGol", typeof(String));
            tbl.Columns.Add("NmGol", typeof(String));
            tbl.Columns.Add("Tipe", typeof(String));
            tbl.Columns.Add("NoUrut", typeof(Int32));

            string sql =
            " select *  "
            + " from ms_gol_perkiraan";

            OleDbCommand cmd = new OleDbCommand(sql, oCon);
            OleDbDataReader rdr = cmd.ExecuteReader();

            List<AdnAkun> lst = new List<AdnAkun>();

            while (rdr.Read())
            {
                DataRow row = tbl.NewRow();
                row["KdGol"] = AdnFungsi.CStr(rdr["kode_gol"]);
                row["NmGol"] = AdnFungsi.CStr(rdr["nama_gol"]);
                row["Tipe"] = AdnFungsi.CStr(rdr["kategori"]);
                row["NoUrut"] = AdnFungsi.CInt(rdr["urutan"], true);

                tbl.Rows.Add(row);
            }
            rdr.Close();

            foreach (DataRow row in tbl.Rows)
            {
                sql = "INSERT INTO ac_sys_gol_akun(kd_gol,nm_gol,tipe,no_urut)Values('" + row["KdGol"] + "','" + row["NmGol"] + "','" + row["Tipe"] + "'," + row["NoUrut"] + ")";

                this.cmd.CommandText = sql;
                this.cmd.ExecuteNonQuery();
            }

            return Sukses; ;
        }
        public short GetTurunan(string KdAkunInduk)
        {
            short hasil = 0;

            string sql =
            " select turunan "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + " = '" + KdAkunInduk.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    hasil= AdnFungsi.CShort(rdr["turunan"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return hasil;
        }
        public Int32 GetTingkatAkhir()
        {
            Int32 hasil = 0;

            string sql =
            " select max(turunan)  "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                object o = cmd.ExecuteScalar();
                hasil = Convert.ToInt32(o);
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }
            return hasil;
        }
        
    }
}
