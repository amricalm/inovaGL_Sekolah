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
    [AdnScObjectAtr("Form: Daftar Pos", "Pos")]
    public partial class FDPos : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();
        public FDPos(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            new AdnDeptDao(this.cnn).SetCombo(comboBoxDept);
        }
        private void FDPos_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();
            dgv.AutoGenerateColumns = false;

            string KdDept = "";
            if (comboBoxDept.SelectedIndex > -1)
            {
                KdDept = comboBoxDept.SelectedValue.ToString().Trim();
            }

            bs.DataSource = new AdnPosDao(this.cnn).GetByDept(KdDept);
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
        private void toolStripButtonTambah_Click(object sender, EventArgs e)
        {
            FMPos ofm = new FMPos(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
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

            FMPos ofm = new FMPos(this.cnn,this.AppName,  AdnModeEdit.BACA, AdnFungsi.CStr(dgv.CurrentRow.Cells["KdPos"]), this);
            ofm.ShowDialog();

        }
        
        public void Reload()
        {
            this.FillDataGridView();
        }

        private void cariButton_Click(object sender, EventArgs e)
        {
            this.FillDataGridView();
        }
    
    }
}
