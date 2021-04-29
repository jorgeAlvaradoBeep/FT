using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ImagesCommands
{
    public class ConvertImageToPNGCommand : ICommand
    {
        public MenuVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public ConvertImageToPNGCommand(MenuVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            VM.ChnageImagesToPNG();
        }
    }
}
