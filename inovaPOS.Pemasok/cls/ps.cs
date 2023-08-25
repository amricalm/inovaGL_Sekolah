using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnPerusahaan
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
        private string _ket;
        private string _bidang_usaha;
        private string _web;
        private decimal _aset;
        private decimal _omset;
        private int _jmh_karyawan;
        private decimal _modal;
        //private decimal _marketing_fee;
        private string _sumber = "";
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
        public string ket
        {
            get { return _ket; }
            set { _ket = value; }
        }
        public string bidang_usaha
        {
            get { return _bidang_usaha; }
            set { _bidang_usaha = value; }
        }
        public string web
        {
            get { return _web; }
            set { _web = value; }
        }
        public decimal aset
        {
            get { return _aset; }
            set { _aset = value; }
        }
        public decimal omset
        {
            get { return _omset; }
            set { _omset = value; }
        }
        public int jmh_karyawan
        {
            get { return _jmh_karyawan; }
            set { _jmh_karyawan = value; }
        }
        public decimal modal
        {
            get { return _modal; }
            set { _modal = value; }
        }
        //public decimal marketing_fee
        //{
        //    get { return _marketing_fee; }
        //    set { _marketing_fee = value; }
        //}
        public string sumber
        {
            get { return _sumber; }
            set { _sumber = value; }
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
