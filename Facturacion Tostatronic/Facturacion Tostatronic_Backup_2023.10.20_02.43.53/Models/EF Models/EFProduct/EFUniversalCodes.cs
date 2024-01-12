using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFProduct
{
    public class EFUniversalCodes
    {
        public string Referencia { get; set; } 
        public string Upc { get; set; }
        public int Prestashop { get; set; }
        public string InternationalSku { get; set; }
    }
}
