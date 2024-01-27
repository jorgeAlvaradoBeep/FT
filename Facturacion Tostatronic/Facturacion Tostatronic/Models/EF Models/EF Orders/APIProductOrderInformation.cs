using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class APIProductOrderInformation
    {
        public APIProductOrderInformation()
        {
            
        }
        public APIProductOrderInformation(string codigoProducto, string nombreEs, string nombreEn, string link)
        {
            CodigoProducto = codigoProducto;
            NombreEs = nombreEs;    
            Link = link;
            NombreEn = nombreEn;
        }
        public string CodigoProducto { get; set; }
        public string NombreEs { get; set; }
        public string NombreEn { get; set; }
        public string Link { get; set; }
    }
}
