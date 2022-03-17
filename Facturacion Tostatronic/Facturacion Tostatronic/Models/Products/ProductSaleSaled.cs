using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class ProductSaleSaled : ProductBase
    {
        private float displayPrice;
        public float DisplayPrice
        {
            get { return displayPrice; }
            set 
            {
                if (displayPrice.Equals(value))
                    return;
                if(value<(MinimumPrice/1.17))
                {
                    throw new ValidationException($"El precio no puede ser menor al precio minimo Marcado: {(MinimumPrice).ToString("$0.00")}");
                }
                    
                SetValue(ref displayPrice, value); 
            }
        }
        private int saledQuantity;
        public int SaledQuantity
        {
            get { return saledQuantity; }
            set 
            {
                if (saledQuantity.Equals(value))
                    return;
                if (value <= 0)
                {
                    throw new ValidationException("La cantidad debe ser siempre mayor a 0");
                }

                SetValue(ref saledQuantity, value);
                Subtotal = saledQuantity * DisplayPrice;
            }
        }

        private float subtotal;

        public float Subtotal
        {
            get { return subtotal; }
            set { SetValue(ref subtotal, value); }
        }

        public ProductSaleSaled()
        {

        }
        public ProductSaleSaled(string code, string name, float existence, float minimumPrice,
            float distributorPrice, float publicPrice, float displayPrice, int saledQuantity) :
            base(code, name, existence, 0, 0, minimumPrice, distributorPrice, publicPrice, string.Empty)
        {
            DisplayPrice = displayPrice;
            SaledQuantity = saledQuantity;
        }
    }
}
