using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFSale
{
    public class EFExcelComissionsM
    {
        public int SaleID { get; set; }
        public float Comission { get; set; }
        public float Shipping { get; set; }
    }
}
