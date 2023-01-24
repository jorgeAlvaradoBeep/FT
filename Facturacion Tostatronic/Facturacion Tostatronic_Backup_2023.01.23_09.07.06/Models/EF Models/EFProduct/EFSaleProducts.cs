using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFProduct
{
    public class EFSaleProducts
    {
        public string idProducto { get; set; }
        public int idVentaPV { get; set; }
        public int cantidadComprada { get; set; }
        public double precioAlMomento { get; set; }
        public int descuento { get; set; }
        public EFProduct productoNavigation { get; set; }
    }
}
