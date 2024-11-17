using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.EF_Models.EF_Orders
{
    public class OrderComplete: BaseNotifyPropertyChanged
    {
        public OrderComplete()
        {
            TipoCambio = 0;
            CostoEnvio = 0;
            CostoAA = 0;
            PorcentajeGanancia = 0;
            SubTotal = 0;
        }
        public int OrdenID { get; set; }
        public string FechaCreacion { get; set; }
        public string FechaFin { get; set; }
        private float? tipoCambio;

        public float? TipoCambio
        {
            get { return tipoCambio; }
            set 
            {
                SetValue(ref tipoCambio, value);
                GetSubTotalMxn();
            }
        }


        private float subtotal;

        public float? SubTotal
        {
            get { return subtotal; }
            set { SetValue(ref subtotal, (float)value); }
        }
        private float? costoEnvio;

        public float? CostoEnvio
        {
            get { return costoEnvio; }
            set 
            {
                SetValue(ref costoEnvio, value); 
                if(CostoEnvio == null || SubTotal == null)
                {
                    return;
                }
                TotalUSD = (decimal)(SubTotal + CostoEnvio);
                GetSubTotalMxn();
            }
        }

        private float? costoAA;

        public float? CostoAA
        {
            get { return costoAA; }
            set 
            { 
                SetValue(ref costoAA, value); 
                if (CostoAA == null || EnvioMx == null)
                {
                    return;
                }
                GastosEA = (decimal)CostoAA + EnvioMx;
                GetSubTotalMxn();
            }
        }

        private int? porcentajeGanancia;

        public int? PorcentajeGanancia
        {
            get { return porcentajeGanancia; }
            set { SetValue(ref porcentajeGanancia, value); }
        }

        public ObservableCollection<ProductOrderComplete> ProductosDeOrdenesNavigation { get; set; }

        #region Variables de Ejecucion
        private decimal totalUSD;

        public decimal TotalUSD
        {
            get { return totalUSD; }
            set { SetValue(ref totalUSD, value); }
        }
        private decimal gastoMaterialMx;

        public decimal GastoMaterialMx
        {
            get { return gastoMaterialMx; }
            set { SetValue(ref gastoMaterialMx, value); }
        }
        private decimal ivaMx;

        public decimal IVAMx
        {
            get { return ivaMx; }
            set { SetValue(ref ivaMx, value); }
        }
        private decimal envioMX;

        public decimal EnvioMx
        {
            get { return envioMX; }
            set 
            {
                SetValue(ref envioMX, value);
                GastosEA = (decimal)CostoAA + EnvioMx; 
            }
        }
        private decimal gastosEA;

        public decimal GastosEA
        {
            get { return gastosEA; }
            set { SetValue(ref gastosEA, value); }
        }

        private decimal totalGastosMx;

        public decimal TotalGastosMx
        {
            get { return totalGastosMx; }
            set { SetValue(ref totalGastosMx, value); }
        }
        void GetSubTotalMxn()
        {
            if(CostoAA == null || CostoEnvio == null || TipoCambio == null || SubTotal == null)
            {
                return;
            }
            EnvioMx = (decimal)(CostoEnvio * TipoCambio);
            GastoMaterialMx = TotalUSD * (decimal)TipoCambio;
            IVAMx = GastoMaterialMx * (decimal)0.16;
            TotalGastosMx = GastoMaterialMx + IVAMx + (decimal)CostoAA;
        }
        #endregion

        public void GetsubTotal()
        {
            if(ProductosDeOrdenesNavigation != null)
            {
                if(ProductosDeOrdenesNavigation.Count > 0)
                {
                    decimal sub = 0;
                    foreach(ProductOrderComplete prod in ProductosDeOrdenesNavigation)
                    {
                        prod.SubTotal = (decimal)(prod.Cantidad * prod.Precio);
                        sub += prod.SubTotal;
                    }
                    SubTotal = (float)sub;
                }
            }
        }
    }
}
