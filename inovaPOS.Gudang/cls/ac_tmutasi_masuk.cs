using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPOS
{
    public class AdnMutasiMasuk
    {
        private string _no_faktur;
        private DateTime _tgl;
        private string _kd_gudang;
        private string _ket;
        private string _kd_term="";
        private decimal _diskon=0;
        private decimal _biaya_kirim = 0;

        private string _uid;
        private DateTime _tgl_tambah;
        private string _uid_edit;
        private DateTime _tgl_edit;
        List<AdnMutasiMasukDtl> _item_df;

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
        public string kd_gudang
        {
            get { return _kd_gudang; }
            set { _kd_gudang = value; }
        }
        public string ket
        {
            get { return _ket; }
            set { _ket = value; }
        }
        public string kd_term
        {
            get { return _kd_term; }
            set { _kd_term = value; }
        }
        public decimal diskon
        {
            get { return _diskon; }
            set { _diskon = value; }
        }
        public decimal biaya_kirim
        {
            get { return _biaya_kirim; }
            set { _biaya_kirim = value; }
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
        public List<AdnMutasiMasukDtl> item_df
        {
            get { return _item_df; }
            set { _item_df = value; }
        }
    }
}
