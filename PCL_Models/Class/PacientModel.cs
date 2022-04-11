using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCL_Models.Class
{
    public class PacientModel
    {
        public string id { get; set; }        

        public string CPF { get; set; }

        public string RG { get; set; }

        public string date { get; set; }

        public string name { get; set; }

        public string states { get; set; }

        public string cities { get; set; }

        public string address { get; set; }

        public string complement { get; set; }
        
        public string neighborhood { get; set; }

        public string CEP { get; set; }
            
        public string telephone { get; set; }

        public string cellPhone1 { get; set; }

        public string cellPhone2 { get; set; }

        public string email { get; set; }
    }
}
