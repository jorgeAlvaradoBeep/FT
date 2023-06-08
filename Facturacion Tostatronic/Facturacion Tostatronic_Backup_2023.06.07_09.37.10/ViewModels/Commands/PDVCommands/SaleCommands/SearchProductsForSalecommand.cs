using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
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
    public class SearchProductsForSalecommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SearchProductsForSalecommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(String.IsNullOrEmpty(VM.CompleteSale.ClientSale.Name))
            {
                MessageBox.Show("Primero debe de seleccionar un cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GettingData = true;
            Response res = await WebService.GetData("cs", VM.ProductCriterialSearch, URLData.product_sale_search);
            if (!res.succes)
            {
                MessageBox.Show("Error: "+res.message+Environment.NewLine+"No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List < ProductSaleSearch > aux = JsonConvert.DeserializeObject<List<ProductSaleSearch>>(res.data.ToString());
            foreach(ProductSaleSearch product in aux)
            {
                if (VM.CompleteSale.PriceType.Equals(1))
                    product.DisplayPrice = product.DistributorPrice;
                else if (VM.CompleteSale.PriceType.Equals(2))
                    product.DisplayPrice = product.PublicPrice;
                else
                    product.DisplayPrice = product.MinimumPrice;
            }
            VM.CompleteSale.SearchedProducts = aux;
            VM.GettingData = false;
        }
    }
}
