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
    [AdnScObjectAtr("Form: Pilih Periode/Bulan", "Biaya")]
    public partial class FPilihPeriode : Form
    {
        private SqlConnection cnn;
        private string AppName;
        private AdnScPengguna Pengguna;
        BindingSource bs = new BindingSource();

        public delegate void ChildFormUpdateHandler(object sender, ChildEventArgs e);
        public event ChildFormUpdateHandler ChildFormUpdate;


        public FPilihPeriode(SqlConnection cnn, string AppName, AdnScPengguna Pengguna)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.Pengguna = Pengguna;

            this.FillDataGridView();
            numericUpDownTahun.Value = DateTime.Now.Year;

        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            bs.DataSource = AdnFungsi.ListBulan(false);
            dgv.DataSource = bs;

            if (dgv.RowCount == 0)
            {
                toolStripButtonPilih.Enabled = false;
            }
            else
            {
                toolStripButtonPilih.Enabled = true;
            }
            this.UseWaitCursor = false;
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Pilih();
            //this.Close();
        }
        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            this.Pilih();
        }

        private void Pilih()
        {
            string Periode = "";
            int JmhPeriode = 0;
            dgv.EndEdit();
            for (int i = 0; i < 12; i++)
            {
                if (AdnFungsi.CBool(dgv.Rows[i].Cells["PilihBulan"],true))
                {
                    if (Periode != "")
                    {
                        Periode += ", ";
                    }
                    Periode = Periode + numericUpDownTahun.Value.ToString() + "-" + AdnFungsi.CInt(dgv.Rows[i].Cells["KdBulan"], true).ToString().PadLeft(2,'0');
                    JmhPeriode++;
                }
            }
            ChildEventArgs args = new ChildEventArgs(Periode,JmhPeriode);
            this.ChildFormUpdate(this, args);
            this.Close();
        }

        private void FPilihPeriode_FormClosing(object sender, FormClosingEventArgs e)
        {
            //this.Pilih();
        }
    }
    public class ChildEventArgs : System.EventArgs
    {
        //public int KdBulan { get; set; }
        //public string NmBulan { get; set; }

        //public ChildEventArgs(int KdBulan, string NmBulan)
        //{
        //    this.KdBulan = KdBulan;
        //    this.NmBulan = NmBulan.ToString().Trim();
        //}
        public string Periode { get; set; }
        public int JmhPeriode { get; set; }

        public ChildEventArgs(string Periode, int JmhPeriode)
        {
            this.Periode = Periode;
            this.JmhPeriode = JmhPeriode;
        }

    }
}
