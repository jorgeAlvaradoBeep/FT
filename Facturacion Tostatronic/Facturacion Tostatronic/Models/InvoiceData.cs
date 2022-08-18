using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models
{
    public class InvoiceData
    {
        public string UsoCFDI { get; set; }
        public string MetodoDePago { get; set; }
        public string FormaDePago { get; set; }
        public string RegimenFiscal { get; set; }
    }
}
