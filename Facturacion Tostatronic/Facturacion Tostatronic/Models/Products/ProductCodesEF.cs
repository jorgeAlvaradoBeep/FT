using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class ProductCodesEF
    {
        public string Referencia { get; set; }
        public string Upc { get; set; }
        public int Prestashop { get; set; }
        public string InternationalSku { get; set; }
    }
}
