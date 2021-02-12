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
    public class SearchProductsForMLPricecommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public MLPriceVM VM { get; set; }

        public SearchProductsForMLPricecommand(MLPriceVM vm)
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
            Response res = await WebService.GetData("cs", VM.ProductCriterialSearch, URLData.product_sale_search);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            VM.SearchedProducts = JsonConvert.DeserializeObject<List<ProductComplete>>(res.data.ToString());
            VM.GettingData = false;
        }
    }
}
