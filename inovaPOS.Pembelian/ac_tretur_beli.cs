using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnTReturBeli
    {
        private string _no_faktur;
        private DateTime _tgl;
        private string _kd_ps;
        private string _ket;
        private string _uid;
        private DateTime _tgl_tambah;
        private string _uid_edit;
        private DateTime _tgl_edit;

        List<AdnTReturBeliDtl> _item_df;

        public string no_faktur
        {
            get { return _no_faktur; }
            set { _no_faktur = value; }
        }
        public DateTime tgl
        {
            get { return _tgl; }
            set { _tgl = value; }
        }
        public string kd_ps
        {
            get { return _kd_ps; }
            set { _kd_ps = value; }
        }
        public string ket
        {
            get { return _ket; }
            set { _ket = value; }
        }

        public List<AdnTReturBeliDtl> item_df
        {
            get { return _item_df; }
            set { _item_df = value; }
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
