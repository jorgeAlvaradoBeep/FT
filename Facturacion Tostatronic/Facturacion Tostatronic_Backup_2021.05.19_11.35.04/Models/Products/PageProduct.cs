using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Products
{
    public class PageProduct
    {
        public string idProduct { get; set; }
        public string publico { get; set; }
        public string distribuidor { get; set; }
        public string minimo { get; set; }
        public string costo { get; set; }
        public float MinPrice { get; set; }
        public float PriceOne { get; set; }
        public float PriceTwo { get; set; }
        public float Pricetrhee { get; set; }
        public float PriceFour { get; set; }
    }
}
