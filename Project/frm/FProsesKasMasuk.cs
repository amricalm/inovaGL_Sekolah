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
    [AdnScObjectAtr("Form: Proses Batch Kas Masuk", "Kas Masuk")]
    public partial class FProsesBatchKasMasuk : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;

        public FProsesBatchKasMasuk(SqlConnection cnn,string AppName,short ModeEdit,string kd,object fInduk)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;

            new AdnAkunDao(this.cnn).SetComboKas(comboBoxKas);

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
            if (new AdnKasMasukDao(this.cnn).BatchBkmDonasi(dateTimePickerTgl.Value, AppVar.AppPengguna))
            {
                MessageBox.Show("Proses Batch Kas Masuk Sukses!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void textBoxJumlahVoucher_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }


        private void dateTimePickerTgl_ValueChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }
        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            string KdKas = "";
            if (comboBoxKas.SelectedIndex > -1)
            {
                KdKas = comboBoxKas.SelectedValue.ToString();
            }
            bs.DataSource = new AdnKasMasukDao(this.cnn).GetDonasiGroupByProgramByPeriode(dateTimePickerTgl.Value, KdKas);
            dgv.DataSource = bs;
            if (dgv.RowCount == 0)
            {
                //toolStripButtonPilih.Enabled = false;
            }
            else
            {
                //toolStripButtonPilih.Enabled = true;
            }

            this.UseWaitCursor = false;
        }

        private void comboBoxKas_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }
      
    }
}
