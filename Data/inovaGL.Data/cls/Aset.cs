using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnAset : AdnBaseClass
    {
        public string KdAset { get; set; }
        public string NmAset { get; set; }
        public string Lokasi   { get; set; }
        public string Departemen  { get; set; }
        public string Merk  { get; set; }
        public string Model  { get; set; }
        public string SerialNo  { get; set; }
        public DateTime TglBeli   { get; set; }
        public string JenisUmur  { get; set; }
        public int Umur  { get; set; }
        public int Qty   { get; set; }
        public decimal Harga   { get; set; }
        public decimal Total  { get; set; }
        public decimal NilaiResidu  { get; set; }
        public decimal NilaiBuku   { get; set; }
        public string CoaAkumulasiPenyusutan { get; set; }
        public string CoaBebanPenyusutan { get; set; }
        public string KdKelompokAset  { get; set; }
        public bool Aktif { get; set; }

        public AdnAset()
        {
            this.KdAset = "";
            this.NmAset = "";
            this.Lokasi= "";
            this.Departemen= "";
            this.Merk = "";
            this.Model = "";
            this.SerialNo = "";
            this.JenisUmur = "";
            this.Umur = 0;
            this.Qty = 0;
            this.Harga = 0;
            this.Total = 0;
            this.NilaiResidu = 0;
            this.NilaiBuku = 0;
            this.KdKelompokAset = "";
            this.CoaAkumulasiPenyusutan = "";
            this.CoaBebanPenyusutan = "";
            this.Aktif = true;
        }
    }
}
