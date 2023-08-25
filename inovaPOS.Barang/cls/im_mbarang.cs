using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnBarang
    {
        private AdnSatuan _satuan = new AdnSatuan();

        private string _kd_barang;
        private string _barcode;
        private string _kd_group;
        private string _nm_barang;
        private string _kd_satuan;
        private decimal _harga_jual;
        private decimal _hpp;
        private string _kd_pemasok;
        private int _stock_min;
        private string _kd_rak;

        private int _saw_qty;
        private decimal _saw_hpp;

        private string _uid;
        private DateTime _tgl_tambah;
        private string _uid_edit;
        private DateTime _tgl_edit;

        public string kd_barang
        {
            get { return _kd_barang; }
            set { _kd_barang = value; }
        }
        public string barcode
        {
            get { return _barcode; }
            set { _barcode = value; }
        }
        public string kd_group
        {
            get { return _kd_group; }
            set { _kd_group = value; }
        }
        public string nm_barang
        {
            get { return _nm_barang; }
            set { _nm_barang = value; }
        }
        public string kd_satuan
        {
            get { return _kd_satuan; }
            set { _kd_satuan = value; }
        }
        public decimal harga_jual
        {
            get { return _harga_jual; }
            set { _harga_jual = value; }
        }
        public decimal hpp
        {
            get { return _hpp; }
            set { _hpp = value; }
        }
        public string kd_pemasok
        {
            get { return _kd_pemasok; }
            set { _kd_pemasok = value; }
        }
        public int stock_min
        {
            get { return _stock_min; }
            set { _stock_min = value; }
        }
        public string kd_rak
        {
            get { return _kd_rak; }
            set { _kd_rak = value; }
        }

        public int saw_qty
        {
            get { return _saw_qty; }
            set { _saw_qty = value; }
        }

        public decimal saw_hpp
        {
            get { return _saw_hpp; }
            set { _saw_hpp = value; }
        }

        public AdnSatuan satuan
        {
            get { return _satuan; }
            set { _satuan = value; }
        }

        #region "Audit"

        public string uid
        {
            get { return _uid; }
            set { _uid = value; }
        }
        public DateTime tgl_tambah
        {
            get { return _tgl_tambah; }
            set { _tgl_tambah = value; }
        }
        public string uid_edit
        {
            get { return _uid_edit; }
            set { _uid_edit = value; }
        }
        public DateTime tgl_edit
        {
            get { return _tgl_edit; }
            set { _tgl_edit = value; }
        }
        #endregion
    }
}
