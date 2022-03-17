using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.Models.Clients
{
    public class CreditClient : BaseNotifyPropertyChanged
    {
        private ClientSale client;

        public ClientSale Client
        {
            get { return client; }
            set { SetValue(ref client, value); }
        }

        private ObservableCollection<CreditSale> sales;

        public ObservableCollection<CreditSale> Sales
        {
            get { return sales; }
            set { SetValue(ref sales, value); }
        }
        private float totalAmountToPay;

        public float TotalAmountToPay
        {
            get { return totalAmountToPay; }
            set { SetValue(ref totalAmountToPay, value); }
        }

        public void GetTotalAmountToPay()
        {
            float aux = 0;
            foreach (CreditSale a in Sales)
            {
                aux += a.AmountToPay;
            }
            TotalAmountToPay = aux;
        }

    }
}
