using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Sales
{
    public class SearchClientVM : BaseNotifyPropertyChanged
    {
        #region comandos
        public SearchClientCommand SearchClientCommand { get; set; }
        public SelectedSalesClient SelectedSalesClient { get; set; }
        #endregion
        public string SearchCriterial { get; set; }
        private List<ClientSale> clients;

        public List<ClientSale> Clients
        {
            get { return clients; }
            set { SetValue(ref clients, value); }
        }

        private bool chargingData;

        public bool ChargingData
        {
            get { return chargingData; }
            set { SetValue( ref chargingData, value); }
        }

        private List<ClientSale> findedClients;

        public List<ClientSale> FindedClients
        {
            get { return findedClients; }
            set { SetValue( ref findedClients, value); }
        }

        public SearchClientVM()
        {
            SearchClientCommand = new SearchClientCommand(this);
            SelectedSalesClient = new SelectedSalesClient(this);
            ChargingData = false;
            FindedClients = new List<ClientSale>();
        }
    }
}
