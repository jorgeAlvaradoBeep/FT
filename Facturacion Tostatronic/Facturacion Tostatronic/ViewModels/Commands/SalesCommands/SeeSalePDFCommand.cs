using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Facturacion_Tostatronic.Models.Products;
using Telerik.Windows.Controls.GridView;
using Facturacion_Tostatronic.Models.CFDI;
using Facturacion_Tostatronic.Models.Clients;
using Newtonsoft.Json;
using System.Security.AccessControl;
using Facturacion_Tostatronic.Models.WareHouse;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SeeSalePDFCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeSalesVM VM { get; set; }
        public SeeSalePDFCommand(SeeSalesVM vm)
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
            Sale item = (Sale)row.DataContext;
            VM.GettinData = true;
            Response r = await WebService.GetData("sale_number", item.idSale.ToString(), URLData.products) ;
            Response rc = await WebService.GetData("rfc", item.rfc, URLData.clients);

            if (r.statusCode == 404)
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
            if (!rc.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("Error al extraer Cliente", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Client client = JsonConvert.DeserializeObject<Client>(rc.data.ToString());
            decimal impuesto = 1.16m;
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = System.IO.Path.Combine(path, "Tostatronic");
            await Task.Run(() =>
            {
                PDFService.CreatePDF(item.idSale.ToString(),DateTime.Now.ToShortDateString(),products,client,impuesto);
                PDFService.Ver($"{path}\\{item.idSale}.pdf");
            });
            VM.GettinData=false;
        }
    }
}
