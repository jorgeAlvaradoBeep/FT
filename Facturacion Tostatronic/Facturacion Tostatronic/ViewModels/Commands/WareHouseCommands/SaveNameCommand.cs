using Facturacion_Tostatronic.ViewModels.WareHouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands
{
    public class SaveNameCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddNewWareHouseVM VM { get; set; }
        public SaveNameCommand(AddNewWareHouseVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(string.IsNullOrEmpty(VM.WH.Name))
            {
                MessageBox.Show("Debe asignar un nombre al almacen", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.EnableEditingName = false;
            VM.LocalizationVisibility = Visibility.Visible;
        }
    }
}
