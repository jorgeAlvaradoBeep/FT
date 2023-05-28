using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SaleSelectedSaleCommand : ICommand
    {
        public SeeSalesVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaleSelectedSaleCommand(SeeSalesVM vm)
        {
            VM = vm;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            CompleteSaleEF item = (CompleteSaleEF)parameter;
            VM.GettinData = true;
            Response r = await WebService.GetData("sale_number", item.idVenta.ToString(), URLData.products);

            if (!r.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("No existen registros de esa venta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var t = r.data;
            List<Product> products = ((JArray)t).Select(x => new Product
            {
                idProduct = (string)x["idProduct"],
                name = (string)x["name"],
                quantity = (string)x["quantity"],
                priceAtMoment = (string)x["priceAtMoment"],
                satCode = (string)x["satCode"]
            }).ToList();
            decimal impuesto = Convert.ToDecimal(item.impuesto);
            VM.ProductsOfSale = products;
            float sub=0;
            foreach (var product in products)
            {
                sub += float.Parse(product.SubTotal);
            }
            VM.SubTotal = sub;
            VM.Iva = (float.Parse(impuesto.ToString())-1)* sub;
            VM.Total = VM.SubTotal + VM.Iva;
            VM.GettinData = false;
        }
    }
}
