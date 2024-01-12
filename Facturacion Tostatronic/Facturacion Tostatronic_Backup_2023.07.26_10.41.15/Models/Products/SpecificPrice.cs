using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class SpecificPrice : BaseNotifyPropertyChanged
    {
        private float quantity;

        public float Quantity
        {
            get { return quantity; }
            set { SetValue(ref quantity, value); }
        }
        private float price;

        public float Price
        {
            get { return price; }
            set { SetValue( ref price, value); }
        }

        public SpecificPrice()
        {
            
        }
        public SpecificPrice(float quantity, float price)
        {
            Quantity = quantity;
            Price = price;
        }
    }
}
