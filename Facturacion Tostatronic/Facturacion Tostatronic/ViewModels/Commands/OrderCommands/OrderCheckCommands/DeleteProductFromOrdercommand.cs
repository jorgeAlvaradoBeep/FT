using Facturacion_Tostatronic.Models.Products;
using System;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.ViewModels.Orders;
using Telerik.Windows.Controls.GridView;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;

namespace Facturacion_Tostatronic.ViewModels.Commands.OrderCommands.OrderCheckCommands
{
    public class DeleteProductFromOrdercommand : ICommand
    {
        public OrderCheckVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public DeleteProductFromOrdercommand(OrderCheckVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var row = parameter as GridViewRow;
            ProductOrderComplete item = (ProductOrderComplete)row.DataContext;
            VM.ProductosEliminados.Add(item);
            VM.ComlpleteOrder.ProductosDeOrdenesNavigation.Remove(item);

        }
    }
}
