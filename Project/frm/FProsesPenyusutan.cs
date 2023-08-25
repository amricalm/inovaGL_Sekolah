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
    [AdnScObjectAtr("Form: Proses Batch Penyusutan", "Proses")]
    public partial class FProsesPenyusutan : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;
        private AdnScPengguna Pengguna;

        public FProsesPenyusutan(SqlConnection cnn,string AppName, AdnScPengguna Pengguna)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            this.Pengguna = Pengguna;

            AdnFungsi.SetComboBulan(comboBoxBulan, false);
            comboBoxBulan.SelectedIndex = DateTime.Now.Month - 1;
            updTahun.Value = DateTime.Now.Year;
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
            int JmhAset = 0;

            //if (comboBoxAkunBiaya.SelectedIndex == -1)
            //{
            //    MessageBox.Show("Akun Biaya Belum Dipilih!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            //    IsValid = false;
            //}

            if (IsValid)
            {

                SqlTransaction Trans = null;
                try
                {
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (AdnFungsi.CBool(dgv.Rows[i].Cells["Pilih"], true))
                        {
                            if (AdnFungsi.CStr(dgv.Rows[i].Cells["Status"]) == "BELUM DIPOSTING")
                            {
                                Trans = this.cnn.BeginTransaction();
                                string Periode = updTahun.Value + comboBoxBulan.SelectedValue.ToString().PadLeft(2, '0');
                                string CoaAkum = AdnFungsi.CStr(dgv.Rows[i].Cells["CoaAkum"]);
                                string CoaBeban = AdnFungsi.CStr(dgv.Rows[i].Cells["CoaBeban"]);
                                decimal NilaiBuku = AdnFungsi.CDec(dgv.Rows[i].Cells["NilaiBuku"]);
                                decimal NilaiResidu = AdnFungsi.CDec(dgv.Rows[i].Cells["NilaiResidu"]);
                                decimal HargaPerolehan = AdnFungsi.CDec(dgv.Rows[i].Cells["HargaPerolehan"]);
                                int UmurEkonomisBulan = AdnFungsi.CInt(dgv.Rows[i].Cells["UmurEkonomisBulan"], true);
                                int JmhBulanSusut = (int)updJmhBulanSusut.Value;

                                string KdAset = AdnFungsi.CStr(dgv.Rows[i].Cells["KdAset"]);
                                if (!(CoaAkum == "" || CoaBeban == ""))
                                {
                                    string Pesan = new AdnAsetDao(this.cnn, this.Pengguna, Trans).BatchJurnalPenyusutan(dateTimePickerJurnal.Value, Periode, KdAset, CoaAkum, CoaBeban, HargaPerolehan, NilaiBuku, NilaiResidu, JmhBulanSusut, UmurEkonomisBulan);
                                    Trans.Commit();
                                    dgv.Rows[i].Cells["Status"].Value = "SUKSES";
                                    dgv.Rows[i].DefaultCellStyle.BackColor = Color.LightGray;
                                    dgv.Rows[i].Cells[0].ReadOnly = true;
                                    dgv.Rows[i].Cells["Pilih"].Value = false;
                                    JmhAset++;
                                }
                                Trans.Dispose();
                            }
                        }//Pilih
                    }
                    MessageBox.Show( JmhAset + " Aset Berhasil Di Susutkan!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception exp)
                {
                    AdnFungsi.LogErr(exp.Message);
                    if (Trans != null)
                    {
                        Trans.Rollback();
                    }
                }
                finally
                {
                    if (Trans != null)
                    {
                        Trans.Dispose();
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

            DataTable lst = new AdnAsetDao(this.cnn).GetByPeriode((int)updTahun.Value, comboBoxBulan.SelectedIndex+1);
            dgv.Rows.Clear();

            for (int i = 0; i < lst.Rows.Count;i++ )
            {
                dgv.Rows.Add();
                dgv.Rows[i].Cells["KdAset"].Value = lst.Rows[i]["KdAset"];
                dgv.Rows[i].Cells["NmAset"].Value = lst.Rows[i]["NmAset"];
                dgv.Rows[i].Cells["NilaiResidu"].Value = lst.Rows[i]["NilaiResidu"];
                dgv.Rows[i].Cells["NilaiBuku"].Value = lst.Rows[i]["NilaiBuku"];
                dgv.Rows[i].Cells["CoaAkum"].Value = lst.Rows[i]["CoaAkum"];
                dgv.Rows[i].Cells["CoaBeban"].Value = lst.Rows[i]["CoaBeban"];
                dgv.Rows[i].Cells["UmurEkonomisBulan"].Value = lst.Rows[i]["UmurEkonomisBulan"];
                dgv.Rows[i].Cells["HargaPerolehan"].Value = lst.Rows[i]["HargaPerolehan"];
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
