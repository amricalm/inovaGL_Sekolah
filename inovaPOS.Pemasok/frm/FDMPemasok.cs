using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;

namespace inovaPOS
{
    [AdnScObjectAtr("Form: Daftar Pemasok","Pemasok")]
    public partial class FDMPemasok : Form
    {
        private AdnScGroupRole fRole;
        private SqlConnection cnn;
        private BindingSource bs = new BindingSource();
        private FMPemasok fInduk;
        private string AppName;

        public FDMPemasok(object fInduk,SqlConnection cnn, string AppName)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.fInduk = (FMPemasok)fInduk;
            this.FillDataGridView("");

            //this.fRole = new AdnScGroupRoleDao(this.cnn).GetByKd(AppVar.AppPengguna.kd_group, this.Name);
            //if (!fRole.tambah) toolStripButtonTambah.Visible = false;
            
        }

        private void FillDataGridView(string sFilter)
        {
            dgv.AutoGenerateColumns = false;
            this.UseWaitCursor = true;
            Application.DoEvents();
            this.bs.DataSource = new AdnPemasokDao(this.cnn).GetAll();
            dgv.Columns["kd"].HeaderText = "Kode";
            dgv.Columns["nm"].HeaderText = "Nama Pemasok";
            dgv.Columns["kd"].DataPropertyName = "kd_ps";
            dgv.Columns["nm"].DataPropertyName = "nm_ps";
            dgv.DataSource = bs; 

            if (dgv.RowCount == 0)
            {
                //toolStripButtonPilih.Enabled = false;
            }
            else
            {
                //toolStripButtonPilih.Enabled = true;
            }
            this.UseWaitCursor = false;
        }
        private void Pilih()
        {
            this.fInduk.GetData(dgv.CurrentRow.Cells["kd"].Value.ToString().Trim());
            this.Close();
        }

        private void toolStripButtonTutup_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripButtonPilih_Click(object sender, EventArgs e)
        {
            this.Pilih();
        }
    }
}
