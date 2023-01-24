using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products.SucursalProduct
{
    public class SucursalProduct: BaseNotifyPropertyChanged
    {

        private ProductComplete product;

        public ProductComplete Product
        {
            get { return product; }
            set { SetValue(ref product, value); }
        }

        private int sucursalQuantity;

        public int SucursalQuantity
        {
            get { return sucursalQuantity; }
            set { SetValue(ref sucursalQuantity, value); }
        }

        private float sucursalPrice;

        public float SucursalPrice
        {
            get { return sucursalPrice; }
            set { SetValue(ref sucursalPrice, value); }
        }
        public SucursalProduct()
        {

        }
        public SucursalProduct(ProductComplete p)
        {
            Product = p;
        }
    }
}
