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
    public class SearchProductsForDisscountPricecommand : ICommand
    {
        public DisscountPricesVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SearchProductsForDisscountPricecommand(DisscountPricesVM vm)
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
            Response res = await WebService.GetData("cs", VM.ProductCriterialSearch, URLData.product_update);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List<ProductComplete> aux = JsonConvert.DeserializeObject<List<ProductComplete>>(res.data.ToString());
            VM.SearchedProducts = aux;
            VM.GettingData = false;
        }
    }
}
