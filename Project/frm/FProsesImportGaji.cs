using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;
using inovaGL.Data;


namespace inovaGL
{
    [AdnScObjectAtr("Form: Proses Batch Import Gaji", "Proses")]
    public partial class FProsesImportGaji : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;
        private AdnScPengguna Pengguna;

        public FProsesImportGaji(SqlConnection cnn,string AppName, AdnScPengguna Pengguna)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            this.Pengguna = Pengguna;

            AdnFungsi.SetComboBulan(comboBoxBulan, false);
            comboBoxBulan.SelectedIndex = DateTime.Now.Month - 1;
            updTahun.Value = DateTime.Now.Year;

            new AdnAkunDao(this.cnn).SetComboKasDetail(comboBoxKas);
            new AdnAkunDao(this.cnn).SetComboBiayaDetail(comboBoxAkunBiaya);
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
            bool IsValid = true;

            if (comboBoxKas.SelectedIndex == -1)
            {
                MessageBox.Show("Kas/Bank Belum Dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                IsValid = false;
            }

            if (comboBoxAkunBiaya.SelectedIndex == -1)
            {
                MessageBox.Show("Akun Biaya Belum Dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
                IsValid = false;
            }

            if (IsValid)
            {

                SqlTransaction Trans = null;
                try
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (AdnFungsi.CStr(dgv.Rows[i].Cells["Status"]) == "BELUM DIPOSTING")
                        {
                            Trans = this.cnn.BeginTransaction();
                            string Kas = comboBoxKas.SelectedValue.ToString();
                            string AkunBiaya = comboBoxAkunBiaya.SelectedValue.ToString();
                            string Periode = updTahun.Value + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');

                            string Pesan = new AdnJurnalDao(this.cnn, this.Pengguna, Trans).BatchJurnalGaji(dateTimePicker1.Value, Periode, Kas, AkunBiaya);
                            Trans.Commit();
                            dgv.Rows[i].Cells["Status"].Value = "SUKSES";
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                            dgv.Rows[i].Cells[0].ReadOnly = true;
                            dgv.Rows[i].Cells["Pilih"].Value = false;
                        }
                    }
                    MessageBox.Show("Semua Transaksi Berhasil Di Jurnal!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                    if (Trans != null)
                    {
                        Trans.Rollback();
                    }
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

            DataTable lst = new AdnGajiDao(this.cnn).GetByPeriode((int)updTahun.Value, comboBoxBulan.SelectedIndex+1);
            dgv.Rows.Clear();

            for (int i = 0; i < lst.Rows.Count;i++ )
            {
                dgv.Rows.Add();
                dgv.Rows[i].Cells["KdDept"].Value = lst.Rows[i]["KdDept"];
                dgv.Rows[i].Cells["NmDept"].Value = lst.Rows[i]["NmDept"];
                dgv.Rows[i].Cells["Jmh"].Value = lst.Rows[i]["Jmh"];
                dgv.Rows[i].Cells["Status"].Value = lst.Rows[i]["Status"];
            }

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
      
    }
}
