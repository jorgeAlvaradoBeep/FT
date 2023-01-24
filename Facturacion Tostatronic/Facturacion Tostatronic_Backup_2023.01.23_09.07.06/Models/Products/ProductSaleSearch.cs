using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class ProductSaleSearch : ProductBase
    {
        private float displayPrice;

        public float DisplayPrice
        {
            get { return displayPrice; }
            set { SetValue(ref displayPrice, value); }
        }
    }
}
