using Facturacion_Tostatronic.ViewModels.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.Clients
{
    public class CancellModifyClientCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public ClientRegimenChangeVM VM { get; set; }

        public CancellModifyClientCommand(ClientRegimenChangeVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.ClientComplete = null;
            VM.SelectedClientType = null;
            VM.RegimenFiscalSelected = null;
        }
    }
}
