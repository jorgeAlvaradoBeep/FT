using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients.CreditClientsCommands;
using Facturacion_Tostatronic.Views.PDV.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.GridView;

namespace Facturacion_Tostatronic.ViewModels.Commands.Clients.Credit
{

    public class GetPaymentForSaleCommand : ICommand
    {
        public ClientsCreditVM VM { get; set; }
        DialogPaymentV sC;
        CreditSale p;
        public event EventHandler CanExecuteChanged;
        public GetPaymentForSaleCommand(ClientsCreditVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var row = parameter as GridViewRow;
            p = (CreditSale)row.DataContext;
            if (p == null)
                return;
            Application.Current.Properties["Total"] = p.AmountToPay;
            sC = new DialogPaymentV();
            sC.Closing += getInfoForClosing;
            sC.ShowDialog();
        }

        private async void getInfoForClosing(object sender, CancelEventArgs e)
        {
            sC.Closing -= getInfoForClosing;
            PaymentM payment = (PaymentM)Application.Current.Properties["PaymentResult"];
            Application.Current.Properties["PaymentResult"] = null;
            VM.IsBusy = true;
            if (payment != null)
            {
                if (payment.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (payment.Payment >= 0)
                {
                    if (payment.Pagado)
                    {
                        SalePayment sp = new SalePayment(p.IDSale, p.AmountToPay, DateTime.Now.ToString(), true);
                        Response res = await WebService.InsertData(sp, URLData.addPaymentToSale);
                        if (!res.succes)
                        {
                            MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            VM.IsBusy = false;
                            return;
                        }
                        else
                        {
                            VM.Client.Sales.Remove(p);
                            //VM.Client.Sales = VM.Client.Sales;
                            VM.Client.GetTotalAmountToPay();
                            VM.IsBusy = false;
                        }
                    }
                    else
                    {
                        SalePayment sp = new SalePayment(p.IDSale, payment.Payment, DateTime.Now.ToString(), false);
                        Response res = await WebService.InsertData(sp, URLData.addPaymentToSale);
                        if (!res.succes)
                        {
                            MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            VM.IsBusy = false;
                            return;
                        }
                        else
                        {
                            p.AmountPayed += payment.Payment;
                            VM.Client.GetTotalAmountToPay();
                            VM.IsBusy = false;
                        }
                    }
                }
            }
        }
    }
}
