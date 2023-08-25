using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Setup Aplikasi", "Setup")]
    public partial class FMSetup : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private string AppName;

        public FMSetup(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.AppName = AppName;
            this.cnn = cnn;

            new inovaGL.Data.AdnAkunDao(this.cnn).SetComboByGolDetail(comboBoxAkunLabaDitahan, "");
            new inovaGL.Data.AdnAkunDao(this.cnn).SetComboByGolDetail(comboBoxAkunLabaTahunBerjalan, "");
            new inovaGL.Data.AdnAkunDao(this.cnn).SetComboByGolDetail(comboBoxAkunIkhtisarLabaRugi, "");

            this.GetData();

        }
        private void FMSetup_KeyDown(object sender, KeyEventArgs e)
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
                        e.Handled = true;
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

        private void Tambah()
        {
            this.DokumenBaru();
        }
        private void Edit()
        {
            panelHdr.Enabled = true;

            //toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            //toolStripButtonHapus.Enabled = true;

            //toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Simpan()
        {
            if (this.IsValid())
            {
                string KdAkunLabaDitahan = "";
                string KdAkunLabaThBerjalan = "";
                string KdAkunIkhtisarLR = "";

                if (comboBoxAkunLabaDitahan.SelectedIndex > -1)
                {
                    KdAkunLabaDitahan = comboBoxAkunLabaDitahan.SelectedValue.ToString();
                }

                if (comboBoxAkunLabaTahunBerjalan.SelectedIndex > -1)
                {
                    KdAkunLabaThBerjalan = comboBoxAkunLabaTahunBerjalan.SelectedValue.ToString();
                }

                if (comboBoxAkunIkhtisarLabaRugi.SelectedIndex > -1)
                {
                    KdAkunIkhtisarLR = comboBoxAkunIkhtisarLabaRugi.SelectedValue.ToString();
                }


                if (AdnFungsi.UpdateSysVar(this.cnn, "periode_mulai", dateTimePickerTglPeriodeAkuntansi.Value.ToString()))
                {
                    AppVar.PeriodeMulai = dateTimePickerTglPeriodeAkuntansi.Value;
                }

                if (AdnFungsi.UpdateSysVar(this.cnn, "LabaDitahan", KdAkunLabaDitahan))
                {
                    AppVar.KdAkunLabaDitahan = KdAkunLabaDitahan;
                }

                if (AdnFungsi.UpdateSysVar(this.cnn, "LabaThBerjalan", KdAkunLabaThBerjalan))
                {
                    AppVar.KdAkunLabaTahunBerjalan = KdAkunLabaThBerjalan;
                }

                if (AdnFungsi.UpdateSysVar(this.cnn, "IkhtisarLR", KdAkunIkhtisarLR))
                {
                    AppVar.KdAkunIkhtisarLabaRugi = KdAkunIkhtisarLR; 
                }
                
                panelHdr.Enabled = false;

                //toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                //toolStripButtonHapus.Enabled = true;

                //toolStripButtonBatal.Enabled = false;
                toolStripButtonSimpan.Enabled = false;
            }
        }
        private void Hapus()
        {

        }
        private void DokumenBaru()
        {

            //toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            //toolStripButtonHapus.Enabled = false;
            //toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Batal()
        {
            this.DokumenBaru();
        }
        public void GetData()
        {
            dateTimePickerTglPeriodeAkuntansi.Value = AppVar.PeriodeMulai;
            comboBoxAkunLabaDitahan.SelectedValue = AppVar.KdAkunLabaDitahan;
            comboBoxAkunLabaTahunBerjalan.SelectedValue = AppVar.KdAkunLabaTahunBerjalan;
            comboBoxAkunIkhtisarLabaRugi.SelectedValue = AppVar.KdAkunIkhtisarLabaRugi;

            //toolStripButtonTambah.Enabled = true;
            toolStripButtonEdit.Enabled = true;
            //toolStripButtonBatal.Enabled = true;
            //toolStripButtonHapus.Enabled = true;
            toolStripButtonSimpan.Enabled = false;

        }
        private bool IsValid()
        {
            string sPesan = "";
            //if (textBoxKd.Text.ToString().Trim() == "")
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Kode Akun";
            //}

            //if (comboBoxGolongan.SelectedIndex ==-1)
            //{
            //    if (sPesan != "") { sPesan = sPesan + ", "; }
            //    sPesan = sPesan + "Golongan";
            //}

            if (sPesan != "")
            {
                sPesan = sPesan + " Harus Diisi.\n";
            }

            if (sPesan == "")
            {
                return true;
            }
            else
            {
                MessageBox.Show(sPesan, this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }


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
   
    }
}
