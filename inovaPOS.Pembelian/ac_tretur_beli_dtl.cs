using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnTReturBeliDtl
    {
        private string _no_faktur;
        private string _kd_barang;
        private string _kd_satuan;
        private int _qty;

        private AdnBarang _barang = new AdnBarang();

        private string _uid;
        private DateTime _tgl_tambah;
        private string _uid_edit;
        private DateTime _tgl_edit;

        public string no_faktur
        {
            get { return _no_faktur; }
            set { _no_faktur = value; }
        }
        public string kd_barang
        {
            get { return _kd_barang; }
            set { _kd_barang = value; }
        }
        public int qty
        {
            get { return _qty; }
            set { _qty = value; }
        }

        public string kd_satuan
        {
            get { return _kd_satuan; }
            set { _kd_satuan = value; }
        }

        public AdnBarang barang
        {
            get { return _barang; }
            set { _barang = value; }
        }

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
    }
}
