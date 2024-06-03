using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Windows.Input;
using System.Windows;
using Telerik.Windows.Controls.GridView;
using System.Threading.Tasks;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class DeleteProductCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeProductVM VM { get; set; }
        public DeleteProductCommand(SeeProductVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var row = parameter as GridViewRow;
            UpdateProductM cC = (UpdateProductM)row.DataContext;
            if (cC != null)
            {
                MessageBoxResult resp = MessageBox.Show($"Esta seguro que desea eliminar el " +
                    $"producto:{Environment.NewLine}{cC.Codigo}: {cC.Nombre}?","Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);
                if (resp == MessageBoxResult.No)
                {
                    MessageBox.Show("No se ejecuto ninguna acción.", "Información");
                    return;
                }
                VM.GettingData = true;
                Response res = await WebService.DeleteDataEFSingle(URLData.deleteProductsNet,cC.Codigo);
                if (!res.succes)
                {
                    MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    return;
                }
                else
                {
                    VM.Products.Remove(cC);
                    //await Task.Run(() => VM.SeeProductsLoadedCommand.Execute(null));
                    MessageBox.Show("Prodcuto Eliminado con exito", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                VM.GettingData = false;
            }
        }
    }
}
