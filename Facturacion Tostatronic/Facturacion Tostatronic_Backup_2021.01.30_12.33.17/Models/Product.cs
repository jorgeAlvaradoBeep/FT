using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models
{
    public class Product : BaseNotifyPropertyChanged
    {
        public string idProduct { get; set; }
        public string name { get; set; }
        public string quantity { get; set; }
        private string _priceAtMoment;

        public string priceAtMoment
        {
            get { return _priceAtMoment; }
            set 
            {  
                SetValue(ref _priceAtMoment ,value);
                float p,q;
                float.TryParse(priceAtMoment, out p);
                float.TryParse(quantity, out q);
                SubTotal = (p * q).ToString();
            }
        }
        private string subTotal;

        public string SubTotal
        {
            get { return subTotal; }
            set { SetValue( ref subTotal, value); }
        }

        public string satCode { get; set; }
    }
    public struct ProductoSat
    {
        public string Descripcion, CodigoSAT;
        public float Cantidad, Precio, Subtotal;

        public ProductoSat(string descripcion, string codigoSAT, float cantidad, float precio, float subtotal)
        {
            Descripcion = descripcion;
            Precio = precio;
            Cantidad = cantidad;
            CodigoSAT = codigoSAT;
            Subtotal = subtotal;
        }
    }
}
