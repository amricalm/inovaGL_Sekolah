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

            AdnFungsi.SetComboBulan(comboBoxBulan, false);
            comboBoxBulan.SelectedIndex = DateTime.Now.Month - 1;
            numericUpDownTahun.Value = DateTime.Now.Year;
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
            //textBoxFile.Text = "";
            //dgv.Rows.Clear();
            //this.DokumenBaru();
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
            string sPeriode = numericUpDownTahun.Value.ToString() + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
            string NoKaryawan = ""; string Nip = "";
            int JmhLembur = 0;
            int JmhKerja = 0;

            List<AdnGajiVar> lst = new List<AdnGajiVar>();
            AdnGajiVar oVar;
            string PeriodeBaris;
            foreach (DataGridViewRow baris in dgv.Rows)
            {
                PeriodeBaris = AdnFungsi.CStr(baris.Cells["Tgl"]).Substring(6, 4) + AdnFungsi.CStr(baris.Cells["Tgl"]).Substring(3, 2);
                if (PeriodeBaris == sPeriode)
                {
                    if (AdnFungsi.CStr(baris.Cells["NoKaryawan"]) != NoKaryawan)
                    {
                        if (NoKaryawan != "")
                        {
                            if (Nip!="")// Nip dan No Karyawan Harus Dipetakan dulu
                            {
                                if (JmhKerja > 0)
                                {
                                    oVar = new AdnGajiVar();
                                    oVar.Nip = Nip;
                                    oVar.Periode = sPeriode;
                                    oVar.KdJenis = MVar.Jenis.HARI_HADIR;
                                    oVar.Jmh = JmhKerja;
                                    lst.Add(oVar);
                                }
                                if (JmhLembur > 0)
                                {
                                    oVar = new AdnGajiVar();
                                    oVar.Nip = Nip;
                                    oVar.Periode = sPeriode;
                                    oVar.KdJenis = MVar.Jenis.HARI_LEMBUR;
                                    oVar.Jmh = JmhLembur;
                                    lst.Add(oVar);
                                }
                            }
                        }
                        NoKaryawan = AdnFungsi.CStr(baris.Cells["NoKaryawan"]);
                        Nip = AdnFungsi.CStr(baris.Cells["Nip"]);
                        JmhKerja = 0; JmhLembur = 0;
                    }
                    if (AdnFungsi.CStr(baris.Cells["WaktuKerja"]) != "")
                    {
                        JmhKerja++;
                    }
                    if (AdnFungsi.CStr(baris.Cells["WaktuLembur"]) != "")
                    {
                        JmhLembur++;
                    }
                }//if (PeriodeBaris == sPeriode)
            }//foreach
            if (NoKaryawan != "")
            {
                if (Nip != "")// Nip dan No Karyawan Harus Dipetakan dulu
                {
                    oVar = new AdnGajiVar();
                    oVar.Nip = Nip;
                    oVar.Periode = sPeriode;

                    if (JmhKerja > 0)
                    {
                        oVar.KdJenis = MVar.Jenis.HARI_HADIR;
                        oVar.Jmh = JmhKerja;
                        lst.Add(oVar);
                    }
                    if (JmhLembur > 0)
                    {
                        oVar.KdJenis = MVar.Jenis.HARI_LEMBUR;
                        oVar.Jmh = JmhLembur;
                        lst.Add(oVar);
                    }
                }
            }

            if (lst.Count > 0)
            {
                SqlTransaction Trans = null;
                try
                {
                    Trans = this.cnn.BeginTransaction();
                    AdnGajiVarDao dao = new AdnGajiVarDao(this.cnn, this.Pengguna, Trans);
                    foreach(AdnGajiVar item in lst)
                    {
                        dao.Simpan(item);
                    }
                    Trans.Commit();
                    MessageBox.Show("Import Transaksi Absen Berhasil!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (SqlException ex)
                {
                    Trans.Rollback();
                    AdnFungsi.LogErr(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    Trans.Rollback();
                    AdnFungsi.LogErr(ex.Message.ToString());
                }
            }
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
            PilihFileText();
        }

        private void PilihFileText()
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
                        //DataSet result = excelReader.AsDataSet();
                        //...
                        //4. DataSet - Create column names from first row
                        excelReader.IsFirstRowAsColumnNames = true;
                        DataSet result = excelReader.AsDataSet();

                        //5. Data Reader methods
                        List<AdnAbsenTrMesin> lst = new List<AdnAbsenTrMesin>();
                        inovaPayroll.Core.AdnKaryawanExtDao ExtDao = new AdnKaryawanExtDao(this.cnn);
                        while (excelReader.Read())
                        {
                            if (excelReader[0] != null)
                            {
                                if (excelReader[3].ToString()!="Name")
                                {
                                    AdnAbsenTrMesin o = new AdnAbsenTrMesin();
                                    o.NoKaryawan = excelReader[0].ToString();
                                    o.NmKaryawan = excelReader[3].ToString();
                                    o.Tgl = excelReader[5].ToString();
                                    o.WaktuKerja = excelReader[17].ToString();
                                    o.WaktuLembur = excelReader[16].ToString();
                                    o.Nip = "";

                                    AdnKaryawanExt ext = ExtDao.Get(o.NoKaryawan, MVar.Ext.MESIN_ABSEN);
                                    if (ext != null)
                                    {
                                        o.Nip = ext.Nip;
                                    }

                                    lst.Add(o);
                                }
                            }
                        }

                        //6. Free resources (IExcelDataReader is IDisposable)
                        excelReader.Close();
                        dgv.AutoGenerateColumns = false;
                        dgv.DataSource = lst;

             
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
