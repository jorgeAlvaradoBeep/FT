using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.CFDI
{
    public class PaymentMethod
    {
        public string PaymentMethodP { get; set; }
        private string description;

        public string Description
        {
            get { return description; }
            set 
            { 
                description = value;
                CompleteName = PaymentMethodP + " - " + Description;
            }
        }
        public string CompleteName { get; set; }
    }
}
