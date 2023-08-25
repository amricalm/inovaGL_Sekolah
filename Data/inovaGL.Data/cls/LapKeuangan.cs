using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Andhana;
using inovaGL;

namespace inovaGL.Data
{
    class Neraca
    {
        public string Item { get; set; }
        public Neraca(string Item)
        {
            this.Item = Item;
        }
    }

    //class AdnTreeNode<T> : IDisposable
    //{
    //    private AdnTreeNode<T> _Parent;
    //    public AdnTreeNode<T> Parent
    //    {
    //        get { return _Parent; }
    //        set
    //        {
    //            if (value == _Parent)
    //            {
    //                return;
    //            }
    //            if (_Parent != null)
    //            {
                    
    //            }
    //        }
    //    }

    //}

    class Neraca2
    {
            //public void Cetak(ICollection<AdnTreeItem> items)
            //{

            //    TreeNode nodeAkun;
            //    Array nodeAkun;

            //    int LastTk = 0;
            //    List<TreeNode> lstNode = new List<TreeNode>();
            //    foreach (AdnTreeItem item in items)
                
            //    {

            //        if (LastTk == item.Tingkat)
            //        {
            //            if (item.Tingkat== 0)
            //            {
            //                nodeAkun = tree.Nodes.Add(item.Nama); // Tk 0
            //                if (lstNode.Count > 0)
            //                {
            //                    if (lstNode[item.Tingkat] != null)
            //                    {
            //                        lstNode[item.Tingkat] = nodeAkun;
            //                    }
            //                    else
            //                    {
            //                        lstNode.Add(nodeAkun);
            //                    }
            //                }
            //                else
            //                {
            //                    lstNode.Add(nodeAkun);
            //                }
                            
            //            }
            //            else
            //            {
            //                nodeAkun = lstNode[item.Tingkat - 1].Nodes.Add(item.Nama);
                            
            //                if (lstNode[item.Tingkat] != null)
            //                {
            //                    lstNode[item.Tingkat] = nodeAkun;
            //                }
            //                else
            //                {
            //                    lstNode.Add(nodeAkun);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            if (item.Tingkat== 0)
            //            {
            //                nodeAkun = tree.Nodes.Add(item.Nama); // Tk 0
            //                if (lstNode[item.Tingkat] != null)
            //                {
            //                    lstNode[item.Tingkat] = nodeAkun;
            //                }
            //                else
            //                {
            //                    lstNode.Add(nodeAkun);
            //                }
            //            }
            //            else
            //            {
            //                nodeAkun = lstNode[item.Tingkat - 1].Nodes.Add(item.Nama);
            //                lstNode.Add(nodeAkun);

            //                if (lstNode[item.Tingkat] != null)
            //                {
            //                    lstNode[item.Tingkat] = nodeAkun;
            //                }
            //                else
            //                {
            //                    lstNode.Add(nodeAkun);
            //                }

            //                LastTk = item.Tingkat;
            //            }
            //        }
            //    }


            //}
        

    }

    public class AdnLapNeracav0
    {

        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "";

        private string pkey = "";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        //private AdnScPengguna pengguna;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnLapNeracav0(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }

        //public AdnLapNeraca(SqlConnection cnn, AdnScPengguna pengguna)
        //{
        //    this.cnn = cnn;
        //    this.cmd = new SqlCommand("", this.cnn);
        //    this.pengguna = pengguna;
        //}

