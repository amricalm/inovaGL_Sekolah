using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using Andhana;
using Andhana.Karyawan;
using inovaGL.Laporan;

namespace inovaGL
{
    public partial class F_APP : Form
    {
        private SqlConnection cnn;
        
        public F_APP()
        {
            InitializeComponent();
            //AppFungsi.SetKoneksiApp();
            AppFungsi.LoadSysVar();
            this.cnn = AppVar.AppConn;
            toolStripStatusLabelThAjar.Text = AppVar.ThAjar;
        }

        private void SetAllMenuItems(ToolStripItemCollection menus)
        {
            //ToolStripItem tsItem;
            ToolStripMenuItem tsMenuItem;

            foreach (ToolStripItem tsItem in menus)
            {
                tsItem.Enabled = true;
                if (tsItem.GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                {
                    tsMenuItem = (ToolStripMenuItem)tsItem;
                    int iPanjang = "toolStripMenuItem".Length;
                    string nm = tsMenuItem.Name.Substring(iPanjang);

                    bool role = new AdnScGroupRoleDao(AppVar.AppConn).GetRoleBaca(AppVar.AppPengguna.kd_group, nm);
                    tsMenuItem.Enabled = role;
                    this.SetAllMenuItems(tsMenuItem.DropDownItems);
                }
                if (AppVar.Organisasi == "Yayasan Mantab Al Hamid")
                {
                    asetToolStripMenuItem.Visible = false;
                    persediaanToolStripMenuItem.Visible = false;
                    toolStripMenuItemFSaldoPiutangSiswa.Visible = false;
                    toolStripMenuItemFSaldoUtangSiswa.Visible = false;
                    toolStripMenuItemFProsesImportLoketPSB.Visible = false;
                    toolStripSeparator20.Visible = false;
                }

                
            }

        }

        private void keluarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void F_APP_Load(object sender, EventArgs e)
        {
            ToolStripMenuItem myMenuItem;
            foreach (ToolStripItem c in menuStrip1.Items)
            {
                if (c.GetType().ToString() == "System.Windows.Forms.ToolStripMenuItem")
                {
                    myMenuItem = (ToolStripMenuItem)c;
                    this.SetAllMenuItems(myMenuItem.DropDownItems);
                }
            }

            if (  AppVar.AppPengguna.kd_group != AdnScVarGroup.SysAdmin)
            {
                this.toolStripMenuItemSecurity.Enabled= true;
            }

            try
            {
                Bitmap img = new Bitmap(@"app.gif", true);
                this.BackgroundImage = img;
            }
            catch (Exception exp)
            {
                string sPesan = "";
                switch (exp.Source)
                {
                    case "System.Drawing":
                        sPesan = "Gambar Tidak Ditemukan. Atau Terjadi Kesalahan pada Gambar!";
                        break;

                }

                MessageBox.Show(exp.Source + ", " + exp.Message + "\n" + sPesan, AppVar.CaptionDialogBox, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            
        }

        private void F_APP_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Anda Akan Keluar Dari Program " + AppVar.AppName + " ?", AppVar.CaptionDialogBox, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                e.Cancel = true;
            }
        }




        private void toolStripMenuItemKaryawan_Click(object sender, EventArgs e)
        {
            FMKyw ofm = new FMKyw(AppVar.AppName, AppVar.AppConn);
            ofm.ShowDialog();
        }


        private void toolStripMenuItemPengguna_Click(object sender, EventArgs e)
        {
            FScPengguna ofm = new FScPengguna(AppVar.AppName, this.cnn);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemGroupPengguna_Click(object sender, EventArgs e)
        {
            FScGroup ofm = new FScGroup(AppVar.AppName, this.cnn, AppVar.AppPengguna);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemKeamananBuatObjectRole_Click(object sender, EventArgs e)
        {
            AdnScFungsi.GetObjekAplikasi(AppVar.AppName, AppDomain.CurrentDomain.GetAssemblies(), "inovaPOS.exe", this.cnn);
        }

        private void toolStripMenuItemReceipt_Click(object sender, EventArgs e)
        {
            FTKasMasuk ofm = new FTKasMasuk(AppVar.AppConn, AppVar.AppName, AdnModeEdit.BARU, "",null, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTReceipt_Click(object sender, EventArgs e)
        {
            FDTKasMasuk ofm = new FDTKasMasuk(AppVar.AppConn, AppVar.AppName,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFProsesBatchReceipt_Click(object sender, EventArgs e)
        {
            FProsesImportLoket ofm = new FProsesImportLoket(AppVar.AppConn, AppVar.AppName, AppVar.AppPengguna);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTKasKeluar_Click(object sender, EventArgs e)
        {
            FDTKasKeluar ofm = new FDTKasKeluar(this.cnn, AppVar.AppName,AppVar.ThAjar);
            ofm.Show();
        }

        private void toolStripMenuItemFD_Click(object sender, EventArgs e)
        {
            FTKasKeluar ofm = new FTKasKeluar(this.cnn, AppVar.AppName, AdnModeEdit.BARU, "", null,AppVar.ThAjar);
            ofm.Show();
        }

        private void toolStripMenuItemFMAkun_Click(object sender, EventArgs e)
        {
            FMAkun ofm = new FMAkun(this.cnn, AppVar.AppName, AdnModeEdit.BARU, "", null);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDAkun_Click(object sender, EventArgs e)
        {
            FDAkun ofm = new FDAkun(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemBukuBank_Click(object sender, EventArgs e)
        {
            //FDBukuBank ofm = new FDBukuBank(AppVar.AppConn,AppVar.AppName);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapTr_Click(object sender, EventArgs e)
        {
            FDlgLapTr ofm = new FDlgLapTr(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "");
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapBB_Click(object sender, EventArgs e)
        {
            //inovaGL.Laporan. FDlgLapBukuBesar ofm = new FDlgLapBukuBesar(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "");
            FDlgLapBB ofm = new FDlgLapBB(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.AppName,AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapNeraca_Click(object sender, EventArgs e)
        {
            FDlgLapNeraca ofm = new FDlgLapNeraca(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi,AppVar.PeriodeMulai, AppVar.KdAkunLabaTahunBerjalan,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapNeracaPercobaan_Click(object sender, EventArgs e)
        {
            FDlgLapNeracaSaldo ofm = new FDlgLapNeracaSaldo(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi,AppVar.PeriodeMulai,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTAnggaran_Click(object sender, EventArgs e)
        {
            FDTAnggaran ofm = new FDTAnggaran(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemMemorial_Click(object sender, EventArgs e)
        {
            FTJurnalUmum ofm = new FTJurnalUmum(this.cnn, AppVar.AppName, AdnModeEdit.BARU, "", null,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTJurnalUmum_Click(object sender, EventArgs e)
        {
            FDTJurnalUmum ofm = new FDTJurnalUmum(this.cnn, AppVar.AppName,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuFTSaldoAwal_Click(object sender, EventArgs e)
        {
            FTSaldoAwal ofm = new FTSaldoAwal(this.cnn, AppVar.AppName, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFMAsetKelompok_Click(object sender, EventArgs e)
        {
            FMAsetKelompok ofm = new FMAsetKelompok(this.cnn, AppVar.AppName, AdnModeEdit.BARU, "", null);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFMAset_Click(object sender, EventArgs e)
        {
            FMAset ofm = new FMAset(this.cnn, AppVar.AppName, AdnModeEdit.BARU, "", null);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDAset_Click(object sender, EventArgs e)
        {
            FDAset ofm = new FDAset(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDAsetKelompok_Click(object sender, EventArgs e)
        {
            FDAsetKelompok ofm = new FDAsetKelompok(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapLR_Click(object sender, EventArgs e)
        {
            FDlgLapLR ofm = new FDlgLapLR(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "");
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFMSetup_Click(object sender, EventArgs e)
        {
            FMSetup ofm = new FMSetup(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFSaldoPiutangSiswa_Click(object sender, EventArgs e)
        {
            inovaGL.Piutang.FTSaldoPiutangSiswa ofm = new inovaGL.Piutang.FTSaldoPiutangSiswa(this.cnn, AppVar.AppName, AppVar.AppPengguna, AppVar.KdSekolah,AppVar.ThAjar,AppVar.KdAkunPenyeimbangSAW, AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFKeuanganTagihan_Click(object sender, EventArgs e)
        {
            EDUSIS.KeuanganTagihan.FKeuanganTagihan ofm = new EDUSIS.KeuanganTagihan.FKeuanganTagihan(this.cnn, AppVar.AppName,AppVar.AppPengguna, AppVar.KdSekolah, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDMBiaya_Click(object sender, EventArgs e)
        {
            EDUSIS.Biaya.FDMBiaya ofm = new EDUSIS.Biaya.FDMBiaya(this.cnn, AppVar.AppName,AppVar.AppPengguna);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFTPembayaran_Click(object sender, EventArgs e)
        {
            EDUSIS.KeuanganPembayaran.FTPembayaran ofm = new EDUSIS.KeuanganPembayaran.FTPembayaran(this.cnn, AppVar.AppName, AppVar.AppPengguna, AppVar.KdSekolah, AppVar.ThAjar,AppVar.ReportPath, AppVar.ReportExt,AppVar.Organisasi);
            ofm.ShowDialog();
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItemFDlgLapHarian_Click(object sender, EventArgs e)
        {
            EDUSIS.KeuanganLaporan.FDlgLapHarian ofm = new EDUSIS.KeuanganLaporan.FDlgLapHarian(this.cnn, AppVar.AppName, AppVar.AppPengguna, AppVar.KdSekolah, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "");
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDBiayaSekolah_Click(object sender, EventArgs e)
        {
            EDUSIS.Biaya.FDBiayaSekolah ofm = new EDUSIS.Biaya.FDBiayaSekolah(this.cnn, AppVar.AppName, AppVar.AppPengguna, AppVar.KdSekolah, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFMPos_Click(object sender, EventArgs e)
        {
            FMPos ofm = new FMPos(this.cnn, AppVar.AppName, AdnModeEdit.BARU, "", null);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDPos_Click(object sender, EventArgs e)
        {
            FDPos ofm = new FDPos(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapAnggaranRealisasi_Click(object sender, EventArgs e)
        {
            FDlgLapAnggaranRealisasi ofm = new FDlgLapAnggaranRealisasi(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi,AppVar.ThAjar,AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapLRbyDept_Click(object sender, EventArgs e)
        {
            FDlgLapLRbyDept ofm = new FDlgLapLRbyDept(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "");
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapNeracaStandar_Click(object sender, EventArgs e)
        {
            FDlgLapNeracaStandar ofm = new FDlgLapNeracaStandar(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.PeriodeMulai, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapLRStandar_Click(object sender, EventArgs e)
        {
            FDlgLapLRStandar ofm = new FDlgLapLRStandar(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "");
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapLRbyDeptStandar_Click(object sender, EventArgs e)
        {
            FDlgLapLRbyDeptStandar ofm = new FDlgLapLRbyDeptStandar(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "");
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFProsesBulanan_Click(object sender, EventArgs e)
        {
            FProsesBulanan ofm = new FProsesBulanan(this.cnn, AppVar.AppName, AppVar.AppPengguna, AppVar.KdSekolah, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTJurnalSiswa_Click(object sender, EventArgs e)
        {
            FDTJurnalSiswa ofm = new FDTJurnalSiswa(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFTJurnalSiswa_Click(object sender, EventArgs e)
        {
            FTJurnalSiswa ofm = new FTJurnalSiswa(this.cnn, AppVar.AppName, AdnModeEdit.BARU, "", null,AppVar.KdSekolah,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapPiutangSiswa_Click(object sender, EventArgs e)
        {
            FDlgLapPiutangSiswaPerKelas ofm = new FDlgLapPiutangSiswaPerKelas(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi,AppVar.PeriodeMulai, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFSaldoUtangSiswa_Click(object sender, EventArgs e)
        {
            inovaGL.Utang.FTSaldoUtangSiswa ofm = new inovaGL.Utang.FTSaldoUtangSiswa(this.cnn, AppVar.AppName, AppVar.AppPengguna, AppVar.KdSekolah, AppVar.ThAjar, AppVar.KdAkunPenyeimbangSAW, AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFProsesImportGaji_Click(object sender, EventArgs e)
        {
            FProsesImportGaji ofm = new FProsesImportGaji(this.cnn, AppVar.AppName, AppVar.AppPengguna);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFProsesImportLoketPSB_Click(object sender, EventArgs e)
        {
            FProsesImportLoketPSB ofm = new FProsesImportLoketPSB(this.cnn, AppVar.AppName, AppVar.AppPengguna);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDMBarang_Click(object sender, EventArgs e)
        {
            //inovaPOS.FDMBarang ofm = new inovaPOS.FDMBarang(this.cnn, AppVar.AppName);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFMBarang_Click(object sender, EventArgs e)
        {
            //inovaPOS.FMBarang ofm = new inovaPOS.FMBarang(this.cnn, AppVar.AppName, AdnVar.ModeEdit.BARU, "", null);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTMutasiMasuk_Click(object sender, EventArgs e)
        {
            //inovaPOS.FDTMutasiMasuk ofm = new inovaPOS.FDTMutasiMasuk(this.cnn, AppVar.AppName);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFDMutasiKeluar_Click(object sender, EventArgs e)
        {
            //inovaPOS.FDTMutasiKeluar ofm = new inovaPOS.FDTMutasiKeluar(this.cnn, AppVar.AppName);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFDMPemasok_Click(object sender, EventArgs e)
        {
            //inovaPOS.FMPemasok ofm = new inovaPOS.FMPemasok(this.cnn, AppVar.AppName);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFMGroupBarang_Click(object sender, EventArgs e)
        {
            //inovaPOS.FMGroupBarang ofm = new inovaPOS.FMGroupBarang(this.cnn, AppVar.AppName);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFMSatuan_Click(object sender, EventArgs e)
        {
            //inovaPOS.FMSatuan ofm = new inovaPOS.FMSatuan(this.cnn, AppVar.AppName);
            //ofm.ShowDialog();
        }

        private void toolStripMenuItemFProsesPenyusutan_Click(object sender, EventArgs e)
        {
            inovaGL.FProsesPenyusutan ofm = new inovaGL.FProsesPenyusutan(this.cnn, AppVar.AppName, AppVar.AppPengguna);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapAnggaranRealisasiBulanBerjalan_Click(object sender, EventArgs e)
        {
            FDlgLapAnggaranRealisasiBulanBerjalan ofm = new FDlgLapAnggaranRealisasiBulanBerjalan(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapAnggaranRealisasiPerTanggal_Click(object sender, EventArgs e)
        {
            FDlgLapAnggaranRealisasiPerTanggal ofm = new FDlgLapAnggaranRealisasiPerTanggal(this.cnn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.ThAjar,AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFProses_Click(object sender, EventArgs e)
        {
            FProses ofm = new FProses(this.cnn, AppVar.AppName, AppVar.PeriodeMulai, AppVar.KdAkunLabaTahunBerjalan,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFProsesImportTransaksiVA_Click(object sender, EventArgs e)
        {
            EDUSIS.VirtualAccount.FProsesImportTransaksiVA ofm = new EDUSIS.VirtualAccount.FProsesImportTransaksiVA(this.cnn, AppVar.AppName, AppVar.AppPengguna,AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFMThAjar_Click(object sender, EventArgs e)
        {
            FMThAjar ofm = new FMThAjar(this.cnn, AppVar.AppName);
            ofm.ShowDialog();
        }


        private void F_APP_Activated(object sender, EventArgs e)
        {
            toolStripStatusLabelThAjar.Text = AppVar.ThAjar;
        }

        private void toolStripMenuItemFDlgLapLRThBerjalan_Click(object sender, EventArgs e)
        {
            FDlgLapLRThBerjalan ofm = new FDlgLapLRThBerjalan(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapPiutangSiswaPerKelas_Click(object sender, EventArgs e)
        {
            FDlgLapPiutangSiswaPerKelas ofm = new FDlgLapPiutangSiswaPerKelas(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi,AppVar.PeriodeMulai, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapPiutangSiswaPerAkun_Click(object sender, EventArgs e)
        {
            FDlgLapPiutangSiswaPerAkun ofm = new FDlgLapPiutangSiswaPerAkun(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.PeriodeMulai, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapUtangSiswaPerAkun_Click(object sender, EventArgs e)
        {
            FDlgLapUtangSiswaPerAkun ofm = new FDlgLapUtangSiswaPerAkun(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.PeriodeMulai, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapUtangSiswaPerKelas_Click(object sender, EventArgs e)
        {
            FDlgLapUtangSiswaPerKelas ofm = new FDlgLapUtangSiswaPerKelas(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.PeriodeMulai, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapSaldoPendapatan_Click(object sender, EventArgs e)
        {
            FDlgLapSaldoPendapatan ofm = new FDlgLapSaldoPendapatan(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, "", AppVar.PeriodeMulai, AppVar.ThAjar,AppVar.KdAkunUangMukaSiswa);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapPengeluaranPerSumberDana_Click(object sender, EventArgs e)
        {
            FDlgLapPengeluaranPerSumberDana ofm = new FDlgLapPengeluaranPerSumberDana(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.AppName, AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDlgLapPenerimaanPerPeriode_Click(object sender, EventArgs e)
        {
            FDlgLapPenerimaanPerPeriode ofm = new FDlgLapPenerimaanPerPeriode(AppVar.AppConn, AppVar.ReportPath, AppVar.ReportExt, AppVar.Organisasi, AppVar.AppName, AppVar.PeriodeMulai);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTNoVA_Click(object sender, EventArgs e)
        {
            EDUSIS.VirtualAccount.FDTNoVA ofm = new EDUSIS.VirtualAccount.FDTNoVA(this.cnn, AppVar.AppName, AppVar.ThAjar);
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFProsesImportAbsen_Click(object sender, EventArgs e)
        {
            inovaPayroll.Core.FProsesImportAbsen ofm = new inovaPayroll.Core.FProsesImportAbsen(this.cnn, AppVar.AppName, AppVar.AppPengguna, AppVar.ThAjar);
            //inovaPayroll.Core.Form1 ofm = new inovaPayroll.Core.Form1();
            ofm.ShowDialog();
        }

        private void toolStripMenuItemFDTNoKaryawanMesinAbsen_Click(object sender, EventArgs e)
        {
            inovaPayroll.Core.FDTNoKaryawanMesinAbsen ofm = new inovaPayroll.Core.FDTNoKaryawanMesinAbsen(this.cnn, AppVar.AppName, AppVar.ThAjar);
            ofm.ShowDialog();
        }





        
 
        
  
    }
}
