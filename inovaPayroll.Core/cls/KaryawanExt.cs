using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaPayroll.Core
{
    public class AdnKaryawanExt
    {
        public string Nip { get; set; }
        public string KdExt { get; set; }
        public string Flag { get; set; }

        public AdnKaryawanExt()
        {
            this.KdExt = "";
            this.Flag = "";
        }
    }
}