        public DataTable  Cetak(int Tingkat,DateTime PeriodeAwal, int Bulan, int Tahun)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("Item", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Nilai", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));

            DataTable tmpTbl = new DataTable("AppTabelSementara");

            tmpTbl.Columns.Add("Item", typeof(String));
            tmpTbl.Columns.Add("Debet", typeof(Decimal));
            tmpTbl.Columns.Add("Kredit", typeof(Decimal));
            tmpTbl.Columns.Add("Nilai", typeof(Decimal));
            tmpTbl.Columns.Add("Turunan", typeof(int));

            DateTime TglSd = new DateTime(Tahun, Bulan,1).AddMonths(1);
            DateTime TglDr = PeriodeAwal;

            int BatasTingkat = 4;
            string sql = "";
            string sqlTr = "";

            string[] sqlTingkat = new string[BatasTingkat];

            sqlTr = " select dtl.kd_akun, mak.nm_akun, mak.kd_induk, sum(debet) debet, sum(kredit) kredit "
                                + " from ac_tjurnal hdr "
                                + " inner join ac_tjurnal_dtl dtl "
                                + "     on hdr.kd_jurnal = dtl.kd_jurnal "
                                + " inner join ac_makun mak "
                                + "     on dtl.kd_akun = mak.kd_akun "
                                + " where turunan = " + BatasTingkat
                                //+ "     and hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd) + "'"
                                + " group by dtl.kd_akun, mak.nm_akun, kd_induk ";

            for (int i = BatasTingkat; i > 0; i--)
            {
                sqlTingkat[i - 1] = sqlTr;
                sql = " select mak.kd_akun, mak.nm_akun, mak.kd_induk,  sum(isnull(dtl.debet,0) + isnull(tr" + i + ".debet,0)) debet , sum(isnull(dtl.kredit,0)+isnull(tr" + i + ".kredit,0)) kredit "
                            + " from ac_makun mak "
                            + " left outer join ac_tjurnal_dtl dtl"
                                + " on mak.kd_akun = dtl.kd_akun"
                            + " left outer join ac_tjurnal hdr "
                            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
                            + " left outer join"
                            + " ("
                            + sqlTr
                            + " ) tr" + i
                            + " on mak.kd_akun = tr" + i + ".kd_induk"
                            + " where turunan =" + (i - 1)
                            //+ "     and hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd) + "'"
                            + " group by mak.kd_akun, mak.nm_akun, mak.kd_induk";
                sqlTr = sql;
            }

            for(int i=0;i<Tingkat;i++)
            {
               sql=sql + " UNION " + sqlTingkat[i];
            }


            sql = " select mak.kd_akun, mak.nm_akun, mak.kd_induk,  mak.turunan, isnull(debet,0)debet , isnull(kredit,0)kredit "
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( " + sql
                  + "  ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " order by mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr=this.cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    int Turunan = AdnFungsi.CInt(rdr["turunan"], true);
                    string Item = AdnFungsi.CStr(rdr["nm_akun"]);

                    DataRow baris = tmpTbl.NewRow();
                    baris["KdAkun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAkun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                    baris["Turunan"] = Turunan;
                    baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);
                    baris["Nilai"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);

                    tmpTbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch(Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }


            int Tk = 0;
            decimal[] HeaderNilai = new decimal[Tingkat+1];
            string[] HeaderItem = new string[Tingkat+1];
            //string[] HeaderItem = new string[Tingkat + 1];
            //string[] HeaderItem = new string[Tingkat + 1];
            for (int i = 0; i < tmpTbl.Rows.Count; i++)
            {
                int Turunan = int.Parse(tmpTbl.Rows[i]["Turunan"].ToString());

                if (Turunan < Tk)
                {
                    for (int x = Tk - 1; x >= Turunan; x--)
                    {
                        DataRow barisTotal = tbl.NewRow();
                        string HdText = "Total " + HeaderItem[x];
                        barisTotal["Item"] = HdText.PadLeft(HdText.Length + x * 8);
                        barisTotal["Debet"] = 0;
                        barisTotal["Kredit"] = 0;
                        barisTotal["Nilai"] = 0;
                        tbl.Rows.Add(barisTotal);
                    }
                }
                Tk = Turunan;
                HeaderItem[Turunan] = tmpTbl.Rows[i]["Item"].ToString();
                HeaderNilai[Turunan] = decimal.Parse(tmpTbl.Rows[i]["Nilai"].ToString());
                
                DataRow baris = tbl.NewRow();
                //baris["Item"] = tmpTbl.Rows[i]["Item"].ToString().PadLeft(tmpTbl.Rows[i]["Item"].ToString().Length + (Turunan * 8));
                baris["KdAkun"] = tmpTbl.Rows[i]["KdAkun"].ToString();
                baris["NmAkun"] = tmpTbl.Rows[i]["NmAkun"].ToString();
                baris["Debet"] = decimal.Parse(tmpTbl.Rows[i]["Debet"].ToString());
                baris["Kredit"] = decimal.Parse(tmpTbl.Rows[i]["Kredit"].ToString());
                //baris["Nilai"] = decimal.Parse(tmpTbl.Rows[i]["Nilai"].ToString());

                tbl.Rows.Add(baris);
            }

            if (Tk>0)
            {
                for (int x = Tk - 1; x >= 0; x--)
                {
                    DataRow barisTotal = tbl.NewRow();
                    string HdText = "Total " + HeaderItem[x];
                    barisTotal["Item"] = HdText.PadLeft(HdText.Length + x * 8);
                    barisTotal["Debet"] = 0;
                    barisTotal["Kredit"] = 0;
                    barisTotal["Nilai"] = 0;
                    tbl.Rows.Add(barisTotal);
                }
            }

            return tbl;

        }


    }

    public class AdnLapLabaRugiv0
    {

        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "";

        private string pkey = "";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        //private AdnScPengguna pengguna;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnLapLabaRugiv0(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }

