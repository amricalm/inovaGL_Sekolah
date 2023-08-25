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
    public class AdnAsetDao
    {
        private const short JUMLAH_KOLOM = 19;
        private const string NAMA_TABEL = "ac_maset";
        
        private string pkey = "kd_aset";
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

        public AdnAsetDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnAsetDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }
        public AdnAsetDao(SqlConnection cnn, AdnScPengguna pengguna,SqlTransaction Trans)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.trn = Trans;
            this.cmd.Transaction = Trans;
            this.pengguna = pengguna;
        }
        private void SetFldNilai(AdnAset o)
        {
            short idx = 0;

            fld[idx] = "kd_aset"; nilai[idx] = o.KdAset.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "nm_aset"; nilai[idx] = o.NmAset.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "kd_klp"; nilai[idx] = o.KdKelompokAset.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "lokasi"; nilai[idx] = o.Lokasi.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "departemen"; nilai[idx] = o.Departemen.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "merk"; nilai[idx] = o.Merk.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "model"; nilai[idx] = o.Model.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "serial_no"; nilai[idx] = o.SerialNo.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "tgl_beli"; nilai[idx] = o.TglBeli.ToString(); tipe[idx] = "d"; idx++;
            fld[idx] = "jenis_umur"; nilai[idx] = o.JenisUmur.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "umur"; nilai[idx] = o.Umur.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "qty"; nilai[idx] = o.Qty.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "harga"; nilai[idx] = o.Harga.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "total"; nilai[idx] = o.Total.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "nilai_residu"; nilai[idx] = o.NilaiResidu.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "nilai_buku"; nilai[idx] = o.NilaiBuku.ToString(); tipe[idx] = "n"; idx++;
            fld[idx] = "coa_akum"; nilai[idx] = o.CoaAkumulasiPenyusutan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "coa_beban"; nilai[idx] = o.CoaBebanPenyusutan.ToString(); tipe[idx] = "s"; idx++;
            fld[idx] = "aktif"; nilai[idx] = o.Aktif.ToString(); tipe[idx] = "b"; idx++;
        }

        public void Simpan(AdnAset o)
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
        public void Update(AdnAset o)
        {
            this.SetFldNilai(o);
            sWhere = this.pkey + "='" + o.KdAset + "'" ;
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

        public AdnAset Get(string kd)
        {
            AdnAset o = null;
            sql =
            " select * "
            + " from " + NAMA_TABEL
            + " where kd_aset ='" + kd.Trim() + "'";

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                if (rdr.Read())
                {
                    o = new AdnAset();
                    o.KdAset = AdnFungsi.CStr(rdr["kd_aset"]);
                    o.NmAset = AdnFungsi.CStr(rdr["nm_aset"]);
                    o.KdKelompokAset = AdnFungsi.CStr(rdr["kd_klp"]);
                    o.Lokasi = AdnFungsi.CStr(rdr["lokasi"]);
                    o.Departemen = AdnFungsi.CStr(rdr["departemen"]);
                    o.Merk = AdnFungsi.CStr(rdr["merk"]);
                    o.Model = AdnFungsi.CStr(rdr["model"]);
                    o.SerialNo = AdnFungsi.CStr(rdr["serial_no"]);
                    o.TglBeli = AdnFungsi.CDate(rdr["tgl_beli"]);
                    o.JenisUmur = AdnFungsi.CStr(rdr["jenis_umur"]);
                    o.Umur = AdnFungsi.CInt(rdr["umur"],true);
                    o.Qty = AdnFungsi.CInt(rdr["qty"],true);
                    o.Harga = AdnFungsi.CDec(rdr["harga"]);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                    o.NilaiResidu = AdnFungsi.CDec(rdr["nilai_residu"]);
                    o.NilaiBuku = AdnFungsi.CDec(rdr["nilai_buku"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"],true);
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
        public List<AdnAset> GetAll()
        {
            List<AdnAset> lst = new List<AdnAset>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAset o = new AdnAset();
                    o.KdAset = AdnFungsi.CStr(rdr["kd_klp"]);
                    o.NmAset = AdnFungsi.CStr(rdr["nm_klp"]);
                    o.KdKelompokAset = AdnFungsi.CStr(rdr["kd_klp"]);
                    o.Lokasi = AdnFungsi.CStr(rdr["lokasi"]);
                    o.Departemen = AdnFungsi.CStr(rdr["departemen"]);
                    o.Merk = AdnFungsi.CStr(rdr["merk"]);
                    o.Model = AdnFungsi.CStr(rdr["model"]);
                    o.SerialNo = AdnFungsi.CStr(rdr["serial_no"]);
                    o.TglBeli = AdnFungsi.CDate(rdr["tgl_beli"]);
                    o.JenisUmur = AdnFungsi.CStr(rdr["jenis_umur"]);
                    o.Umur = AdnFungsi.CInt(rdr["umur"], true);
                    o.Qty = AdnFungsi.CInt(rdr["qty"], true);
                    o.Harga = AdnFungsi.CDec(rdr["harga"]);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                    o.NilaiResidu = AdnFungsi.CDec(rdr["nilai_residu"]);
                    o.NilaiBuku = AdnFungsi.CDec(rdr["nilai_buku"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"], true);
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
        public List<AdnAset> GetByKelompok(string KdKelompokAset)
        {
            List<AdnAset> lst = new List<AdnAset>();
            sql =
            " select * "
            + " from " + NAMA_TABEL;
            
            if (KdKelompokAset.ToString().Trim() != "")
            {
                sql += " WHERE kd_klp ='" + KdKelompokAset.ToString().Trim() + "'";
            }

            try
            {
                cmd.CommandText = sql;
                rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    AdnAset o = new AdnAset();
                    o.KdAset = AdnFungsi.CStr(rdr["kd_aset"]);
                    o.NmAset = AdnFungsi.CStr(rdr["nm_aset"]);
                    o.KdKelompokAset = AdnFungsi.CStr(rdr["kd_klp"]);
                    o.Lokasi = AdnFungsi.CStr(rdr["lokasi"]);
                    o.Departemen = AdnFungsi.CStr(rdr["departemen"]);
                    o.Merk = AdnFungsi.CStr(rdr["merk"]);
                    o.Model = AdnFungsi.CStr(rdr["model"]);
                    o.SerialNo = AdnFungsi.CStr(rdr["serial_no"]);
                    o.TglBeli = AdnFungsi.CDate(rdr["tgl_beli"]);
                    o.JenisUmur = AdnFungsi.CStr(rdr["jenis_umur"]);
                    o.Umur = AdnFungsi.CInt(rdr["umur"], true);
                    o.Qty = AdnFungsi.CInt(rdr["qty"], true);
                    o.Harga = AdnFungsi.CDec(rdr["harga"]);
                    o.Total = AdnFungsi.CDec(rdr["total"]);
                    o.NilaiResidu = AdnFungsi.CDec(rdr["nilai_residu"]);
                    o.NilaiBuku = AdnFungsi.CDec(rdr["nilai_buku"]);
                    o.Aktif = AdnFungsi.CBool(rdr["aktif"], true);
                    o.CoaAkumulasiPenyusutan = AdnFungsi.CStr(rdr["coa_akum"]);
                    o.CoaBebanPenyusutan = AdnFungsi.CStr(rdr["coa_beban"]);
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

        public DataTable GetByPeriode(int Tahun, int Bulan)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("KdAset", typeof(String));
            tbl.Columns.Add("NmAset", typeof(String));
            tbl.Columns.Add("CoaAkum", typeof(String));
            tbl.Columns.Add("CoaBeban", typeof(String));
            tbl.Columns.Add("NilaiBuku", typeof(Decimal));
            tbl.Columns.Add("NilaiResidu", typeof(Decimal));
            tbl.Columns.Add("HargaPerolehan", typeof(Decimal));
            tbl.Columns.Add("UmurEkonomisBulan", typeof(int));
            tbl.Columns.Add("Status", typeof(string));

            string Periode = Tahun.ToString() + Bulan.ToString().PadLeft(2, '0');

            sql =
            " SELECT mas.kd_aset,nm_aset, (total-isnull(nilai_susut,0)) nilai_buku, nilai_residu, "
            + "     coa_akum, coa_beban, umur , mas.total as harga_perolehan"
            + " FROM ac_maset mas "
            + " LEFT OUTER JOIN "
            + " ("
            + "     SELECT tag, sum(dtl.kredit) nilai_susut "
            + "     FROM ac_tjurnal  hdr "
            + "     INNER JOIN ac_tjurnal_dtl dtl "
            + "         ON hdr.kd_jurnal = dtl.kd_jurnal "
            + "     WHERE periode <= '" + Periode + "'"
            + "     GROUP BY tag "
            + " ) jur "
            + " ON mas.kd_aset = jur.tag ";


            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                row = tbl.NewRow();
                row["KdAset"] = AdnFungsi.CStr(rdr["kd_aset"]);
                row["NmAset"] = AdnFungsi.CStr(rdr["nm_aset"]);
                row["NilaiBuku"] = AdnFungsi.CDec(rdr["nilai_buku"]);
                row["NilaiResidu"] = AdnFungsi.CDec(rdr["nilai_residu"]);
                row["HargaPerolehan"] = AdnFungsi.CDec(rdr["harga_perolehan"]);
                row["UmurEkonomisBulan"] = AdnFungsi.CInt(rdr["umur"], true);
                row["CoaAkum"] = AdnFungsi.CStr(rdr["coa_akum"]);
                row["CoaBeban"] = AdnFungsi.CStr(rdr["coa_beban"]);
                row["Status"] = "BELUM DIPOSTING";

                tbl.Rows.Add(row);

            }
            rdr.Close();
            //}
            //catch (DbException exp)
            //{
            //    throw new Exception(exp.Message.ToString());
            //}

            return tbl;

        }

        public bool SetCombo(System.Windows.Forms.ComboBox cbo)
        {
            DataTable lst = new DataTable();
            DataRow row;

            string KolomValue = "kd_aset";
            string KolomDisplay = "nm_aset";

            string Value = "KdAset";
            string Display = "NmAset";

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

            string KolomValue = "kd_aset";
            string KolomDisplay = "nm_aset";

            string Value = "kd_aset";
            string Display = "nm_aset";

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

            cbo.DataPropertyName = "kd_aset";
            cbo.DisplayMember = Display;
            cbo.ValueMember = Value;

            cbo.DataSource = lst;

            return true;
        }

        public string BatchJurnalPenyusutan(DateTime Tgl, string Periode, string KdAset, string CoaAkum, string CoaBeban, decimal HargaPerolehan, decimal NilaiBuku, decimal NilaiResidu, int JumlahBulanSusut, int UmurEkonomisBulan)
        {
            string Pesan = "";

            DataTable tbl = new DataTable("AppTabel");
            DataRow row;
            tbl.Columns.Add("Nis", typeof(string));
            tbl.Columns.Add("ThAjar", typeof(string));
            tbl.Columns.Add("KdSekolah", typeof(string));
            tbl.Columns.Add("NoKwitansi", typeof(string));
            tbl.Columns.Add("KdBiaya", typeof(string));
            tbl.Columns.Add("Ket", typeof(string));
            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Jmh", typeof(Decimal));
            tbl.Columns.Add("Status", typeof(string));

            //-------------- Menentukan No Bukti
            string KdJurnal = "";
            string sPrefix = "SST" + Tgl.ToString("yy") + Tgl.ToString("MM");
            string sql =
            "SELECT isnull(max(right(rtrim(kd_jurnal),4)),0) as kd  "
            + " FROM ac_tjurnal "
            + " WHERE left(kd_jurnal, 7)= '" + sPrefix + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            KdJurnal = sPrefix + iMax.ToString().PadLeft(4, '0');
            //=============== END 
            
            int cacah = 0;
            decimal NilaiSusut = 0;

            NilaiSusut = this.HitungNilaiSusut((UmurEkonomisBulan / 12), JumlahBulanSusut, HargaPerolehan, NilaiResidu, NilaiBuku, inovaGL.Data.MetodaPenyusutan.GarisLurus);

            //Header Jurnal
            AdnJurnalUmum oJurnalUmum = null;
            List<AdnJurnalUmumDtl> lstJuDtl = null;

            AdnJurnal o = null;
            List<AdnJurnalDtl> lstItem = null;

            o = new AdnJurnal();
            lstItem = new List<AdnJurnalDtl>();

            oJurnalUmum = new AdnJurnalUmum();
            lstJuDtl = new List<AdnJurnalUmumDtl>();

            oJurnalUmum.KdJU = KdJurnal;
            oJurnalUmum.KdJurnal = oJurnalUmum.KdJU;
            oJurnalUmum.Tgl = Tgl;
            oJurnalUmum.Deskripsi = " Penyusutan, #" + KdAset + "#, Periode #" + Periode + "#";
            
            oJurnalUmum.Sumber = "ProsesPenyusutan";
            oJurnalUmum.JenisJurnal = AdnJurnalVar.JenisJurnal.JSUSUT;
            oJurnalUmum.Periode = Periode;
            oJurnalUmum.Tag = KdAset;

            o.KdJurnal = KdJurnal;
            o.Tgl = Tgl;
            o.Sumber = "ProsesPenyusutan";
            o.Deskripsi = " Penyusutan, #" + KdAset + "#, Periode #" + Periode + "#";
            o.JenisJurnal = AdnJurnalVar.JenisJurnal.JSUSUT;

            o.ThAjar = "";
            o.ThAjarTagihan = "";
            o.KdSiswa = 0;
            o.Nis = "";
            o.KdSekolah = "";
            o.NoKwitansi = "";

            o.Tag = KdAset;
            o.Periode = Periode;


            //Debet
            AdnJurnalUmumDtl itemJuDebet = new AdnJurnalUmumDtl();

            itemJuDebet.KdJU = oJurnalUmum.KdJU;
            itemJuDebet.KdAkun = CoaBeban;
            itemJuDebet.NoUrut = cacah; cacah++;
            itemJuDebet.Memo = " Penyusutan, #" + KdAset + "#, Periode #" + Periode + "#";
            itemJuDebet.Debet = NilaiSusut;
            itemJuDebet.KdDept = "";
            itemJuDebet.KdProject = "";
            itemJuDebet.Kredit = 0;

            lstJuDtl.Add(itemJuDebet);

            AdnJurnalDtl itemDebet = new AdnJurnalDtl();

            itemDebet.KdJurnal = o.KdJurnal;
            itemDebet.KdAkun = CoaBeban;
            itemDebet.NoUrut = cacah; cacah++;
            itemDebet.Memo = " Penyusutan, #" + KdAset + "#, Periode #" + Periode + "#";
            itemDebet.Debet = NilaiSusut;
            itemDebet.Kredit = 0;

            lstItem.Add(itemDebet);

            //Kredit

            AdnJurnalUmumDtl itemJuKredit = new AdnJurnalUmumDtl();

            itemJuKredit.KdJU = KdJurnal;
            itemJuKredit.KdAkun = CoaAkum;
            itemJuKredit.NoUrut = cacah; cacah++;
            itemJuKredit.Debet = 0;
            itemJuKredit.Kredit = NilaiSusut;
            itemJuKredit.KdDept = "";
            itemJuKredit.KdProject = "";
            itemJuKredit.Memo = " Penyusutan, #" + KdAset + "#, Periode #" + Periode + "#";

            lstJuDtl.Add(itemJuKredit);

            AdnJurnalDtl itemKredit = new AdnJurnalDtl();

            itemKredit.KdJurnal = KdJurnal;
            itemKredit.KdAkun = CoaAkum;
            itemKredit.NoUrut = cacah; cacah++;
            itemKredit.Debet = 0;
            itemKredit.Kredit = NilaiSusut;
            itemKredit.KdDept = "";
            itemKredit.Memo = " Penyusutan, #" + KdAset + "#, Periode #" + Periode + "#";

            lstItem.Add(itemKredit);

            if (o != null)
            {
                o.ItemDf = lstItem;
                oJurnalUmum.ItemDf = lstJuDtl;
                AdnJurnalUmumDao JuDao = new AdnJurnalUmumDao(this.cnn, this.pengguna, this.trn);
                AdnJurnalDao dao = new AdnJurnalDao(this.cnn, this.pengguna, this.trn);

                JuDao.Simpan(oJurnalUmum);
                dao.Simpan(o);
            }

            return Pesan;

        }

        private decimal HitungNilaiSusut(int UmurEkonomisDalamTahun, int JumlahBulanSusut, decimal HargaPerolehan, decimal NilaiResidu, decimal NilaiBuku, string MetodaPenyusutan)
        {
            decimal NilaiSusut = 0;
            if (UmurEkonomisDalamTahun > 0)
            {
                switch (MetodaPenyusutan.ToString().ToUpper())
                {
                    case inovaGL.Data.MetodaPenyusutan.GarisLurus:

                        NilaiSusut = (decimal)(((double)JumlahBulanSusut / 12d) * (double)((HargaPerolehan - NilaiResidu) / UmurEkonomisDalamTahun));
                        break;

                    case inovaGL.Data.MetodaPenyusutan.SaldoMenurun:
                        if (HargaPerolehan > 0)
                        {
                            double PerUmurEkonomis = 1d / (double)UmurEkonomisDalamTahun;
                            double NRperHP = (double)(NilaiResidu / HargaPerolehan);

                            double TarifTetap = 1d - Math.Pow(NRperHP, PerUmurEkonomis);

                            NilaiSusut = (decimal)TarifTetap * NilaiBuku;
                        }
                        break;
                }
            }
            return NilaiSusut;
        }

    }
}
