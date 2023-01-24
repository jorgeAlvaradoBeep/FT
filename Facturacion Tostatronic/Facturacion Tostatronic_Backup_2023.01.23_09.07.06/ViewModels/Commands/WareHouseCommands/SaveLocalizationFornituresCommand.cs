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
    public class SaveLocalizationFornituresCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddNewWareHouseVM VM { get; set; }
        public SaveLocalizationFornituresCommand(AddNewWareHouseVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(VM.SelectedLocalizationItem.OrganizationFornitures.Count==0)
            {
                MessageBox.Show("Debe asignar al menos un mueble en cada localizacion", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                int index = VM.WH.Localizations.ToList().FindIndex(a => a.Name == VM.SelectedLocalizationItem.Name);
                VM.WH.Localizations[index].OrganizationFornitures = VM.SelectedLocalizationItem.OrganizationFornitures;
                VM.FornitureVisibility = Visibility.Hidden;
                MessageBox.Show("Mueble(s) Guardados con exito", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
                
        }
    }
}
