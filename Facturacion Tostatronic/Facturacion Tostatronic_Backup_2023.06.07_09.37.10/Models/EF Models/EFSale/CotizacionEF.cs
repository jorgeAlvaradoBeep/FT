using Facturacion_Tostatronic.Models.EF_Models.EFClientF;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFSale
{
    public class CotizacionEF
    {
        public int idCotizacion { get; set; }
        public int idUsuario { get; set; }
        public int idCliente { get; set; }
        public string fechaCotizacion { get; set; }
        public float impuesto { get; set; }
        public EFClient idClienteNavigation { get; set; }
        public List<EFQuoteProduct> ProductosDeCotizacions { get; set; }
    }
}
