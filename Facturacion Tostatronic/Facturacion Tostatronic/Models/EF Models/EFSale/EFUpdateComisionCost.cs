using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFSale
{
    public class EFUpdateComisionCost
    {
        public int Id { get; set; }
        public int VentaId { get; set; }
        public decimal CostoMercancia { get; set; }
    }
}
