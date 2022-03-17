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
        }
    }
}
