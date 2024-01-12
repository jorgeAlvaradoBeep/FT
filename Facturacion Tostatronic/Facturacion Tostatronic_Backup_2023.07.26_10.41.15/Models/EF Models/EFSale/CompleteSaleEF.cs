using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Facturacion_Tostatronic.Models.EF_Models.EFClientF;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;

namespace Facturacion_Tostatronic.Models.EF_Models.EFSale
{
    public class CompleteSaleEF
    {
        public int idVenta { get; set; }
        public int idUsuario { get; set; }
        public int idCliente { get; set; }
        public bool pagada { get; set; }
        public string fechaDeVenta { get; set; }
        public bool cancelada { get; set; }
        public double impuesto { get; set; }
        public int facturada { get; set; }
        public int subTotal { get; set; }
        public int iva { get; set; }
        public int total { get; set; }
        public List<EFProduct.EFProduct> ProductosDeVenta { get; set; }
        public EFClient idClienteNavigation { get; set; }
    }
}
