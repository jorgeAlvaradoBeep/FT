using Facturacion_Tostatronic.Models.EF_Models.EFClientF;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFSale
{
    public class CompleteSaleWProductsEF
    {
        public int idVenta { get; set; }
        public int idUsuario { get; set; }
        public int idCliente { get; set; }
        public bool pagada { get; set; }
        public string fechaDeVenta { get; set; }
        public bool cancelada { get; set; }
        public double impuesto { get; set; }
        public int facturada { get; set; }
        public float subTotal { get; set; }
        public float iva { get; set; }
        public float total { get; set; }
        public List<EFSaleProducts> productosDeVenta { get; set; }
        public EFClient idClienteNavigation { get; set; }
    }
}
