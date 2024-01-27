using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class ProductOrderComplete
    {
        public ProductOrderComplete()
        {
        }
        public ProductOrderComplete(string codigoProducto, int cantidad, float precio, float targetPrice)
        {
            CodigoProducto = codigoProducto;
            Cantidad = cantidad;
            Precio = precio;
            TargetPrice = targetPrice;
        }
        public ProductOrderComplete(string codigoProducto, int cantidad, float precio, float targetPrice, bool nuevo)
        {
            CodigoProducto = codigoProducto;
            Cantidad = cantidad;
            Precio = precio;
            TargetPrice = targetPrice;
            Nuevo = nuevo;
        }
        public int IDOrden { get; set; }
        public string CodigoProducto { get; set; }
        private int cantidad;

        public int Cantidad
        {
            get { return cantidad; }
            set 
            { 
                cantidad = value;
                Modificado = true;
            }
        }
        private float precio;

        public float Precio
        {
            get { return precio; }
            set { precio = value; Modificado = true; }
        }
        private float targetPrice;

        public float TargetPrice
        {
            get { return targetPrice; }
            set { targetPrice = value; Modificado = true; }
        }

        private string nombreEs;

        public string NombreEs
        {
            get { return nombreEs; }
            set { nombreEs = value; ModificadoProducto = true; }
        }
        private string nombreEn;

        public string NombreEn
        {
            get { return nombreEn; }
            set { nombreEn = value; ModificadoProducto = true; }
        }
        private string link;

        public string Link
        {
            get { return link; }
            set { link = value; ModificadoProducto = true; }
        }

        public bool Nuevo { get; set; } = false;
        public bool Modificado { get; set; } = false;
        public bool ProductInfoExist { get; set; } = false;
        public decimal SubTotal { get; set; }
        public decimal PrecioMXN { get; set; }
        public float PorcentajeOrden { get; set; }
        public decimal CostoEnvio { get; set; }
        public decimal CostoEnvioPP { get; set; }
        public decimal CostoAI { get; set; }
        public decimal Costo { get; set; }
        public decimal MinimoRecomendado { get; set; }
        public decimal SubMinimoRecomendado { get; set; }
        public decimal Minimo { get; set; }
        public float PorcentajeMinimo { get; set; }
        public decimal SubMinimo { get; set; }
        public decimal DistribuidorRecomendado { get; set; }
        public decimal Distribuidor { get; set; }
        public float PorcentajeDistribuidor { get; set; }
        public decimal SubDistribuidor { get; set; }
        public decimal PublicoRecomendado { get; set; }
        public decimal Publico { get; set; }
        public float PorcentajePublico { get; set; }
        public decimal SubPublico { get; set; }

        public bool ModificadoProducto { get; set; } = false;

    }
}
