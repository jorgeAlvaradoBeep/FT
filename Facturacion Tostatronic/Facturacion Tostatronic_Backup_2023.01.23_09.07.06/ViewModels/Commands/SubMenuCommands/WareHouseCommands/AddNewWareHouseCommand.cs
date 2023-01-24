using Facturacion_Tostatronic.ViewModels.Menus;
using Facturacion_Tostatronic.Views.WareHouseViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SubMenuCommands.WareHouseCommands
{
    public class AddNewWareHouseCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public WareHouseMenuVM VM { get; set; }
        public AddNewWareHouseCommand(WareHouseMenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            WareHouseContetView wHCV = new WareHouseContetView(new AddNewWareHouseV());
            wHCV.ShowDialog();
        }
    }
}
