using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Services
{
    class Response
    {
        public bool succes { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}
