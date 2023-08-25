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

namespace EDUSIS.KeuanganPembayaran
{
    [AdnScObjectAtr("Form: Pembayaran ", "Pembayaran")]
    public partial class FTBayar : Form
    {
        private SqlConnection cnn;
        private string AppName;
        private AdnScPengguna Pengguna;
        FTPembayaran fInduk = null;

        public FTBayar(SqlConnection cnn, string AppName, AdnScPengguna Pengguna,decimal Jmh, FTPembayaran fInduk)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna = Pengguna;

            this.fInduk = fInduk;

            textBoxJumlah.Text = Jmh.ToString("N0");
            textBoxBayar.Focus();
            textBoxKembali.Text = (-Jmh).ToString("N0");

        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonSimpan_Click(object sender, EventArgs e)
        {
            this.fInduk.Simpan();
            this.Close();
        }

        private void textBoxBayar_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = AdnFungsi.CekAngka(e.KeyChar);
        }

        private void textBoxBayar_TextChanged(object sender, EventArgs e)
        {
            decimal Kembali = AdnFungsi.CDec(textBoxBayar) - AdnFungsi.CDec(textBoxJumlah) ;
            textBoxKembali.Text = Kembali.ToString("N0");
            textBoxBayar.Text = AdnFungsi.CDec(textBoxBayar).ToString("N0");
            textBoxBayar.SelectionStart = textBoxBayar.TextLength;
        }

        private void textBoxBayar_KeyDown(object sender, KeyEventArgs e)
        {
            if (AdnFungsi.CDec(textBoxKembali) >= 0)
            {
                switch (e.KeyCode)
                {
                    case Keys.Return:
                        this.fInduk.Simpan();
                        this.Close();
                        break;
                    default:
                        break;
                }
            }
        }



    }
}
