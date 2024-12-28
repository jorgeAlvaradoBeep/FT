using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.CheckPricesCommands;
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
        private ProductOrderComplete selectedItem;

        public ProductOrderComplete SelectedItem
        {
            get { return selectedItem; }
            set { SetValue(ref selectedItem, value); }
        }

        #endregion
        #region Comandos
        public ICommand UpdateTPCommand { get; }
        public SaverTargetPricesOrderCommand SaverTargetPricesOrderCommand { get; set; }
        public StablishOrderCommand StablishOrderCommand { get; set; }

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
                UpdateTPCommand = new RelayCommand<ProductOrderComplete>(UpdateTP);
                SaverTargetPricesOrderCommand = new SaverTargetPricesOrderCommand(this);
                StablishOrderCommand = new StablishOrderCommand(this);
            }
            else
            {
                OrdenComplete = new OrderComplete();
            }
        }
        public async Task SetOrder()
        {
            await Task.Run(() => SetProducsInfo());
            await Task.Run(async() => await SetNewPorcentajes());
            //Ahora verificamos y extraemos los porcentajes ya existentes
            
        }
        private async void UpdateTP(ProductOrderComplete editedItem)
        {
            var aux = OrdenComplete.ProductosDeOrdenesNavigation.FirstOrDefault(p => p.CodigoProducto == editedItem.CodigoProducto);
            aux.TargetPrice = editedItem.TargetPrice;
            aux.SubTarget = (decimal)aux.TargetPrice*aux.Cantidad;
            aux.PorcentajeOrden = editedItem.PorcentajeOrden;
            GettingData = true;
            Task.Run(async () => await GetSubTotalMxn());
            GettingData = false;
        }
        private async Task SetNewPorcentajes()
        {
            Response response;
            response = await WebService.GetDataNode(URLData.GetEstablecerOrden, OrdenComplete.OrdenID.ToString());
            if(response.succes)
            {
                List<DetalleOrdenEF> products = JsonConvert.DeserializeObject<List<DetalleOrdenEF>>(response.data.ToString());
                foreach (ProductOrderComplete product in OrdenComplete.ProductosDeOrdenesNavigation)
                {
                    DetalleOrdenEF aux = products.FirstOrDefault(p => p.codigoProducto == product.CodigoProducto);
                    if (aux != null)
                    {
                        product.PorcentajeOrden = (float)aux.porcentaje;
                        decimal newCostoEnvio = OrdenComplete.GastosEA * aux.porcentaje;
                        product.CostoEnvio = newCostoEnvio;
                        newCostoEnvio = newCostoEnvio / product.Cantidad;
                        product.CostoEnvioPP = newCostoEnvio;
                        product.CostoAI = product.PrecioMXN + product.CostoEnvioPP;
                        product.Costo = product.CostoAI * 1.16m;
                        decimal porWininw = OrdenComplete.PorcentajeGanancia / 100m;
                        product.MinimoRecomendado = (product.Costo / (1 - porWininw));
                        product.Minimo = aux.minimo;
                        product.Distribuidor = aux.distribuidor;
                        product.Publico = aux.publico;
                        product.DistribuidorRecomendado = ((product.MinimoRecomendado + 4) / (1 - 0.040484m));
                        product.PublicoRecomendado = product.DistribuidorRecomendado / (1 - 0.15m);
                    }
                }
                float newPercentage = 0;
                foreach (var item in OrdenComplete.ProductosDeOrdenesNavigation)
                {
                    newPercentage += item.PorcentajeOrden;
                }
                TotalPorcentaje = (decimal)newPercentage;
            }
        }

        private async Task GetSubTotalMxn()
        {
            decimal newSubtotal=0;
            float newPorcentaje = 0;
            float newPorcentajeOrden = 0;
            foreach (var item in OrdenComplete.ProductosDeOrdenesNavigation)
            {
                newSubtotal += item.SubTarget;
                newPorcentaje += item.PorcentajeTarget;
                newPorcentajeOrden += item.PorcentajeOrden;
            }
            foreach (var item in OrdenComplete.ProductosDeOrdenesNavigation)
            {
                item.PorcentajeTarget = (float)item.SubTarget / (float)newSubtotal;
                item.PrecioMXNTarget = (decimal)(item.TargetPrice * OrdenComplete.TipoCambio);
                item.CostoEnvioTarget = (decimal)item.PorcentajeTarget * OrdenComplete.GastosEA;   
                item.CostoEnvio = (decimal)item.PorcentajeOrden * OrdenComplete.GastosEA;   
                item.CostoPPTaerget = item.CostoEnvioTarget / item.Cantidad;
                item.CostoEnvioPP = item.CostoEnvio / item.Cantidad;
                item.CostoTarget = (item.CostoPPTaerget + item.PrecioMXNTarget)*1.16m;
                item.Costo = (item.CostoEnvioPP + item.PrecioMXN)*1.16m;
                float porWinDe = (float)(OrdenComplete.PorcentajeGanancia) / 100;
                item.MinimoRecomendadoTarget = (item.CostoTarget / (1 - (decimal)porWinDe));
                item.MinimoRecomendado = (item.Costo / (1 - (decimal)porWinDe));
            }
            SubTotalTarget = newSubtotal;
            PorcentageTarget = newPorcentaje;
            TotalPorcentaje = (decimal)newPorcentajeOrden;
        }

        void SetProducsInfo()
        {
            SubTotalTarget = 0;
            PorcentageTarget = 0;
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
                if (product.TargetPrice == 0)
                {
                    product.TargetPrice = product.Precio;
                    product.SubTarget = product.SubTotal;
                    product.PrecioMXNTarget = product.PrecioMXN;
                    product.PorcentajeTarget = product.PorcentajeOrden;
                    product.CostoEnvioTarget = product.CostoEnvio;
                    product.CostoPPTaerget = product.CostoEnvioPP;
                    product.CostoTarget = product.Costo;
                    product.MinimoRecomendadoTarget = product.MinimoRecomendado;
                }
                else
                {
                    product.SubTarget = (decimal)(product.Cantidad * product.TargetPrice);
                    product.PrecioMXNTarget = (decimal)(product.TargetPrice * OrdenComplete.TipoCambio);
                }
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
                        }
                        else
                        {
                            product.MinimoActual = 0;
                            product.DistribuidorActual = 0;
                            product.PublicoActual = 0;
                        }
                    }
                    else
                    {
                        product.MinimoActual = 0;
                        product.DistribuidorActual = 0;
                        product.PublicoActual = 0;
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
                SubTotalTarget += product.SubTarget;
                porcentageTarget += product.PorcentajeTarget;
            }
        }

    }
}
