using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.ViewModels.Sales;
using Facturacion_Tostatronic.Views.PDV.Sales;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands
{
    public class CPCallSearchClientCommand : ICommand
    {
        private ComplexProductsVM VM;

        public CPCallSearchClientCommand(ComplexProductsVM vm)
        {
            VM = vm;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
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
            ClientSale client = (ClientSale)Application.Current.Properties["SelectedClient"];
            if (client != null)
            {
                VM.CompleteSale.ClientSale = client;
                VM.CompleteSale.PriceType = VM.CompleteSale.ClientSale.ClientType;
            }
        }
    }
}