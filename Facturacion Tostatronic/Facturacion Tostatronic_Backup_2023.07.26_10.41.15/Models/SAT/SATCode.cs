using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.SAT
{
    public class SATCode
    {
        public int Id { get; set; }
        public string CClaveProdServ { get; set; }
        public string Descripción { get; set; }

        public string CompleteName
        {
            get { return ($"{CClaveProdServ} - {Descripción}"); }
        }

    }
}
