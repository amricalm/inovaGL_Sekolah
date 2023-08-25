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

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Daftar Gudang", "Gudang")]
    public partial class FDMGudang : Form
    {
        private SqlConnection cnn;
        private string AppName;
        BindingSource bs = new BindingSource();

        public FDMGudang(SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;

            //comboBoxPemasok.ValueMember = "kd_ps";
            //comboBoxPemasok.DisplayMember = "nm_ps";
            //comboBoxPemasok.DataSource = new AdnPemasokDao(this.cnn).GetAll();

            ////--- ComboBox Group
            //comboBoxGroup.DisplayMember = "nm_group";
            //comboBoxGroup.ValueMember = "kd_group";
            //comboBoxGroup.DataSource = new AdnGroupBarangDao(this.cnn).GetAll();
            ////--- END ---------------

        }
        private void FDMBarang_Load(object sender, EventArgs e)
        {
            dgv.AutoGenerateColumns = false;
            this.FillDataGridView();
        }

        private void FillDataGridView()
        {
            this.UseWaitCursor = true;
            Application.DoEvents();

            bs.DataSource = new AdnGudangDao(this.cnn).GetByArgs("");
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
            //FMBarang ofm = new FMBarang(this.cnn,this.AppName, AdnModeEdit.BARU, "", this);
            //ofm.ShowDialog();
        }
        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            //this.Pilih();
        }

        private void Pilih()
        {
            //panelHdr.Enabled = true;
            //panelDtl.Enabled = true;

            //FMBarang ofm = new FMBarang(this.cnn,this.AppName,  AdnModeEdit.BACA, dgv.CurrentRow.Cells["kd_barang"].Value.ToString(), this);
            //ofm.ShowDialog();

        }
        
        public void Reload()
        {
            //this.FillDataGridView();
        }

        private void cariButton_Click(object sender, EventArgs e)
        {
            //this.FillDataGridView();
        }
    }
}
