using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.WareHouseCommands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Menus
{
    public class WareHouseMenuVM : BaseNotifyPropertyChanged
    {
        public AddNewWareHouseCommand AddNewWareHouseCommand { get; set; }
        public AddFornitureSpacesCommand AddFornitureSpacesCommand { get; set; }
        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetValue(ref isBusy, value); }
        }

        public WareHouseMenuVM()
        {
            AddNewWareHouseCommand = new AddNewWareHouseCommand(this);
            AddFornitureSpacesCommand = new AddFornitureSpacesCommand(this);
            IsBusy = false;
        }
    }
}
