using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFProduct
{
    public class EFProduct
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public int existencia { get; set; }
        public int cantidadMinima { get; set; }
        public double precioCompra { get; set; }
        public double precioPublico { get; set; }
        public double precioDistribuidor { get; set; }
        public double precioMinimo { get; set; }
        public bool eliminado { get; set; }
        public string imagen { get; set; }
    }
}
