using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls.GridView;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class DeleteProductFromSalecommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public DeleteProductFromSalecommand(SaleVM vm)
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
            ProductSaleSaled item = (ProductSaleSaled)row.DataContext;
            MessageBoxResult messageBoxResult = MessageBox.Show($"Esta seguro que desea eliminar a: {item.Name}?", "Delete Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                VM.CompleteSale.SaledProducts.Remove(item);
                VM.CompleteSale.SubTotal -= item.Subtotal;
            }
                
        }
    }
}
