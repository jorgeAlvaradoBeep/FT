using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Orders
{
    public class EstablecerPreciosVM:INotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "EstablecerPreciosVM";
        #region Propiedades
        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set
            {
                if (gettingData != value)
                {
                    gettingData = value;
                    OnPropertyChanged(nameof(GettingData));
                }
            }
        }
        private OrderComplete orderComplete;

        public OrderComplete OrderComplete
        {
            get { return orderComplete; }
            set
            {
                if (orderComplete != value)
                {
                    orderComplete = value;
                    OnPropertyChanged(nameof(OrderComplete));
                }
            }
        }
        private ObservableCollection<int> ordenes;

        public ObservableCollection<int> Ordenes
        {
            get { return ordenes; }
            set
            {
                if (ordenes != value)
                {
                    ordenes = value;
                    OnPropertyChanged(nameof(Ordenes));
                }
            }
        }
        private int selectedOrder;

        public int SelectedOrder
        {
            get { return selectedOrder; }
            set
            {
                if (selectedOrder != value)
                {
                    selectedOrder = value;
                    OnPropertyChanged(nameof(SelectedOrder));
                }
            }
        }
        private decimal costoTotal;

        public decimal CostoTotal
        {
            get => costoTotal;
            set
            {
                if (costoTotal != value)
                {
                    costoTotal = value;
                    OnPropertyChanged(nameof(CostoTotal));
                }
            }
        }


        public List<APIProductOrderInformation> productInformationList { get; set; }
        public List<UpdateProductM> AllProducts { get; set; }
        
        #endregion
        #region Comandos
        #endregion

        public EstablecerPreciosVM(OrderComplete _OrderComplete)
        {
            GettingData = false;
            this.OrderComplete = _OrderComplete;
        }
        public EstablecerPreciosVM()
        {
            GettingData = false;
            CostoTotal = 0;
        }
        public async Task GetOrders()
        {
            GettingData = true;
            Response response = await WebService.GetDataNode(URLData.GetEstablecerOrdenes,"");
            if (response.succes)
            {
                Ordenes = JsonConvert.DeserializeObject<ObservableCollection<int>>(response.data.ToString());
            }
            GettingData = false;
        }
        public async Task GetOrderComplete()
        {
            GettingData = true;
            Response res = await WebService.GetDataNode(URLData.GetEstablecerOrden, SelectedOrder.ToString());
            if (res.succes)
            {
                var detalle = JsonConvert.DeserializeObject<List<DetalleOrdenExtendido>>(res.data.ToString());
                res = await WebService.GetDataForInvoice(URLData.ProductOrderInfo);
                
                if (res.succes)
                {
                    productInformationList = JsonConvert.DeserializeObject<List<APIProductOrderInformation>>(res.data.ToString());
                }
                else
                    MessageBox.Show("Error al traer la lista información de los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                decimal costoTotal = 0;
                foreach (var item in detalle)
                {
                    item.Nombre = productInformationList.Where(p => p.CodigoProducto == item.codigoProducto).ToList()[0].NombreEs;
                    costoTotal += (decimal)item.precio * item.cantidad;
                }
                CostoTotal = costoTotal;
            }
            GettingData = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
