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
    [AdnScObjectAtr("Form: Daftar Saldo Awal", "Saldo Awal")]
    public partial class FTSaldoAwal : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        private string ThAjar;
        public FTSaldoAwal(SqlConnection cnn, string AppName, string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ThAjar = ThAjar;

            dateTimePickerTgl.Focus();
            dateTimePickerTgl.Format = DateTimePickerFormat.Short;
            dateTimePickerTgl.Value = AppVar.PeriodeMulai;

            new AdnSysAkunGolonganDao(this.cnn).SetCombo(comboBoxGolongan,true);
            comboBoxGolongan.SelectedIndex = -1;

            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            dgv.AutoGenerateColumns = false;
            Application.DoEvents();

            string KdGol = "";
            if (comboBoxGolongan.SelectedIndex > -1)
            {
                KdGol = comboBoxGolongan.SelectedValue.ToString().Trim();
            }


            bs.DataSource = new AdnSaldoAwalDao(this.cnn).GetDf("", KdGol, dateTimePickerTgl.Value, this.ThAjar);
            dgv.DataSource = bs;

            this.HitungTotal();


            if (dgv.RowCount != 0)
            {
                toolStripButtonSimpan.Enabled = true;
            }

            this.UseWaitCursor = false;
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void Simpan()
        {
            //MessageBox.Show(dateTimePickerTgl.Text);
            dgv.EndEdit();
            if (this.IsValid())
            {

                string KdAkunPenyeimbangSaw = AppVar.KdAkunPenyeimbangSAW;
                decimal Selisih = 0;
                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    decimal Debet = 0;
                    decimal Kredit = 0;
                    string KdAkun = (baris.Cells["KdAkun"].Value ?? "").ToString();
                    string Lampiran= AdnFungsi.CStr(baris.Cells["Lampiran"]);
                    
                    Debet = AdnFungsi.CDec(baris.Cells["Debet"]);
                    Kredit = AdnFungsi.CDec(baris.Cells["Kredit"]);

                   
                    if (KdAkun != KdAkunPenyeimbangSaw)
                    {
                        if (Lampiran == "0")
                        {
                            AdnSaldoAwal o = new AdnSaldoAwal();
                            o.KdAkun = KdAkun;
                            o.Tgl = dateTimePickerTgl.Value;
                            o.Debet = Debet;
                            o.Kredit = Kredit;

                            Selisih = Selisih + Debet - Kredit;
                            AdnSaldoAwalDao dao = new AdnSaldoAwalDao(this.cnn);

                            // Hapus dulu Saldo Awalnya
                            dao.Hapus(o.KdAkun, o.Tgl);
                            // baru Simpan Saldo Awalnya
                            dao.Simpan(o);
                        }
                    }
                }

                if (Selisih != 0)
                {
                    AdnSaldoAwal o = new AdnSaldoAwal();
                    o.KdAkun = KdAkunPenyeimbangSaw;
                    o.Tgl = dateTimePickerTgl.Value;

                    if (Selisih < 0)
                    {
                        o.Debet = Selisih*-1;
                        o.Kredit = 0;
                    }
                    else
                    {
                        o.Debet =0;
                        o.Kredit = Selisih;
                    }
                    
                    AdnSaldoAwalDao dao = new AdnSaldoAwalDao(this.cnn);
                    dao.Hapus(o.KdAkun, o.Tgl);
                    dao.Simpan(o);
                }

                MessageBox.Show("Berhasil disimpan!", AppVar.CaptionDialogBox,MessageBoxButtons.OK,MessageBoxIcon.Information);
                //dateTimePickerTgl.Refresh();
                //((DataTable)dgv.DataSource).Rows.Clear();
            }
        }

        private bool IsValid()
        {
            string sPesan = "";

            if (dateTimePickerTgl.Text.Replace("/", "").ToString().Trim() == "")
            {

                if (sPesan != "") { sPesan = sPesan + ", "; }
                sPesan = sPesan + "Tanggal";
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

        private void HitungTotal()
        {
            decimal TotalDebet = 0;
            decimal TotalKredit = 0;
            for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            {
                TotalDebet = TotalDebet + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["debet"]);
                TotalKredit = TotalKredit + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["kredit"]);
            }
            textBoxTotalKredit.Text = TotalKredit.ToString("N0");
            textBoxTotalDebet.Text = TotalDebet.ToString("N0");
        }

        private void FTSaldoAwal_Load(object sender, EventArgs e)
        {
            if (AppVar.KdAkunPenyeimbangSAW.ToString().Trim() == "")
            {
                MessageBox.Show("Akun Penyeimbang Saldo Awal Belum Ditentukan!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                toolStripButtonSimpan.Enabled = false;
                dgv.Enabled = false;
            }
        }

        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            dgv.EditingControl.KeyPress += new KeyPressEventHandler(EditingControl_KeyPress);
        }
        void EditingControl_KeyPress(object sender, KeyPressEventArgs e)
        {

            switch (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name)
            {
                case "KdAkun":
                    break;

                case "debet":
                case "kredit":
                    e.Handled = AdnFungsi.CekAngka(e.KeyChar);
                    break;


            }
        }

        private void dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            this.HitungTotal();
        }

        private void dgv_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["Lampiran"]) == "1" ||
                AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["KdAkun"])==AppVar.KdAkunPenyeimbangSAW)
            {
                MessageBox.Show("Saldo Awal ini Tidak Bisa Diedit!", AppVar.CaptionDialogBox, MessageBoxButtons.OK, MessageBoxIcon.Information);
                e.Cancel = true;
            }
            else if (AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["KdGol"]) == AdnVar.GolonganAKun.PIUTANG)
            {
                if (MessageBox.Show("Anda Harus Mengisi Saldo Awal melalui Saldo Awal Piutang!", AppVar.CaptionDialogBox, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            else if (AdnFungsi.CStr(dgv.Rows[e.RowIndex].Cells["KdGol"]) == AdnVar.GolonganAKun.UTANG)
            {
                if (MessageBox.Show("Anda Harus Mengisi Saldo Awal melalui Saldo Awal Utang!", AppVar.CaptionDialogBox, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            
        }
    }
}
