using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.WooCommerceModels
{
    public class CRUDActionClass
    {
        public List<Create> create { get; set; }
        public List<Update> update { get; set; }
        public List<Delete> delete { get; set; }
    }
}
