using System;

namespace Facturacion_Tostatronic.Models.Sales
{
    public class ProductosComplejos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int IdCotizacion { get; set; }
        public string Sku { get; set; }
        public int? IdPlataforma { get; set; }
        public string CodigoPlataforma { get; set; }

        public ProductosComplejos()
        {
            Nombre = "";
            Sku = "";
            CodigoPlataforma = "";
        }
    }
}