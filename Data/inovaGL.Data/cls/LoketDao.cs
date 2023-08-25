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
    public class AdnLoketDao
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

        public AdnLoketDao(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }
        public AdnLoketDao(SqlConnection cnn, AdnScPengguna pengguna)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
            this.pengguna = pengguna;
        }

        public AdnLoketDao(SqlConnection cnn, AdnScPengguna pengguna, SqlTransaction trn)
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
            " SELECT year(hdr.tanggal) tahun, month(hdr.tanggal) bulan, day(hdr.tanggal) tgl, sum(rupiah) jmh,posting "
            + " FROM tbtr_loket_header hdr "
            + " INNER JOIN tbtr_loket_detail dtl "
            + "     ON HDR.th_ajar = DTL.th_ajar"
            + "     AND HDR.kd_sekolah = DTL.kd_sekolah"
            + "     AND HDR.nis = DTL.nis"
            + "     AND HDR.no_bayar = DTL.no_bayar"
            + " WHERE year(hdr.tanggal) =" + Tahun + " AND month(hdr.tanggal) = " + Bulan
                //+ "     AND posting = 0 "
             + "     AND flag = 'INTERN'"
            + " GROUP BY year(hdr.tanggal) , month(hdr.tanggal) , day(hdr.tanggal),posting  "
            + " ORDER BY year(hdr.tanggal), month(hdr.tanggal), day(hdr.tanggal)  ";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                row = tbl.NewRow();
                string Status = "BELUM DIPOSTING";
                if (AdnFungsi.CBool(rdr["posting"], true))
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

        public string BatchJurnalLoket(DateTime Tgl, DateTime TglLoket, int ModeTunai, string Kas, string KasPindah)
        {
            string  Pesan = "";

            //DataTable tbl = new DataTable("AppTabel");
            //DataRow row;
            //tbl.Columns.Add("Nis", typeof(string));
            //tbl.Columns.Add("ThAjar", typeof(string));
            //tbl.Columns.Add("KdSekolah", typeof(string));
            //tbl.Columns.Add("NoKwitansi", typeof(string));
            //tbl.Columns.Add("KdBiaya", typeof(string));
            //tbl.Columns.Add("Ket", typeof(string));
            //tbl.Columns.Add("Tgl", typeof(DateTime));
            //tbl.Columns.Add("Jmh", typeof(Decimal));
            //tbl.Columns.Add("Status", typeof(string));

            //-------------- Menentukan No Bukti
            string KdJurnal = "";
            string sPrefix = "LOK" + Tgl.ToString("yy") + Tgl.ToString("MM");
            string sql =
            "SELECT isnull(max(cast(substring(kd_jurnal,8,6) as int)),0) as kd  "
            + " FROM ac_tjurnal "
            + " WHERE left(kd_jurnal, 7)= '" +sPrefix + "'";

            cmd.CommandText = sql;
            int iMax = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
            KdJurnal = sPrefix + iMax.ToString().PadLeft(6, '0');
            //=============== END 

            sql =
            " SELECT hdr.no_kwitansi, dtl.kd_biaya, year(hdr.tanggal) tahun_periode, month(hdr.tanggal) bulan_periode,  "
            + "     hdr.kd_siswa,  dtl.rupiah jmh, bulan as bulan_bayar"
            + " FROM tbtr_loket_header hdr "
            + " INNER JOIN tbtr_loket_detail dtl "
            + "     ON HDR.th_ajar = DTL.th_ajar"
            + "     AND HDR.kd_sekolah = DTL.kd_sekolah"
            + "     AND HDR.nis = DTL.nis"
            + "     AND HDR.no_bayar = DTL.no_bayar"
            + " INNER JOIN tbtr_loket_sppdetail_bulan spp "
            + "     ON spp.th_ajar = DTL.th_ajar"
            + "     AND spp.kd_sekolah = DTL.kd_sekolah"
            + "     AND spp.nis = DTL.nis"
            + "     AND spp.no_bayar = DTL.no_bayar"
            + "     AND spp.kd_biaya = DTL.kd_biaya"
            + " WHERE year(hdr.tanggal) =" + TglLoket.Year + " AND month(hdr.tanggal) = " + TglLoket.Month + " AND day(hdr.tanggal) = " + TglLoket.Day
            + "     AND posting = 0 "
            + "     AND flag = 'INTERN'";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(rdr);

            sql =
            " SELECT hdr.tanggal, hdr.nis, hdr.kd_sekolah, hdr.no_kwitansi, dtl.kd_biaya, "
            + "     hdr.jumlah total, bya.kd_akun_piutang, hdr.kas_perkiraan, bya.kd_jenis, dtl.jmh_bulan, "
            + "     hdr.kd_siswa, bya.kd_akun_pendapatan, bya.kd_akun_deposit, dtl.rupiah jmh, hdr.th_ajar, hdr.th_ajar_tagihan"
            + " FROM tbtr_loket_header hdr "
            + " INNER JOIN tbtr_loket_detail dtl "
            + "     ON HDR.th_ajar = DTL.th_ajar"
            + "     AND HDR.kd_sekolah = DTL.kd_sekolah"
            + "     AND HDR.nis = DTL.nis"
            + "     AND HDR.no_bayar = DTL.no_bayar"
            + " INNER JOIN ms_biaya bya "
            + "     ON dtl.kd_biaya = bya.kd_biaya "
            + " WHERE year(hdr.tanggal) =" + TglLoket.Year + " AND month(hdr.tanggal) = " + TglLoket.Month + " AND day(hdr.tanggal) = " + TglLoket.Day
            + "     AND posting = 0 "
            + "     AND flag = 'INTERN'";

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
                    o.Sumber = "LOKET";
                    o.Deskripsi = " Pembayaran Siswa, #" + LastNoKwitansi + "#";
                    o.JenisJurnal = AdnJurnalVar.JenisJurnal.JKAS;

                    o.ThAjar = AdnFungsi.CStr(rdr["th_ajar"]);
                    o.ThAjarTagihan = AdnFungsi.CStr(rdr["th_ajar_tagihan"]);
                    o.KdSiswa = AdnFungsi.CInt(rdr["kd_siswa"],true);
                    o.Nis = AdnFungsi.CStr(rdr["nis"]);
                    o.KdSekolah = AdnFungsi.CStr(rdr["kd_sekolah"]);
                    o.NoKwitansi = LastNoKwitansi;


                    //Debet
                    AdnJurnalDtl itemDebet = new AdnJurnalDtl();

                    itemDebet.KdJurnal = o.KdJurnal;
                    itemDebet.KdAkun = AdnFungsi.CStr(rdr["kas_perkiraan"]);

                    if (Kas.Trim() != "" && KasPindah.Trim() != "")
                    {
                        if(Kas.Trim() == AdnFungsi.CStr(rdr["kas_perkiraan"]))
                        {
                            itemDebet.KdAkun = KasPindah ;// Pindah Akun Kas
                        }
                    }

                    itemDebet.NoUrut = cacah; cacah++;
                    itemDebet.Debet = AdnFungsi.CDec(rdr["total"]);
                    itemDebet.Kredit = 0;

                    lstItem.Add(itemDebet);

                }

                // --------------------------- Baris Kredit -------------------------------------------------------
                AdnJurnalDtl itemKredit = new AdnJurnalDtl();

                decimal JmhPembayaranPiutang = 0;
                decimal JmhDeposit = 0;
                bool AdaPembayaranDepositSPP = false;

                itemKredit.KdJurnal = KdJurnal;
                if (ModeTunai == 1)
                {
                    itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_pendapatan"]);
                }
                else
                {
                    switch (AdnFungsi.CStr(rdr["kd_jenis"]))
                    {
                        case EDUSIS.Shared.EdusisVar.JenisBiaya.SPP: 
                            if (o.ThAjar == o.ThAjarTagihan)
                            {
                                itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_piutang"]);

                                dt.DefaultView.RowFilter = "no_kwitansi ='" + o.NoKwitansi + "'";
                                DataTable dtOutput = dt.DefaultView.ToTable();
                                if (dtOutput.Rows.Count > 0)
                                {
                                    string Periode = "";
                                    JmhPembayaranPiutang = 0;
                                    JmhDeposit = 0;
                                    decimal JmhPembayaranItem = AdnFungsi.CDec(rdr["jmh"]);
                                    int JmhBulan = AdnFungsi.CInt(rdr["jmh_bulan"],true);
                                    //----- Verifikasi Bulan Pembayaran terhadap Periode Piutang
                                    for (int i = 0; i < dtOutput.Rows.Count;i++ )
                                    {
                                        Periode = dtOutput.Rows[i]["tahun_periode"].ToString() + dtOutput.Rows[i]["bulan_periode"].ToString().PadLeft(2, '0');
                                        int bulan = AdnFungsi.CInt(dtOutput.Rows[i]["bulan_bayar"], true);
                                        string BulanPembayaran = "";
                                        if (bulan < 7)
                                        {
                                            BulanPembayaran = o.ThAjar.Substring(5, 4) + bulan.ToString().PadLeft(2, '0');
                                        }
                                        else
                                        {
                                            BulanPembayaran = o.ThAjar.Substring(0, 4) + bulan.ToString().PadLeft(2, '0');
                                        }

                                        if (Int32.Parse(BulanPembayaran) > Int32.Parse(Periode))
                                        {
                                            if (JmhBulan > 0)//Antisipasi error Tak Terhingga
                                            {
                                                AdaPembayaranDepositSPP = true;
                                                JmhDeposit = JmhDeposit + JmhPembayaranItem / JmhBulan;
                                            }
                                        }
                                        else
                                        {
                                            if (JmhBulan > 0)//Antisipasi error Tak Terhingga
                                            {
                                                JmhPembayaranPiutang = JmhPembayaranPiutang + JmhPembayaranItem / JmhBulan;
                                            }
                                        }

                                    }//== Verifikasi Bulan Pembayaran terhadap Periode Piutang

                                }
                            }
                            else
                            {
                                itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_deposit"]);
                            }
                            break;
                        case EDUSIS.Shared.EdusisVar.JenisBiaya.PENGEMBANGAN: 
                        case EDUSIS.Shared.EdusisVar.JenisBiaya.OPERASIONAL:
                        case EDUSIS.Shared.EdusisVar.JenisBiaya.KEGIATAN:
                            if (o.ThAjar == o.ThAjarTagihan)
                            {
                                itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_piutang"]);
                            }
                            else
                            {
                                itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_deposit"]);
                            }
                            break;
                        default:
                            itemKredit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_pendapatan"]);
                            break;
                    }
                    
                    
                }
                itemKredit.KdDept = o.KdSekolah;
                itemKredit.NoUrut = cacah; cacah++;
                itemKredit.Debet = 0;
                if (AdaPembayaranDepositSPP)
                {
                    
                    itemKredit.Kredit = JmhPembayaranPiutang;
                    if (JmhPembayaranPiutang > 0)
                    {
                        lstItem.Add(itemKredit);
                    }

                    AdnJurnalDtl itemKreditDeposit = new AdnJurnalDtl();

                    itemKreditDeposit.KdJurnal = KdJurnal;
                    itemKreditDeposit.KdAkun = AdnFungsi.CStr(rdr["kd_akun_deposit"]);
                    itemKreditDeposit.KdDept = o.KdSekolah;
                    itemKreditDeposit.NoUrut = cacah; cacah++;
                    itemKreditDeposit.Debet = 0;
                    itemKreditDeposit.Kredit = JmhDeposit;
                    lstItem.Add(itemKreditDeposit);
                }
                else
                {
                    itemKredit.Kredit = AdnFungsi.CDec(rdr["jmh"]);
                    lstItem.Add(itemKredit);
                }

                
                //======================================== Baris Kredit =================================================
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
                sql = "UPDATE tbtr_loket_header  "
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

        public DataTable GetDepositBiayaSPP(string KdSekolah, string ThAjarTagihan, int Bulan)
        {
            //Mencari Pembayaran Deposit
            sql =
           " SELECT hdr.no_kwitansi, dtl.kd_biaya, year(hdr.tanggal) tahun_periode, month(hdr.tanggal) bulan_periode, hdr.nis, "
           + "     hdr.kd_siswa,  dtl.rupiah jmh, bulan as bulan_bayar"
           + " FROM tbtr_loket_header hdr "
           + " INNER JOIN tbtr_loket_detail dtl "
           + "     ON HDR.th_ajar = DTL.th_ajar"
           + "     AND HDR.kd_sekolah = DTL.kd_sekolah"
           + "     AND HDR.nis = DTL.nis"
           + "     AND HDR.no_bayar = DTL.no_bayar"
           + " INNER JOIN tbtr_loket_sppdetail_bulan spp "
           + "     ON spp.th_ajar = DTL.th_ajar"
           + "     AND spp.kd_sekolah = DTL.kd_sekolah"
           + "     AND spp.nis = DTL.nis"
           + "     AND spp.no_bayar = DTL.no_bayar"
           + "     AND spp.kd_biaya = DTL.kd_biaya"
           + " WHERE hdr.kd_sekolah ='" + KdSekolah + "'"
           + "     AND hdr.th_ajar_tagihan ='" + ThAjarTagihan + "'"
           + "     AND spp.bulan  =  " + Bulan
           + "     AND hdr.flag = 'INTERN'";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            DataTable dt = new DataTable();
            dt.Load(rdr);

            return dt;
        }

        public string Unposting(DateTime TglLoket)
        {
            string Pesan = "";

            sql = "delete"
               + " from ac_tjurnal"
               + " where sumber ='LOKET'"
               + " and tgl ='" + AdnFungsi.SetSqlTglEN(TglLoket) + "'";

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            sql = "UPDATE tbtr_loket_header  "
                + " SET posting = 0"
                + " WHERE year(tanggal) =" + TglLoket.Year + " AND month(tanggal) = " + TglLoket.Month + " AND day(tanggal) = " + TglLoket.Day;

            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();

            Pesan = "SUKSES";
            return Pesan;
        }

        public DataTable GetRingkasan(long KdSiswa, string ThAjar, string KdSekolah, string Tk)
        {
            DataTable tbl = new DataTable("AppTabel");
            DataRow row;

            tbl.Columns.Add("KdBiaya", typeof(String));
            tbl.Columns.Add("NmBiaya", typeof(String));
            tbl.Columns.Add("KdJenis", typeof(String));
            tbl.Columns.Add("Beban", typeof(Decimal));
            tbl.Columns.Add("Dibayar", typeof(String));
            tbl.Columns.Add("SisaTagihan", typeof(Decimal));
            tbl.Columns.Add("ItemBulan", typeof(String));
            tbl.Columns.Add("JmhBulan", typeof(int));

            sql = " select bs.kd_biaya, bya.nm_biaya, bs.tingkat,bya.kd_jenis, rupiah, isnull(dibayar,0) dibayar, tr.item_bulan, tr.jmh_bulan "
            + " From biaya_sekolah bs"
            + " INNER JOIN ms_biaya bya"
            + "     ON bya.kd_biaya = bs.kd_biaya "
            + " LEFT OUTER JOIN "
    
            + " ("
            + "  select hdr.nis, dtl.kd_biaya, dtl.item_bulan,dtl.jmh_bulan, sum(isnull(dtl.rupiah,0)) dibayar"
            + " from tbtr_loket_header hdr"
            + " inner join tbtr_loket_detail dtl"
            + "     ON HDR.th_ajar = DTL.th_ajar"
            + "     AND HDR.kd_sekolah = DTL.kd_sekolah"
            + "     AND HDR.nis = DTL.nis"
            + "     AND HDR.no_bayar = DTL.no_bayar"
            + " WHERE hdr.th_ajar_tagihan = '" + ThAjar + "'"
            + "     AND hdr.kd_siswa = " + KdSiswa 
            + " GROUP BY hdr.th_ajar,hdr.kd_sekolah,"
            + " hdr.nis,"
            + " dtl.kd_biaya, dtl.item_bulan, dtl.jmh_bulan "
            + "  ) TR"
    
            + " ON bya.kd_biaya = TR.kd_biaya "
    
            + " where bs.kd_sekolah = '" + KdSekolah +"'"
            + " and bs.th_ajar ='" + ThAjar + "'"
            + " and bs.tingkat ='" +Tk + "'"
            + " and (bya.kd_jenis = '" + EDUSIS.Shared.EdusisVar.JenisBiaya.KEGIATAN+ "' OR bya.kd_jenis ='" 
                            + EDUSIS.Shared.EdusisVar.JenisBiaya.PSB  + "')"

            + " order by bs.kd_biaya";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                row = tbl.NewRow();
                row["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                row["NmBiaya"] = AdnFungsi.CStr(rdr["nm_biaya"]);
                row["KdJenis"] = AdnFungsi.CStr(rdr["kd_jenis"]);
                row["Beban"] = AdnFungsi.CDec(rdr["rupiah"]);
                row["Dibayar"] = AdnFungsi.CDec(rdr["dibayar"]).ToString("N0");
                row["SisaTagihan"] = AdnFungsi.CDec(rdr["rupiah"]) - AdnFungsi.CDec(rdr["dibayar"]);
                row["ItemBulan"] = AdnFungsi.CStr(rdr["item_bulan"]);
                row["JmhBulan"] = AdnFungsi.CInt(rdr["jmh_bulan"],true);

                tbl.Rows.Add(row);
            }

            rdr.Close();


            sql = " select tr.tanggal, bs.kd_biaya, bya.nm_biaya, bs.tingkat,bya.kd_jenis, rupiah, isnull(dibayar,0) dibayar, tr.item_bulan, tr.jmh_bulan "
           + " From biaya_sekolah bs"
           + " INNER JOIN ms_biaya bya"
           + "     ON bya.kd_biaya = bs.kd_biaya "
           + " LEFT OUTER JOIN "

           + " ("
           + "  select hdr.tanggal, hdr.nis, dtl.kd_biaya, dtl.item_bulan,dtl.jmh_bulan, sum(isnull(dtl.rupiah,0)) dibayar"
           + " from tbtr_loket_header hdr"
           + " inner join tbtr_loket_detail dtl"
           + "     ON HDR.th_ajar = DTL.th_ajar"
           + "     AND HDR.kd_sekolah = DTL.kd_sekolah"
           + "     AND HDR.nis = DTL.nis"
           + "     AND HDR.no_bayar = DTL.no_bayar"
           + " WHERE hdr.th_ajar_tagihan = '" + ThAjar + "'"
           + "     AND hdr.kd_siswa = " + KdSiswa
           + " GROUP BY hdr.th_ajar,hdr.kd_sekolah, hdr.nis,"
           + " dtl.kd_biaya, dtl.item_bulan, dtl.jmh_bulan, hdr.tanggal "
           + "  ) TR"

           + " ON bya.kd_biaya = TR.kd_biaya "

           + " where bs.kd_sekolah = '" + KdSekolah + "'"
           + " and bs.th_ajar ='" + ThAjar + "'"
           + " and bs.tingkat ='" + Tk + "'"
           + " and (bya.kd_jenis = '" +  EDUSIS.Shared.EdusisVar.JenisBiaya.SPP + "')"

           + " order by tr.tanggal";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            string ItemBulan = "";
            decimal JmhDibayar = 0;
            bool Ditulis = false;
            row = tbl.NewRow();

            while (rdr.Read())
            {
                if (!Ditulis)
                {        
                    row["KdBiaya"] = AdnFungsi.CStr(rdr["kd_biaya"]);
                    row["NmBiaya"] = AdnFungsi.CStr(rdr["nm_biaya"]);
                    row["KdJenis"] = AdnFungsi.CStr(rdr["kd_jenis"]);
                    row["Beban"] = AdnFungsi.CDec(rdr["rupiah"]);
                    
                    row["JmhBulan"] = AdnFungsi.CInt(rdr["jmh_bulan"], true);
                    Ditulis = true;
                }

                JmhDibayar = JmhDibayar +  AdnFungsi.CDec(rdr["dibayar"]);
                if (ItemBulan != "")
                {
                    ItemBulan = ItemBulan + ",";
                }
                ItemBulan= ItemBulan+ AdnFungsi.CStr(rdr["item_bulan"]);    
            }

            if (rdr.HasRows)
            {
                row["Dibayar"] = ItemBulan;
                row["ItemBulan"] = ItemBulan;
                row["SisaTagihan"] = AdnFungsi.CDec(row["Beban"]) * 12 - JmhDibayar;
                tbl.Rows.Add(row);
            }

            rdr.Close();
            return tbl;
        }

    }
}
