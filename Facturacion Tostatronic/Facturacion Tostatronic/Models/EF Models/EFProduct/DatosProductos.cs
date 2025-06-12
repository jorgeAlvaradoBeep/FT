using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFProduct
{
    public class DatosProductos
    {
        public string Codigo { get; set; }
        public string Link { get; set; }
        public decimal? Peso { get; set; }
        public decimal? PesoNeto { get; set; }
        public decimal? Largo { get; set; }
        public decimal? Ancho { get; set; }
        public decimal? Alto { get; set; }
        public decimal? LargoEm { get; set; }
        public decimal? AnchoEm { get; set; }
        public decimal? AltoEm { get; set; }

        // Si en tu modelo necesitas la referencia al Producto, puedes agregarla así:
         public EFProduct Producto { get; set; }
    }
}
