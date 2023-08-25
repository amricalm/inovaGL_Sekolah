using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Andhana;
using inovaGL.Data;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Daftar Akun", "Akun")]
    public partial class FDTAnggaran : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        public FDTAnggaran(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            
            new AdnThAjarDao(this.cnn).SetCombo(comboBoxThAjar);
            new EDUSIS.Shared.AdnSekolahDao(this.cnn).SetCombo(comboBoxSekolah);
        }
        private void FDTReceipt_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            this.FillDataGridView();
            comboBoxThAjar.SelectedValue= AppVar.ThAjar;
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            string ThAjar = "";
            string KdSekolah = "";
            if (comboBoxThAjar.SelectedIndex > -1)
            {
                ThAjar = comboBoxThAjar.SelectedValue.ToString().Trim();
            }

            if (comboBoxSekolah.SelectedIndex > -1)
            {
                KdSekolah = comboBoxSekolah.SelectedValue.ToString().Trim();
            }

            bs.DataSource = new AdnAnggaranDao(this.cnn).GetDf(ThAjar, KdSekolah);
            dgv.DataSource = bs;

            if (dgv.RowCount == 0)
            {
                toolStripButtonPilih.Enabled = false;
            }
            else
            {
                //toolStripButtonPilih.Enabled = true;
            }
            this.UseWaitCursor = false;
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            FMAkun ofm = new FMAkun(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
            ofm.ShowDialog();
        }
        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            this.Pilih();
        }

        private void Pilih()
        {
            panelHdr.Enabled = true;
            //panelDtl.Enabled = true;

            //FMAkun ofm = new FMAkun(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdAkun"]), this);
            //ofm.ShowDialog();

        }
        
        public void Reload()
        {
            this.FillDataGridView();
        }

        private void cariButton_Click(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }

        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.Simpan();
        }

        #region "Prosedur CRUD"
        private void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {
                toolStripButtonSimpan.Enabled = false;
                this.UseWaitCursor = true;

                AdnAnggaran o = new AdnAnggaran();
                AdnAnggaranDao dao = new AdnAnggaranDao(this.cnn, AppVar.AppPengguna);
                try
                {

                    foreach (DataGridViewRow baris in dgv.Rows)
                    {
                        if (!baris.IsNewRow)
                        {

                            o.KdAkun = (baris.Cells["KdAkun"].Value ?? "").ToString();
                            o.ThAjar = comboBoxThAjar.SelectedValue.ToString().Trim();
                            o.KdSekolah = comboBoxSekolah.SelectedValue.ToString().Trim();

                            for (int i = 3; i < dgv.Columns.Count; i++)
                            {
                                o.Bulan = AppFungsi.KonversiNamaBulanToAngka(dgv.Columns[i].Name.ToString());
                                o.Nilai = AdnFungsi.CDec(baris.Cells[i]);
                                if (dao.Update(o) == 0)
                                {
                                    dao.Simpan(o);
                                }
                            }
                        }
                    }

                }
                catch (Exception exp)
                {
                    MessageBox.Show("Terjadi Kesalahan. \n " + exp.Message.ToString() , this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    AdnFungsi.LogErr(exp.Message);
                }
                finally
                {
                    MessageBox.Show("Proses Simpan Selesai!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                //panelHdr.Enabled = false;
                //panelDtl.Enabled = false;

                //toolStripButtonTambah.Enabled = true;
                //toolStripButtonEdit.Enabled = true;
                //toolStripButtonHapus.Enabled = true;

                //toolStripButtonBatal.Enabled = false;
                //toolStripButtonSimpan.Enabled = false;
                toolStripButtonSimpan.Enabled = true;
                this.UseWaitCursor = false;
            }//end isValid
        }

        private bool IsValid()
        {
            string sPesan = "";

            if (comboBoxThAjar.SelectedIndex <0)
            {
                sPesan = sPesan + "Tahun Ajar";
            }


            if (sPesan != "")
            {
                sPesan = sPesan + " Harus Diisi.\n";
            }

            //foreach (DataGridViewRow baris in dgv.Rows)
            //{
            //    if (!baris.IsNewRow)
            //    {
            //        //validasi
            //        decimal Debet = AdnFungsi.CDec(baris.Cells["Debet"]);
            //        decimal Kredit = AdnFungsi.CDec(baris.Cells["Kredit"]);
            //        string KdAkun = AdnFungsi.CStr(baris.Cells["KdAkun"]);

            //        if (KdAkun == "")
            //        {
            //            //abaikan...
            //        }
            //        else
            //        {
            //            if ((Debet-Kredit) == 0)
            //            {
            //                //abaikan...
            //            }
            //        }
            //    }
            //}

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
        #endregion

        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dgv.EditingControl.KeyPress += new KeyPressEventHandler(EditingControl_KeyPress);
        }

        void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {

            switch (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name)
            {

                case "Jul":
                case "Agt":
                case "Sep":
                case "Okt":
                case "Nov":
                case "Des":
                case "Jan":
                case "Feb":
                case "Mar":
                case "Apr":
                case "Mei":
                case "Jun":
                    e.Handled = AdnFungsi.CekAngka(e.KeyChar);
                    break;


            }
        }

        private void dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > 2)
            {

                DataGridView o = (DataGridView)sender;
                decimal total = 0;
                for (int i = 3; i < o.Columns.Count; i++)
                {
                    total = total + AdnFungsi.CDec(o.Rows[e.RowIndex].Cells[i]);
                }
                o.Rows[e.RowIndex].Cells["Total"].Value = total;
            }
        }

    }
}