        public DataTable Cetak(string JenisLaporan, int Tingkat, DateTime aTglDr,DateTime aTglSd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Nilai", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));

            DataTable tmpTbl = new DataTable("AppTabelSementara");

            tmpTbl.Columns.Add("NoUrut", typeof(int));
            tmpTbl.Columns.Add("Grup", typeof(String));
            tmpTbl.Columns.Add("KdAkun", typeof(String));
            tmpTbl.Columns.Add("NmAkun", typeof(String));
            tmpTbl.Columns.Add("Debet", typeof(Decimal));
            tmpTbl.Columns.Add("Kredit", typeof(Decimal));
            tmpTbl.Columns.Add("Nilai", typeof(Decimal));
            tmpTbl.Columns.Add("Turunan", typeof(int));

            
            DateTime TglDr = aTglDr;
            DateTime TglSd = aTglSd;

            int BatasTingkat = 4;
            string sql = "";
            string sqlTr = "";

            string[] sqlTingkat = new string[BatasTingkat];

            sqlTr = " select dtl.kd_akun, mak.nm_akun, mak.kd_induk, sum(debet) debet, sum(kredit) kredit "
                                + " from ac_tjurnal hdr "
                                + " inner join ac_tjurnal_dtl dtl "
                                + "     on hdr.kd_jurnal = dtl.kd_jurnal "
                                + " inner join ac_makun mak "
                                + "     on dtl.kd_akun = mak.kd_akun "
                                + " where turunan = " + BatasTingkat
                                + "     and hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd) + "'"
                                + " group by dtl.kd_akun, mak.nm_akun, kd_induk ";

            for (int i = BatasTingkat; i > 0; i--)
            {
                sqlTingkat[i - 1] = sqlTr;
                sql = " select mak.kd_akun, mak.nm_akun, mak.kd_induk,  sum(isnull(dtl.debet,0) + isnull(tr" + i + ".debet,0)) debet , sum(isnull(dtl.kredit,0)+isnull(tr" + i + ".kredit,0)) kredit "
                            + " from ac_makun mak "
                            + " left outer join ac_tjurnal_dtl dtl"
                                + " on mak.kd_akun = dtl.kd_akun"
                            + " left outer join ac_tjurnal hdr "
                            + "     on hdr.kd_jurnal = dtl.kd_jurnal "
                            + " left outer join"
                            + " ("
                            + sqlTr
                            + " ) tr" + i
                            + " on mak.kd_akun = tr" + i + ".kd_induk"
                            + " where turunan =" + (i - 1)
                            + "     and hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd) + "'"
                            + " group by mak.kd_akun, mak.nm_akun, mak.kd_induk";
                sqlTr = sql;
            }

            for (int i = 0; i < Tingkat; i++)
            {
                sql = sql + " UNION " + sqlTingkat[i];
            }


            sql = " select sys.no_urut, sys.nm_gol, mak.kd_akun, mak.nm_akun, mak.kd_induk,  mak.turunan, isnull(debet,0)debet , isnull(kredit,0)kredit "
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( " + sql
                  + "  ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() +  "' "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    int Turunan = AdnFungsi.CInt(rdr["turunan"], true);
                    string Item = AdnFungsi.CStr(rdr["nm_akun"]);

                    DataRow baris = tmpTbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"],true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["nm_gol"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                    baris["Turunan"] = Turunan;
                    baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);
                    //baris["Nilai"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);

                    tmpTbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }


            int Tk = 0;
            decimal[] HeaderNilai = new decimal[Tingkat + 1];
            string[] HeaderItem = new string[Tingkat + 1];
            for (int i = 0; i < tmpTbl.Rows.Count; i++)
            {
                //int Turunan = int.Parse(tmpTbl.Rows[i]["Turunan"].ToString());

                //if (Turunan < Tk)
                //{
                //    for (int x = Tk - 1; x >= Turunan; x--)
                //    {
                //        DataRow barisTotal = tbl.NewRow();
                //        string HdText = "Total " + HeaderItem[x];
                //        //barisTotal["Item"] = HdText.PadLeft(HdText.Length + x * 8);
                //        barisTotal["Debet"] = 0;
                //        barisTotal["Kredit"] = 0;
                //        barisTotal["Nilai"] = 0;
                //        tbl.Rows.Add(barisTotal);
                //    }
                //}
                //Tk = Turunan;
                //HeaderItem[Turunan] = tmpTbl.Rows[i]["Item"].ToString();
                //HeaderNilai[Turunan] = decimal.Parse(tmpTbl.Rows[i]["Nilai"].ToString());

                DataRow baris = tbl.NewRow();
                //baris["Item"] = tmpTbl.Rows[i]["Item"].ToString().PadLeft(tmpTbl.Rows[i]["Item"].ToString().Length + (Turunan * 8));
                baris["NoUrut"] = tmpTbl.Rows[i]["NoUrut"].ToString();
                baris["Grup"] = tmpTbl.Rows[i]["Grup"].ToString();
                baris["KdAkun"] = tmpTbl.Rows[i]["KdAKun"].ToString();
                baris["NmAkun"] = tmpTbl.Rows[i]["NmAKun"].ToString();
                baris["Debet"] = decimal.Parse(tmpTbl.Rows[i]["Debet"].ToString());
                baris["Kredit"] = decimal.Parse(tmpTbl.Rows[i]["Kredit"].ToString());
                //baris["Nilai"] = decimal.Parse(tmpTbl.Rows[i]["Nilai"].ToString());

                tbl.Rows.Add(baris);
            }

            //if (Tk > 0)
            //{
            //    for (int x = Tk - 1; x >= 0; x--)
            //    {
            //        DataRow barisTotal = tbl.NewRow();
            //        string HdText = "Total " + HeaderItem[x];
            //        barisTotal["Item"] = HdText.PadLeft(HdText.Length + x * 8);
            //        barisTotal["Debet"] = 0;
            //        barisTotal["Kredit"] = 0;
            //        barisTotal["Nilai"] = 0;
            //        tbl.Rows.Add(barisTotal);
            //    }
            //}
            
            return tbl;

        }


    }


    public class AdnLapNeraca
    {

        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "";

        private string pkey = "";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        //private AdnScPengguna pengguna;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnLapNeraca(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }

