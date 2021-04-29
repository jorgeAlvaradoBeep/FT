using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Services;
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
    public class SaveProductsTospaceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public AddProductsToSpaceVM VM { get; set; }

        public SaveProductsTospaceCommand(AddProductsToSpaceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(VM.NewProductInspace.Count>0)
            {
                VM.IsBusy = true;
                Response res = await WebService.InsertData(VM.NewProductInspace, URLData.save_new_product_to_space);
                if (!res.succes)
                {
                    MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.IsBusy = false;
                    return;
                }
                VM.IsBusy = false;
                VM.NewProductInspace.Clear();
                MessageBox.Show("Productos agregados correctamente","Exito",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show("Al menos debe de agregar un producto nuevo al espacio" +
                    "", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
