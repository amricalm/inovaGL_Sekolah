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
    public class AdnAsetKelompokDao
    {
        private const short JUMLAH_KOLOM = 5;
        private const string NAMA_TABEL = "ac_maset_klp";
        
        private string pkey = "kd_klp";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnAsetKelompokDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnAsetKelompokDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnAsetKelompok o)
        {
            short idx = 0;

            fld[idx] = "kd_klp"; nilai[idx] = o.KdKelompokAset.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_klp"; nilai[idx] = o.NmKelompokAset.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "keterangan"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "coa_akum"; nilai[idx] = o.CoaAkumulasiPenyusutan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "coa_beban"; nilai[idx] = o.CoaBebanPenyusutan.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnAsetKelompok o)
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
                throw new Exception(exp.Message.ToString());
            }
        }
        public void Update(AdnAsetKelompok o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdKelompokAset + "'" ;
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

        public AdnAsetKelompok Get(string kd)
        {
            AdnAsetKelompok o = null;
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where kd_klp ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnAsetKelompok();
                    o.KdKelompokAset = AdnFungsi.CStr(rdr["kd_klp"]);
                    o.NmKelompokAset = AdnFungsi.CStr(rdr["nm_klp"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["keterangan"]);
                    o.CoaAkumulasiPenyusutan = AdnFungsi.CStr(rdr["coa_akum"]);
                    o.CoaBebanPenyusutan = AdnFungsi.CStr(rdr["coa_beban"]);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                throw new Exception(exp.Message.ToString());
            }

            return o;
        }
        public List<AdnAsetKelompok> GetAll()
        {
            List<AdnAsetKelompok> lst = new List<AdnAsetKelompok>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAsetKelompok o = new AdnAsetKelompok();
                    o.KdKelompokAset = AdnFungsi.CStr(rdr["kd_klp"]) ;
                    o.NmKelompokAset = AdnFungsi.CStr(rdr["nm_klp"]);
                    o.Keterangan = AdnFungsi.CStr(rdr["keterangan"]);
                    o.CoaAkumulasiPenyusutan = AdnFungsi.CStr(rdr["coa_akum"]);
                    o.CoaBebanPenyusutan = AdnFungsi.CStr(rdr["coa_beban"]);
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

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_klp";
            string KolomDisplay = "nm_klp";

            string Value = "KdKelompokAset";
            string Display = "NmKelompokAset";

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

            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;
            cbo.DataSource = lst;

            return true;
        }
        public bool SetComboDgv(System.Windows.Forms.DataGridViewComboBoxColumn cbo, bool TambahBarisKosong)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_klp";
            string KolomDisplay = "nm_klp";

            string Value = "kd_klp";
            string Display = "nm_klp";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue + "," + KolomDisplay
            + " FROM  " + NAMA_TABEL
            + " ORDER BY " + KolomDisplay;

            if (TambahBarisKosong)
            {
                row = lst.NewRow();
                row[Value] = "";
                row[Display] = "";
                lst.Rows.Add(row);
            }

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

            cbo.DataPropertyName = "kd_klp";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

    }
}
