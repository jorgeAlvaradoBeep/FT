using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.Clients.ModifyClient
{
    public class ModifyClientRegimenCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public ClientRegimenChangeVM VM { get; set; }

        public ModifyClientRegimenCommand(ClientRegimenChangeVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(VM.ClientComplete!=null)
            {
                if(VM.RegimenFiscalSelected!=null || VM.SelectedClientType!= null)
                {
                    VM.GettingData = true;
                    Response res = await WebService.ModifyData(VM.ClientComplete, URLData.editRegimenFiscalesNet);
                    if (!res.succes)
                    {
                        MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }
                    else
                    {
                        VM.GettingData = false;
                        MessageBox.Show($"Cliente Modificado Con Exito", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                        VM.ClientComplete = null;
                        VM.SelectedClientType = null;
                        VM.RegimenFiscalSelected = null;

                    }
                }
                else
                    MessageBox.Show("No realizo ningun cambio","Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
