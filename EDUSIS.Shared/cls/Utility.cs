using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDUSIS.Shared
{
    class Utility
    {
        //internal static string TingkatSekolah(string KdSekolah)
        //{
        //    string hasil = "";
        //    try
        //    {


        //    }
        //    catch (Exception exp)
        //    {
        //        Andhana.AdnFungsi.LogErr(exp.Message.ToString());
        //    }
        //    return hasil;
        //}
    }

    public class AdnSekolah : Andhana.AdnBaseClass
    {
        public string KdSekolah { get; set; }
        public string NmSekolah { get; set; }
        public int Tingkat { get; set; }
        public string NSS { get; set; }
        public string Alamat{ get; set; }
        public string Kelurahan { get; set; }
        public string KdPos { get; set; }
        public string Kecamatan { get; set; }
        public string Kabupaten { get; set; }
        public string Propinsi { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string SMS { get; set; }
        public string Web { get; set; }

        public AdnSekolah()
        {
            this.NSS = "";
            this.Alamat = "";
            this.Kelurahan = ""; 
            this.Kecamatan = "";
            this.KdPos = "";
            this.Kabupaten= "";
            this.Propinsi= "";
            this.Telp = "";
            this.Fax = "";
            this.Email= "";
            this.SMS= "";
            this.Web= "";
        }
    }

    public class AdnThAjar : Andhana.AdnBaseClass
    {
        public string ThAjar { get; set; }
        public string Keterangan { get; set; }

        public AdnThAjar()
        {
            this.Keterangan = "";
        }
    }

    public class AdnCatering: Andhana.AdnBaseClass
    {
        public string KdCatering { get; set; }
        public string NmCatering { get; set; }
    }

    public class AdnEskul : Andhana.AdnBaseClass
    {
        public string KdEskul { get; set; }
        public string NmEskul { get; set; }
    }

    public class AdnSanggar : Andhana.AdnBaseClass
    {
        public string KdSanggar { get; set; }
        public string NmSanggar { get; set; }
    }
}
