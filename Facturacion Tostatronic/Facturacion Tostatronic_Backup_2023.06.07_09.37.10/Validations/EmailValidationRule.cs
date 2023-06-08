using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Facturacion_Tostatronic.Validations
{
    public class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string email = value.ToString();
            try
            {
                if(string.IsNullOrWhiteSpace(email))
                {
                    return new ValidationResult(false, $"El email: {email} no es valido");
                }
                MailAddress m = new MailAddress(email);

                return new ValidationResult(true, null);
            }
            catch (FormatException)
            {
                return new ValidationResult(false, $"El email: {email} no es valido");
            }
        }
    }
}
