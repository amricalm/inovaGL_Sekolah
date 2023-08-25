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

namespace inovaGL.Piutang
{
    [AdnScObjectAtr("Form: Daftar Saldo Awal Piutang Siswa", "Saldo Awal")]
    public partial class FTSaldoPiutangSiswa : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        private AdnScPengguna Pengguna;
        private string KdSekolah = "";
        private string ThAjar = "";
        public FTSaldoPiutangSiswa(SqlConnection cnn, string AppName, AdnScPengguna Pengguna, string KdSekolah,string ThAjar)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna=Pengguna;
            this.KdSekolah = KdSekolah.ToString().Trim();
            this.ThAjar = ThAjar.ToString().Trim();
            dateTimePickerTgl.Focus();

            new EDUSIS.Kelas.AdnKelasDao(this.cnn).SetCombo(comboBoxKelas, KdSekolah);
            comboBoxKelas.SelectedIndex = -1;

            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            dgv.AutoGenerateColumns = false;

            string Kelas = "";
            string Nis ="";
            textBoxNmSiswa.Text = "";
            if (comboBoxKelas.SelectedIndex > -1)
            {
                Kelas = comboBoxKelas.SelectedValue.ToString().Trim();
                if (listBoxSiswa.Items.Count>0)
                {
                    Nis = listBoxSiswa.SelectedValue.ToString().Trim();
                    textBoxNmSiswa.Text = listBoxSiswa.Text;
                }
            }

            bs.DataSource = null;
            int KdSiswa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetKdSiswa(Nis,this.KdSekolah);
            if (KdSiswa != 0)
            {
                bs.DataSource = new AdnSaldoAwalDao(this.cnn).Get(KdSiswa, this.ThAjar, dateTimePickerTgl.Value);
            }
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
            dgv.EndEdit();
            if (this.IsValid())
            {
                int Tingkat = new EDUSIS.Shared.AdnSekolahDao(this.cnn).Get(this.KdSekolah).Tingkat;

                string Nis = listBoxSiswa.SelectedValue.ToString().Trim();
                int KdSiswa = new EDUSIS.Siswa.AdnSiswaDao(this.cnn).GetKdSiswa(Nis, this.KdSekolah);
                
                //--- Header SAW
                AdnSaldoAwal o = new AdnSaldoAwal();
                o.KdSaldoAwal ="";
                o.KdSiswa =KdSiswa;
                o.KdSekolah = this.KdSekolah;
                o.ThAjar = this.ThAjar;
                o.Tgl = dateTimePickerTgl.Value;
                o.Periode = "";
                o.Total = AdnFungsi.CDec(textBoxTotal);
                o.DfItem = new List<AdnSaldoAwalDtl>();
                //--- END --- Header SAW

                foreach (DataGridViewRow baris in dgv.Rows)
                {
                    // --- Rincian Biaya SAW
                    AdnSaldoAwalDtl dtl = new AdnSaldoAwalDtl();
                    dtl.KdSaldoAwal= o.KdSaldoAwal;
                    dtl.KdDtl=0;
                    dtl.KdBiaya = AdnFungsi.CStr(baris.Cells["KdBiaya"]);
                    dtl.Jmh = AdnFungsi.CDec(baris.Cells["Jmh"]);
                    dtl.ItemBulan = "";
                    dtl.Keterangan = AdnFungsi.CStr(baris.Cells["Keterangan"]);

                    dtl.DfPeriode = new List<AdnSaldoAwalDtlPeriode>();

                    string StrPeriode = AdnFungsi.CStr(baris.Cells["ItemBulan"]);
                    if (StrPeriode.Trim() != "")
                    {
                        string[] ArrPeriode = StrPeriode.Split(',');
                        foreach (string item in ArrPeriode)
                        {
                            // Periode/Bulan Tagihan
                            AdnSaldoAwalDtlPeriode oPeriode = new AdnSaldoAwalDtlPeriode();
                            oPeriode.Periode = item;

                            dtl.DfPeriode.Add(oPeriode);
                            // --- END --- Periode/Bulan Tagihan
                        }
                    }
                    o.DfItem.Add(dtl);
                    // --- END --- Rincian Biaya Tagihan
                }
                
                // Simpan Data
                SqlTransaction Trans=null;
                Trans = this.cnn.BeginTransaction();
                AdnSaldoAwalDao dao = new AdnSaldoAwalDao(this.cnn, this.Pengguna, Trans);
                dao.Simpan(o);
                Trans.Commit();
                // --- END --- Simpan Data

                MessageBox.Show("Berhasil disimpan!", this.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            for (int iBaris = 0; iBaris <= dgv.Rows.Count - 1; iBaris++)
            {
                TotalDebet = TotalDebet + AdnFungsi.CDec(dgv.Rows[iBaris].Cells["Jmh"]);
            }
            textBoxTotal.Text = TotalDebet.ToString("N0");
        }

        private void comboBoxKelas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxKelas.SelectedIndex > -1)
            {
                new EDUSIS.KelasSiswa.AdnKelasSiswaDao(this.cnn).SetListBox(listBoxSiswa, this.KdSekolah, this.ThAjar, comboBoxKelas.SelectedValue.ToString());
            }
            else
            {
                listBoxSiswa.DataSource = null;
            }
        }

        private void listBoxSiswa_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }


    }
}
