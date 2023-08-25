using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL
{
    public class AdnDeptDao
    {
        private const short JUMLAH_KOLOM = 2;
        private const string NAMA_TABEL = "ac_mdept";
        
        private string pkey = "kd_dept";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnDeptDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnDeptDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnDept o)
        {
            short idx = 0;

            fld[idx] = "kd_dept"; nilai[idx] = o.KdDept.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_dept"; nilai[idx] = o.NmDept.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnDept o)
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
        public void Update(AdnDept o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdDept + "'" ;
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

        public List<AdnDept> Get(string kd)
        {
            List<AdnDept> lst = new List<AdnDept>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            if (kd != "")
            {
                sql +=" where kd_dept ='" + kd.Trim() + "'";
            }

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnDept o = new AdnDept();
                    o.KdDept = AdnFungsi.CStr(rdr["kd_dept"]) ;
                    o.NmDept = AdnFungsi.CStr(rdr["nm_dept"]);
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

            string KolomValue = "kd_dept";
            string KolomDisplay = "nm_dept";

            string Value = "KdDept";
            string Display = "NmDept";

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

            string KolomValue = "kd_dept";
            string KolomDisplay = "nm_dept";

            string Value = "kd_dept";
            string Display = "nm_dept";

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

            cbo.DataPropertyName = "kd_dept";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

    }
}
