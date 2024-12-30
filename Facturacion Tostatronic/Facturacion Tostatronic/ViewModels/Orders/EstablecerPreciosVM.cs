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
                    if (CostoTotal > 0)
                    {
                        IvaPagado = CostoTotal-(CostoTotal / 1.16m);
                    }
                }
            }
        }
        private ObservableCollection<DetalleOrdenExtendido> _detallesOrden;

        // Propiedad pública que expondrá la colección (para binding en XAML)
        public ObservableCollection<DetalleOrdenExtendido> DetallesOrden
        {
            get => _detallesOrden;
            set
            {
                if (_detallesOrden != value)
                {
                    _detallesOrden = value;
                    OnPropertyChanged(nameof(DetallesOrden));
                }
            }
        }

        #region Totales
        private decimal subTotalMinimo;
        public decimal SubTotalMinimo
        {
            get => subTotalMinimo;
            set
            {
                if (subTotalMinimo != value)
                {
                    subTotalMinimo = value;
                    OnPropertyChanged(nameof(SubTotalMinimo));
                    if(SubTotalMinimo > 0)
                    {
                        IvaMinimo = SubTotalMinimo-(SubTotalMinimo / 1.16m);
                        ImpuestoAPagarMinimo = IvaMinimo - IvaPagado;
                        VentaBrutaMinimo = SubTotalMinimo - ImpuestoAPagarMinimo;
                        GananciaRealMinimo = VentaBrutaMinimo - CostoTotal;
                        if(CostoTotal > 0 && GananciaRealMinimo>0)
                            PorcentajeGananciaMinimo =GananciaRealMinimo / CostoTotal;
                    }
                }
            }
        }

        private decimal subTotalDistribuidor;
        public decimal SubTotalDistribuidor
        {
            get => subTotalDistribuidor;
            set
            {
                if (subTotalDistribuidor != value)
                {
                    subTotalDistribuidor = value;
                    OnPropertyChanged(nameof(SubTotalDistribuidor));
                    if(subTotalDistribuidor > 0)
                    {
                        IvaDistribuidor = SubTotalDistribuidor - (SubTotalDistribuidor / 1.16m);
                        ImpuestoAPagarDistribuidor = IvaDistribuidor - IvaPagado;
                        VentaBrutaDistribuidor = SubTotalDistribuidor - ImpuestoAPagarDistribuidor;
                        GananciaRealDistribuidor = VentaBrutaDistribuidor - CostoTotal;
                        PorcentajeGananciaDistribuidor = GananciaRealDistribuidor / CostoTotal;
                    }
                }
            }
        }

        private decimal subTotalPublico;
        public decimal SubTotalPublico
        {
            get => subTotalPublico;
            set
            {
                if (subTotalPublico != value)
                {
                    subTotalPublico = value;
                    OnPropertyChanged(nameof(SubTotalPublico));
                    if(subTotalPublico > 0)
                    {
                        IvaPublico = SubTotalPublico - (SubTotalPublico / 1.16m);
                        ImpuestoAPagarPublico = IvaPublico - IvaPagado;
                        VentaBrutaPublico = SubTotalPublico - ImpuestoAPagarPublico;
                        GananciaRealPublico = VentaBrutaPublico - CostoTotal;
                        PorcentajeGananciaPublico = GananciaRealPublico / CostoTotal;
                    }
                }
            }
        }
        private decimal ivaMinimo;
        public decimal IvaMinimo
        {
            get => ivaMinimo;
            set
            {
                if (ivaMinimo != value)
                {
                    ivaMinimo = value;
                    OnPropertyChanged(nameof(IvaMinimo));
                }
            }
        }

        private decimal ivaDistribuidor;
        public decimal IvaDistribuidor
        {
            get => ivaDistribuidor;
            set
            {
                if (ivaDistribuidor != value)
                {
                    ivaDistribuidor = value;
                    OnPropertyChanged(nameof(IvaDistribuidor));
                }
            }
        }

        private decimal ivaPublico;
        public decimal IvaPublico
        {
            get => ivaPublico;
            set
            {
                if (ivaPublico != value)
                {
                    ivaPublico = value;
                    OnPropertyChanged(nameof(IvaPublico));
                }
            }
        }
        private decimal ivaPagado;
        public decimal IvaPagado
        {
            get => ivaPagado;
            set
            {
                if (ivaPagado != value)
                {
                    ivaPagado = value;
                    OnPropertyChanged(nameof(IvaPagado));
                }
            }
        }
        private decimal impuestoAPagarMinimo;
        public decimal ImpuestoAPagarMinimo
        {
            get => impuestoAPagarMinimo;
            set
            {
                if (impuestoAPagarMinimo != value)
                {
                    impuestoAPagarMinimo = value;
                    OnPropertyChanged(nameof(ImpuestoAPagarMinimo));
                }
            }
        }

        private decimal impuestoAPagarDistribuidor;
        public decimal ImpuestoAPagarDistribuidor
        {
            get => impuestoAPagarDistribuidor;
            set
            {
                if (impuestoAPagarDistribuidor != value)
                {
                    impuestoAPagarDistribuidor = value;
                    OnPropertyChanged(nameof(ImpuestoAPagarDistribuidor));
                }
            }
        }

        private decimal impuestoAPagarPublico;
        public decimal ImpuestoAPagarPublico
        {
            get => impuestoAPagarPublico;
            set
            {
                if (impuestoAPagarPublico != value)
                {
                    impuestoAPagarPublico = value;
                    OnPropertyChanged(nameof(ImpuestoAPagarPublico));
                }
            }
        }
        private decimal ventaBrutaMinimo;
        public decimal VentaBrutaMinimo
        {
            get => ventaBrutaMinimo;
            set
            {
                if (ventaBrutaMinimo != value)
                {
                    ventaBrutaMinimo = value;
                    OnPropertyChanged(nameof(VentaBrutaMinimo));
                }
            }
        }

        private decimal ventaBrutaDistribuidor;
        public decimal VentaBrutaDistribuidor
        {
            get => ventaBrutaDistribuidor;
            set
            {
                if (ventaBrutaDistribuidor != value)
                {
                    ventaBrutaDistribuidor = value;
                    OnPropertyChanged(nameof(VentaBrutaDistribuidor));
                }
            }
        }

        private decimal ventaBrutaPublico;
        public decimal VentaBrutaPublico
        {
            get => ventaBrutaPublico;
            set
            {
                if (ventaBrutaPublico != value)
                {
                    ventaBrutaPublico = value;
                    OnPropertyChanged(nameof(VentaBrutaPublico));
                }
            }
        }

        private decimal gananciaRealMinimo;
        public decimal GananciaRealMinimo
        {
            get => gananciaRealMinimo;
            set
            {
                if (gananciaRealMinimo != value)
                {
                    gananciaRealMinimo = value;
                    OnPropertyChanged(nameof(GananciaRealMinimo));
                }
            }
        }

        private decimal gananciaRealDistribuidor;
        public decimal GananciaRealDistribuidor
        {
            get => gananciaRealDistribuidor;
            set
            {
                if (gananciaRealDistribuidor != value)
                {
                    gananciaRealDistribuidor = value;
                    OnPropertyChanged(nameof(GananciaRealDistribuidor));
                }
            }
        }

        private decimal gananciaRealPublico;
        public decimal GananciaRealPublico
        {
            get => gananciaRealPublico;
            set
            {
                if (gananciaRealPublico != value)
                {
                    gananciaRealPublico = value;
                    OnPropertyChanged(nameof(GananciaRealPublico));
                }
            }
        }

        private decimal porcentajeGananciaMinimo;
        public decimal PorcentajeGananciaMinimo
        {
            get => porcentajeGananciaMinimo;
            set
            {
                if (porcentajeGananciaMinimo != value)
                {
                    porcentajeGananciaMinimo = value;
                    OnPropertyChanged(nameof(PorcentajeGananciaMinimo));
                }
            }
        }

        private decimal porcentajeGananciaDistribuidor;
        public decimal PorcentajeGananciaDistribuidor
        {
            get => porcentajeGananciaDistribuidor;
            set
            {
                if (porcentajeGananciaDistribuidor != value)
                {
                    porcentajeGananciaDistribuidor = value;
                    OnPropertyChanged(nameof(PorcentajeGananciaDistribuidor));
                }
            }
        }

        private decimal porcentajeGananciaPublico;
        public decimal PorcentajeGananciaPublico
        {
            get => porcentajeGananciaPublico;
            set
            {
                if (porcentajeGananciaPublico != value)
                {
                    porcentajeGananciaPublico = value;
                    OnPropertyChanged(nameof(PorcentajeGananciaPublico));
                }
            }
        }
        #endregion
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

                res = await WebService.GetDataNode(URLData.OrderSingle, SelectedOrder.ToString());
                if (res.succes)
                {
                    OrderComplete = JsonConvert.DeserializeObject<OrderComplete>(res.data.ToString());
                }
                else
                    MessageBox.Show("Error al traer la lista información de los productos.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                decimal costoTotal = 0;
                decimal porGanan = (decimal)OrderComplete.PorcentajeGanancia / 100;
                decimal subPub=0, subDis=0, subMin =0;
                foreach (var item in detalle)
                {
                    item.Nombre = productInformationList.Where(p => p.CodigoProducto == item.codigoProducto).ToList()[0].NombreEs;
                    item.MinimoRecomendado = (((decimal)item.precio / 1.16m) / (1 - porGanan)) * 1.16m;
                    item.DistribuidorRecomendado = (((item.MinimoRecomendado/ 1.16m)+4)/(1-0.0349m))*1.16m;
                    item.PublicoRecomendado = (((decimal)item.precio / 1.16m) / (1 - (porGanan+0.2m))) * 1.16m;
                    costoTotal += (decimal)item.precio * item.cantidad;
                    subPub += item.SubPublico;
                    subDis += item.SubDistribuidor;
                    subMin += item.SubMinimo;
                }
                CostoTotal = costoTotal;
                SubTotalPublico = subPub;
                SubTotalDistribuidor = subDis;
                SubTotalMinimo = subMin;
                DetallesOrden = new ObservableCollection<DetalleOrdenExtendido>(detalle);
            }
            GettingData = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void GetSubTotalMinimo()
        {
            var sub = DetallesOrden.Sum(p =>p.SubMinimo);
            SubTotalMinimo = sub;
        }
        public void GetSubTotalDistribuidor()
        {
            var sub = DetallesOrden.Sum(p => p.SubDistribuidor);
            SubTotalDistribuidor = sub;
        }
        public void GetSubTotalPublico()
        {
            var sub = DetallesOrden.Sum(p => p.SubPublico);
            SubTotalPublico = sub;
        }
    }
}
