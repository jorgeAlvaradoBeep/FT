using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Views;
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
    public class SearchProductCommand : ICommand
    {
        public AssingUCVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SearchProductCommand(AssingUCVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            WaitPlease pw = new WaitPlease();
            pw.Show();
            Response res = await WebService.GetData("idProduct", VM.ProductID, URLData.product_complete_codes);
            if (!res.succes)
            {
                MessageBox.Show(res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                pw.Close();
                return;
            }
            VM.Products = JsonConvert.DeserializeObject<List<ProductCodes>>(res.data.ToString());
            pw.Close();
        }
    }
}
