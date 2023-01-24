using Facturacion_Tostatronic.Views.Menus;
using Facturacion_Tostatronic.Views.Menus.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.MenuCommands
{
    public class ViewWarehouseMenuCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public ViewWarehouseMenuCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SubMenuContent sm = new SubMenuContent(new WareHouseSubMenu());
            sm.ShowDialog();
        }
    }
}
