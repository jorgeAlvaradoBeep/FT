using Facturacion_Tostatronic.Models.CFDI;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.Clients.AddClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;

namespace Facturacion_Tostatronic.ViewModels.Clients.AddClient
{
    public class AddClientVM : BaseNotifyPropertyChanged, IPageViewModel
    {
		private ClientComplete client;

		public ClientComplete Client
		{
			get { return client; }
			set { SetValue(ref client, value); }
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
                switch(selectedClientType)
                {
                    case "Distribuidor":
                        Client.IdTipoCliente = 1;
                        break;
                    case "Publico":
                        Client.IdTipoCliente = 2;
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
                if(RegimenFiscalSelected!=null)
                    Client.RegimenFiscal = regimenFiscalSelected.RegimenFiscalP;
            }
        }


        private List<RegimenFiscal> regimenFiscal;

        public List<RegimenFiscal> RegimenFiscal
        {
            get { return regimenFiscal; }
            set { SetValue(ref regimenFiscal, value); }
        }

        private bool regimenFiscalRequired;

        public bool RegimenFiscalRequired
        {
            get { return regimenFiscalRequired; }
            set 
            { 
                SetValue(ref regimenFiscalRequired, value);
                if(!regimenFiscalRequired)
                    return;
                if (RegimenFiscal.Count > 0)
                    return;
                GettingData = true;
                Task.Run(() =>
                {
                    Response rmp = WebService.GetDataForInvoiceNoAsync(URLData.regimenFiscalesNet);
                    if (rmp.succes)
                    {
                        RegimenFiscal = JsonConvert.DeserializeObject<List<RegimenFiscal>>(rmp.data.ToString());
                    }
                    else
                        MessageBox.Show("Error al traer la información solicitada","Error",MessageBoxButton.OK,MessageBoxImage.Error);  
                });
                GettingData = false;
            }
        }


        private bool gettingData;

        public bool GettingData
        {
            get { return gettingData; }
            set { SetValue(ref gettingData, value); }
        }

        public SaveClientCommand SaveClientCommand { get; set; }

        public AddClientVM()
		{
            ClientType = new List<string>()
                {
                    "Distribuidor",
                    "Publico"
                };
            RegimenFiscal = new List<RegimenFiscal>();
            Client = new ClientComplete();
            GettingData = false;
            SaveClientCommand = new SaveClientCommand(this);

        }

        //Seccion de validaciones
        public string Error
        {
            get
            {
                return ValidateText();
            }
        }

        public string Name { get; set; } = "AddClientVM";

        public string this[string columnName]
        {
            get
            {
                switch (columnName)
                {
                    case "Client.Nombres": return this.ValidateText();
                    case "Client.Rfc": return this.ValidateRFC();
                }

                return null;
            }
        }

        public string ValidateRFC()
        {
            if (string.IsNullOrEmpty(Client.Rfc))
            {
                return "El RFC no puede ir vacio";
            }
            else if (!Regex.Match(Client.Rfc, @"^([A-ZÑ\x26]{3,4}([0-9]{2})(0[1-9]|1[0-2])(0[1-9]|1[0-9]|2[0-9]|3[0-1]))([A-Z\d]{3})?$").Success)
            {
                return "El RFC no es correcto, favor de validarlo.";
            }

            return null;
        }

        public string ValidateText()
        {
            if (string.IsNullOrEmpty(Client.Nombres))
            {
                return "El nombre o razon social no pueden ir vacios";
            }
            //else if (this.Text.Length < 5)
            //{
            //    return "Text cannot be less than 5 symbols.";
            //}

            return null;
        }

    }
}
