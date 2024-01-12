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
    public class EmailsValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string emails = value.ToString();
            if (!Regex.Match(emails, @"^(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*;\s*|\s*$))+").Success)
                return new ValidationResult(false, $"Alguno de los correos es incorrecto");
            return new ValidationResult(true, null);
        }
    }
}
