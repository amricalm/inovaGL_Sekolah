using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using Andhana;

namespace inovaGL.Data
{
    public class AdnLoketPSBDao
    {
        private const short JUMLAH_KOLOM = 5;
        private const string NAMA_TABEL = "";
        
        private string pkey = "";
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

        public AdnLoketPSBDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnLoketPSBDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }

        public AdnLoketPSBDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.cmd.Transaction = trn;
            this.pengguna = pengguna;
            this.trn = trn;

        }
        public DataTable GetByPeriode(int Tahun, int Bulan)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("Tgl", typeof(DateTime));
            tbl.Columns.Add("Jmh", typeof(Decimal));
            tbl.Columns.Add("Status", typeof(string));

            sql =
            " SELECT year(hdr.tgl) tahun, month(hdr.tgl) bulan, day(hdr.tgl) tgl, posting, sum(jmh) jmh "
            + " FROM psb_bayar_hdr hdr "
            + " INNER JOIN psb_bayar_dtl dtl "
            + "     ON HDR.no_kwitansi = DTL.no_kwitansi"
            + " WHERE year(hdr.tgl) =" + Tahun + " AND month(hdr.tgl) = " + Bulan
            //+ "     AND posting = 0 "
            + " GROUP BY year(hdr.tgl) , month(hdr.tgl) , day(hdr.tgl) ,posting "
            + " ORDER BY year(hdr.tgl), month(hdr.tgl), day(hdr.tgl)  ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                row = tbl.NewRow();
                string Status = "BELUM DIPOSTING";
                if (AdnFungsi.CInt(rdr["posting"],true)==1)
                {
                    Status = "SUDAH DIPOSTING";
                }
                row["Tgl"] = new DateTime(AdnFungsi.CInt(rdr["tahun"], true), AdnFungsi.CInt(rdr["bulan"], true), AdnFungsi.CInt(rdr["tgl"], true));
                row["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);
                row["Status"] = Status;

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
        public DataTable GetByNis(string KdSekolah, string ThAjarPsb, string Nis)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("Nis", typeof(string));
            tbl.Columns.Add("KdBiaya", typeof(string));
            tbl.Columns.Add("KdAkunDeposit", typeof(string));
            tbl.Columns.Add("KdAkunPiutang", typeof(string));
            tbl.Columns.Add("Jmh", typeof(Decimal));
            //tbl.Columns.Add("Status", typeof(string));

            sql =
            " SELECT  dtl.kd_biaya, bya.kd_akun_deposit, bya.kd_akun_piutang, jmh "
            + " FROM psb_bayar_hdr hdr "
            + " INNER JOIN psb_bayar_dtl dtl "
            + "     ON HDR.no_kwitansi = DTL.no_kwitansi"
            + " INNER JOIN ms_biaya bya"
            + "     ON bya.kd_biaya dtl.kd_biaya "
            + " INNER JOIN ms_siswa sis "
            + "     ON hdr.nis = sis.nis "
            + " WHERE hdr.kd_sekolah ='" + KdSekolah + "'"
            + "     hdr.th_ajar = '" + ThAjarPsb + "'"
            + "     hdr.nis      ='" + Nis + "'"
            + "     AND posting = 0 ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                row = tbl.NewRow();
                //row["Nis"] = AdnFungsi.CStr(rdr["nis"]);
                row["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                row["KdAkunDeposit"] = AdnFungsi.CStr(rdr["kd_akun_deposit"]);
                row["KdAkunPiutang"] = AdnFungsi.CStr(rdr["kd_akun_piutang"]);
                row["Jmh"] = AdnFungsi.CDec(rdr["jmh"]);
                //row["Status"] = "BELUM DIPOSTING";

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
        public string BatchJurnalLoket(DateTime Tgl, DateTime TglLoket, int ModeTunai, string Kas, string KasPindah)
        {
            string  Pesan = "";

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
            string sPrefix = "LOK" + Tgl.ToString("yy") + Tgl.ToString("MM");
            string sql =
            //"SELECT isnull(max(right(rtrim(kd_jurnal),4)),0) as kd  "
            "SELECT isnull(max(cast(substring(kd_jurnal,8,6) as int)),0) as kd  "
            + " FROM ac_tjurnal "
            + " WHERE left(kd_jurnal, 7)= '" + sPrefix + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            //KdJurnal = sPrefix + iMax.ToString().PadLeft(4, '0');
            KdJurnal = sPrefix + iMax.ToString().PadLeft(6, '0');
            //=============== END 


            sql =
            " SELECT hdr.tgl, hdr.no, hdr.kd_sekolah, hdr.no_kwitansi, dtl.kd_biaya, "
            + "     hdr.total, bya.kd_akun_piutang, hdr.akun_kas, bya.kd_jenis, "
            + "     bya.kd_akun_pendapatan, bya.kd_akun_deposit, dtl.jmh, hdr.th_ajar"
            + " FROM psb_bayar_hdr hdr "
            + " INNER JOIN psb_bayar_dtl dtl "
            + "     ON HDR.no_kwitansi = DTL.no_kwitansi"
            + " INNER JOIN ms_biaya bya "
            + "     ON dtl.kd_biaya = bya.kd_biaya "
            + " INNER JOIN psb_siswa sis "
            + "     ON hdr.no = sis.no "
            + "      AND hdr.th_ajar = sis.th_ajar "
            + "     AND hdr.kd_sekolah = sis.sekolah_tujuan"
            + " WHERE year(hdr.tgl) =" + TglLoket.Year + " AND month(hdr.tgl) = " + TglLoket.Month + " AND day(hdr.tgl) = " + TglLoket.Day
            + "     AND posting = 0 ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            List<AdnJurnal> lstJurnal= new List<AdnJurnal>();

            int cacah = 0;
            string LastNoKwitansi = "";

            AdnJurnal o = null;

            List<AdnJurnalDtl> lstItem=null;
            //ArrayList ListKM = new ArrayList();
            while (rdr.Read())
            {
                if (LastNoKwitansi != AdnFungsi.CStr(rdr["no_kwitansi"]))
                {
                    if (LastNoKwitansi != "")//Bukan Pertama Kali
                    {
                        o.ItemDf = lstItem;
                        lstJurnal.Add(o);

                        iMax++;
                        KdJurnal = sPrefix + iMax.ToString().PadLeft(3, '0');
                    }

                    LastNoKwitansi = AdnFungsi.CStr(rdr["no_kwitansi"]);

                    o = new AdnJurnal();
                    lstItem = new List<AdnJurnalDtl>();

                    o.KdJurnal= KdJurnal;
                    o.Tgl = Tgl;
                    o.Sumber = "LOKET PSB";
                    o.Deskripsi = " Pembayaran PSB, #" + LastNoKwitansi + "#";
                    o.JenisJurnal = AdnJurnalVar.JenisJurnal.JKAS;

                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.ThAjarTagihan = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.KdSiswa = 0;
                    o.Nis = AdnFungsi.CStr(rdr["no"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NoKwitansi = LastNoKwitansi;


                    //Debet
                    AdnJurnalDtl itemDebet = new AdnJurnalDtl();

                    itemDebet.KdJurnal = o.KdJurnal;
                    itemDebet.KdAkun = AdnFungsi.CStr(rdr["akun_kas"]);

                    if (Kas.Trim() != "" && KasPindah.Trim() != "")
                    {
                        if(Kas.Trim() == AdnFungsi.CStr(rdr["akun_kas"]))
                        {
                            itemDebet.KdAkun = KasPindah ;// Pindah Akun Kas
                        }
                    }

                    itemDebet.NoUrut = cacah; cacah++;
                    itemDebet.Debet = AdnFungsi.CDec(rdr["total"]);
                    itemDebet.Kredit = 0;

                    lstItem.Add(itemDebet);

                }

                AdnJurnalDtl itemKredit = new AdnJurnalDtl();

                itemKredit.KdJurnal = KdJurnal;
                if (ModeTunai == 1)
                {
                    itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_pendapatan"]);
                }
                else
                {
                    switch (AdnFungsi.CStr(rdr["kd_jenis"]))
                    {
                        case EDUSIS.Shared.EdusisVar.JenisBiaya.PSB:
                        case EDUSIS.Shared.EdusisVar.JenisBiaya.KEGIATAN:
                            itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_deposit"]);
                            break;
                        
                        default:
                            itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_pendapatan"]);
                            break;
                    }
                   
                    
                }
                itemKredit.NoUrut = cacah; cacah++;
                itemKredit.Debet = 0;
                itemKredit.Kredit = AdnFungsi.CDec(rdr["jmh"]);
                itemKredit.KdDept = o.KdSekolah;

                lstItem.Add(itemKredit);
            }

            if (o != null)
            {
                o.ItemDf = lstItem;
                lstJurnal.Add(o);
            }

            rdr.Close();
            AdnJurnalDao dao = new AdnJurnalDao(this.cnn,this.pengguna,this.trn);
            foreach (AdnJurnal item in lstJurnal)
            {
                dao.Simpan(item);
                sql = "UPDATE psb_bayar_hdr  "
                + " SET posting = 1"
                + " WHERE no_kwitansi  ='" + item.NoKwitansi + "'";

                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }

            return Pesan;

        }
      
        private string SimpanJurnalHeader()
        {
            string Pesan="";

            AdnJurnalDao dao = new AdnJurnalDao(this.cnn);
            AdnJurnal Jurnal = new AdnJurnal();
            Jurnal.KdJurnal = "";
            

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            return Pesan;
        }
        public string Unposting(DateTime TglLoket)
        {
            string Pesan = "";

            sql = "delete"
               + " from ac_tjurnal"
               + " where sumber ='LOKET PSB'"
               + " and tgl ='" + AdnFungsi.SetSqlTglEN(TglLoket) + "'";

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            sql = "UPDATE psb_bayar_hdr  "
                + " SET posting = 0"
                + " WHERE year(tgl) =" + TglLoket.Year + " AND month(tgl) = " + TglLoket.Month + " AND day(tgl) = " + TglLoket.Day;

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            Pesan = "SUKSES";
            return Pesan;
        }
    }
}
