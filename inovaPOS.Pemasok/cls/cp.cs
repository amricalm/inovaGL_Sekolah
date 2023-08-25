using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnContactPerson
    {
        private int _kd_cp;
        private string _kd_ps;
        private string _nm_lengkap;
        private string _jabatan;
        private string _telp;
        private string _hp;
        private string _email;
        private string _ket;
        private string _uid;
        private DateTime _tgl_tambah;
        private string _uid_edit;
        private DateTime _tgl_edit;

        public int kd_cp
        {
            get { return _kd_cp; }
            set { _kd_cp = value; }
        }
        public string kd_ps
        {
            get { return _kd_ps; }
            set { _kd_ps = value; }
        }
        public string nm_lengkap
        {
            get { return _nm_lengkap; }
            set { _nm_lengkap = value; }
        }
        public string jabatan
        {
            get { return _jabatan; }
            set { _jabatan = value; }
        }
        public string telp
        {
            get { return _telp; }
            set { _telp = value; } 
        }
        public string hp
        {
            get { return _hp; }
            set { _hp = value; }
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
