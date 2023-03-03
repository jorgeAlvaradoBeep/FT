using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class WooCommerceProduct
    {
        public string Sku { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Sale_price { get; set; }
        public string Regular_price { get; set; }
        public int Stock_quantity { get; set; }
        public List<WooCommerceProduct> Variantes { get; set; }
        public object tiered_pricing_fixed_rules { get; set; }
    }
}
