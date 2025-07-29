using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands
{
    public class CPChangeSaledQuantityCommand : ICommand
    {
        private ComplexProductsVM VM;

        public CPChangeSaledQuantityCommand(ComplexProductsVM vm)
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
            return true;
        }

        public void Execute(object parameter)
        {
            var product = parameter as ProductSaleSaled;
            if (product != null)
            {
                if (product.SaledQuantity <= 0)
                {
                    MessageBox.Show("La cantidad debe ser mayor a 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    product.SaledQuantity = 1;
                }
                
                product.CalculateSubtotal();
                VM.CompleteSale.CalculateTotals();
            }
        }
    }
}