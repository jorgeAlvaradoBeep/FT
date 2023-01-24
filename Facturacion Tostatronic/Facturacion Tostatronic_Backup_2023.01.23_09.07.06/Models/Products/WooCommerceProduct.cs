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
        public float Sale_price { get; set; }
        public int Stock_quantity { get; set; }
        public List<WooCommerceProduct> Variantes { get; set; }
    }
}
