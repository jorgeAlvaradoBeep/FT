using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
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

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SearchClientCommand : ICommand
    {
        public SearchClientVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SearchClientCommand(SearchClientVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.ChargingData = true;
            Response res = await WebService.GetData("cs", VM.SearchCriterial, URLData.sales_client_search);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.ChargingData = false;
                return;
            }
            VM.Clients = JsonConvert.DeserializeObject<List<ClientSale>>(res.data.ToString());
            VM.ChargingData = false;
        }
    }
}
