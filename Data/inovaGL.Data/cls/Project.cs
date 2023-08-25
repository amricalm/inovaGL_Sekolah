using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inovaGL
{
    public class AdnProject
    {
        private string _kd_project;
        private string _nm_project;

        public string kd_project
        {
            get { return _kd_project; }
            set { _kd_project = value; }
        }
        public string nm_project
        {
            get { return _nm_project; }
            set { _nm_project = value; }
        }

        public override string ToString()
        {
            return this.nm_project;
        }
    
    }
}
