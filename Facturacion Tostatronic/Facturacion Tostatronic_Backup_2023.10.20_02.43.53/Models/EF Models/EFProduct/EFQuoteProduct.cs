using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFProduct
{
    public class EFQuoteProduct
    {
        public string IdProducto { get; set; }
        public int CantidadCotizada { get; set; }
        public double PrecioAlMomento { get; set; }
        public EFProduct ProductoNavigation { get; set; }
    }
}
