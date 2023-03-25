using Facturacion_Tostatronic.Models.EF_Models.EFSale;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Telerik.Windows.Controls.GridView;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;

namespace Facturacion_Tostatronic.ViewModels.Commands.SalesCommands
{
    public class SeeQuotePDFCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeQuatitionsVM VM { get; set; }
        public SeeQuotePDFCommand(SeeQuatitionsVM vm)
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
            CotizacionEF item = (CotizacionEF)row.DataContext;
            VM.GettinData = true;
            Response r = await WebService.GetDataNode(URLData.getProductsForQuote, item.idCotizacion.ToString());

            if (!r.succes)
            {
                if (!string.IsNullOrEmpty(r.message))
                    MessageBox.Show(r.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("No existen registros de esa venta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            item.ProductosDeCotizacions = JsonConvert.DeserializeObject<List<EFQuoteProduct>>(r.data.ToString());
            decimal impuesto = Convert.ToDecimal(item.impuesto);
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = System.IO.Path.Combine(path, "Tostatronic");
            await Task.Run(() =>
            {
                List<Product> auxL = new List<Product>();
                foreach(EFQuoteProduct product in item.ProductosDeCotizacions)
                {
                    Product auxProduct = new Product();
                    auxProduct.idProduct = product.IdProducto;
                    auxProduct.quantity = product.CantidadCotizada.ToString();
                    auxProduct.name = product.ProductoNavigation.nombre;
                    auxProduct.priceAtMoment = product.PrecioAlMomento.ToString();
                    auxProduct.SubTotal = (product.CantidadCotizada*product.PrecioAlMomento).ToString();
                    auxL.Add(auxProduct);

                }
                PDFService.CreatePDF(item.idCotizacion.ToString(), DateTime.Now.ToShortDateString(), auxL, item.idClienteNavigation, impuesto);
                PDFService.Ver($"{path}\\{item.idCotizacion}.pdf");
            });
            VM.GettinData = false;
        }
    }
}
