using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.QuoteCommands
{

    public class SearchQuoteCommand : ICommand
    {
        public SearchQuoteVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SearchQuoteCommand(SearchQuoteVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.GettingData = true;

            Response res = await WebService.GetDataNode(URLData.quotes, VM.CriterialSearch);
            VM.GettingData = false;
            if (res.succes)
            {
                VM.ListOfSearchedQuotes = JsonConvert.DeserializeObject<List<Quotes>>(res.data.ToString());
            }
            else
                MessageBox.Show($"Error al consultar: {res.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
