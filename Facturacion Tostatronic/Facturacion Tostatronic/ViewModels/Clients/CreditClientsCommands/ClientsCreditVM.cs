using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.Clients.Credit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Clients.CreditClientsCommands
{
    public class ClientsCreditVM : BaseNotifyPropertyChanged
    {
        public CallSearchClientCreditCommand CallSearchClientCreditCommand { get; set; }
        public GetPaymentForSaleCommand GetPaymentForSaleCommand { get; set; }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetValue(ref isBusy, value); }
        }

        private CreditClient client;

        public CreditClient Client
        {
            get { return client; }
            set { SetValue(ref client, value); }
        }


        public ClientsCreditVM()
        {
            IsBusy = false;
            CallSearchClientCreditCommand = new CallSearchClientCreditCommand(this);
            GetPaymentForSaleCommand = new GetPaymentForSaleCommand(this);
            Client = new CreditClient();
            client.Sales = new System.Collections.ObjectModel.ObservableCollection<CreditSale>();
        }
    }
}
