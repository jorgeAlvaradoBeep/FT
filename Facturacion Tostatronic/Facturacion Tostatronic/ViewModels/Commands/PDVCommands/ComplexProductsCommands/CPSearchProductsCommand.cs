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

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands
{
    public class CPSearchProductsCommand : ICommand
    {
        private ComplexProductsVM VM;

        public CPSearchProductsCommand(ComplexProductsVM vm)
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

        public async void Execute(object parameter)
        {
            if (VM.CompleteSale.ClientSale == null || String.IsNullOrEmpty(VM.CompleteSale.ClientSale.Name))
            {
                MessageBox.Show("Primero debe de seleccionar un cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            if (string.IsNullOrEmpty(VM.ProductCriterialSearch))
            {
                return;
            }

            VM.GettingData = true;
            Response res = await WebService.GetData("cs", VM.ProductCriterialSearch, URLData.product_sale_search);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontraron coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            
            List<ProductSaleSearch> aux = JsonConvert.DeserializeObject<List<ProductSaleSearch>>(res.data.ToString());
            foreach (ProductSaleSearch product in aux)
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