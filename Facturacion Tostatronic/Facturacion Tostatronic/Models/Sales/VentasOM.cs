using System;
using System.Collections.Generic;
using Facturacion_Tostatronic.Services;

namespace Facturacion_Tostatronic.Models.Sales
{
    public class VentasOM
    {
        public int IDSale { get; set; }
        public Cliente ClientSale { get; set; }
        public int PriceType { get; set; }
        public List<ProductoDeVentaOM> SaledProducts { get; set; }
        public bool NeedFactura { get; set; }
        public string FechaDeVenta { get; set; }
        public int SalerID { get; set; }

        public VentasOM()
        {
            IDSale = 1;
            FechaDeVenta = "";
            SaledProducts = new List<ProductoDeVentaOM>();
        }
    }

    public class Cliente
    {
        public int IdCliente { get; set; }
        public int IdTipoCliente { get; set; }
        public string Nombres { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Rfc { get; set; }
        public string Telefono { get; set; }
        public string Domicilio { get; set; }
        public int CodigoPostal { get; set; }
        public string Colonia { get; set; }
        public string CorreoElectronico { get; set; }
        public string Celular { get; set; }
        public string Descripcion { get; set; }
        public string RegimenFiscal { get; set; }
        public bool Eliminado { get; set; }

        public Cliente()
        {
            Nombres = "";
            ApellidoPaterno = "";
            ApellidoMaterno = "";
            Rfc = "";
            Telefono = "";
            Domicilio = "";
            Colonia = "";
            CorreoElectronico = "";
            Celular = "";
            Descripcion = "";
            RegimenFiscal = "";
        }
    }

    public class ProductoDeVentaOM
    {
        public string IdProducto { get; set; }
        public float PrecioAlMomento { get; set; }
        public float CantidadComprada { get; set; }
        public float Descuento { get; set; }
    }
}