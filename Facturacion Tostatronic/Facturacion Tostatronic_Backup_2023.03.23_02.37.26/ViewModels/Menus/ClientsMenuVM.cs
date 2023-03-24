using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.ClientsCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Menus
{
    public class ClientsMenuVM : BaseNotifyPropertyChanged
    {

        public CallCreditClientCreditCommand CallCreditClientCreditCommand { get; set; }
        public CallNewClientViewCommand CallNewClientViewCommand { get; set; }
        public CallSeeClientViewCommand CallSeeClientViewCommand { get; set; }
        public CallClientOrdersViewCommand CallClientOrdersViewCommand { get; set; }
        public CallClientRegimenChangeCommand CallClientRegimenChangeCommand { get; set; }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetValue(ref isBusy, value); }
        }

        public ClientsMenuVM()
        {
            IsBusy = false;
            CallCreditClientCreditCommand = new CallCreditClientCreditCommand(this);
            CallNewClientViewCommand = new CallNewClientViewCommand();
            CallSeeClientViewCommand = new CallSeeClientViewCommand();
            CallClientOrdersViewCommand = new CallClientOrdersViewCommand();
            CallClientRegimenChangeCommand = new CallClientRegimenChangeCommand();
        }
    }
}
