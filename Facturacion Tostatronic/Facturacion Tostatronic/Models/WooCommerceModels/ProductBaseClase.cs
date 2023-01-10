using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.WooCommerceModels
{
    public class ProductBaseClase
    {
        public int id { get; set; }
        public string sale_price { get; set; }
        public string regular_price { get; set; }
        public int stock_quantity { get; set; }
    }
}
