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
                decimal p;
                int q;
                decimal.TryParse(value, out p);
                p = Math.Round(p, 4);
                SetValue(ref _priceAtMoment, p.ToString());
                int.TryParse(quantity, out q);
                decimal sub = p * q;
                sub = Math.Round(sub, 4);
                SubTotal = sub.ToString();
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
