using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnSaldoAwal : AdnBaseClass
    {
        public string KdAkun { get; set; }
        public DateTime Tgl { get; set; }
        public decimal Debet { get; set; }
        public decimal Kredit { get; set; }
        
        public AdnAkun Akun { get; set; }

        public AdnSaldoAwal()
        {
            this.KdAkun = "";
            this.Debet = 0;
            this.Kredit= 0;

            this.Akun = new AdnAkun();
        }
    }
}
