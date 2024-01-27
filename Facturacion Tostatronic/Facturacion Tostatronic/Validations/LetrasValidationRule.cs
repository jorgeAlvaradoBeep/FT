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
    public class LetrasValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || !Regex.IsMatch(value.ToString(), @"^[a-zA-Z]{1,2}$"))
            {
                return new ValidationResult(false, "Ingresa de 1 a 2 letras (sin números).");
            }
            return ValidationResult.ValidResult;
        }
    }
}
