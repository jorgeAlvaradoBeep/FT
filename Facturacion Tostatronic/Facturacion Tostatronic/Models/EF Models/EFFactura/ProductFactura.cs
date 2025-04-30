using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EFFactura
{
    public class ProductFactura : BaseNotifyPropertyChanged
    {
        public string idProduct { get; set; }
        public string name { get; set; }
        public int quantity { get; set; }
        private decimal _priceAtMoment;

        public decimal priceAtMoment
        {
            get { return _priceAtMoment; }
            set
            {
                SetValue(ref _priceAtMoment, value);
                SubTotal = Math.Round(value * quantity,2);
            }
        }
        private decimal subTotal;

        public decimal SubTotal
        {
            get { return subTotal; }
            set { SetValue(ref subTotal, value); }
        }

        public string satCode { get; set; }
    }
    public struct ProductoSat2
    {
        public string Descripcion, CodigoSAT;
        public decimal Precio, Subtotal;
        public int Cantidad;

        public ProductoSat2(string descripcion, string codigoSAT, int cantidad, decimal precio, decimal subtotal)
        {
            Descripcion = descripcion;
            Precio = precio;
            Cantidad = cantidad;
            CodigoSAT = codigoSAT;
            Subtotal = subtotal;
        }
    }
}
