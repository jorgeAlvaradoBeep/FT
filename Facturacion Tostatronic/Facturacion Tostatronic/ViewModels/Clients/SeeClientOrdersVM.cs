using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace Facturacion_Tostatronic.ViewModels.Clients
{
    public class SeeClientOrdersVM : BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "SeeClientOrdersVM";
        private ObservableCollection<ClientComplete> clientes;

        public ObservableCollection<ClientComplete> Clients
        {
            get { return clientes; }
            set { SetValue(ref clientes, value); }
        }
        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }

        private ObservableCollection<ClientOrder> clientOrders;

        public ObservableCollection<ClientOrder> ClientOrders
        {
            get { return clientOrders; }
            set { SetValue(ref clientOrders, value); }
        }


        private ClientComplete clientComplete;

        public ClientComplete ClientComplete
        {
            get { return clientComplete; }
            set 
            { 
                SetValue(ref clientComplete, value); 
                if(ClientComplete!=null)
                {
                    SaleProducts = new ObservableCollection<EFSaleProducts>();
                    new Action(async () => await GetTotalesO())();
                }
            }
        }

        private ClientOrder selectedClient;

        public ClientOrder SelectedClient
        {
            get { return selectedClient; }
            set 
            { 
                SetValue(ref selectedClient, value);
                if (SelectedClient != null)
                {
                    Task.Run(() =>
                    {
                        GettingData = true;
                        Response rmp = WebService.GetDataForInvoiceNoAsync(URLData.getOrderData + SelectedClient.IdVenta);
                        if (rmp.succes)
                        {
                            SaleProducts = JsonConvert.DeserializeObject<ObservableCollection<EFSaleProducts>>(rmp.data.ToString());
                        }
                        else
                        {
                            SaleProducts = new ObservableCollection<EFSaleProducts>();
                            MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        GettingData = false;
                    });
                }
            }
        }

        private ObservableCollection<EFSaleProducts> saleProducts;

        public ObservableCollection<EFSaleProducts> SaleProducts
        {
            get { return saleProducts; }
            set { SetValue(ref saleProducts, value); }
        }


        public SeeClientOrdersVM()
        {
            GettingData = true;
            SaleProducts = new ObservableCollection<EFSaleProducts>();
            Task.Run(() =>
            {
                Response rmp = WebService.GetDataForInvoiceNoAsync(URLData.getClients);
                if (rmp.succes)
                {
                    Clients = JsonConvert.DeserializeObject<ObservableCollection<ClientComplete>>(rmp.data.ToString());
                }
                else
                { 
                    Clients = new ObservableCollection<ClientComplete>();   
                    MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                GettingData = false;
            });
        }

        async Task GetTotalesO()
        {
            GettingData = true;
            Response rmp = await WebService.GetDataForInvoice(URLData.getClientOrders + ClientComplete.IdCliente);
            if (rmp.succes)
            {
                ClientOrders = JsonConvert.DeserializeObject<ObservableCollection<ClientOrder>>(rmp.data.ToString());
                foreach (ClientOrder aux in ClientOrders)
                {
                    rmp = await WebService.GetDataForInvoice(URLData.getTotalForSale + aux.IdVenta);
                    aux.SubTotal = float.Parse(rmp.data.ToString());
                }
            }
            else
            {
                ClientOrders = new ObservableCollection<ClientOrder>();
                MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            GettingData = false;
        }
    }
}
