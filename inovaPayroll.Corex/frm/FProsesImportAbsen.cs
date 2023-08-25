using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;
using System.IO;
using Excel;

namespace inovaPayroll.Core
{
    [AdnScObjectAtr("Form: Proses Batch Import Transaksi Virtual Account", "Proses")]
    public partial class FProsesImportAbsen : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;
        private AdnScPengguna Pengguna;
        private string ThAjar;

        public FProsesImportAbsen(SqlConnection cnn,string AppName, AdnScPengguna Pengguna, string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            this.Pengguna = Pengguna;
            this.ThAjar = ThAjar;

            //AdnFungsi.SetComboBulan(comboBoxBulan, false);
            //comboBoxBulan.SelectedIndex = DateTime.Now.Month - 1;
            //updTahun.Value = DateTime.Now.Year;

            //new AdnAkunDao(this.cnn).SetComboKasDetail(comboBoxKas);
            //new AdnAkunDao(this.cnn).SetComboKasDetail(comboBoxKasPindah);
        }
        
        private void Tambah()
        {
            this.DokumenBaru();
        }
        private void Edit()
        {
            this.ModeEdit = AdnModeEdit.UBAH;

            panelHdr.Enabled = true;
           
            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;

        }
        private void Simpan()
        {
        }
        private void Hapus()
        {

        }
        private void DokumenBaru()
        {
            AdnFungsi.Bersih(this, true);
            this.ModeEdit = AdnModeEdit.BARU;
            panelHdr.Enabled = true;

            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = false;
            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Batal()
        {
            textBoxFile.Text = "";
            dgv.Rows.Clear();
            this.DokumenBaru();
        }
        private void GetData(string Kd)
        {
        }
       
        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            this.Tambah();
        }
        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            this.Edit();
        }
        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.Simpan();
        }
        private void toolStripButtonBatal_Click(object sender, EventArgs e)
        {
            this.Batal();
        }
        private void toolStripButtonHapus_Click(object sender, EventArgs e)
        {
            this.Hapus();
        }
        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FTVoucherGenerator_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (MessageBox.Show("Yakin Jendela Ini Akan Ditutup?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        this.Close();
                    }
                    break;

                case Keys.F12:
                    if (toolStripButtonSimpan.Enabled)
                    {
                        this.Simpan();
                    }
                    break;

                case Keys.F11:
                    if (toolStripButtonTambah.Enabled)
                    {
                        this.Tambah();
                    }
                    break;

                case Keys.F10:
                    if (toolStripButtonEdit.Enabled)
                    {
                        this.Edit();
                        e.Handled =true;
                    }
                    break;

                case Keys.D:
                    if (Control.ModifierKeys == Keys.Control && toolStripButtonHapus.Enabled)
                    {
                        this.Hapus();
                    }
                    break;

                case Keys.N:
                    if (Control.ModifierKeys == Keys.Control && toolStripButtonBatal.Enabled)
                    {
                        this.Batal();
                    }
                    break;
            }
        }

        private void buttonProses_Click(object sender, EventArgs e)
        {
            VacMuamalat();
        }


        private void dateTimePickerTgl_ValueChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }
        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            //DataTable lst = new AdnLoketDao(this.cnn).GetByPeriode((int)updTahun.Value, comboBoxBulan.SelectedIndex+1);
            //dgv.Rows.Clear();

            //for (int i = 0; i < lst.Rows.Count;i++ )
            //{
                //dgv.Rows.Add();
                //dgv.Rows[i].Cells["Tgl"].Value = lst.Rows[i]["Tgl"];
                //dgv.Rows[i].Cells["Jmh"].Value = lst.Rows[i]["Jmh"];
                //dgv.Rows[i].Cells["Status"].Value = lst.Rows[i]["Status"];
            //}

            if (dgv.RowCount == 0)
            {
                buttonProses.Enabled = false;
                //toolStripButtonPilih.Enabled = false;
            }
            else
            {
                buttonProses.Enabled=true;
                //toolStripButtonPilih.Enabled = true;
            }

            this.UseWaitCursor = false;
        }

        private void comboBoxKas_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

        private void comboBoxBulan_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

        private void updTahun_ValueChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

        private void buttonPilihFile_Click(object sender, EventArgs e)
        {
            //PilihFileTextMuamalat();
            PilihFileTextBNI();
        }

        private void VacMuamalat()
        {
            //SqlTransaction Trans = null;
            //if (textBoxFile.Text.Trim() == "")
            //{
            //    MessageBox.Show("Pilih File Transaksi Terlebih Dahulu!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //{
            //    List<EDUSIS.KeuanganPembayaran.AdnLoket> lstPembayaran = new List<EDUSIS.KeuanganPembayaran.AdnLoket>();
            //    try
            //    {
            //        Trans = this.cnn.BeginTransaction();
            //        string NoVA = "";

            //        AdnTmpVa o = new AdnTmpVa();
            //        string[] tmp = textBoxFile.Text.ToString().Trim().Split('\\');
            //        o.NmFile = tmp[tmp.Length - 1];

            //        for (int i = 0; i < dgv.Rows.Count; i++)
            //        {
            //            AdnTmpVaDtl dtl = new AdnTmpVaDtl();
            //            dtl.Baris = AdnFungsi.CStr(dgv.Rows[i].Cells["IsiLengkap"]);
            //            o.ItemDf.Add(dtl);

            //            NoVA = AdnFungsi.CStr(dgv.Rows[i].Cells["NoVA"]);
            //            EDUSIS.Siswa.AdnSiswa sis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetByNoVA(NoVA);

            //            EDUSIS.KeuanganPembayaran.AdnLoket olk = new EDUSIS.KeuanganPembayaran.AdnLoket();
            //            olk.KasPerkiraan = "";//Harus ADA pilihan Bank di Form
            //            olk.KdKwitansi = "";
            //            olk.KdSekolah = AppVar.KdSekolah;
            //            olk.KdSiswa = sis.KdSiswa;
            //            olk.Nis = sis.NIS;
            //            olk.NoBayar = 0;
            //            olk.Posting = false;
            //            olk.Sumber = "VA";
            //            olk.ThAjar = AppVar.ThAjar;
            //            olk.ThAjarTagihan = AppVar.ThAjar;
            //            olk.Total = 0;
                        
            //            EDUSIS.KeuanganPembayaran.AdnLoketDtl item = new EDUSIS.KeuanganPembayaran.AdnLoketDtl();
            //            item.KdBiaya = "";
            //            item.ItemBulan = "";
            //            item.Diskon = 0;
            //            item.Jmh = 0;
            //            item.JmhBulan = 0;
            //            item.KdSekolah = olk.KdSekolah;
            //            item.Nis = olk.Nis;
            //            item.NoBayar = olk.NoBayar;
            //            item.Potongan = 0;
            //            item.Tag = null;
            //            item.Tgl = olk.Tgl;
            //            item.ThAjar = olk.ThAjar;
            //            olk.DfItem.Add(item);

            //            lstPembayaran.Add(olk);
            //        }
            //        AdnTmpVaDao dao = new AdnTmpVaDao(this.cnn, this.Pengguna, Trans);
            //        EDUSIS.KeuanganPembayaran.AdnLoketDao DaoLoket = new EDUSIS.KeuanganPembayaran.AdnLoketDao(this.cnn, this.Pengguna, Trans);
                    
            //        dao.Simpan(o);
            //        foreach (EDUSIS.KeuanganPembayaran.AdnLoket ItemPembayaran in lstPembayaran)
            //        {
            //            DaoLoket.Simpan(ItemPembayaran);
            //        }
            //        Trans.Commit();
            //        MessageBox.Show("Semua Transaksi Berhasil Di Import!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    }
            //    catch (Exception exp)
            //    {
            //        AdnFungsi.LogErr("Terjadi Kesalahan.\n" + exp.Message,true);
            //        if (Trans != null)
            //        {
            //            Trans.Rollback();
            //        }
            //    }
            //}//f (textBoxFile.Text.Trim() == "")
        }

        private void VacBNI()
        {
            //SqlTransaction Trans = null;
            //if (textBoxFile.Text.Trim() == "")
            //{
            //    MessageBox.Show("Pilih File Transaksi Terlebih Dahulu!", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
            //else
            //{
            //    List<EDUSIS.KeuanganPembayaran.AdnLoket> lstPembayaran = new List<EDUSIS.KeuanganPembayaran.AdnLoket>();
            //    try
            //    {
            //        Trans = this.cnn.BeginTransaction();
            //        string NoVA = "";

            //        AdnTmpVa o = new AdnTmpVa();
            //        string[] tmp = textBoxFile.Text.ToString().Trim().Split('\\');
            //        o.NmFile = tmp[tmp.Length - 1];
















            //        for (int i = 0; i < dgv.Rows.Count; i++)
            //        {
            //            AdnTmpVaDtl dtl = new AdnTmpVaDtl();
            //            dtl.Baris = AdnFungsi.CStr(dgv.Rows[i].Cells["IsiLengkap"]);
            //            o.ItemDf.Add(dtl);

            //            NoVA = AdnFungsi.CStr(dgv.Rows[i].Cells["NoVA"]);
            //            EDUSIS.Siswa.AdnSiswa sis = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetByNoVA(NoVA);

            //            EDUSIS.KeuanganPembayaran.AdnLoket olk = new EDUSIS.KeuanganPembayaran.AdnLoket();
            //            olk.KasPerkiraan = "";//Harus ADA pilihan Bank di Form
            //            olk.KdKwitansi = "";
            //            olk.KdSekolah = AppVar.KdSekolah;
            //            olk.KdSiswa = sis.KdSiswa;
            //            olk.Nis = sis.NIS;
            //            olk.NoBayar = 0;
            //            olk.Posting = false;
            //            olk.Sumber = "VA";
            //            olk.ThAjar = AppVar.ThAjar;
            //            olk.ThAjarTagihan = AppVar.ThAjar;
            //            olk.Total = 0;

            //            EDUSIS.KeuanganPembayaran.AdnLoketDtl item = new EDUSIS.KeuanganPembayaran.AdnLoketDtl();
            //            item.KdBiaya = "";
            //            item.ItemBulan = "";
            //            item.Diskon = 0;
            //            item.Jmh = 0;
            //            item.JmhBulan = 0;
            //            item.KdSekolah = olk.KdSekolah;
            //            item.Nis = olk.Nis;
            //            item.NoBayar = olk.NoBayar;
            //            item.Potongan = 0;
            //            item.Tag = null;
            //            item.Tgl = olk.Tgl;
            //            item.ThAjar = olk.ThAjar;
            //            olk.DfItem.Add(item);

            //            lstPembayaran.Add(olk);
            //        }
            //        AdnTmpVaDao dao = new AdnTmpVaDao(this.cnn, this.Pengguna, Trans);
            //        EDUSIS.KeuanganPembayaran.AdnLoketDao DaoLoket = new EDUSIS.KeuanganPembayaran.AdnLoketDao(this.cnn, this.Pengguna, Trans);

            //        dao.Simpan(o);
            //        foreach (EDUSIS.KeuanganPembayaran.AdnLoket ItemPembayaran in lstPembayaran)
            //        {
            //            DaoLoket.Simpan(ItemPembayaran);
            //        }
            //        Trans.Commit();
            //        MessageBox.Show("Semua Transaksi Berhasil Di Import!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    }
            //    catch (Exception exp)
            //    {
            //        AdnFungsi.LogErr("Terjadi Kesalahan.\n" + exp.Message, true);
            //        if (Trans != null)
            //        {
            //            Trans.Rollback();
            //        }
            //    }
            //}//f (textBoxFile.Text.Trim() == "")
        }

        private void PilihFileTextMuamalat()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxFile.Text = dlg.FileName.ToString();

                if (textBoxFile.Text.Trim() == "")
                {
                    MessageBox.Show("Pilih File Transaksi Terlebih Dahulu!", this.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        System.IO.StreamReader sr = new
                        System.IO.StreamReader(textBoxFile.Text.Trim());
                        int i = 0;
                        dgv.Rows.Clear();
                        while (sr.Peek() >= 0)
                        {
                            string IsiLengkap = sr.ReadLine();
                            string[] Arr = IsiLengkap.Split(',');
                            if (Arr.Length == 6)
                            {
                                dgv.Rows.Add();
                                dgv.Rows[i].Cells["NoVA"].Value = Arr[0].ToString();
                                dgv.Rows[i].Cells["NmSiswa"].Value = Arr[1].ToString();
                                dgv.Rows[i].Cells["Jmh"].Value = AdnFungsi.CDec(Arr[2].ToString());
                                dgv.Rows[i].Cells["Tgl"].Value = Arr[3].ToString();
                                dgv.Rows[i].Cells["Ket"].Value = Arr[4].ToString() + "," + Arr[5].ToString();
                                dgv.Rows[i].Cells["NoVA"].Value = Arr[0].ToString();

                                dgv.Rows[i].Cells["IsiLengkap"].Value = IsiLengkap.Trim();

                                i++;
                            }
                        }
                        sr.Close();
                        if (dgv.Rows.Count > 0)
                        {
                            buttonProses.Enabled = true;
                        }
                        else
                        {
                            buttonProses.Enabled = false;
                        }
                    }
                    catch (Exception exp)
                    {
                        AdnFungsi.LogErr(exp.Message);
                    }
                }


            }
        }
        private void PilihFileTextBNI()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBoxFile.Text = dlg.FileName.ToString();

                if (textBoxFile.Text.Trim() == "")
                {
                    MessageBox.Show("Pilih File Transaksi Terlebih Dahulu!", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        FileStream stream = File.Open(textBoxFile.Text.Trim(), FileMode.Open, FileAccess.Read);

                        //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        //...
                        //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                        //IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        //...
                        //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                        DataSet result = excelReader.AsDataSet();
                        //...
                        //4. DataSet - Create column names from first row
                        //excelReader.IsFirstRowAsColumnNames = true;
                        //DataSet result = excelReader.AsDataSet();

                        //5. Data Reader methods
                        //List<AdnBniTransaksi> lst = new List<AdnBniTransaksi>();
                        //while (excelReader.Read())
                        //{
                        //    if (excelReader[15] != null)
                        //    {
                        //        if (excelReader[2].ToString()!="Post Date")
                        //        {
                        //            AdnBniTransaksi o = new AdnBniTransaksi();
                        //            o.Tgl = excelReader[2].ToString();
                        //            o.Cabang = excelReader[3].ToString();
                        //            o.NoJurnal = excelReader[6].ToString();
                        //            o.Deskripsi = excelReader[7].ToString();
                        //            //o.Jmh = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits excelReader[11].ToString();

                        //            decimal Jmh = decimal.Parse(excelReader[11].ToString(), System.Globalization.CultureInfo.GetCultureInfo("en-US").NumberFormat);
                        //            o.Jmh = Jmh.ToString();
                        //            o.DK = excelReader[14].ToString();
                        //            o.Saldo = excelReader[15].ToString();
                        //            o.NoVac = this.GetVac(o.Deskripsi);

                        //            lst.Add(o);    
                        //        }
                        //    }
                        //}

                        //6. Free resources (IExcelDataReader is IDisposable)
                        excelReader.Close();
                        dgv.AutoGenerateColumns = false;
                        //dgv.DataSource = lst;

             
                        if (dgv.Rows.Count > 0)
                        {
                            buttonProses.Enabled = true;
                        }
                        else
                        {
                            buttonProses.Enabled = false;
                        }
                    }
                    catch (Exception exp)
                    {
                        AdnFungsi.LogErr(exp.Message);
                    }
                }


            }
        }


        private string GetVac(string Tulisan)
        {
            string Hasil = "";
            string pattern = @"\b9880\w+\b";
            Match o = Regex.Match(Tulisan, pattern);

            Hasil = o.Value;
            return Hasil;
        }

        private void buttonVerifikasi_Click(object sender, EventArgs e)
        {
            //DataTable ext = new AdnVacDao(this.cnn).GetSiswaExt("9880222209030948", "BNI");
            
            //EDUSIS.Siswa.AdnSiswa Siswa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetByNoVA("9880222209030948");
            //EDUSIS.KeuanganPembayaran.FTPembayaranVac ofm = new EDUSIS.KeuanganPembayaran.FTPembayaranVac(this.cnn,"", this.Pengguna,"",this.ThAjar, "", "","", AdnFungsi.CInt(ext.Rows[0]["KdSiswa"],true));
            //ofm.ShowDialog();
            
        }

        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //decimal JmhTransfer = AdnFungsi.CDec(dgv.Rows[e.RowIndex].Cells["Jmh"]);
            //string NoVA = AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["NoVA"]);
            //DataTable ext = new AdnVacDao(this.cnn).GetSiswaExt(NoVA, "BNI");

            //if (ext.Rows.Count > 0)
            //{
            //    EDUSIS.KeuanganPembayaran.FTPembayaranVac ofm = new EDUSIS.KeuanganPembayaran.FTPembayaranVac(this.cnn, "", this.Pengguna, "", this.ThAjar, "", "", "", AdnFungsi.CInt(ext.Rows[0]["KdSiswa"], true), JmhTransfer);
            //    ofm.ShowDialog();
            //}
            //else
            //{
            //    MessageBox.Show("Data Siswa Tidak Ditemukan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
        }

        private void dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
