using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class APIProductosOrdenes
    {
        public APIProductosOrdenes()
        {
            
        }
        public APIProductosOrdenes(int idOrden, string codigoProducto, int cantidad, float precio, float targetPrice)
        {
            IDOrden = idOrden;
            CodigoProducto = codigoProducto;
            Cantidad = cantidad;
            Precio = precio;
            TargetPrice = targetPrice;
        }
        public int IDOrden { get; set; }
        public string CodigoProducto { get; set; }
        public int Cantidad { get; set; }
        public float Precio { get; set; }
        public float TargetPrice { get; set; }
    }
}
