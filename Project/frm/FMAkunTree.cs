using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Andhana;
using inovaGL.Definisi;
using inovaGL.Data;

namespace inovaGL
{
    [AdnScObjectAtr("Form: Struktur Akun", "Akun")]
    public partial class FMAkunTree : Andhana.AdnEntriForm
    {
        private SqlConnection cnn;
        private short ModeEdit;
        private BindingSource bs =new BindingSource();
        private string AppName;
        //private FDTVoucher fInduk;

        public FMAkunTree(SqlConnection cnn,string AppName,short ModeEdit,string kd,object fInduk)
        {
            InitializeComponent();
            this.cnn = cnn;
            this.AppName = AppName;
            this.ModeEdit = ModeEdit;

            List<AdnTreeItem> lst = new List<AdnTreeItem>();
            List<AdnAkun> lstAkun = new AdnAkunDao(this.cnn).GetAll();
            foreach (AdnAkun item in lstAkun)
            {
                lst.Add(new AdnTreeItem(item.KdAkun, item.NmAkun, item.Turunan));
            }
            //this.PopulateTree(treeViewAkun, lst);
            this.InitTree(treeViewAkun, lst);
            //this.InitTreeView();
            //if (this.ModeEdit == AdnModeEdit.BACA)
            //{
            //    this.GetData(kd);
            //}
            //else
            //{
            //    this.DokumenBaru();
            //}

        }
        
        private void Tambah()
        {
            this.DokumenBaru();
        }
        private void Edit()
        {
            this.ModeEdit = AdnModeEdit.UBAH;

            panelHdr.Enabled = true;
           
            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = true;

            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;

        }
        private void Simpan()
        {
        }
        private void Hapus()
        {

        }
        private void DokumenBaru()
        {
            AdnFungsi.Bersih(this, true);
            this.ModeEdit = AdnModeEdit.BARU;
            panelHdr.Enabled = true;


            toolStripButtonTambah.Enabled = false;
            toolStripButtonEdit.Enabled = false;
            toolStripButtonHapus.Enabled = false;
            toolStripButtonBatal.Enabled = true;
            toolStripButtonSimpan.Enabled = true;
        }
        private void Batal()
        {
            this.DokumenBaru();
        }
        private void GetData(string Kd)
        {
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

        private void FTVoucherGenerator_KeyDown(object sender, KeyEventArgs e)
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
                        e.Handled =true;
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
        private void InitTreeView()
        {
            treeViewAkun.BeginUpdate();
            int i = 0;
            DataTable lst;
            TreeNode nodeAkun;
            
            int TkAkhir =new AdnAkunDao(this.cnn).GetTingkatAkhir();

            lst = new AdnAkunDao(this.cnn).GetByTingkat(i);
            i++;
            foreach (DataRow row in lst.Rows)
            {
                short iNode = 0;
                treeViewAkun.Nodes.Add(row["NmAkun"].ToString());
                nodeAkun = treeViewAkun.Nodes[iNode];
                for (i = 1; i <= TkAkhir; i++)
                {
                    lst = new AdnAkunDao(this.cnn).GetByTingkat(i);
                    short iNodeAnak = 0;
                    foreach (DataRow rowAnak in lst.Rows)
                    {
                        nodeAkun.Nodes.Add(rowAnak["NmAkun"].ToString());
                        //nodeAkun = nodeAkun.Nodes[iNodeAnak];
                        iNodeAnak++;
                    }

                }
                iNode++;
            }

            treeViewAkun.EndUpdate();
        }
        private void PopulateTree(TreeView tree, ICollection<AdnTreeItem> items)
        {
            tree.Nodes.Clear();
            List<TreeNode> roots = new List<TreeNode>();
            roots.Add(tree.Nodes.Add("Items"));
            foreach (AdnTreeItem item in items)
            {
                //if (item.Tingkat == 0)
                //{
                //    roots.Add(tree.Nodes.Add(item.Nama));
                //}
                //else
                //{
                    if (item.Tingkat == roots.Count) roots.Add(roots[roots.Count - 1].LastNode);
                    roots[item.Tingkat].Nodes.Add(item.Nama);
                //}
            }
        }

        private void InitTree(TreeView tree, ICollection<AdnTreeItem> items)
        {
            tree.BeginUpdate();
            //treeViewAkun.BeginUpdate();
            //int i = 0;
            //DataTable lst;
            TreeNode nodeAkun;

            //int TkAkhir = new AdnAkunDao(this.cnn).GetTingkatAkhir();

            //lst = new AdnAkunDao(this.cnn).GetByTingkat(i);
            
            int LastTk = 0;
            List<TreeNode> lstNode = new List<TreeNode>();
            foreach (AdnTreeItem item in items)
            
            {
                if (item.Nama=="KEWAJIBAN")
                {

                }
                if (LastTk == item.Tingkat)
                {
                    if (item.Tingkat== 0)
                    {
                        nodeAkun = tree.Nodes.Add(item.Nama); // Tk 0
                        if (lstNode.Count > 0)
                        {
                            if (lstNode[item.Tingkat] != null)
                            {
                                lstNode[item.Tingkat] = nodeAkun;
                            }
                            else
                            {
                                lstNode.Add(nodeAkun);
                            }
                        }
                        else
                        {
                            lstNode.Add(nodeAkun);
                        }
                        
                    }
                    else
                    {
                        nodeAkun = lstNode[item.Tingkat - 1].Nodes.Add(item.Nama);
                        
                        if (lstNode[item.Tingkat] != null)
                        {
                            lstNode[item.Tingkat] = nodeAkun;
                        }
                        else
                        {
                            lstNode.Add(nodeAkun);
                        }
                    }
                }
                else
                {
                    if (item.Tingkat== 0)
                    {
                        nodeAkun = tree.Nodes.Add(item.Nama); // Tk 0
                        if (lstNode[item.Tingkat] != null)
                        {
                            lstNode[item.Tingkat] = nodeAkun;
                        }
                        else
                        {
                            lstNode.Add(nodeAkun);
                        }
                    }
                    else
                    {
                        nodeAkun = lstNode[item.Tingkat - 1].Nodes.Add(item.Nama);
                        lstNode.Add(nodeAkun);

                        if (lstNode[item.Tingkat] != null)
                        {
                            lstNode[item.Tingkat] = nodeAkun;
                        }
                        else
                        {
                            lstNode.Add(nodeAkun);
                        }

                        LastTk = item.Tingkat;
                    }
                }
            }

            //treeViewAkun.EndUpdate();
            tree.EndUpdate();
        }

    }

}
