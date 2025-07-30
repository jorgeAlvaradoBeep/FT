using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using Facturacion_Tostatronic.Views.Products;
using System;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands
{
    public class CPSearchProductFromSaleCommand : ICommand
    {
        private ComplexProductsVM VM;

        public CPSearchProductFromSaleCommand(ComplexProductsVM vm)
        {
            VM = vm;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return parameter != null;
        }

        public void Execute(object parameter)
        {
            var row = parameter as GridViewRow;
            if (row != null)
            {
                var product = row.Item as ProductSaleSaled;
                if (product != null)
                {
                    SeeProductsV view = new SeeProductsV();
                    view.ShowDialog();
                }
            }
        }
    }
}