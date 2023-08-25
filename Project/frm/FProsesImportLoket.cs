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
    [AdnScObjectAtr("Form: Proses Batch Import Loket", "Proses")]
    public partial class FProsesImportLoket : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;
        private AdnScPengguna Pengguna;

        public FProsesImportLoket(SqlConnection cnn,string AppName, AdnScPengguna Pengguna)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            this.Pengguna = Pengguna;

            AdnFungsi.SetComboBulan(comboBoxBulan, false);
            comboBoxBulan.SelectedIndex = DateTime.Now.Month - 1;
            updTahun.Value = DateTime.Now.Year;

            new AdnAkunDao(this.cnn).SetComboKasDetail(comboBoxKas);
            new AdnAkunDao(this.cnn).SetComboKasDetail(comboBoxKasPindah);
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
            SqlTransaction Trans = null;
            try
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (AdnFungsi.CBool(dgv.Rows[i].Cells["Pilih"], true) && AdnFungsi.CStr(dgv.Rows[i].Cells["Status"]) == "BELUM DIPOSTING")
                    {
                        Trans = this.cnn.BeginTransaction();
                        string Kas ="";
                        string KasPindah="";
                        DateTime TglJurnal;

                        if (checkBoxTgl.Checked)
                        {
                            TglJurnal = AdnFungsi.CDate(dgv.Rows[i].Cells["Tgl"]);
                        }
                        else
                        {
                            TglJurnal = dateTimePickerTgl.Value;
                        }

                        if(comboBoxKas.SelectedIndex>0)
                        {
                             Kas =comboBoxKas.SelectedValue.ToString();
                        }

                        if(comboBoxKasPindah.SelectedIndex>0)
                        {
                            KasPindah = comboBoxKasPindah.SelectedValue.ToString();
                        }
                        string Pesan = new AdnLoketDao(this.cnn, this.Pengguna, Trans).BatchJurnalLoket(TglJurnal, AdnFungsi.CDate(dgv.Rows[i].Cells["Tgl"]), AppVar.ModeTunai, Kas, KasPindah);
                        Trans.Commit();
                        dgv.Rows[i].Cells["Status"].Value = "SUDAH DIPOSTING";
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

        private void dateTimePickerTgl_ValueChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }
        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            DataTable lst = new AdnLoketDao(this.cnn).GetByPeriode((int)updTahun.Value, comboBoxBulan.SelectedIndex+1);
            dgv.Rows.Clear();

            for (int i = 0; i < lst.Rows.Count;i++ )
            {
                dgv.Rows.Add();
                dgv.Rows[i].Cells["Tgl"].Value = lst.Rows[i]["Tgl"];
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

        private void checkBoxTgl_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox) sender;
            if (chk.Checked)
            {
                dateTimePickerTgl.Visible = false;
                labelTgl.Visible = false;
            }
            else
            {
                dateTimePickerTgl.Visible = true;
                labelTgl.Visible = true;
            }
        }

        private void updTahun_ValueChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

        private void buttonUnposting_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Yakin UNPOSTING akan dilakukan?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                SqlTransaction Trans = null;
                try
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (AdnFungsi.CBool(dgv.Rows[i].Cells["Pilih"], true) && AdnFungsi.CStr(dgv.Rows[i].Cells["Status"]) == "SUDAH DIPOSTING")
                        {
                            Trans = this.cnn.BeginTransaction();
                            DateTime TglJurnal;

                            if (checkBoxTgl.Checked)
                            {
                                TglJurnal = AdnFungsi.CDate(dgv.Rows[i].Cells["Tgl"]);
                            }
                            else
                            {
                                TglJurnal = dateTimePickerTgl.Value;
                            }

                            string Pesan = new AdnLoketDao(this.cnn, this.Pengguna, Trans).Unposting(AdnFungsi.CDate(dgv.Rows[i].Cells["Tgl"]));
                            Trans.Commit();
                            dgv.Rows[i].Cells["Status"].Value = "BELUM DIPOSTING";
                            dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                            dgv.Rows[i].Cells[0].ReadOnly = true;
                            dgv.Rows[i].Cells["Pilih"].Value = false;
                        }
                    }
                    MessageBox.Show("Proses UNPOSTING Berhasil!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

      
    }
}
