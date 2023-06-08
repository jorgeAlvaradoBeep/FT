using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Facturacion_Tostatronic.Validations
{
    public class CPValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string cp = value.ToString();
            try
            {
                if (string.IsNullOrWhiteSpace(cp))
                {
                    return new ValidationResult(false, $"El CP: {cp} no puede estar vacio o con espacios");
                }
               if(cp.Length!=5)
                {
                    return new ValidationResult(false, $"El CP: {cp} debe contener 5 digitos");
                }
                int invoiceN;
                if (!int.TryParse(cp, out invoiceN))
                    return new ValidationResult(false, $"El valor: {cp} no es un valor valido");
                return new ValidationResult(true, null);
            }
            catch (Exception e)
            {
                return new ValidationResult(false, $"El CP: {cp} no es valido");
            }
        }
    }
}
