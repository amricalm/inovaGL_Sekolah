using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace inovaGL.Data
{
    public class AdnDept : AdnBaseClass
    {
        public string KdDept { get; set; }
        public string NmDept { get; set; }

        public AdnDept()
        {
            this.KdDept = "";
            this.NmDept= "";
        }
    }

}
