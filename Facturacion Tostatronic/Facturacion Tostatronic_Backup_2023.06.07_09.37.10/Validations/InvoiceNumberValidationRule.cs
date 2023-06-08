using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Facturacion_Tostatronic.Validations
{
    public class InvoiceNumberValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string invoice = value.ToString();
            int invoiceN;
            if (!int.TryParse(invoice, out invoiceN))
                return new ValidationResult(false, $"El valor: {invoice} no es un valor valido");
            return new ValidationResult(true, null);
        }
    }
}
