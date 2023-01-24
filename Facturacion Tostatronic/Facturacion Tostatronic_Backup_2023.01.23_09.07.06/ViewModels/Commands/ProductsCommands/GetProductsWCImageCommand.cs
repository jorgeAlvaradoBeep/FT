using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class GetProductsWCImageCommand : ICommand
    {
        public UpdateImageVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public GetProductsWCImageCommand(UpdateImageVM vm)
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
            Response res = await WebService.GetData("cs", "", URLData.product_updtae_image);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            List<ProductBase> cp = JsonConvert.DeserializeObject<List<ProductBase>>(res.data.ToString());
            List<ProductBase> productsWNI = new List<ProductBase>();
            foreach (ProductBase product in cp)
            {
                if(!File.Exists(Path.Combine(VM.imagePath,product.Image)))
                    productsWNI.Add(product);
            }
            VM.SearchProducts = productsWNI;
            VM.GettingData = false;
        }
    }
}
