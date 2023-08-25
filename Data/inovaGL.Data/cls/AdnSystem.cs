using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaGL.Data
{

    public class AdnLaporanModel
    {
        public static string EdusisModel = "";
    }


    class AdnBulan
    {
        public int Bulan { get; set; }
        public string NamaBulan { get; set; }

        public AdnBulan(int aBulan, string aNamaBulan)
        {
            this.Bulan = aBulan;
            this.NamaBulan=aNamaBulan;
        }
    }

    public class AdnSysBulan
    {
        public void SetComboBoxBulan(System.Windows.Forms.ComboBox cbo)
        {
            cbo.Items.Add(new AdnBulan(1, "Januari"));
            cbo.Items.Add(new AdnBulan(2, "Februari"));
            cbo.Items.Add(new AdnBulan(3, "Maret"));
            cbo.Items.Add(new AdnBulan(4, "April"));
            cbo.Items.Add(new AdnBulan(5, "Mei"));
            cbo.Items.Add(new AdnBulan(6, "Juni"));
            cbo.Items.Add(new AdnBulan(7, "Juli"));
            cbo.Items.Add(new AdnBulan(8, "Agustus"));
            cbo.Items.Add(new AdnBulan(9, "September"));
            cbo.Items.Add(new AdnBulan(10, "Oktober"));
            cbo.Items.Add(new AdnBulan(11, "November"));
            cbo.Items.Add(new AdnBulan(12, "Desember"));
            cbo.DisplayMember = "NamaBulan";
            cbo.ValueMember = "Bulan";

        }
    }

    public class AdnJurnalVar
    {
        public struct JenisJurnal
        {
            public const string JKAS  ="JKAS";// Penerimaan Kas untuk pembayaran di loket (Bisa sekalian bayar SPP, DPG, DKT, dll)
            public const string JSAW_PIUTANG = "JSP"; //Saldo Awal Piutang [ Piutang > Pendapatan ]
            public const string JSAW_UTANG = "JSU"; //Saldo Awal Utang [ Pendapatan di Muka> Pendapatan ]

            public const string JGAJ = "JGAJ";// Jurnal Gaji Glondongan Per Periode
            
            public const string JBL_PIU_PDP = "JBL01";
            public const string JDT_PIU_KEW = "JDT01"; //Piutang pada Kewajiban
            public const string JDP_PIU_KEW = "JDP01"; //Piutang pada Kewajiban

            public const string JDT_KEW_PDP = "JDT02";//Kewajiban [Pendapatan Di Muka] pada Pendapatan
            public const string JDP_KEW_PDP = "JDP02";//Kewajiban [Pendapatan Di Muka]  pada Pendapatan

            public const string JSUSUT = "JSUSUT";//Jurnal Penyusutan Aset

            public const string JDP_DEP_PIU = "JDP_ADJ01";//Jurnal Adjusment (Penyesuaian) Deposit Dana Pengembangan
            public const string JDT_DEP_PIU = "JDT_ADJ01";//Jurnal Adjusment (Penyesuaian) Deposit Dana Tahunan
        }
    }

    public struct MetodaPenyusutan
    {
        public const string GarisLurus = "GARIS_LURUS";
        public const string SaldoMenurun = "SALDO_MENURUN";
    }

}
