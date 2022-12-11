using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Clients
{
    public class ClientOrder
    {
        public int IdVenta { get; set; }
        public int IdUsuario { get; set; }
        public int IdCliente { get; set; }
        public bool Pagada { get; set; }
        public string FechaDeVenta { get; set; }
        public bool Cancelada { get; set; }
        public float Impuesto { get; set; }
        public int Facturada { get; set; }

        public float Total
        {
            get 
            { 
                return SubTotal * Impuesto; 
            }
        }

        public float IVA
        {
            get 
            { 
                if(Impuesto==1)
                    return 0;
                return SubTotal * 0.16f;
            }
        }


        public float SubTotal { get; set; }
    }
}
