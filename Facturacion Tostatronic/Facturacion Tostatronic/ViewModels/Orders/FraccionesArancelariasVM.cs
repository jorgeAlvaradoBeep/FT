using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.FraccionesCommands;
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

namespace Facturacion_Tostatronic.ViewModels.Orders
{
    public class FraccionesArancelariasVM : INotifyPropertyChanged, IPageViewModel
    {
        #region Properties
        public string Name { get; set; } = "FraccionesArancelariasVM";
        private bool _gettingData;

        public bool GettingData
        {
            get => _gettingData;
            set
            {
                if (_gettingData != value)
                {
                    _gettingData = value;
                    OnPropertyChanged(); // Notifica el cambio de la propiedad
                }
            }
        }
        private ObservableCollection<APIOrder> _orders;
        public ObservableCollection<APIOrder> Orders
        {
            get => _orders;
            set
            {
                if (_orders != value)
                {
                    _orders = value;
                    OnPropertyChanged();
                }
            }
        }
        private ObservableCollection<FraccionArancelaria> _fraccionesOrden;
        public ObservableCollection<FraccionArancelaria> FraccionesOrden
        {
            get => _fraccionesOrden;
            set
            {
                if (_fraccionesOrden != value)
                {
                    _fraccionesOrden = value;
                    OnPropertyChanged();
                }
            }
        }
        private APIOrder _selectedItem;
        public APIOrder SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem != value)
                {
                    _selectedItem = value;
                    OnPropertyChanged();
                }
            }
        }
        private FraccionArancelaria selectedFraccion;

        public FraccionArancelaria SelectedFraccion
        {
            get { return selectedFraccion; }
            set 
            { 
                if(selectedFraccion != value)
                {
                    selectedFraccion = value;
                    OnPropertyChanged();
                }
            }
        }
        private string progresVal;

        public string ProgresVal
        {
            get { return progresVal; }
            set
            {
                if (progresVal != value)
                {
                    progresVal = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<FraccionArancelaria> Fracciones { get; set; }
        public List<APIProductOrderInformation> productInformationList { get; set; }
        public APIOrder sl { get; set; }

        #endregion
        #region Commands
        public SaveFraccionesCommand SaveFraccionesCommand { get; set; }
        public FraccionesExcelFileCommand FraccionesExcelFileCommand { get; set; }
        public GeneratePICommand GeneratePICommand { get; set; }
        #endregion

        public FraccionesArancelariasVM()
        {
            SaveFraccionesCommand = new SaveFraccionesCommand(this);
            FraccionesExcelFileCommand = new FraccionesExcelFileCommand(this);
            GeneratePICommand = new GeneratePICommand(this);
        }

        public async Task GetOrders()
        {
            GettingData = true;
            Response res = await WebService.GetDataForInvoice(URLData.OpenOrders);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
                return;
            }
            Orders = JsonConvert.DeserializeObject<ObservableCollection<APIOrder>>(res.data.ToString());
            //Ahora Extraemos todas las fracciones arancelarias
            res = await WebService.GetDataForInvoice(URLData.FraccionesArancelarias);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
                return;
            }
            Fracciones = JsonConvert.DeserializeObject<List<FraccionArancelaria>>(res.data.ToString());
            //Ahora extraemos la informacion de los productos
            res = await WebService.GetDataForInvoice(URLData.ProductOrderInfo);
            if (res.succes)
            {
                productInformationList = JsonConvert.DeserializeObject<List<APIProductOrderInformation>>(res.data.ToString());
            }
            else
                MessageBox.Show("Error al traer la lista información de los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            GettingData = false;
        }
        public async Task GetOrderData()
        {
            GettingData = true;
            Response res = await WebService.GetDataNode(URLData.Orders, SelectedItem.OrdenID.ToString());
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
                return;
            }
            var aux = JsonConvert.DeserializeObject<List<APIOrder>>(res.data.ToString());
            sl = aux[0];
            if (sl.ProductosDeOrdenesNavigation.Count > 0)
            {
                ObservableCollection<FraccionArancelaria> listaProductos = new ObservableCollection<FraccionArancelaria>();
                foreach (APIProductosOrdenes pro in sl.ProductosDeOrdenesNavigation)
                {
                    string name = productInformationList.Where(p => p.CodigoProducto == pro.CodigoProducto).ToList()[0].NombreEs;
                    var fa = Fracciones.Where(f => f.Codigo == pro.CodigoProducto).ToList();
                    FraccionArancelaria fraccionArancelaria = new FraccionArancelaria();
                    if (fa.Any())
                    {
                        fraccionArancelaria = fa[0];
                        fraccionArancelaria.Nuevo = false;
                    }
                    else
                        fraccionArancelaria.Nuevo = true;

                    fraccionArancelaria.Descripcion = name;
                    fraccionArancelaria.Codigo = pro.CodigoProducto;
                    fraccionArancelaria.Modificado = false;
                    listaProductos.Add(fraccionArancelaria);
                }
                FraccionesOrden = listaProductos;
            }
            GettingData = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
