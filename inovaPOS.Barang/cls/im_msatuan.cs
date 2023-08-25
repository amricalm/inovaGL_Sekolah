using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnSatuan
    {
        private string _kd_satuan;
        private string _nm_satuan;
        private string _uid;
        private DateTime _tgl_tambah;
        private string _uid_edit;
        private DateTime _tgl_edit;

        public string kd_satuan
        {
            get { return _kd_satuan; }
            set { _kd_satuan = value; }
        }
        public string nm_satuan
        {
            get { return _nm_satuan; }
            set { _nm_satuan = value; }
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
