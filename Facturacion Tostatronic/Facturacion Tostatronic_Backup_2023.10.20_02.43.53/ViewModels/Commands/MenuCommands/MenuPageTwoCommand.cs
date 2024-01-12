using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.MenuCommands
{
    public class MenuPageTwoCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public MenuPageTwoCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.MenuPageOne = System.Windows.Visibility.Hidden;
            VM.MenuPageTwo = System.Windows.Visibility.Visible;
        }
    }
}
