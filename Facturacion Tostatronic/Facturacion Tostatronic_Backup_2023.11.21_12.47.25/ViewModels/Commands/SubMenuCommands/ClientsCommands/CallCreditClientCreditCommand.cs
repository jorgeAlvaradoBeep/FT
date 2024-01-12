using Facturacion_Tostatronic.ViewModels.Menus;
using Facturacion_Tostatronic.Views.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.ClientsCommands
{

    public class CallCreditClientCreditCommand : ICommand
    {
        public ClientsMenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public CallCreditClientCreditCommand(ClientsMenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ClientsCreditV cV = new ClientsCreditV();
            cV.ShowDialog();
        }
    }
}
