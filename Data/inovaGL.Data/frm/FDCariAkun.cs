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
    public partial class FDCariAkun : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        //private object fInduk = null;
        //private object fIndukBB = null;
        public delegate void ChildFormUpdateHandler(object sender, ChildEventArgs e);
        public event ChildFormUpdateHandler ChildFormUpdate;
        private string KdGol = "";

        public FDCariAkun(SqlConnection cnn, string AppName,object fInduk,object fIndukBB)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
        }

        public FDCariAkun(SqlConnection cnn, string AppName, object fInduk, object fIndukBB, string KdGol)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.KdGol = KdGol;
        }

        private void FDCariAkun_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            string Kd = "";
            string Nm = "";

            if (Kd != "")
            {
                Kd = textBoxKd.Text.ToString().Trim();
            }

            if (Nm != "")
            {
                Nm = textBoxNm.Text.ToString().Trim();
            }

            if (this.KdGol!="")
            {
                bs.DataSource = new AdnAkunDao(this.cnn).GetDfByGolDetail(this.KdGol);
            }
            else
            {
                bs.DataSource = new AdnAkunDao(this.cnn).GetDf();
            }
            
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
            this.Close();
        }

        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            this.Pilih();
        }

        private void Pilih()
        {
            if (dgv.Rows.Count>0)
            {
                ChildEventArgs args = new ChildEventArgs(AdnFungsi.CStr(dgv.CurrentRow.Cells["KdAkun"]), AdnFungsi.CStr(dgv.CurrentRow.Cells["NmAkun"]));
                this.ChildFormUpdate(this, args);

                //if (this.fInduk.GetType().Name.ToString() == "FMAkun")
                //{Form o = new Form();
                //    o.
                //    (this.fInduk)..SetDataAkunInduk(AdnFungsi.CStr(dgv.CurrentRow.Cells["KdAkun"]), AdnFungsi.CStr(dgv.CurrentRow.Cells["NmAkun"]));
                //}
                //if (this.fIndukBB != null)
                //{
                //    ((FDlgLapBB)this.fIndukBB).SetKdAkun(AdnFungsi.CStr(dgv.CurrentRow.Cells["KdAkun"]), AdnFungsi.CStr(dgv.CurrentRow.Cells["NmAkun"]));
                //}
            }
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

        private void FilterData()
        {
            string sKd = textBoxKd.Text.ToString().Trim();
            string sNm = textBoxNm.Text.ToString().Trim();

            bs.Filter = "KdAKun LIKE '" + sKd + "*' AND NmAkun LIKE '*" + sNm + "*'";
        }


        private void textBoxNm_TextChanged(object sender, EventArgs e)
        {
            this.FilterData();
        }

        private void textBoxKd_TextChanged(object sender, EventArgs e)
        {
            string sKd = textBoxKd.Text.ToString().Trim();
            string sNm = textBoxNm.Text.ToString().Trim();

            bs.Filter = "KdAKun LIKE '" + sKd + "*'";
        }

        private void dgv_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.Pilih();
                    break;
            }
        }

        private void dgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.Pilih();
        }



    }

    public class ChildEventArgs : System.EventArgs
    {
        public string KdAkun { get; set; }
        public string NmAkun { get; set; }

        public ChildEventArgs(string KdAkun, string NmAkun)
        {
            this.KdAkun = KdAkun.ToString().Trim();
            this.NmAkun = NmAkun.ToString().Trim();
        }
    }
}
