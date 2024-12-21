using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EF_Orders;
using Facturacion_Tostatronic.Models.EF_Models.EFProduct;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using Facturacion_Tostatronic.Views.Pages.Products;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.LoadNewPricesCommands
{
    public class SelectedProductToUpdateCommand : ICommand
    {
        public LoadNewPricesVM VM { get; set; }

        public SelectedProductToUpdateCommand(LoadNewPricesVM vm)
        {
            VM = vm;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            ProductCompleteNewArrival p = (ProductCompleteNewArrival)parameter;
            if (p == null)
                return;
            VM.IsBusy = true;
            EFProduct updateProductMs;
            Response rmp = await WebService.GetDataNode(URLData.getProductsNet,p.Code);
            if (rmp.succes)
            {
                updateProductMs = await Task.Run(() => JsonConvert.DeserializeObject<EFProduct>(rmp.data.ToString()));
            }
            else
            {
                MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
                return;
            }
            if (updateProductMs == null)
            {
                //Aqui se llamara a la ventana de agregar nuevo producto
                MessageBox.Show("Error al traer la información solicitada", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
                return;
            }
            p.OldStock = (int)updateProductMs.existencia;
            p.Existence = p.OldStock + p.NewStock;
            p.MinimumQuantity = updateProductMs.cantidadMinima;
            //Seccion para obtener la informcion de los precios
            List<Fields> fields = new List<Fields>();
            Fields nf = new Fields()
            {
                FieldName = "sku",
                FieldValue = p.Code
            };
            fields.Add(nf);
            var res2 = await WebService.GetSingleDataWooCommercer(URLData.wcProducts, fields);
            if (!res2.IsSuccessful)
            {
                MessageBox.Show($"Error al traer el producto {p.Name} de la pagina WEB" +
                    $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.IsBusy = false;
                return;
            }
            else
            {

                List<WooCommerceProduct> aux = JsonConvert.DeserializeObject<List<WooCommerceProduct>>(res2.Content.ToString());
                if (aux == null)
                {
                    MessageBox.Show($"Error al traer el producto {p.Name} de la pagina WEB" +
                    $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.IsBusy = false;
                    return;
                }
                if (aux.Count == 0)
                {
                    MessageBox.Show($"Error al traer el producto {p.Name} de la pagina WEB" +
                    $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.IsBusy = false;
                    return;
                }
                WooCommerceProduct product = aux[0];
                p.WooID = product.Id;
                p.WooParentID = product.Parent_id;
                Dictionary<int, decimal> root;
                bool isTired = false;
                try
                {
                    root = JsonConvert.DeserializeObject<Dictionary<int, decimal>>(product.tiered_pricing_fixed_rules.ToString());
                    isTired = true;
                }
                catch (Exception)
                {
                    root = null;
                    isTired = false;
                }
                if (!isTired)
                {
                    p.SpecificPrices = GetSpecificPrices(p);
                    VM.CurrentPage = new UpdateProductNOVM(p);
                    VM.IsBusy = false;
                    return;
                }
                if (root != null)
                {
                    p.SpecificPrices = root
                    .Select(entry => new SpecificPrice
                    {
                        Quantity = entry.Key,   // El Key es int
                        Price = 0     // El Value es decimal
                    })
                    .ToList();
                                }
                }
            p.SpecificPrices = GetSpecificPrices(p);
            VM.CurrentPage = new UpdateProductNOVM(p);
            VM.IsBusy = false;
        }
        List<SpecificPrice> GetSpecificPrices(ProductCompleteNewArrival product)
        {
            float minPricePage = product.MinimumPrice / 0.94f;
            float div = (product.PublicPrice - minPricePage) / 5f;
            if(product.SpecificPrices != null)
            {
                if(product.SpecificPrices.Count == 5)
                {
                    float price = product.PublicPrice;
                    foreach (var item in product.SpecificPrices)
                    {
                        price = price - div;
                        item.Price = price;
                    }
                }
                return product.SpecificPrices;
            }
            float qtyDiv = product.Existence / 5f;
            List<SpecificPrice> specPrices = new List<SpecificPrice>();
            specPrices.Add(new SpecificPrice(qtyDiv, product.PublicPrice - div));
            specPrices.Add(new SpecificPrice(qtyDiv * 2, product.PublicPrice - div * 2));
            specPrices.Add(new SpecificPrice(qtyDiv * 3, product.PublicPrice - div * 3));
            specPrices.Add(new SpecificPrice(qtyDiv * 4, product.PublicPrice - div * 4));
            specPrices.Add(new SpecificPrice(qtyDiv * 5, product.PublicPrice - div * 5));
            return specPrices;
        }
    }
}
