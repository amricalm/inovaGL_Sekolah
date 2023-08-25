using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Andhana;

namespace inovaGL
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
                                + " from ac_tkm hdr "
                                + " inner join ac_tkm_dtl dtl "
                                + "     on hdr.kd_tkm = dtl.kd_tkm "
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
                            + " left outer join ac_tkm_dtl dtl"
                                + " on mak.kd_akun = dtl.kd_akun"
                            + " left outer join ac_tkm hdr "
                            + "     on hdr.kd_tkm = dtl.kd_tkm "
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
                  + "       on mak.kd_akun = tr.kd_akun ";

            try
            {
                cmd.CommandText = sql;
                rdr=this.cmd.ExecuteReader();

                while (rdr.Read())
                {
                    int Turunan = AdnFungsi.CInt(rdr["turunan"], true);
                    string Item = AdnFungsi.CStr(rdr["nm_akun"]);

                    DataRow baris = tmpTbl.NewRow();
                    baris["Item"] = Item.PadLeft(Item.Length + (Turunan * 8));
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
            for (int i = 0; i < tmpTbl.Rows.Count; i++)
            {
                int Turunan = int.Parse(tmpTbl.Rows[i]["Turunan"].ToString());

                if (Turunan < Tk)
                {
                    for (int x = Tk-1; x >=Turunan; x--)
                    {
                        DataRow barisTotal = tbl.NewRow();
                        string HdText = "TOTAL" + HeaderItem[x];
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
                baris["Item"] = tmpTbl.Rows[i]["Item"].ToString();
                baris["Debet"] = decimal.Parse(tmpTbl.Rows[i]["Debet"].ToString());
                baris["Kredit"] = decimal.Parse(tmpTbl.Rows[i]["Kredit"].ToString());
                baris["Nilai"] = decimal.Parse(tmpTbl.Rows[i]["Nilai"].ToString());

                tbl.Rows.Add(baris);
            }

            if (Tk>0)
            {
                for (int x = Tk - 1; x >= 0; x--)
                {
                    DataRow barisTotal = tbl.NewRow();
                    string HdText = "TOTAL" + HeaderItem[x];
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
}
