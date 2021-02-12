using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class ChangeSaledQuantityCommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public ChangeSaledQuantityCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            //ProductSaleSaled product = (ProductSaleSaled)parameter;
            //if(product.SaledQuantity<=0)
            //{
            //    product.SaledQuantity = 1;
            //}
            //else if(product.DisplayPrice<product.MinimumPrice)
            //{
            //    product.DisplayPrice = product.MinimumPrice;
            //}
            VM.CompleteSale.GetSubtotal();
        }
    }
}
