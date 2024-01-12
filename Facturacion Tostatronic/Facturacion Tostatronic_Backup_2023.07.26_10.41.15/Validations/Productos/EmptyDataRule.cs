using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Facturacion_Tostatronic.Validations.Productos
{
    public class EmptyDataRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string data = value.ToString();
            if(string.IsNullOrEmpty(data))
                return new ValidationResult(false, "El Campo no puede ser vacio");
            return new ValidationResult(true, null);
        }
    }
}
