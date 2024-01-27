using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class OrderComplete
    {
        public int OrdenID { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaFin { get; set; }
        public float? TipoCambio { get; set; }
        public float? SubTotal { get; set; }
        public float? CostoEnvio { get; set; }
        public float? CostoAA { get; set; }
        public int? PorcentajeGanancia { get; set; }
        public ObservableCollection<ProductOrderComplete> ProductosDeOrdenesNavigation { get; set; }
    }
}
