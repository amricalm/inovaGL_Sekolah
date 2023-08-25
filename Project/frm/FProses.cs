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
    [AdnScObjectAtr("Form: Proses", "Proses")]
    public partial class FProses : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private string AppName;
        private DateTime PeriodeMulai;
        private string KdAkunLabaDitahan;
        private string KdAkunLabaThBerjalan;
        private string ThAjar;

        public FProses(SqlConnection cnn, string AppName, DateTime PeriodeMulai, string KdAkunLabaThBerjalan, string ThAjar)
        {
            InitializeComponent();
            this.AppName = AppName;
            this.cnn = cnn;
            this.PeriodeMulai = PeriodeMulai;
            this.KdAkunLabaThBerjalan = KdAkunLabaThBerjalan;
            this.ThAjar = ThAjar;
            dtpTglSd.Value = PeriodeMulai.AddYears(1).AddDays(-1);
            this.GetData();

        }
        private void FProses_KeyDown(object sender, KeyEventArgs e)
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

            //toolStripButtonTambah.Enabled = true;
            toolStripButtonEdit.Enabled = false;
            //toolStripButtonBatal.Enabled = true;
            //toolStripButtonHapus.Enabled = true;
            toolStripButtonSimpan.Enabled = false;

        }
        private bool IsValid()
        {
            string sPesan = "";

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
   

        private void FProses_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void buttonProsesTutupPeriode_Click(object sender, EventArgs e)
        {
            DataTable lst = new inovaGL.Data.AdnLapNeraca(this.cnn).CetakFormatRincian("NRC", this.PeriodeMulai, dtpTglSd.Value, this.KdAkunLabaThBerjalan, this.ThAjar);
            decimal NilaiLbThBerjalan = 0;

            for (int i = 0; i < lst.Rows.Count; i++)
            {
                if (lst.Rows[i]["KdAkun"].ToString() == KdAkunLabaThBerjalan)
                {
                    NilaiLbThBerjalan = AdnFungsi.CDec(lst.Rows[i]["Kredit"]) - AdnFungsi.CDec(lst.Rows[i]["Debet"]);
                }
            }


            for (int i = 0; i < lst.Rows.Count;i++ )
            {
                inovaGL.Data.AdnSaldoAwal o = new  inovaGL.Data.AdnSaldoAwal();
                o.KdAkun = lst.Rows[i]["KdAkun"].ToString();
                o.Tgl = dtpTglSd.Value.AddDays(1);

                if (o.KdAkun.ToString() == AppVar.KdAkunLabaTahunBerjalan)
                {
                    o.Debet = 0;
                    o.Kredit = 0;
                }
                else if(o.KdAkun.ToString() == AppVar.KdAkunLabaDitahan)
                {
                    o.Debet = AdnFungsi.CDec(lst.Rows[i]["Debet"]);
                    o.Kredit = AdnFungsi.CDec(lst.Rows[i]["Kredit"]) + NilaiLbThBerjalan;
                }
                else
                {
                    o.Debet = AdnFungsi.CDec(lst.Rows[i]["Debet"]);
                    o.Kredit = AdnFungsi.CDec(lst.Rows[i]["Kredit"]);
                }

                inovaGL.Data.AdnSaldoAwalDao dao = new inovaGL.Data.AdnSaldoAwalDao(this.cnn);
                dao.Hapus(o.KdAkun, dtpTglSd.Value.AddDays(1));
                dao.Simpan(o);

                if (AdnFungsi.UpdateSysVar(this.cnn, "periode_mulai",dtpTglSd.Value.AddDays(1).ToString()))
                {
                    AppVar.PeriodeMulai = dtpTglSd.Value.AddDays(1);
                }   
            }
            MessageBox.Show("Proses Tutup Buku Berhasil!", AppVar.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }



    }
}
