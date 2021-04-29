using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.CFDI
{
    public class CFDIUse
    {
        public string CFDIUseP { get; set; }
        private string description;
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                CompleteName = CFDIUseP + " - " + Description;
            }
        }
        public string CompleteName { get; set; }
    }
}
