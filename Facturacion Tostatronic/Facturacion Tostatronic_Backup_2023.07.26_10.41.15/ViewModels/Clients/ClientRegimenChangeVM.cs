using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.CFDI;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.Clients;
using Facturacion_Tostatronic.ViewModels.Commands.Clients.ModifyClient;
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
    public class ClientRegimenChangeVM:BaseNotifyPropertyChanged, IPageViewModel
    {
        public string Name { get; set; } = "ClientRegimenChangeVM";
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
        private ClientComplete clientComplete;

        public ClientComplete ClientComplete
        {
            get { return clientComplete; }
            set
            {
                SetValue(ref clientComplete, value);
                if(ClientComplete!=null)
                {
                    if(ClientComplete.RegimenFiscal!=null && !string.IsNullOrEmpty(ClientComplete.RegimenFiscal))
                    {
                        RegimenFiscalSelected = RegimenFiscal.Where(x=> x.RegimenFiscalP== ClientComplete.RegimenFiscal).First();
                    }
                    if (ClientComplete.IdTipoCliente == 1)
                        SelectedClientType = "Distribuidor";
                    else if (ClientComplete.IdTipoCliente == 2)
                        SelectedClientType = "Publico";
                    Editable = true;
                }
                else
                {
                    Editable=false;
                }
            }
        }

        private bool editable;

        public bool Editable
        {
            get { return editable; }
            set { SetValue(ref editable, value); }
        }
        private List<string> clientType;
        public List<string> ClientType
        {
            get { return clientType; }
            set { SetValue(ref clientType, value); }
        }

        private string selectedClientType;

        public string SelectedClientType
        {
            get { return selectedClientType; }
            set
            {
                SetValue(ref selectedClientType, value);
                switch (selectedClientType)
                {
                    case "Distribuidor":
                        ClientComplete.IdTipoCliente = 1;
                        break;
                    case "Publico":
                        ClientComplete.IdTipoCliente = 2;
                        break;
                }
            }
        }

        private RegimenFiscal regimenFiscalSelected;

        public RegimenFiscal RegimenFiscalSelected
        {
            get { return regimenFiscalSelected; }
            set
            {
                SetValue(ref regimenFiscalSelected, value);
                if (RegimenFiscalSelected != null)
                    ClientComplete.RegimenFiscal = regimenFiscalSelected.RegimenFiscalP;
            }
        }


        private List<RegimenFiscal> regimenFiscal;

        public List<RegimenFiscal> RegimenFiscal
        {
            get { return regimenFiscal; }
            set { SetValue(ref regimenFiscal, value); }
        }

        public ModifyClientRegimenCommand ModifyClientRegimenCommand { get; set; }
        public CancellModifyClientCommand CancellModifyClientCommand { get; set; }

        public ClientRegimenChangeVM()
        {
            Editable=false;
            GettingData = true;
            ModifyClientRegimenCommand = new ModifyClientRegimenCommand(this);
            CancellModifyClientCommand = new CancellModifyClientCommand(this);
            ClientType = new List<string>()
                {
                    "Distribuidor",
                    "Publico"
                };
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
                rmp = WebService.GetDataForInvoiceNoAsync(URLData.regimenFiscalesNet);
                if (rmp.succes)
                {
                    RegimenFiscal = JsonConvert.DeserializeObject<List<RegimenFiscal>>(rmp.data.ToString());
                }
                else
                    MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                GettingData = false;
            });
        }
    }
}
