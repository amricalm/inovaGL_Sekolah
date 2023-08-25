using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnPemasok
    {
        private string _kd_ps;
        private string _nm_ps;
        private string _alamat;
        private string _kota;
        private string _pos;
        private string _propinsi;
        private string _telp;
        private string _fax;
        private string _email;
        private string _web;
        private string _cp;
        private string _uid;
        private DateTime _tgl_tambah;
        private string _uid_edit;
        private DateTime _tgl_edit;

        public string kd_ps
        {
            get { return _kd_ps; }
            set { _kd_ps = value; }
        }
        public string nm_ps
        {
            get { return _nm_ps; }
            set { _nm_ps = value; }
        }
        public string alamat
        {
            get { return _alamat; }
            set { _alamat = value; }
        }
        public string kota
        {
            get { return _kota; }
            set { _kota = value; }
        }
        public string pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public string propinsi
        {
            get { return _propinsi; }
            set { _propinsi = value; }
        }
        public string telp
        {
            get { return _telp; }
            set { _telp = value; }
        }
        public string fax
        {
            get { return _fax; }
            set { _fax = value; }
        }
        public string email
        {
            get { return _email; }
            set { _email = value; }
        }
        public string web
        {
            get { return _web; }
            set { _web = value; }
        }
        public string cp
        {
            get { return _cp; }
            set { _cp = value; }
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
