using Bukimedia.PrestaSharp.Entities.AuxEntities;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Orders
{
    public class OrderCheckVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        #region Properties
        public string Name { get; set; } = "OrderCheckVM";
        private bool gettinData;

        public bool GettingData
        {
            get { return gettinData; }
            set { SetValue(ref gettinData, value); }
        }
        private bool isProductExtractionenabled;

        public bool IsProductExtractionenabled
        {
            get { return isProductExtractionenabled; }
            set { SetValue(ref isProductExtractionenabled, value); }
        }

        public bool OrderListAvailable
        {
            get { return !IsProductExtractionenabled; }
        }
        private string progressVal;

        public string ProgressVal
        {
            get { return progressVal; }
            set { SetValue(ref progressVal, value); }
        }
        private ObservableCollection<APIOrder> orders;

        public ObservableCollection<APIOrder> Orders
        {
            get { return orders; }
            set { SetValue(ref orders, value); }
        }
        private APIOrder selectedOrder;

        public APIOrder SelectedOrder
        {
            get { return selectedOrder; }
            set 
            { 
                if (value == null || value==SelectedOrder)
                    return;
                SetValue(ref selectedOrder, value);
                IsProductExtractionenabled = true;
                Task.Run((() =>
                {
                    GetOrderData();
                }));

            }
        }

        private OrderComplete orderComplete;

        public OrderComplete ComlpleteOrder
        {
            get { return orderComplete; }
            set { SetValue(ref orderComplete, value); }
        }

        private ObservableCollection<UpdateProductM> allProducts;

        public ObservableCollection<UpdateProductM> AllProducts
        {
            get { return allProducts; }
            set { SetValue(ref allProducts, value); }
        }

        public readonly SynchronizationContext _syncContext;

        #endregion

        #region Commands
        public GetAvailableOrdersCommand GetAvailableOrdersCommand { get; set; }
        public ImportExcelFileCommand ImportExcelFileCommand { get; set; }
        #endregion

        public OrderCheckVM()
        {
            IsProductExtractionenabled = false;
            Orders = new ObservableCollection<APIOrder>();
            ComlpleteOrder = new OrderComplete();
            AllProducts = new ObservableCollection<UpdateProductM>();
            ComlpleteOrder.ProductosDeOrdenesNavigation = new ObservableCollection<ProductOrderComplete>();
            GetAvailableOrdersCommand = new GetAvailableOrdersCommand(this);
            ImportExcelFileCommand = new ImportExcelFileCommand(this);
        }

        async void GetOrderData()
        {
            GettingData = true;
            Response res = await WebService.GetDataNode(URLData.Orders,SelectedOrder.OrdenID.ToString());
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
                return;
            }
            var aux = JsonConvert.DeserializeObject<List<APIOrder>>(res.data.ToString());
            APIOrder sl = aux[0];
            res = await WebService.GetDataForInvoice(URLData.getProductsNet);
            if (res.succes)
            {
                AllProducts = JsonConvert.DeserializeObject<ObservableCollection<UpdateProductM>>(res.data.ToString());
            }
            else
                MessageBox.Show("Error al traer productos de busqueda", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            ComlpleteOrder.OrdenID = sl.OrdenID;
            ComlpleteOrder.FechaCreacion = sl.FechaCreacion;
            ComlpleteOrder.CostoAA = sl.CostoAA;
            ComlpleteOrder.PorcentajeGanancia = sl.PorcentajeGanancia;
            ComlpleteOrder.CostoEnvio = sl.CostoEnvio;
            ComlpleteOrder.TipoCambio = sl.TipoCambio;
            if(sl.ProductosDeOrdenesNavigation.Count>0)
            {
                ObservableCollection<ProductOrderComplete> listaProductos = new ObservableCollection<ProductOrderComplete>();
                foreach (APIProductosOrdenes pro in sl.ProductosDeOrdenesNavigation) 
                {
                    listaProductos.Add(new ProductOrderComplete(pro.CodigoProducto, pro.Cantidad, pro.Precio, pro.TargetPrice));
                }
                //ComlpleteOrder.ProductosDeOrdenesNavigation = listaProductos;
                res = await WebService.GetDataForInvoice(URLData.ProductOrderInfo);
                List < APIProductOrderInformation > productInformationList = new List < APIProductOrderInformation >();
                if (res.succes)
                {
                    productInformationList = JsonConvert.DeserializeObject<List<APIProductOrderInformation>>(res.data.ToString());
                }
                else
                    MessageBox.Show("Error al traer la lista información de los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                if (productInformationList.Count > 0)
                {
                    foreach (ProductOrderComplete product in listaProductos)
                    {
                        var pt = productInformationList.Where(pr => pr.CodigoProducto == product.CodigoProducto).ToList();
                        if (pt.Count > 0)
                        {
                            var p = pt[0];
                            if (p != null)
                            {
                                if (!string.IsNullOrEmpty(p.CodigoProducto))
                                {
                                    product.NombreEs = p.NombreEs;
                                    product.NombreEn = p.NombreEn;
                                    product.Link = p.Link;
                                    product.ProductInfoExist = true;
                                    product.Modificado = false;
                                    product.Nuevo = false;
                                }
                            }
                        }

                    }
                }
                if(listaProductos.Count > 0) 
                {
                    foreach(var product in listaProductos) 
                    {
                        ComlpleteOrder.ProductosDeOrdenesNavigation.Add(product);
                    }
                }
            }
            GettingData = false;
            IsProductExtractionenabled = true;
        }
    }
}
