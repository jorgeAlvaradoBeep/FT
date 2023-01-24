using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Facturacion_Tostatronic.Views.PDV.Quotes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class GetQuoteCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SaleVM VM { get; set; }
        SearchQuote sQ;
        public GetQuoteCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if(VM.CompleteSale.SaledProducts.Count>0)
            {
                MessageBox.Show("Hay una venta actual." + Environment.NewLine +
                    "Antes de cargar una cotización, termine su venta actual o cancelela");
                return;
            }
            sQ = new SearchQuote();
            sQ.Closing += getInfoForClosing;
            sQ.ShowDialog();

        }

        private async void getInfoForClosing(object sender, CancelEventArgs e)
        {
            sQ.Closing -= getInfoForClosing;
            Quotes quote = (Quotes)Application.Current.Properties["SelectedQuote"];
            Application.Current.Properties["SelectedQuote"] = null;
            if (quote!=null)
            {
                ClientSale cSQ = new ClientSale();
                cSQ.ID = quote.id_cliente;
                cSQ.RFC = quote.rfc;
                cSQ.Name = quote.CompleteName;
                cSQ.ClientType = quote.id_tipo_cliente;
                VM.CompleteSale.ClientSale = cSQ;
                VM.CompleteSale.PriceType = VM.CompleteSale.ClientSale.ClientType;

                //Aqui pedimos los productos 

                VM.GettingData = true;

                Response res = await WebService.GetDataNode(URLData.productsOfQuote, quote.id_cotizacion.ToString());
                VM.GettingData = false;

                if (res.succes)
                {
                    var resultInfo = JsonConvert.DeserializeObject<dynamic>(res.data.ToString());
                    ObservableCollection<ProductSaleSaled> productList = new ObservableCollection<ProductSaleSaled>();
                    ProductSaleSaled ps;
                    foreach (var aux in resultInfo)
                    {
                        int cc =aux["cantidad_cotizada"];
                        ps = new ProductSaleSaled();
                        ps.Code = aux["id_producto"];
                        ps.Name = aux["nombre"];
                        ps.Existence = aux["existencia"];
                        ps.MinimumPrice = aux["precio_minimo"];
                        ps.DistributorPrice = aux["precio_distribuidor"];
                        ps.PublicPrice = aux["precio_publico"];
                        if(aux["precio_al_momento"] < (ps.MinimumPrice / 1.17))
                        {
                            ps.DisplayPrice = aux["precio_minimo"];
                        }
                        else
                            ps.DisplayPrice = aux["precio_al_momento"];
                        ps.SaledQuantity = aux["cantidad_cotizada"];
                        productList.Add(ps);
                    }
                    VM.CompleteSale.SaledProducts = productList;
                    if (quote.impuesto > 1)
                        VM.CompleteSale.NeedFactura = true;
                    else
                        VM.CompleteSale.GetSubtotal();
                }
                else
                    MessageBox.Show($"Error al consultar: {res.message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                
                
            }
        }
    }
}
