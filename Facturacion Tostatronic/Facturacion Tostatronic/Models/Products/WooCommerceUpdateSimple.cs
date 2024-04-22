using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class WooCommerceUpdateSimple
    {

        public WooCommerceUpdateSimple()
        {
            tiered_pricing_fixed_rules = new Dictionary<int, decimal>();
        }
        public string regular_price { get; set; }
        public int stock_quantity { get; set; }
        public Dictionary<int, decimal> tiered_pricing_fixed_rules { get; set; }
    }
}
