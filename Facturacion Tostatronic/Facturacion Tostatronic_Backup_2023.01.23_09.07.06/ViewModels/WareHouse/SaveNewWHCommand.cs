using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.WareHouse
{
    public class SaveNewWHCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddNewWareHouseVM VM { get; set; }
        public SaveNewWHCommand(AddNewWareHouseVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (VM.SavePosition.Equals(0))
            {
                if (VM.WH.Localizations.Count == 0)
                {
                    MessageBox.Show("Debe asignar al menos una localizacion", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                MessageBox.Show("Se han guardado correctamente las localizaciones, " +
                                "ahora seleccione una localizacion y agregue los " +
                                "muebles de organizacion que hay disponibles para esa " +
                                "localizacion", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                VM.EnabledEditingLocalizationName = true;
                VM.SavePosition = 1;
            }
            else if(VM.SavePosition.Equals(1))
            {
                VM.IsBusy = true;
                foreach (LocalizationM wh in VM.WH.Localizations)
                {
                    if (wh.OrganizationFornitures.Count == 0)
                    {
                        MessageBox.Show("Debe asignar al menos un mueble organizador" +
                            Environment.NewLine + "Recuerde guardar los muebles antes de pasar a otra" +
                            "localizacion.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.IsBusy = false;
                        return;
                    }
                }
                Response res = await WebService.InsertData(VM.WH, URLData.save_new_warehouse);
                if(!res.succes)
                {
                    MessageBox.Show("Error al guardar en la BD." +
                        Environment.NewLine + "Motivos: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.IsBusy = false;
                    return;
                }
                VM.IsBusy = false;
                MessageBox.Show("Almacen Guardado", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                VM.Initilizate();
            }
        }
    }
}
