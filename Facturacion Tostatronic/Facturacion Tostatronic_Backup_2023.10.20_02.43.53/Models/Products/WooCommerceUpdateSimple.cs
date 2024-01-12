using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class WooCommerceUpdateSimple
    {
        public string regular_price { get; set; }
        public int stock_quantity { get; set; }
        public string tiered_pricing_fixed_rules { get; set; }
    }
}
