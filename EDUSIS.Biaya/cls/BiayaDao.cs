using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace EDUSIS.Biaya
{
    public class AdnBiayaDao
    {
        private const short JUMLAH_KOLOM = 12;
        private const string NAMA_TABEL = "ms_biaya";
        
        private string pkey = "kd_biaya";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        private AdnScPengguna pengguna;


        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnBiayaDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnBiayaDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnBiaya o)
        {
            short idx = 0;

            fld[idx] = "kd_biaya"; nilai[idx] = o.KdBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_biaya"; nilai[idx] = o.NmBiaya.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_jenis"; nilai[idx] = o.KdJenis.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "keterangan"; nilai[idx] = o.Keterangan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "gabung"; nilai[idx] = o.Gabungan.ToString(); tipe[idx] = "b"; idx++;
            fld[idx] = "rutin"; nilai[idx] = o.LaporanRutin.ToString(); tipe[idx] = "b"; idx++;
            fld[idx] = "psb"; nilai[idx] = o.LaporanPSB.ToString(); tipe[idx] = "b"; idx++;
            fld[idx] = "tdk_dijurnal"; nilai[idx] = o.TidakDijurnal.ToString(); tipe[idx] = "b"; idx++;
            fld[idx] = "kd_akun_piutang"; nilai[idx] = o.KdAkunPiutang.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_akun_pendapatan"; nilai[idx] = o.KdAkunPendapatan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_akun_kewajiban"; nilai[idx] = o.KdAkunKewajiban.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_akun_deposit"; nilai[idx] = o.KdAkunDeposit.ToString(); tipe[idx] = "s"; idx++;
        }

        public void Simpan(AdnBiaya o)
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
            }
        }
        public void Update(AdnBiaya o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdBiaya + "'" ;
            sql = AdnFungsi.SetStringUpdateQry(NAMA_TABEL, fld, nilai, tipe, sWhere,pengguna.nm_login);

            try
            {
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
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
                AdnFungsi.LogErr(exp.Message);
            }
        }

        public AdnBiaya Get(string Kd)
        {
            AdnBiaya o = null;

            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where " + this.pkey + "='" + Kd + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    o = new AdnBiaya();
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]) ;
                    o.NmBiaya = AdnFungsi.CStr(rdr["nm_biaya"]);
                    o.KdJenis = AdnFungsi.CStr(rdr["kd_jenis"]);

                    o.Keterangan = AdnFungsi.CStr(rdr["keterangan"]);
                    o.LaporanPSB = AdnFungsi.CBool(rdr["psb"],true);
                    o.TidakDijurnal = AdnFungsi.CBool(rdr["tdk_dijurnal"], true);
                    o.LaporanRutin = AdnFungsi.CBool(rdr["rutin"], true);
                    o.Gabungan = AdnFungsi.CBool(rdr["gabung"], true);

                    o.KdAkunPiutang = AdnFungsi.CStr(rdr["kd_akun_piutang"]);
                    o.KdAkunPendapatan = AdnFungsi.CStr(rdr["kd_akun_pendapatan"]);
                    o.KdAkunKewajiban = AdnFungsi.CStr(rdr["kd_akun_kewajiban"]);
                    o.KdAkunDeposit = AdnFungsi.CStr(rdr["kd_akun_deposit"]);

                }
                rdr.Close();
            }
            catch(DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return o; 
        }
        public List<AdnBiaya> GetAll()
        {
            List<AdnBiaya> lst = new List<AdnBiaya>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnBiaya o = new AdnBiaya();
                    o.KdBiaya = AdnFungsi.CStr(rdr["kd_biaya"]);
                    o.NmBiaya = AdnFungsi.CStr(rdr["nm_biaya"]);
                    o.KdJenis = AdnFungsi.CStr(rdr["kd_jenis"]);

                    o.Keterangan = AdnFungsi.CStr(rdr["keterangan"]);
                    o.LaporanPSB = AdnFungsi.CBool(rdr["psb"], true);
                    o.TidakDijurnal = AdnFungsi.CBool(rdr["tdk_dijurnal"], true);
                    o.LaporanRutin = AdnFungsi.CBool(rdr["rutin"], true);
                    o.Gabungan = AdnFungsi.CBool(rdr["gabung"], true);

                    o.KdAkunPiutang = AdnFungsi.CStr(rdr["kd_akun_piutang"]);
                    o.KdAkunPendapatan = AdnFungsi.CStr(rdr["kd_akun_pendapatan"]);
                    o.KdAkunKewajiban = AdnFungsi.CStr(rdr["kd_akun_kewajiban"]);
                    o.KdAkunDeposit = AdnFungsi.CStr(rdr["kd_akun_deposit"]);

                    lst.Add(o);
                }
                rdr.Close();
            }
            catch (DbException exp)
            {
                AdnFungsi.LogErr(exp.Message);
            }

            return lst;
        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_biaya";
            string KolomDisplay = "nm_biaya";

            string Value = "KdBiaya";
            string Display = "NmBiaya";

            lst.Columns.Add(Value, typeof(string));
            lst.Columns.Add(Display, typeof(string));

            string sql =
            "SELECT " + KolomValue 
            + " FROM  " + NAMA_TABEL
            + " ORDER BY " + KolomDisplay;

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
                AdnFungsi.LogErr(exp.Message);
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

            string KolomValue = "kd_biaya";
            string KolomDisplay = "nm_biaya";

            string Value = "KdBiaya";
            string Display = "NmBiaya";

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
                AdnFungsi.LogErr(exp.Message);
            }

            cbo.DataPropertyName = "KdBiaya";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

    }
}
