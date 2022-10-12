using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Facturacion_Tostatronic.ViewModels.Clients
{
    public class SeeClientOrdersVM : BaseNotifyPropertyChanged
    {
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
                    Task.Run(() =>
                    {
                        Response rmp = WebService.GetDataForInvoiceNoAsync(URLData.getClientOrders + ClientComplete.IdCliente);
                        if (rmp.succes)
                        {
                            ClientOrders = JsonConvert.DeserializeObject<ObservableCollection<ClientOrder>>(rmp.data.ToString());
                        }
                        else
                        {
                            ClientOrders = new ObservableCollection<ClientOrder>();
                            MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        GettingData = false;
                    });
                }
            }
        }


        public SeeClientOrdersVM()
        {
            GettingData = true;
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
    }
}
