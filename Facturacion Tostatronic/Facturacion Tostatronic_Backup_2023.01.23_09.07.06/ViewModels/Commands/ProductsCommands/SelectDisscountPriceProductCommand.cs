using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class SelectDisscountPriceProductCommand : ICommand
    {
        public DisscountPricesVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SelectDisscountPriceProductCommand(DisscountPricesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ProductComplete p = (ProductComplete)parameter;
            if (p == null)
                return;
            VM.BaseProductVisibility = Visibility.Visible;
            VM.SearchProductVisibility = Visibility.Hidden;
            VM.Product = p;

        }
    }
}
