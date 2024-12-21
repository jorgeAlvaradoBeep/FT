using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands.LoadNewPricesCommands
{
    public class UpdateProductInfoCommand : ICommand
    {
        public UpdateProductNOVM VM { get; set; }

        public UpdateProductInfoCommand(UpdateProductNOVM vm)
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
            if (VM.ProductoAActualizar.MinimumQuantity <= 0)
            {
                MessageBox.Show($"Error: La cantidad minima no puede ser 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (VM.ProductoAActualizar.BuyPrice <= 0)
            {
                MessageBox.Show($"Error: El precio de compra no puede ser 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (VM.ProductoAActualizar.PublicPrice <= VM.ProductoAActualizar.DistributorPrice)
            {
                MessageBox.Show($"Error: El precio publico no puede ser menor o igual al PD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (VM.ProductoAActualizar.DistributorPrice <= VM.ProductoAActualizar.MinimumPrice)
            {
                MessageBox.Show($"Error: El precio distribuidor no puede ser menor o igual al PM", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (VM.ProductoAActualizar.MinimumPrice <= VM.ProductoAActualizar.BuyPrice)
            {
                MessageBox.Show($"Error: El precio minimo no puede ser menor o igual al precio de costo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GettingData = true;

            Response res = await WebService.InsertData(VM.ProductoAActualizar, URLData.product_update);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            string messages = string.Empty;
            messages += $"Producto {VM.ProductoAActualizar.Name} actualizado en la base de datos local{Environment.NewLine}";
            string errMSG = string.Empty;
            WooCommerceUpdateSimple updatedProduct = new WooCommerceUpdateSimple();
            updatedProduct.regular_price = VM.ProductoAActualizar.PublicPrice.ToString();
            updatedProduct.stock_quantity = (int)VM.ProductoAActualizar.Existence;
            decimal d;
            foreach (SpecificPrice sP in VM.ProductoAActualizar.SpecificPrices)
            {
                d = Decimal.Round((decimal)sP.Price, 2);
                updatedProduct.tiered_pricing_fixed_rules.Add((int)sP.Quantity, d);
            }
            string json = JsonConvert.SerializeObject(updatedProduct,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            IRestResponse res2;
            if (VM.ProductoAActualizar.WooParentID != 0)
                res2 = await WebService.ModifyDataWooCommercer(URLData.wcProducts + $"/{VM.ProductoAActualizar.WooParentID}/variations/{VM.ProductoAActualizar.WooID}", json);
            else
                res2 = await WebService.ModifyDataWooCommercer(URLData.wcProducts + $"/{VM.ProductoAActualizar.WooID}", json);
            if (!res2.IsSuccessful)
            {
                errMSG = $"Error al actualizar el producto {VM.ProductoAActualizar.Name} en la pagina WEB" +
                    $"{Environment.NewLine}Error: {res2.ErrorMessage}";
            }
            else
            {
                messages += "Producto actualizado en la pagina WEB" + Environment.NewLine;
            }

            //messages += await SetSpecificPrices();

            VM.GettingData = false;
            VM.ProductoAActualizar = new ProductCompleteNewArrival();
            MessageBox.Show("Exitos: " + Environment.NewLine + messages + "Errores:" + Environment.NewLine + errMSG);
        }
    }
}
