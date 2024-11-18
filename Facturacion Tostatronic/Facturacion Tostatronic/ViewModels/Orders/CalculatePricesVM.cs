using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Orders
{
    public class CalculatePricesVM : BaseNotifyPropertyChanged
    {
        #region Propiedades
        private OrderComplete orderComplete;

        public OrderComplete OrdenComplete
        {
            get { return orderComplete; }
            set { SetValue(ref orderComplete, value); }
        }
        private bool gettinData;

        public bool GettingData
        {
            get { return gettinData; }
            set { SetValue(ref gettinData, value); }
        }

        private decimal gastosEA;

        public decimal GastosEA
        {
            get { return gastosEA; }
            set { SetValue(ref gastosEA, value); }
        }
        private decimal totalPorcentaje;

        public decimal TotalPorcentaje
        {
            get { return totalPorcentaje; }
            set { SetValue(ref totalPorcentaje, value); }
        }
        public IReadOnlyList<UpdateProductM> _allProducts;

        private decimal subTotalTarget;

        public decimal SubTotalTarget
        {
            get { return subTotalTarget; }
            set { SetValue(ref subTotalTarget, value); }
        }

        private float porcentageTarget;

        public float PorcentageTarget
        {
            get { return porcentageTarget; }
            set { SetValue(ref porcentageTarget, value); }
        }

        #endregion
        #region Comandos
        public ICommand UpdateTPCommand { get; }
        #endregion

        public CalculatePricesVM()
        {
            if(Application.Current.Properties["CalculatePrices"] != null)
            {
                OrdenComplete = (OrderComplete)Application.Current.Properties["CalculatePrices"];
                _allProducts = (IReadOnlyList<UpdateProductM>)Application.Current.Properties["AllProducts"];
                Application.Current.Properties["CalculatePrices"]=null;
                Application.Current.Properties["AllProducts"] = null;
                GastosEA = 0;
                TotalPorcentaje = 0;
                Task.Run(() => SetProducsInfo());
                UpdateTPCommand = new RelayCommand<ProductOrderComplete>(UpdateTP);
            }
            else
            {
                OrdenComplete = new OrderComplete();
            }
        }
        private async void UpdateTP(ProductOrderComplete editedItem)
        {
            var aux = OrdenComplete.ProductosDeOrdenesNavigation.FirstOrDefault(p => p.CodigoProducto == editedItem.CodigoProducto);
            aux.TargetPrice = editedItem.TargetPrice;
            aux.SubTarget = (decimal)aux.TargetPrice*aux.Cantidad;
            GettingData = true;
            Task.Run(async () => await GetSubTotalMxn());
            GettingData = false;
        }

        private async Task GetSubTotalMxn()
        {
            decimal newSubtotal=0;
            float newPorcentaje = 0;
            foreach (var item in OrdenComplete.ProductosDeOrdenesNavigation)
            {
                newSubtotal += item.SubTarget;
                newPorcentaje += item.PorcentajeTarget;
            }
            foreach (var item in OrdenComplete.ProductosDeOrdenesNavigation)
            {
                item.PorcentajeTarget = (float)item.SubTarget / (float)newSubtotal;
                item.PrecioMXNTarget = (decimal)(item.TargetPrice * OrdenComplete.TipoCambio);
                item.CostoEnvioTarget = (decimal)item.PorcentajeTarget * OrdenComplete.GastosEA;   
                item.CostoPPTaerget = item.CostoEnvioTarget / item.Cantidad;
                item.CostoTarget = (item.CostoPPTaerget + item.PrecioMXNTarget)*1.16m;
            }
            SubTotalTarget = newSubtotal;
            PorcentageTarget = newPorcentaje;
        }

        void SetProducsInfo()
        {
            if (OrdenComplete.ProductosDeOrdenesNavigation == null)
            {
                return;
            }
            decimal porWininw = (decimal)orderComplete.PorcentajeGanancia / 100m;
            foreach (ProductOrderComplete product in OrdenComplete.ProductosDeOrdenesNavigation)
            {
                product.PrecioMXN = (decimal)(product.Precio*OrdenComplete.TipoCambio);
                product.PorcentajeOrden = (float)((float)product.SubTotal / orderComplete.SubTotal);
                product.CostoEnvio = (decimal)product.PorcentajeOrden * orderComplete.GastosEA;
                product.CostoEnvioPP =  product.CostoEnvio/product.Cantidad;
                product.CostoAI = product.PrecioMXN+product.CostoEnvioPP;
                product.Costo = product.CostoAI * (decimal)1.16;
                product.MinimoRecomendado = (product.Costo / (1 - porWininw));
                product.DistribuidorRecomendado = ((product.MinimoRecomendado+4)/ (1-0.040484m));
                product.PublicoRecomendado = product.DistribuidorRecomendado/ (1-0.15m);
                //En esta seccion validamos si el producto tiene un historial de precios
                if (_allProducts != null)
                {
                    if (_allProducts.Count > 0)
                    {
                        UpdateProductM productM = _allProducts.FirstOrDefault(p => p.Codigo == product.CodigoProducto);
                        if (productM != null)
                        {
                            product.MinimoActual = (decimal)productM.PrecioMinimo;
                            product.DistribuidorActual = (decimal)productM.PrecioDistribuidor;
                            product.PublicoActual = (decimal)productM.PrecioPublico;
                            product.CostoActual = (decimal)productM.PrecioCompra;
                            product.TargetPrice = product.Precio;
                            product.SubTarget = product.SubTotal;
                            product.PrecioMXNTarget = product.PrecioMXN;
                            product.PorcentajeTarget = product.PorcentajeOrden;
                            product.CostoEnvioTarget = product.CostoEnvio;  
                            product.CostoPPTaerget = product.CostoEnvioPP;
                            product.CostoTarget = product.Costo;
                            product.MinimoRecomendadoTarget = product.MinimoRecomendado;
                        }
                    }
                }
                else
                {
                    product.MinimoActual = 0;
                    product.DistribuidorActual = 0;
                    product.PublicoActual = 0;
                }
                GastosEA += product.CostoEnvio;
                TotalPorcentaje += (decimal)product.PorcentajeOrden; 
            }
        }

    }
}
