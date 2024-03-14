using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class OrderComplete: BaseNotifyPropertyChanged
    {
        public int OrdenID { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaFin { get; set; }
        public float? TipoCambio { get; set; }

        private float subtotal;

        public float? SubTotal
        {
            get { return subtotal; }
            set { SetValue(ref subtotal, (float)value); }
        }
        public float? CostoEnvio { get; set; }
        public float? CostoAA { get; set; }
        public int? PorcentajeGanancia { get; set; }
        public ObservableCollection<ProductOrderComplete> ProductosDeOrdenesNavigation { get; set; }

        public void GetsubTotal()
        {
            if(ProductosDeOrdenesNavigation != null)
            {
                if(ProductosDeOrdenesNavigation.Count > 0)
                {
                    decimal sub = 0;
                    foreach(ProductOrderComplete prod in ProductosDeOrdenesNavigation)
                    {
                        prod.SubTotal = (decimal)(prod.Cantidad * prod.Precio);
                        sub += prod.SubTotal;
                    }
                    SubTotal = (float)sub;
                }
            }
        }
    }
}
