using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Facturacion_Tostatronic.Validations
{
    public class RFCValidationRule : System.Windows.Controls.ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string rfc = value.ToString();
            if (!Regex.Match(rfc, @"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$").Success)
                return new ValidationResult(false, $"El rfc: {rfc} no es valido");
            return new ValidationResult(true, null);
        }
    }
}
