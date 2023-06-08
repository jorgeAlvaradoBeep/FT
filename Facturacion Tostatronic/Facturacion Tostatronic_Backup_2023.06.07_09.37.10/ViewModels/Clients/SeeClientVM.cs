using Facturacion_Tostatronic.Models.CFDI;
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
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Commands.Clients.ModifyClient;

namespace Facturacion_Tostatronic.ViewModels.Clients
{
    public class SeeClientVM : BaseNotifyPropertyChanged, IPageViewModel
    {
		private string searchCriterial;

		public string SearchCriterial
        {
			get { return searchCriterial; }
			set 
            { 
                SetValue(ref searchCriterial, value);
                ApplyFilter(searchCriterial);
            }
		}

		private bool gettingData;

		public bool GettingData
		{
			get { return gettingData; }
			set { SetValue(ref gettingData, value); }
		}

		private ObservableCollection<ClientComplete> clientes;

		public ObservableCollection<ClientComplete> Clients
        {
			get { return clientes; }
			set { SetValue(ref clientes, value); }
		}
        public IReadOnlyList<ClientComplete> _allClients;

        private bool editing;

        public bool Editing
        {
            get { return editing; }
            set { SetValue(ref editing, value); }
        }
        private ClientComplete selectedClient;

        public ClientComplete SelectedClient
        {
            get { return selectedClient; }
            set { SetValue(ref selectedClient, value); }
        }


        public ModifyClientCommand ModifyClientCommand { get; set; }

        public string Name { get; set; } = "SeeClientVM";

        public SeeClientVM()
		{
			GettingData= false;
            Clients = new ObservableCollection<ClientComplete>();
            GettingData = true;
            ModifyClientCommand = new ModifyClientCommand(this);
            Task.Run(() =>
            {
                Response rmp = WebService.GetDataForInvoiceNoAsync(URLData.getClients);
                if (rmp.succes)
                {
                    GetProductsToList(rmp);
                }
                else
                    MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
            });
            SelectedClient = null;
        }

        void GetProductsToList(Response res)
        {
            _allClients = JsonConvert.DeserializeObject<IReadOnlyList<ClientComplete>>(res.data.ToString());
            foreach (ClientComplete a in _allClients)
            {
                Clients.Add(a);
            }
        }

        private void ApplyFilter(string textToSearch)
        {
            if (this._allClients == null)
            {
                return;
            }

            var filteredEmployees = string.IsNullOrEmpty(textToSearch)
                ? this._allClients
                : this._allClients.Where(p => p.Nombres.ToLower().Contains(textToSearch.ToLower()) || p.ApellidoPaterno.ToLower().Contains(textToSearch.ToLower())
                || p.ApellidoMaterno.ToLower().Contains(textToSearch.ToLower()) || p.Rfc.ToLower().Contains(textToSearch.ToLower()));

            this.Clients.Clear();

            foreach (var employee in filteredEmployees)
            {
                this.Clients.Add(employee);
            }

        }

    }
}
