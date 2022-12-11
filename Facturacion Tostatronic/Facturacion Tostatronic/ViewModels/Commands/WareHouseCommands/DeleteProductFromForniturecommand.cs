using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.ViewModels.WareHouse;
using Telerik.Windows.Controls.GridView;
using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands
{
    public class DeleteProductFromForniturecommand : ICommand
    {
        public AddProductsToSpaceVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public DeleteProductFromForniturecommand(AddProductsToSpaceVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var row = parameter as GridViewRow;
            ProductInSpace item = (ProductInSpace)row.DataContext;
            MessageBoxResult messageBoxResult = MessageBox.Show($"Esta seguro que desea eliminar a: {item.Product.Name}?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                VM.SelectedSpace.Products.Remove(item);
                if(VM.NewProductInspace.Contains(item)) { VM.NewProductInspace.Remove(item); return; }
                VM.IsBusy = true;
                Response rmp = await WebService.DeleteDataEF(URLData.productoEnEspacio, new string[2] {item.Product.Code,item.IDSpace.ToString()});
                if (rmp.succes)
                {
                    MessageBox.Show("Eliminado con exito", "Eliminación Correcta", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"Error al eliminar en la BD: {rmp.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                VM.IsBusy = false;
            }

        }
    }
}
