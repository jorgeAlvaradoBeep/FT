using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands
{
    public class CPSaleProductCommand : ICommand
    {
        private ComplexProductsVM VM;

        public CPSaleProductCommand(ComplexProductsVM vm)
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
            var selectedProduct = parameter as ProductSaleSearch;
            if (selectedProduct == null)
            {
                return;
            }

            // Check if product already exists in the sale
            var existingProduct = VM.CompleteSale.SaledProducts.FirstOrDefault(p => p.Code == selectedProduct.Code);
            
            if (existingProduct != null)
            {
                // Product already exists, increase quantity
                existingProduct.SaledQuantity++;
            }
            else
            {
                // Add new product to sale
                var saledProduct = new ProductSaleSaled
                {
                    Code = selectedProduct.Code,
                    Name = selectedProduct.Name,
                    SaledQuantity = 1,
                    DisplayPrice = selectedProduct.DisplayPrice,
                    Existence = selectedProduct.Existence
                };
                
                VM.CompleteSale.SaledProducts.Add(saledProduct);
            }

            // Update totals
            VM.CompleteSale.GetSubtotal();
        }
    }
}