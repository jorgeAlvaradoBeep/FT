using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands
{
    public class CPDeleteProductCommand : ICommand
    {
        private ComplexProductsVM VM;

        public CPDeleteProductCommand(ComplexProductsVM vm)
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
                    VM.CompleteSale.SaledProducts.Remove(product);
                    VM.CompleteSale.CalculateTotals();
                }
            }
        }
    }
}