        public DataTable CetakFormatRincian(string JenisLaporan, DateTime PeriodeAwal, DateTime Tgl, string KdAkunLabaThBerjalan, string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Nilai", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));

            decimal LabaThBerjalan = this.GetLabaThBerjalan(PeriodeAwal,Tgl);

            DateTime TglDr = PeriodeAwal;
            DateTime TglSd = Tgl;

            string sql = " select sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk, "
                  + " isnull(debet,0)debet , isnull(kredit,0)kredit "
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "

                  + "     left outer join ms_siswa sis "
                  + "     on hdr.kd_siswa = sis.kd_siswa "
                  + "     left join kelas_siswa ks "
                  + "         on ks.kd_sekolah = sis.kd_sekolah "
                  + "         and ks.nis  = sis.nis "
                  + "         and ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"

                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "   AND mak.tipe = 'DTL' "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                    baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

                //SALDO AWAL ditambahkan dengan MUTASI
                sql = " select sys.no_urut, sys.nm_gol, mak.kd_akun, mak.nm_akun, mak.dk, "
                 + " isnull(debet,0)debet , isnull(kredit,0)kredit "
                 + " from ac_makun mak "
                 + " left outer join  "
                 + " ( "
                 + "       SELECT mak.kd_akun,  isnull(debet,0)debet , isnull(kredit,0)kredit "
                 + "       FROM ac_makun mak "
                 + "       INNER JOIN ac_tsaldo_awal dtl "
                 + "           ON mak.kd_akun = dtl.kd_akun "
                 + "       WHERE dtl.tgl ='" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
                 + " ) tr "
                 + "       on mak.kd_akun = tr.kd_akun "
                 + " inner join ac_sys_gol_akun sys "
                 + "       on mak.kd_gol = sys.kd_gol "
                 + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                 + "   AND mak.tipe = 'DTL' "
                 + " order by sys.no_urut, mak.kd_akun ";

                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                {
                    DataRow baris = tbl.Rows[i];
                    decimal cDebet = 0;
                    decimal cKredit = 0;

                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        cDebet = (decimal)baris["Debet"] - (decimal)baris["Kredit"] + AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);

                        if (cDebet < 0)
                        {
                            baris["Debet"] = 0;
                            baris["Kredit"] = cDebet * (-1);
                        }
                        else
                        {
                            baris["Debet"] = cDebet;
                            baris["Kredit"] = 0;
                        }
                    }
                    else
                    {
                        cKredit = (decimal)baris["Kredit"] - (decimal)baris["Debet"] + AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);

                        if (cKredit < 0)
                        {
                            baris["Kredit"] = 0;
                            baris["Debet"] = cKredit * (-1);
                        }
                        else
                        {
                            baris["Kredit"] = cKredit;
                            baris["Debet"] = 0;
                        }
                    }

