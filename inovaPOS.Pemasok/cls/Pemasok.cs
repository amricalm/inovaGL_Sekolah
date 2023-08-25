using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnPemasok
    {
        private string _kd_pemasok;
        private string _nm_ps;
        private string _nm_kontak;
        private string _alamat;
        private string _kota;
        private string _pos;
        private string _telp;
        private string _fax;
        private string _email;

        public string kd_pemasok
        {
            get { return _kd_pemasok; }
            set { _kd_pemasok = value; }
        }
        
        public string nm_ps
        {
            get { return _nm_ps; }
            set { _nm_ps = value; }
        }

        public string nm_kontak
        {
            get { return _nm_kontak; }
            set { _nm_kontak = value; }
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

    }
}
