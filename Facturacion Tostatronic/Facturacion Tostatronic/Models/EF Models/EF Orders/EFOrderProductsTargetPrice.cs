using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class EFOrderProductsTargetPrice
    {
        public EFOrderProductsTargetPrice()
        {
            
        }
        public EFOrderProductsTargetPrice(int idOrden,string codigo, float targetPrice)
        {
            IDOrden = idOrden;
            TargetPrice = targetPrice;
            CodigoProducto = codigo;
        }
        public int IDOrden { get; set; }
        public float TargetPrice { get; set; }
        public string CodigoProducto { get; set; }
    }
}
