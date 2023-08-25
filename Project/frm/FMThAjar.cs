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
    public partial class FMThAjar : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private string AppName;

        public FMThAjar(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.AppName = AppName;
            this.cnn = cnn;

            new inovaGL.Data.AdnThAjarDao(this.cnn).SetCombo(comboBoxThAjar);
            this.GetData();

        }
        private void FMThAjar_KeyDown(object sender, KeyEventArgs e)
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
                if (comboBoxThAjar.SelectedIndex > -1)
                {
                    AppVar.ThAjar = comboBoxThAjar.SelectedValue.ToString();

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
            comboBoxThAjar.SelectedValue = AppVar.ThAjar;

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

            if (comboBoxThAjar.SelectedIndex == -1)
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tahun Ajar ";
            }

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

        private void FMThAjar_Load(object sender, EventArgs e)
        {

        }
   
    }
}
