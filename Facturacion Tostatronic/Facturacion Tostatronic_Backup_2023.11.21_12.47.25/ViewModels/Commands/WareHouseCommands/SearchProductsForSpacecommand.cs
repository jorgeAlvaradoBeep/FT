using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.WareHouse;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.WareHouse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.WareHouseCommands
{
    public class SearchProductsForSpacecommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public AddProductsToSpaceVM VM { get; set; }

        public SearchProductsForSpacecommand(AddProductsToSpaceVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            VM.IsBusy = true;
            Response res = await WebService.GetData("cs", VM.ProductCriterialSearch, URLData.product_sale_search);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
                return;
            }
            List<ProductSaleSearch> aux = JsonConvert.DeserializeObject<List<ProductSaleSearch>>(res.data.ToString());
            VM.SerchedProducts = await Task.Run(() => GetList(aux));
            VM.IsBusy = false;
        }
        List<ProductInSpace> GetList(List<ProductSaleSearch> aux)
        {
            List<ProductInSpace> serchedProducts = new List<ProductInSpace>();
            foreach (ProductSaleSearch product in aux)
            {
                ProductInSpace np = new ProductInSpace();
                np.IDSpace = VM.SelectedSpace.ID;
                np.Product = product;
                serchedProducts.Add(np);
            }
            return serchedProducts;
        }
    }
}
