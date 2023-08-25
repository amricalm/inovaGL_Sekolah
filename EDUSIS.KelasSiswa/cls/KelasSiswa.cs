using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Andhana;

namespace EDUSIS.KelasSiswa
{
    public class AdnKelasSiswa : AdnBaseClass
    {
        public string ThAjar { get; set; }
        public string KdSekolah { get; set; }
        public string Kelas { get; set; }
        public string NIS { get; set; }
        public EDUSIS.Kelas.AdnKelas oKelas { get; set; }
        public EDUSIS.Siswa.AdnSiswa oSiswa { get; set; }
    }
}
