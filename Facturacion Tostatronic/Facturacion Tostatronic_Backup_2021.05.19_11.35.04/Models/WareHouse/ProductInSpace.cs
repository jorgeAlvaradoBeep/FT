using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.WareHouse
{
    public class ProductInSpace: BaseNotifyPropertyChanged
    {
        public int IDSpace { get; set; }
        public ProductBase Product { get; set; }
    }
}
