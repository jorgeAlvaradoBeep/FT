using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.QuoteCommands
{
    public class QuoteSelectedCommand : ICommand
    {
        public SearchQuoteVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public QuoteSelectedCommand(SearchQuoteVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Quotes selectedQuote = (Quotes)parameter;
            Application.Current.Properties["SelectedQuote"] = selectedQuote;

        }
    }
}
