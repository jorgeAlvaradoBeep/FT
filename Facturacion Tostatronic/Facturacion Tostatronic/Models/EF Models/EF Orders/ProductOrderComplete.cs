using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class ProductOrderComplete : BaseNotifyPropertyChanged
    {
        public ProductOrderComplete()
        {
            Modificado = false;
            ModificadoProducto = false;
        }
        public ProductOrderComplete(bool nuevo)
        {
            Modificado = false;
            ModificadoProducto = false;
            Nuevo = nuevo;
        }
        public ProductOrderComplete(string codigoProducto, int cantidad, float precio, float targetPrice)
        {
            CodigoProducto = codigoProducto;
            Cantidad = cantidad;
            Precio = precio;
            TargetPrice = targetPrice;
            Modificado=false; 
            ModificadoProducto=false;
            ProductInfoExist=false;
        }
        public ProductOrderComplete(string codigoProducto, int cantidad, float precio, float targetPrice, bool nuevo)
        {
            CodigoProducto = codigoProducto;
            Cantidad = cantidad;
            Precio = precio;
            TargetPrice = targetPrice;
            Nuevo = nuevo;
            Modificado = false; 
            ModificadoProducto = false;
            ProductInfoExist = false;
        }
        public int IDOrden { get; set; }
        public string CodigoProducto { get; set; }
        private int cantidad;

        public int Cantidad
        {
            get { return cantidad; }
            set 
            { 
                SetValue(ref cantidad, value);
                SubTotal = (decimal)(Precio * Cantidad);
                Modificado = true;
            }
        }
        private float precio;

        public float Precio
        {
            get { return precio; }
            set 
            {
                SetValue(ref precio, value);
                SubTotal = (decimal)(Precio * Cantidad);
                Modificado = true;
            }
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

        public bool Nuevo { get; set; }
        private bool modificado;

        public bool Modificado
        {
            get { return modificado; }
            set { SetValue(ref modificado, value); }
        }

        public bool ProductInfoExist { get; set; } = false;
        private decimal subTotal;

        public decimal SubTotal
        {
            get { return subTotal; }
            set { SetValue(ref subTotal, value); }
        }

        public decimal PrecioMXN { get; set; }
        public float PorcentajeOrden { get; set; }
        public decimal CostoEnvio { get; set; }
        private decimal costoEnvioPP;

        public decimal CostoEnvioPP
        {
            get { return costoEnvioPP; }
            set { SetValue(ref costoEnvioPP, value); }
        }

        public decimal CostoAI { get; set; }
        private decimal costo;

        public decimal Costo
        {
            get { return costo; }
            set { SetValue(ref costo, value); }
        }

        public decimal CostoActual { get; set; }
        public decimal MinimoRecomendado { get; set; }
        public decimal MinimoActual { get; set; }
        private bool minimoMenor;
        public bool MinimoMenor => MinimoActual < MinimoRecomendado;
        public bool CostoMenor => CostoActual < Costo;

        public decimal SubMinimoRecomendado { get; set; }
        public decimal Minimo { get; set; }
        public float PorcentajeMinimo { get; set; }
        public decimal SubMinimo { get; set; }
        public decimal DistribuidorRecomendado { get; set; }
        public decimal DistribuidorActual { get; set; }
        public decimal Distribuidor { get; set; }
        public float PorcentajeDistribuidor { get; set; }
        public decimal SubDistribuidor { get; set; }
        public decimal PublicoRecomendado { get; set; }
        public decimal PublicoActual { get; set; }
        public decimal Publico { get; set; }
        public float PorcentajePublico { get; set; }
        public decimal SubPublico { get; set; }
        private bool modificadoProducto;

        public bool ModificadoProducto
        {
            get { return modificadoProducto; }
            set { SetValue(ref modificadoProducto, value); }
        }

        #region TargetPrice
        private decimal subTarget;

        public decimal SubTarget
        {
            get { return subTarget; }
            set { SetValue(ref subTarget, value); }
        }
        private decimal precioMxnTarget;

        public decimal PrecioMXNTarget
        {
            get { return precioMxnTarget; }
            set { SetValue(ref precioMxnTarget, value); }
        }

        private float porcentajeTarget;

        public float PorcentajeTarget
        {
            get { return porcentajeTarget; }
            set { SetValue(ref porcentajeTarget, value); }
        }
        private decimal costoEnvioTarget;

        public decimal CostoEnvioTarget
        {
            get { return costoEnvioTarget; }
            set { SetValue(ref costoEnvioTarget, value); }
        }
        private decimal costoPPTarget;

        public decimal CostoPPTaerget
        {
            get { return costoPPTarget; }
            set { SetValue(ref costoPPTarget, value); }
        }
        private decimal costoTarget;

        public decimal CostoTarget
        {
            get { return costoTarget; }
            set { SetValue(ref costoTarget, value);
                CostoMenorTarget = CostoActual < costoTarget;
            }
        }
        private decimal miniRecomendadoTarget;

        public decimal MinimoRecomendadoTarget
        {
            get { return miniRecomendadoTarget; }
            set { SetValue(ref miniRecomendadoTarget, value);
                BMinimoRecomendadoTarget = MinimoActual < MinimoRecomendadoTarget;
            }
        }
        private bool bMiniRecomendadoTarget;

        public bool BMinimoRecomendadoTarget
        {
            get { return bMiniRecomendadoTarget; }
            set { SetValue(ref bMiniRecomendadoTarget, value); }
        }

        private bool costoMenorTarget;

        public bool CostoMenorTarget
        {
            get { return costoMenorTarget; }
            set { SetValue(ref costoMenorTarget, value); }
        }
        #endregion
    }
}
