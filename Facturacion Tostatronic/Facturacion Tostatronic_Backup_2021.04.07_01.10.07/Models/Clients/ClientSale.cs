using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Clients
{
    public class ClientSale
    {
        public int ID { get; set; }
        public int ClientType { get; set; }
        public string RFC { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
    }
}
