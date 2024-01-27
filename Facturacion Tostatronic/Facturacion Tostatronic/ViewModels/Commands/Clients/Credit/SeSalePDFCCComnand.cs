using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients.CreditClientsCommands;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Telerik.Windows.Controls.GridView;
using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;

namespace Facturacion_Tostatronic.ViewModels.Commands.Clients.Credit
{
    public class SeSalePDFCCComnand : ICommand
    {
        public ClientsCreditVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SeSalePDFCCComnand(ClientsCreditVM vm)
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
            var p = (CreditSale)row.DataContext;
            if (p == null)
                return;
            VM.IsBusy = true;
            Response r = await WebService.GetDataNode(URLData.getOrderComplete, p.IDSale.ToString());

            if (!r.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("No existen registros de esa venta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            List<CompleteSaleWProductsEF> cS = JsonConvert.DeserializeObject<List<CompleteSaleWProductsEF>>(r.data.ToString());
            List<Product> products = new List<Product>();
            await Task.Run(() =>
            {
                foreach (EFSaleProducts pr in cS[0].productosDeVenta)
                {
                    products.Add(new Product
                    {
                        idProduct = pr.idProducto,
                        name = pr.productoNavigation.nombre,
                        quantity = pr.cantidadComprada.ToString(),
                        priceAtMoment = pr.precioAlMomento.ToString(),
                        satCode = string.Empty
                    });
                }
                decimal impuesto = Convert.ToDecimal(cS[0].impuesto);
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                path = System.IO.Path.Combine(path, "Tostatronic");
            
                PDFService.CreatePDF(p.IDSale.ToString(), DateTime.Now.ToShortDateString(), products, cS[0].idClienteNavigation, impuesto, "Venta");
                PDFService.Ver($"{path}\\{cS[0].idVenta}.pdf");
            });
            VM.IsBusy = false;
        }
    }
}