                    // Sisip Laba Tahun Berjalan
                    if (AdnFungsi.CStr(baris["KdAkun"]) == KdAkunLabaThBerjalan.ToString().Trim())
                    {
                        if (LabaThBerjalan < 0)
                        {
                            baris["Kredit"] = 0;
                            baris["Debet"] = LabaThBerjalan * (-1);
                        }
                        else
                        {
                            baris["Kredit"] = LabaThBerjalan;
                            baris["Debet"] = 0;
                        }
                    }
                    i++;
                }
                rdr.Close();
            }

            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }
        public DataTable CetakFormatStandar(string JenisLaporan, DateTime PeriodeAwal, DateTime Tgl,string ThAjar)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Nilai", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));

            decimal LabaThBerjalan = this.GetLabaThBerjalan(PeriodeAwal, Tgl);

            DateTime TglDr = PeriodeAwal;
            DateTime TglSd = Tgl;

            string sql = " SELECT sys.kd_gol, sys.nm_gol, sys.no_urut, sys.grup_laporan, sys.dk, "
                  + " isnull(debet,0)debet , isnull(kredit,0)kredit "
                  + " from ac_sys_gol_akun sys "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT sys.kd_gol, sys.nm_gol, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       INNER JOIN ac_sys_gol_akun sys "
                  + "           ON mak.kd_gol = sys.kd_gol "

                  + "     left outer join ms_siswa sis "
                  + "     on hdr.kd_siswa = sis.kd_siswa "
                  + "     left outer join kelas_siswa ks "
                  + "         on ks.kd_sekolah = sis.kd_sekolah "
                  + "         and ks.nis  = sis.nis "
                  + "         and ks.th_ajar ='" + ThAjar.ToString().Trim() + "'"


                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "           AND sys.laporan  ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "       GROUP BY sys.kd_gol, sys.nm_gol"
                  + " ) tr "
                  + "       on sys.kd_gol = tr.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + " order by sys.no_urut ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_gol"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_gol"]);
                    baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();


                sql = " select sys.no_urut, sys.nm_gol,sys.kd_gol, sys.dk, "
                 + " isnull(debet,0)debet , isnull(kredit,0)kredit "
                 + " from ac_sys_gol_akun sys "
                 + " left outer join  "
                 + " ( "
                 + "       SELECT sys.kd_gol, sys.nm_gol,  isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                 + "       FROM ac_makun mak "
                 + "       INNER JOIN ac_tsaldo_awal dtl "
                 + "           ON mak.kd_akun = dtl.kd_akun "
                 + "       INNER JOIN ac_sys_gol_akun sys "
                 + "           ON mak.kd_gol = sys.kd_gol "
                 + "       WHERE dtl.tgl ='" + AdnFungsi.SetSqlTglEN(TglDr) + "'"
                 + "           AND sys.laporan  ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                 + "       GROUP BY sys.kd_gol, sys.nm_gol"
                 + " ) tr "
                 + "       on sys.kd_gol = tr.kd_gol "
                 + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                 + " order by sys.no_urut ";

                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();
                int i = 0;
                while (rdr.Read())
                {
                    DataRow baris = tbl.Rows[i];
                    decimal cDebet = 0;
                    decimal cKredit = 0;

                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        cDebet = (decimal)baris["Debet"] - (decimal)baris["Kredit"] + AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);

                        if (cDebet < 0)
                        {
                            baris["Debet"] = 0;
                            baris["Kredit"] = cDebet*(-1);
                        }
                        else
                        {
                            baris["Debet"] = cDebet;
                            baris["Kredit"]=0;
                        }

                    }
                    else
                    {
                        cKredit = (decimal)baris["Kredit"] - (decimal)baris["Debet"] + AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        

                        if (cKredit < 0)
                        {
                            baris["Kredit"] = 0;
                            baris["Debet"] = cKredit*(-1);
                        }
                        else
                        {
                            baris["Kredit"] = cKredit;
                            baris["Debet"] = 0;
                        }
                    }

                    if (AdnFungsi.CStr(baris["KdAkun"]).ToUpper() == "MOD")
                    {

                        baris["Kredit"] = (decimal)baris["Kredit"] + LabaThBerjalan;
                    }

                    i++;
                }
                rdr.Close();


            }

            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }

        public decimal GetLabaThBerjalan(DateTime PeriodeMulai, DateTime TglSd)
        {
            decimal hasil = 0;

            string sql = ""
            + " select (isnull(sum(kredit),0) - isnull(sum(debet),0)) selisih "
            + " from ac_tjurnal hdr"
            + " inner join ac_tjurnal_dtl dtl"
            + "     on hdr.kd_jurnal = dtl.kd_jurnal"
            + " inner join ac_makun mak"
            + "     on mak.kd_akun = dtl.kd_akun"
            + " inner join ac_sys_gol_akun sys "
            + "     on mak.kd_gol = sys.kd_gol "
            + " where sys.laporan ='LR' "
            + "     and hdr.tgl >='" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'";

            cmd.CommandText = sql;
            hasil = Convert.ToDecimal(cmd.ExecuteScalar());

            return hasil;
        }
    }

    public class AdnLapLabaRugi
    {

        private const short JUMLAH_KOLOM = 9;
        private const string NAMA_TABEL = "";

        private string pkey = "";
        private string sql;
        private string sWhere;

        private SqlConnection cnn;
        private SqlCommand cmd;
        private SqlDataReader rdr;
        //private AdnScPengguna pengguna;

        private string[] fld = new string[JUMLAH_KOLOM];
        private string[] nilai = new string[JUMLAH_KOLOM];
        private string[] tipe = new string[JUMLAH_KOLOM];

        public AdnLapLabaRugi(SqlConnection cnn)
        {
            this.cnn = cnn;
            this.cmd = new SqlCommand("", this.cnn);
        }

        public DataTable Cetak(string JenisLaporan, DateTime aTglDr,DateTime aTglSd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Nilai", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));


            DateTime TglDr = aTglDr;
            DateTime TglSd = aTglSd;


            sql = " select sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk, "
                  + " isnull(debet,0)debet , isnull(kredit,0)kredit "
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "   AND mak.tipe = 'DTL' "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);

                    decimal Nilai = 0;
                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        Nilai = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        baris["Debet"] = Nilai;
                        baris["Kredit"] = 0;
                        baris["Nilai"] = Nilai;
                    }
                    else
                    {
                        Nilai = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        baris["Kredit"] = Nilai;
                        baris["Debet"] = 0;
                        baris["Nilai"] = Nilai;
                    }
                    
                    //baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    //baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }
        public DataTable CetakFormatStandar(string JenisLaporan, DateTime aTglDr, DateTime aTglSd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Nilai", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));

            DateTime TglDr = aTglDr;
            DateTime TglSd = aTglSd;

            string sql = " SELECT sys.kd_gol, sys.nm_gol, sys.no_urut, sys.grup_laporan, sys.dk, "
                  + " isnull(debet,0)debet , isnull(kredit,0)kredit "
                  + " from ac_sys_gol_akun sys "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT sys.kd_gol, sys.nm_gol, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       INNER JOIN ac_sys_gol_akun sys "
                  + "           ON mak.kd_gol = sys.kd_gol "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "           AND sys.laporan  ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "       GROUP BY sys.kd_gol, sys.nm_gol"
                  + " ) tr "
                  + "       on sys.kd_gol = tr.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + " order by sys.no_urut ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_gol"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_gol"]);

                    decimal Nilai = 0;
                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        Nilai = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        baris["Debet"] = Nilai;
                        baris["Kredit"] = 0;
                        baris["Nilai"] = Nilai;
                    }
                    else
                    {
                        Nilai = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        baris["Kredit"] = Nilai;
                        baris["Debet"] = 0;
                        baris["Nilai"] = Nilai;
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }
        public DataTable CetakFormatThBerjalan(string JenisLaporan, DateTime PeriodeMulai, DateTime aTglDr, DateTime aTglSd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Nilai", typeof(Decimal));
            tbl.Columns.Add("DebetThBerjalan", typeof(Decimal));
            tbl.Columns.Add("KreditThBerjalan", typeof(Decimal));
            tbl.Columns.Add("NilaiThBerjalan", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));


            DateTime TglDr = aTglDr;
            DateTime TglSd = aTglSd;


            sql = " select sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk, "
                  + " isnull(tr.debet,0)debet , isnull(tr.kredit,0)kredit,  "
                  + " isnull(thb.debet,0)debet_thb , isnull(thb.kredit,0)kredit_thb  "
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "

                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(PeriodeMulai) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) thb "
                  + "       on mak.kd_akun = thb.kd_akun "


                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "   AND mak.tipe = 'DTL' "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);

                    decimal Nilai = 0, NilaiThBerjalan = 0;
                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        Nilai = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        NilaiThBerjalan = AdnFungsi.CDec(rdr["debet_thb"]) - AdnFungsi.CDec(rdr["kredit_thb"]);
                        baris["Debet"] = Nilai;
                        baris["Kredit"] = 0;
                        baris["Nilai"] = Nilai;

                        baris["DebetThBerjalan"] = NilaiThBerjalan;
                        baris["KreditThBerjalan"] = 0;
                        baris["NilaiThBerjalan"] = NilaiThBerjalan;

                    }
                    else
                    {
                        Nilai = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        NilaiThBerjalan = AdnFungsi.CDec(rdr["kredit_thb"]) - AdnFungsi.CDec(rdr["debet_thb"]);
                        baris["Kredit"] = Nilai;
                        baris["Debet"] = 0;
                        baris["Nilai"] = Nilai;

                        baris["KreditThBerjalan"] = NilaiThBerjalan;
                        baris["DebetThBerjalan"] = 0;
                        baris["NilaiThBerjalan"] = NilaiThBerjalan;
                    }


                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }
        public DataTable CetakByDept(string JenisLaporan, DateTime aTglDr, DateTime aTglSd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));


            DateTime TglDr = aTglDr;
            DateTime TglSd = aTglSd;

            //Cetak Kolom Nilai Dept
            string sKolomDept = ""; string sSumKolomDept = "";
            string sql =
                "select kd_dept,nm_dept "
                + "from ac_mdept "
                + " order by kd_dept"; 

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int BarisDept = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomDept != "")
                {
                    sKolomDept = sKolomDept + ",";
                    sSumKolomDept = sSumKolomDept + ",";
                }

                sKolomDept = sKolomDept + "'debet" + BarisDept.ToString() + "' = case when dtl.kd_dept = '" + fld[0].ToString().Trim() + "' then (dtl.debet) end, 'kredit" + BarisDept.ToString() + "' = case when dtl.kd_dept = '" + fld[0].ToString().Trim() + "' then (dtl.kredit) end";
                sSumKolomDept = sSumKolomDept + "isnull(sum(debet" + BarisDept.ToString() + "),0)debet" + BarisDept.ToString() + ", isnull(sum(kredit" + BarisDept.ToString() + "),0)kredit" + BarisDept.ToString();

                //Tambah Kolom
                tbl.Columns.Add("d" + BarisDept.ToString(), typeof(Decimal));

                BarisDept++;
            }
            rdr.Close();

            //== END  Cetak Kolom Nilai Program

            sql = " select sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk ";

            if (sSumKolomDept != "")
            {
                sql = sql + "," + sSumKolomDept;
            }

            //+ " isnull(debet,0)debet , isnull(kredit,0)kredit "
            sql = sql
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun ";

            if (sKolomDept != "")
            {
                sql = sql + "," + sKolomDept;
            }

            //isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "

            sql = sql
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                //+ "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "   AND mak.tipe = 'DTL' "
                  + " GROUP BY sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);

                    
                    for (int i = 1; i < BarisDept; i++)
                    {
                        if (AdnFungsi.CStr(rdr["dk"]).Trim().ToUpper() ==  AdnVar.SaldoNormal.DEBET)
                        {
                            baris["d" + i.ToString()] = Convert.ToDecimal(rdr["debet" + i.ToString()]) - Convert.ToDecimal(rdr["kredit" + i.ToString()]);
                        }
                        else
                        {
                            baris["d" + i.ToString()] = Convert.ToDecimal(rdr["kredit" + i.ToString()]) - Convert.ToDecimal(rdr["debet" + i.ToString()]);
                        }
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;
        }
        public DataTable CetakByDeptFormatStandar(string JenisLaporan, DateTime aTglDr, DateTime aTglSd)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));


            DateTime TglDr = aTglDr;
            DateTime TglSd = aTglSd;

            //Cetak Kolom Nilai Dept
            string sKolomDept = ""; string sSumKolomDept = "";
            string sql =
                "select kd_dept,nm_dept "
                + "from ac_mdept "
                + " order by kd_dept";

            cmd.CommandText = sql;
            rdr = cmd.ExecuteReader();

            ArrayList prg = new ArrayList();
            int BarisDept = 1;
            while (rdr.Read())
            {
                object[] fld = new object[rdr.FieldCount];
                rdr.GetValues(fld);
                prg.Add(fld);

                if (sKolomDept != "")
                {
                    sKolomDept = sKolomDept + ",";
                    sSumKolomDept = sSumKolomDept + ",";
                }

                sKolomDept = sKolomDept + "'debet" + BarisDept.ToString() + "' = case when dtl.kd_dept = '" + fld[0].ToString().Trim() + "' then sum(dtl.debet) end, 'kredit" + BarisDept.ToString() + "' = case when dtl.kd_dept = '" + fld[0].ToString().Trim() + "' then sum(dtl.kredit) end";
                sSumKolomDept = sSumKolomDept + "isnull(sum(debet" + BarisDept.ToString() + "),0)debet" + BarisDept.ToString() + ", isnull(sum(kredit" + BarisDept.ToString() + "),0)kredit" + BarisDept.ToString();

                //Tambah Kolom
                tbl.Columns.Add("d" + BarisDept.ToString(), typeof(Decimal));

                BarisDept++;
            }
            rdr.Close();

            //== END  Cetak Kolom Nilai Program

            sql = " select sys.no_urut, sys.grup_laporan, sys.kd_gol, sys.nm_gol, sys.dk ";

            if (sSumKolomDept != "")
            {
                sql = sql + "," + sSumKolomDept;
            }

            sql = sql
                  + " from ac_sys_gol_akun sys "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT sys.kd_gol, sys.nm_gol ";

            if (sKolomDept != "")
            {
                sql = sql + "," + sKolomDept;
            }


            sql = sql
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       INNER JOIN ac_sys_gol_akun sys "
                  + "           ON mak.kd_gol = sys.kd_gol "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "           AND sys.laporan  ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "       GROUP BY sys.kd_gol, sys.nm_gol,dtl.kd_dept "
                  + " ) tr "
                  + "       on sys.kd_gol = tr.kd_gol "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + " GROUP BY sys.no_urut, sys.grup_laporan, sys.kd_gol, sys.nm_gol, sys.dk "
                  + " order by sys.no_urut ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_gol"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_gol"]);


                    for (int i = 1; i < BarisDept; i++)
                    {
                        if (AdnFungsi.CStr(rdr["dk"]).Trim().ToUpper() == AdnVar.SaldoNormal.DEBET)
                        {
                            baris["d" + i.ToString()] = Convert.ToDecimal(rdr["debet" + i.ToString()]) - Convert.ToDecimal(rdr["kredit" + i.ToString()]);
                        }
                        else
                        {
                            baris["d" + i.ToString()] = Convert.ToDecimal(rdr["kredit" + i.ToString()]) - Convert.ToDecimal(rdr["debet" + i.ToString()]);
                        }
                    }

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;
        }
        public DataTable Anggaran(string JenisLaporan, string ThAjar, string KdSekolah)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Anggaran", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));
            tbl.Columns.Add("Realisasi", typeof(Decimal));


            DateTime TglDr = new DateTime(AdnFungsi.CInt(ThAjar.Substring(0, 4),true), 7, 1);
            DateTime TglSd = new DateTime(AdnFungsi.CInt(ThAjar.Substring(5, 4), true), 6, 30);


            sql = " select sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk, "
                  + " isnull(debet,0)debet , isnull(kredit,0)kredit , agr.anggaran"
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       AND dtl.kd_dept = '" + KdSekolah + "'"
                  + "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " inner join "
                  + " ( "
                  + "       select kd_akun, sum(nilai) anggaran "
                  + "       from ac_tanggaran "
                  + "       where th_ajar ='" + ThAjar.Trim() + "'"
                  + "       and kd_sekolah ='" + KdSekolah.Trim() + "'"
                  + "       group by kd_akun "
                  + " ) agr "
                  + "       on mak.kd_akun = agr.kd_akun "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "       AND mak.tipe = 'DTL' "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                    baris["Anggaran"] = AdnFungsi.CDec(rdr["anggaran"]);

                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        baris["Debet"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        baris["Kredit"] = 0;
                        baris["Realisasi"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                    }
                    else
                    {
                        baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        baris["Debet"] = 0;
                        baris["Realisasi"] = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                    }

                    //baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    //baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }
        public DataTable AnggaranPerBulan(int Bulan, string JenisLaporan, string ThAjar, int Tahun, string KdSekolah)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Anggaran", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));
            tbl.Columns.Add("Realisasi", typeof(Decimal));


            //DateTime TglDr = new DateTime(AdnFungsi.CInt(ThAjar.Substring(0, 4), true), 1, 1);
            //DateTime TglSd = new DateTime(AdnFungsi.CInt(ThAjar.Substring(5, 4), true), 12, 31);


            sql = " select sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk, "
                  + " isnull(debet,0)debet , isnull(kredit,0)kredit , agr.anggaran"
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       WHERE year(hdr.tgl) ='" + Tahun + "' AND month(hdr.tgl) =" + Bulan
                  + "       AND dtl.kd_dept ='" + KdSekolah + "'"
                  + "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " inner join "
                  + " ( "
                  + "       select kd_akun, sum(nilai) anggaran "
                  + "       from ac_tanggaran "
                  + "       where th_ajar ='" + ThAjar.Trim() + "'"
                  + "           and bulan = " + Bulan 
                  + "           and kd_sekolah = '" + KdSekolah + "'"
                  + "       group by kd_akun "
                  + " ) agr "
                  + "       on mak.kd_akun = agr.kd_akun "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "       AND mak.tipe = 'DTL' "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                    baris["Anggaran"] = AdnFungsi.CDec(rdr["anggaran"]);

                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        baris["Debet"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        baris["Kredit"] = 0;
                        baris["Realisasi"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                    }
                    else
                    {
                        baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        baris["Debet"] = 0;
                        baris["Realisasi"] = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                    }

                    //baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    //baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }
        public DataTable AnggaranPerTanggal(string JenisLaporan, string ThAjar, string KdSekolah, int Bulan, int Tahun)
        {
            DataTable tbl = new DataTable("AppTabel");

            tbl.Columns.Add("NoUrut", typeof(int));
            tbl.Columns.Add("Grup", typeof(String));
            tbl.Columns.Add("DK", typeof(String));
            tbl.Columns.Add("KdAkun", typeof(String));
            tbl.Columns.Add("NmAkun", typeof(String));
            tbl.Columns.Add("Debet", typeof(Decimal));
            tbl.Columns.Add("Kredit", typeof(Decimal));
            tbl.Columns.Add("Anggaran", typeof(Decimal));
            tbl.Columns.Add("Turunan", typeof(int));
            tbl.Columns.Add("Realisasi", typeof(Decimal));

            int TglAkhir = DateTime.DaysInMonth(Tahun, Bulan);

            DateTime TglDr = new DateTime(AdnFungsi.CInt(ThAjar.Substring(0, 4), true),7, 1);
            DateTime TglSd = new DateTime(Tahun, Bulan, TglAkhir);
            
            sql = " select sys.no_urut, sys.grup_laporan, mak.kd_akun, mak.nm_akun, mak.dk, "
                  + " isnull(debet,0)debet , isnull(kredit,0)kredit , agr.anggaran"
                  + " from ac_makun mak "
                  + " left outer join  "
                  + " ( "
                  + "       SELECT mak.kd_akun, mak.nm_akun, isnull(sum(debet),0)debet , isnull(sum(kredit),0)kredit "
                  + "       FROM ac_makun mak "
                  + "       INNER JOIN ac_tjurnal_dtl dtl "
                  + "           ON mak.kd_akun = dtl.kd_akun "
                  + "       INNER JOIN ac_tjurnal hdr "
                  + "           ON hdr.kd_jurnal = dtl.kd_jurnal "
                  + "       WHERE hdr.tgl >='" + AdnFungsi.SetSqlTglEN(TglDr) + "' AND hdr.tgl <'" + AdnFungsi.SetSqlTglEN(TglSd.AddDays(1)) + "'"
                  + "       AND dtl.kd_dept = '" + KdSekolah + "'"
                  + "       GROUP BY mak.kd_akun, mak.nm_akun "
                  + " ) tr "
                  + "       on mak.kd_akun = tr.kd_akun "
                  + " inner join ac_sys_gol_akun sys "
                  + "       on mak.kd_gol = sys.kd_gol "
                  + " inner join "
                  + " ( "
                  + "       select kd_akun, sum(nilai) anggaran "
                  + "       from ac_tanggaran "
                  + "       where th_ajar ='" + ThAjar.Trim() + "'"
                  + "       and bulan = " + Bulan
                  + "       and kd_sekolah ='" + KdSekolah.Trim() + "'"
                  + "       group by kd_akun "
                  + " ) agr "
                  + "       on mak.kd_akun = agr.kd_akun "
                  + " where sys.laporan ='" + JenisLaporan.ToString().ToUpper().Trim() + "' "
                  + "       AND mak.tipe = 'DTL' "
                  + " order by sys.no_urut, mak.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr = this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    DataRow baris = tbl.NewRow();
                    baris["NoUrut"] = AdnFungsi.CInt(rdr["no_urut"], true);
                    baris["Grup"] = AdnFungsi.CStr(rdr["grup_laporan"]);
                    baris["KdAKun"] = AdnFungsi.CStr(rdr["kd_akun"]);
                    baris["NmAKun"] = AdnFungsi.CStr(rdr["nm_akun"]);
                    baris["Anggaran"] = AdnFungsi.CDec(rdr["anggaran"]);

                    if (AdnFungsi.CStr(rdr["DK"]) == "DEBET")
                    {
                        baris["Debet"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                        baris["Kredit"] = 0;
                        baris["Realisasi"] = AdnFungsi.CDec(rdr["debet"]) - AdnFungsi.CDec(rdr["kredit"]);
                    }
                    else
                    {
                        baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                        baris["Debet"] = 0;
                        baris["Realisasi"] = AdnFungsi.CDec(rdr["kredit"]) - AdnFungsi.CDec(rdr["debet"]);
                    }

                    //baris["Debet"] = AdnFungsi.CDec(rdr["debet"]);
                    //baris["Kredit"] = AdnFungsi.CDec(rdr["kredit"]);

                    tbl.Rows.Add(baris);
                }
                rdr.Close();

            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }

            return tbl;

        }

        

    }
}
