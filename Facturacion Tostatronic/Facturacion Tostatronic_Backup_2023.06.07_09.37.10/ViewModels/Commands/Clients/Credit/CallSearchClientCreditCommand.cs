using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients.CreditClientsCommands;
using Facturacion_Tostatronic.Views.PDV.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.Clients.Credit
{
    public class CallSearchClientCreditCommand : ICommand
    {
        public ClientsCreditVM VM { get; set; }
        SearchClientV sC;
        public event EventHandler CanExecuteChanged;
        public CallSearchClientCreditCommand(ClientsCreditVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            sC = new SearchClientV();
            sC.Closing += getInfoForClosing;
            sC.ShowDialog();
        }

        private async void getInfoForClosing(object sender, CancelEventArgs e)
        {
            sC.Closing -= getInfoForClosing;
            ClientSale client = (ClientSale)Application.Current.Properties["SelectedClient"];
            if (client != null)
            {
                VM.Client.Client = client;
                Application.Current.Properties["SelectedClient"] = null;
                VM.IsBusy = true;

                Response res = await WebService.GetDataNode(URLData.creditSales, client.ID.ToString());
                
                if (res.succes)
                {
                    VM.Client.Sales = JsonConvert.DeserializeObject<ObservableCollection<CreditSale>>(res.data.ToString());
                    VM.Client.Sales = await Task.Run(() => GetPaymentsBySale(VM.Client.Sales));
                    float totalUnpayedAmount = 0;
                    foreach (CreditSale a in VM.Client.Sales)
                    {
                        totalUnpayedAmount += a.AmountToPay;
                    }
                    VM.Client.TotalAmountToPay = totalUnpayedAmount;
                }
                else
                    MessageBox.Show($"Error al consultar: {res.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
            }
        }

        private ObservableCollection<CreditSale> GetPaymentsBySale(ObservableCollection<CreditSale> sales)
        {
            foreach (CreditSale a in sales)
            {
                Response res = WebService.GetDataNodeNoAsync(URLData.creditPayments, a.IDSale.ToString());
                if (res.succes && res.data.ToString()!= null)
                {
                    var aux = JsonConvert.DeserializeObject<dynamic>(res.data.ToString());
                    a.AmountPayed = aux[0]["AmountPayed"];
                }
                    
                else
                    a.AmountPayed = 0;
            }
            return sales;
        }
    }
}
