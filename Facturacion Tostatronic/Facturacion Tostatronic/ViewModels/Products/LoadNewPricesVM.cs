using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.LoadNewPricesCommands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Products
{
    public class LoadNewPricesVM: INotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "LoadNewPricesVM";
        private bool isBusy;

		public bool IsBusy
        {
			get { return isBusy; }
			set 
            {
                if (isBusy != value)
                {
                    isBusy = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
                }
            }
		}

        private string porcentajeDeAvance;

        public string PorcentajeDeAvance
        {
            get { return porcentajeDeAvance; }
            set
            {
                if (porcentajeDeAvance != value)
                {
                    porcentajeDeAvance = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
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

        private List<ProductCompleteNewArrival> productosActualizar;

        public List<ProductCompleteNewArrival> ProductosActualizar
        {
            get { return productosActualizar; }
            set
            {
                if (productosActualizar != value)
                {
                    productosActualizar = value;
                    OnPropertyChanged(); // Notifica cambio en "Nombre"
                }
            }
        }
        private object _currentPage;
        public object CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage != value)
                {
                    _currentPage = value;
                    OnPropertyChanged();
                }
            }
        }
        private ProductCompleteNewArrival selectedProduct;

        public ProductCompleteNewArrival SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                if (selectedProduct != value)
                {
                    selectedProduct = value;
                    OnPropertyChanged(nameof(SelectedProduct));
                }
            }
        }

        public List<APIProductOrderInformation> productInformationList { get; set; }
        public List<UpdateProductM> AllProducts { get; set; }
        #region Commands
        //public LoadProductsFromExcelCommand LoadProductsFromExcelCommand { get; set; }
        public SelectedProductToUpdateCommand SelectedProductToUpdateCommand { get; set; }
        #endregion
        public LoadNewPricesVM()
        {
            IsBusy = false;
            PorcentajeDeAvance = "0%";
            ProductosActualizar = new List<ProductCompleteNewArrival>();
            CurrentPage = null;

            //Comandos
            //LoadProductsFromExcelCommand = new LoadProductsFromExcelCommand(this);
            SelectedProductToUpdateCommand = new SelectedProductToUpdateCommand(this);
        }
        public async Task GetOrders()
        {
            IsBusy = true;
            Response response = await WebService.GetDataNode(URLData.GetEstablecerOrdenes, "");
            if (response.succes)
            {
                Ordenes = JsonConvert.DeserializeObject<ObservableCollection<int>>(response.data.ToString());
            }
            IsBusy = false;
        }

        public async Task GetOrderComplete()
        {
            IsBusy = true;
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

                res = await WebService.GetDataNode(URLData.getProductsNet, "");
                if (res.succes)
                {
                    AllProducts = JsonConvert.DeserializeObject<List<UpdateProductM>>(res.data.ToString());
                }
                else
                    MessageBox.Show("Error al traer la lista información de los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                List< ProductCompleteNewArrival > products = new List<ProductCompleteNewArrival>();
                ProductCompleteNewArrival pr;
                foreach (var item in detalle)
                {
                    pr = new ProductCompleteNewArrival();
                    pr.Name = productInformationList.Where(p => p.CodigoProducto == item.codigoProducto).ToList()[0].NombreEs;
                    pr.Code = item.codigoProducto;
                    pr.NewStock = item.cantidad;
                    pr.MinimumPrice = (float)item.minimo;
                    pr.DistributorPrice = (float)item.distribuidor;
                    pr.PublicPrice = (float)item.publico;
                    pr.BuyPrice = item.precio;
                    if (AllProducts != null)
                    {
                        if (AllProducts.Count > 0)
                        {
                            var product = AllProducts.Where(p => p.Codigo == item.codigoProducto).FirstOrDefault();
                            if (product != null)
                            {
                                pr.NewProduct = false;
                                pr.Name = product.Nombre;
                                pr.OldStock = (int)product.Existencia;
                                pr.MinimumQuantity = product.CantidadMinima;
                                pr.Existence = pr.NewStock + pr.OldStock;
                            }
                            else
                                pr.NewProduct = true;
                        }
                    }
                    pr.Existence = pr.NewStock+pr.OldStock;
                    products.Add(pr);
                }
                ProductosActualizar = products;
            }
            IsBusy = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        // Método auxiliar para levantar el evento. 
        // [CallerMemberName] permite no tener que especificar el nombre de la propiedad manualmente.
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
