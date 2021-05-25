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
using Telerik.Windows.Controls.GridView;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SearchProductFromSalecommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SearchProductFromSalecommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var row = parameter as GridViewRow;
            ProductSaleSaled item = (ProductSaleSaled)row.DataContext;
            VM.GettingData = true;
            Response res = await WebService.GetData("product_id", item.Code, URLData.search_product_localization);
            VM.GettingData = false;
            if (res.succes)
            {
                MessageBox.Show($"El producto lo puede localizar en:{Environment.NewLine}{res.message}", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
                MessageBox.Show($"Error al consultar: {Environment.NewLine}{res.message.ToString()}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

        }
    }
}
