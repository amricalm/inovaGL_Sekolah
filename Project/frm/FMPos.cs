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
using inovaGL.Definisi;
using inovaGL.Laporan;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Input Pos", "Pos")]
    public partial class FMPos : Andhana.AdnBaseForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private string AppName;
        private FDPos fInduk;

        private int LastBaris = 0;
        private BindingSource bs=new BindingSource();

        private int WM_KEYDOWN = 0x100;

        public FMPos(SqlConnection cnn,string AppName,short ModeEdit,string kd, object fInduk)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;
            this.fInduk = (FDPos)fInduk;

            bs.DataSource = new AdnAkunDao(this.cnn).GetDf(AdnVar.Klasifikasi.DETAIL,false);
            dgvAkun.DataSource = bs;

            new AdnDeptDao(this.cnn).SetCombo(comboBoxDept);

            dgv.AutoGenerateColumns = false;
            FillDataGridView("");
           
            if (this.ModeEdit == AdnModeEdit.BACA)
            {
                this.GetData(kd);
            }
            else
            {
                this.DokumenBaru();
            }
        }
        private void FTReceipt_KeyDown(object sender, KeyEventArgs e)
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
                    }
                    break;

                case Keys.D:
                    if (Control.ModifierKeys == Keys.Control)
                    {

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
        private void FillDataGridView(string sFilter)
        {
            this.UseWaitCursor = true;
            Application.DoEvents();         

            this.UseWaitCursor = false;
        }

        #region "Prosedur CRUD"
        private void Simpan()
        {
            dgv.EndEdit();
            if (this.IsValid())
            {
                AdnPos o = new AdnPos();

                o.KdPos= textBoxKd.Text;
                o.NmPos = textBoxNm.Text.ToString().Trim();
                o.KdDept = comboBoxDept.SelectedValue.ToString();

                o.ItemDf = new List<AdnPosDtl>();

                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    if (!baris.IsNewRow)
                    {
                        string KdAkun = (baris.Cells["KdAkun"].Value ?? "").ToString();

                        if (KdAkun != "" )
                        {

                            AdnPosDtl item = new AdnPosDtl();
                            item.KdPos = o.KdPos;
                            item.KdAkun = KdAkun;

                            o.ItemDf.Add(item);

                        }
                    }
                }


                SqlTransaction Trans = null;
                Trans = this.cnn.BeginTransaction();

                AdnPosDao dao = new AdnPosDao(this.cnn,AppVar.AppPengguna,Trans);
                try
                {
                    switch (this.ModeEdit)
                    {
                        case AdnModeEdit.BARU:
                            dao.Simpan(o);
                            break;

                        case AdnModeEdit.UBAH:
                            dao.Update(o);
                            break;
                    }
                    Trans.Commit();
                }
                catch (Exception exp)
                {
                    Trans.Rollback();
                    AdnFungsi.LogErr(exp.Message);
                }
                
                panelHdr.Enabled = false;
                panelDtl.Enabled = false;

                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonHapus.Enabled = true;

                toolStripButtonBatal.Enabled = false;
                toolStripButtonSimpan.Enabled = false;
            }//end isValid
        }
        private void Edit()
        {
            this.ModeEdit = AdnModeEdit.UBAH;
            panelHdr.Enabled = true;
            panelDtl.Enabled = true;

            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Batal()
        {
            this.DokumenBaru();

        }
        private void Hapus()
        {
            if (MessageBox.Show("Hapus Data, Kode = " + textBoxKd.Text.ToString() + " ?", this.AppName, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                AdnPosDao dao = new AdnPosDao(this.cnn);
                dao.Hapus(textBoxKd.Text.ToString());

                this.Batal();
            }
        }
        private void Tambah()
        {
            this.DokumenBaru();
        }
        //END - Prosedur CRUD

        #endregion
        private void DokumenBaru()
        {
            try
            {
                AdnFungsi.Bersih(this,true);
                this.ModeEdit = AdnModeEdit.BARU;
                dgv.Rows.Clear();

                //if (this.AdnRole.tambah)
                //{
                toolStripButtonSimpan.Enabled = true;
                panelHdr.Enabled = true;
                panelDtl.Enabled = true;
                //}
                //else
                //{
                //    toolStripButtonSimpan.Enabled =false;
                //    panelHdr.Enabled = false;
                //}

                toolStripButtonTambah.Enabled = false;
                toolStripButtonEdit.Enabled = false;
                toolStripButtonHapus.Enabled = false;
                toolStripButtonBatal.Enabled = true;
            }
            catch (Exception exp)
            {
                AdnFungsi.LogErr(exp.Message.ToString());
            }
            finally
            {
                textBoxKd.Enabled = true;
                textBoxKd.Focus();
            }
        }
        private bool IsValid()
        {
            string sPesan = "";

            if (textBoxKd.Text.ToString().Trim() == "")
            {
                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "No. Bukti";
            }
            if (comboBoxDept.SelectedIndex<0)
            {

                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Perkiraan Kas";
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
        public void GetData(string kd)
        {
            AdnPosDao dao = new AdnPosDao(this.cnn);
            AdnPos o = dao.Get(kd);
            

            if (o != null)
            {
                this.ModeEdit = AdnModeEdit.BACA;

                textBoxKd.Text = kd;
                textBoxNm.Text = o.NmPos.Trim();
                comboBoxDept.SelectedValue = o.KdDept;

                int iBaris = 0;
                if (o.ItemDf.Count > 0)
                {
                    dgv.Rows.Add(o.ItemDf.Count);
                    foreach (AdnPosDtl item in o.ItemDf)
                    {
                        dgv.Rows[iBaris].Cells["KdAkun"].Value = item.KdAkun;
                        if (item.Akun != null)
                        {
                            dgv.Rows[iBaris].Cells["NmAkun"].Value = item.Akun.NmAkun;
                        }
                        iBaris++;

                    }
                }
                toolStripButtonTambah.Enabled = true;
                toolStripButtonEdit.Enabled = true;
                toolStripButtonBatal.Enabled = true;
                toolStripButtonHapus.Enabled = true;
                toolStripButtonSimpan.Enabled = false;

                textBoxKd.Enabled = false;
                textBoxKd.Focus();

            }
        }

        #region "toolStripButton"
        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            this.Tambah();
        }
        private void toolStripButtonEdit_Click(object sender, EventArgs e)
        {
            this.Edit();
        }
        private void toolStripButtonBatal_Click(object sender, EventArgs e)
        {
            this.Batal();
        }
        private void toolStripButtonHapus_Click(object sender, EventArgs e)
        {
            this.Hapus();
        }
        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.Simpan();
        }
        #endregion
        
        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dgv.EditingControl.KeyPress+=new KeyPressEventHandler(EditingControl_KeyPress);
        }
        
        private void dgv_Enter(object sender, EventArgs e)
        {
            if (dgv.Rows.Count == 0)
            {
                dgv.Rows.Add();
            }
        }
        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            this.TambahBaris();
        }
        void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {   
            
            switch (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name)
            {
                case "KdAkun":

                    this.LastBaris = dgv.CurrentCell.RowIndex;

                    panelAkun.Visible = true;
                    Point Posisi = dgv.Location;
                    Posisi.Y= Posisi.Y+ (dgv.CurrentCellAddress.Y+1)*dgv.CurrentCell.Size.Height+3;
                    Posisi.X = Posisi.X + dgv.RowHeadersWidth;
                    panelAkun.Location = Posisi;

                    string Isi = ((DataGridViewTextBoxEditingControl)sender).Text.ToString().Trim() +((char)e.KeyChar).ToString();
                    dgv.CancelEdit();
                    textBoxKdAkun.Focus();
                    textBoxKdAkun.Text = Isi;
                    textBoxKdAkun.SelectionStart =Isi.Length;

                    break;
                  
                    
            }

        }
       
        private void TambahBaris()
        {
            int BarisAkhir = dgv.Rows.Count - 1;
            if (AdnFungsi.CStr(dgv.Rows[BarisAkhir].Cells["NmAkun"]) != "")
                
            {
                dgv.Rows.Add();
            }
        }

        private void FMPos_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (this.fInduk != null)
            //{
            //    this.fInduk.Reload();
            //}
        }

        private void textBoxKdAkun_TextChanged(object sender, EventArgs e)
        {
            string sKd = textBoxKdAkun.Text.ToString().Trim();
            if (panelAkun.Visible)
            {
                bs.Filter = "KdAkun LIKE '" + sKd + "*'";
            }
        }

        private void textBoxKdAkun_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    if (textBoxKdAkun.Text.ToString().Trim() == "")
                    {
                        MessageBox.Show("Kode Akun Harus Diisi!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgv.Rows[this.LastBaris].Cells[0].Value = "";
                        dgv.Rows[this.LastBaris].Cells[1].Value = "";
                    }
                    else
                    {
                        if (dgvAkun.Rows.Count > 0)
                        {
                            dgv.Rows[this.LastBaris].Cells[0].Value = dgvAkun.SelectedRows[0].Cells[0].Value.ToString().Trim();
                            dgv.Rows[this.LastBaris].Cells[1].Value = dgvAkun.SelectedRows[0].Cells[1].Value.ToString().Trim();
                            textBoxKdAkun.Text = "";
                            dgv.CurrentCell = this.dgv[0, this.LastBaris];
                        }
                        else
                        {
                            dgv.Rows[this.LastBaris].Cells[0].Value = "";
                            dgv.Rows[this.LastBaris].Cells[1].Value = "";
                            textBoxKdAkun.Text = "";

                            MessageBox.Show("Kode Akun Tidak Benar!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                            dgv.CurrentCell = this.dgv[0, this.LastBaris];
                        }
                    }

                    dgv.Focus();
                    panelAkun.Visible = false;

                    break;
                case Keys.Down:
                    if (dgvAkun.Rows.Count > 0)
                    {
                        dgvAkun.Focus();
                        dgvAkun.CurrentCell = dgvAkun[0, 0];
                    }
                    break;
            }

        }

        private void dgvAkun_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    dgv.Focus();
                    panelAkun.Visible = false;
                    if (dgvAkun.Rows.Count > 0)
                    {
                        dgv.Rows[this.LastBaris].Cells[0].Value = dgvAkun.SelectedRows[0].Cells[0].Value.ToString().Trim();
                        dgv.Rows[this.LastBaris].Cells[1].Value = dgvAkun.SelectedRows[0].Cells[1].Value.ToString().Trim();
                        textBoxKdAkun.Text = "";
                        dgv.CurrentCell = this.dgv[0, this.LastBaris];
                    }
                    break;
            }
        }

        private void textBoxKdAkun_Leave(object sender, EventArgs e)
        {
            if (!dgvAkun.Focused)
            {
                panelAkun.Visible = false;
                textBoxKdAkun.Text="";
            }
        }

        private void dgvAkun_Leave(object sender, EventArgs e)
        {
            if (!textBoxKdAkun.Focused)
            {
                panelAkun.Visible = false;
                textBoxKdAkun.Text = "";
            }
        }

        private void dgvAkun_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvAkun.Rows.Count > 0)
            {
                dgv.Rows[this.LastBaris].Cells[0].Value = dgvAkun.SelectedRows[0].Cells[0].Value.ToString().Trim();
                dgv.Rows[this.LastBaris].Cells[1].Value = dgvAkun.SelectedRows[0].Cells[1].Value.ToString().Trim();
                textBoxKdAkun.Text = "";
                dgv.CurrentCell = this.dgv[0, this.LastBaris];
            }
            else
            {
                dgv.Rows[this.LastBaris].Cells[0].Value = "";
                dgv.Rows[this.LastBaris].Cells[1].Value = "";
                textBoxKdAkun.Text = "";

                MessageBox.Show("Kode Akun Tidak Benar!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                dgv.CurrentCell = this.dgv[0, this.LastBaris];
            }
            dgv.Focus();
            panelAkun.Visible = false;
        }

        private void textBoxKd_Validating(object sender, CancelEventArgs e)
        {
            string Kd = ((TextBox)sender).Text.ToString().Trim();
            if (Kd != "")
            {
                this.GetData(Kd);
            }
        }

        // Detect F1 through F9 during preprocessing and modify F3.
        public override bool PreProcessMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                Keys keyCode = (Keys)m.WParam & Keys.KeyCode;

                // Detect F1 through F9.
                switch (keyCode)
                {
                    case Keys.F1:
                    case Keys.F2:
                    case Keys.F3:
                    case Keys.F4:
                    case Keys.F5:
                    case Keys.F6:
                    case Keys.F7:
                    case Keys.F8:
                    case Keys.F9:

                        MessageBox.Show("Control.PreProcessMessage: '" +
                          keyCode.ToString() + "' pressed.");

                        // Replace F3 with F1, so that ProcessKeyMessage will  
                        // receive F1 instead of F3.
                        if (keyCode == Keys.F3)
                        {
                            m.WParam = (IntPtr)Keys.F1;
                            MessageBox.Show("Control.PreProcessMessage: '" +
                                keyCode.ToString() + "' replaced by F1.");
                        }
                        break;
                }
            }

            // Send all other messages to the base method.
            return base.PreProcessMessage(ref m);
        }


    }
}
