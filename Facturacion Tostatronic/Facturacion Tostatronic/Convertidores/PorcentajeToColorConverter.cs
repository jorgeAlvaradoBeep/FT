using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Facturacion_Tostatronic.Convertidores
{
    public class PorcentajeToColorConverter : IValueConverter
    {
        // value: el valor que se está enlazando (TotalPorcentaje)
        // targetType: el tipo de la propiedad destino (Brush)
        // parameter, culture: no usados generalmente
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal porcentaje)
            {
                // 1.0 == 100%   y   1.002 == 100.20%
                if (porcentaje >= 1.0m && porcentaje <= 1.002m)
                {
                    return Brushes.Green;
                }
                else
                {
                    return Brushes.Red;
                }
            }

            // Si no es double, por seguridad retornamos un color por defecto
            return Brushes.Black;
        }

        // Solo necesario si piensas soportar la conversión inversa (no suele ser el caso)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
