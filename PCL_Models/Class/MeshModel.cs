using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCL_Models.Class
{
    public class MeshModel
    {
        public int id { get; set; }

        public string name { get; set; }

        public byte[] mesh { get; set; }

        public int project_id { get; set; }
    }
}
