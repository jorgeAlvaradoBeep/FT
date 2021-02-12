using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.ViewModels.Sales;
using Facturacion_Tostatronic.Views.PDV.Sales;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class CallSearchClientViewCommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public CallSearchClientViewCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SearchClientV sC = new SearchClientV();
            sC.Closing += getInfoForClosing;
            sC.ShowDialog();
        }

        private void getInfoForClosing(object sender, CancelEventArgs e)
        {
            ClientSale client = (ClientSale)Application.Current.Properties["SelectedClient"]; ;
            if (client != null)
            {
                VM.CompleteSale.ClientSale = client;
                VM.CompleteSale.PriceType = VM.CompleteSale.ClientSale.ClientType;
            }
            
        }
    }
}
