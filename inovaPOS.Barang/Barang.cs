using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class Barang
    {
        private string _kd_barang;
        private string _nm_barang;
        private string _barcode;
        private string _kd_satuan;
        private decimal _harga_jual;

        public string kd_barang
        {
            get { return _kd_barang; }
            set { _kd_barang = value; }
        }

        public string nm_barang
        {
            get { return _nm_barang; }
            set { _nm_barang = value; }
        }

        public decimal harga_jual
        {
            get { return _harga_jual; }
            set { _harga_jual = value; }
        }
    }
}
