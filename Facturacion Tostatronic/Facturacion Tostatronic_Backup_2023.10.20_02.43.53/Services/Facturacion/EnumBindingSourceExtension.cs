using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace Facturacion_Tostatronic.Services.Facturacion
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        public Type EnumType { get; set; }
        public EnumBindingSourceExtension(Type enumType)
        {
            if (enumType is null || !enumType.IsEnum)
                throw new Exception("El enum dado no debe ser nulo");
            EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Enum.GetValues(EnumType);
        }
    }
}
