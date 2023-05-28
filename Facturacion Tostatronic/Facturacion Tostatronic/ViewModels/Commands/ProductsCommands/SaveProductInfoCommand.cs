using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class SaveProductInfoCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeProductVM VM { get; set; }
        public SaveProductInfoCommand(SeeProductVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            var row = parameter as GridViewRow;
            UpdateProductM cC = (UpdateProductM)row.DataContext;
            if (cC != null)
            {
                if (string.IsNullOrEmpty(cC.Nombre))
                {
                    MessageBox.Show("El nombre del producto no puede ir vacio", "Error De dato", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                VM.GettingData = true;
                Response res = await WebService.ModifyData(cC, URLData.editProductsNet + cC.Codigo);
                if (!res.succes)
                {
                    MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    return;
                }
                else
                {
                    //Vamos a actualizar los datos en la pagina
                    //Primero Validamos si el producto es variacion o simple

                    //Nueva sección de actualización de producto en la pagína.
                    List<Fields> fields = new List<Fields>();
                    Fields nf = new Fields()
                    {
                        FieldName = "sku",
                        FieldValue = cC.Codigo
                    };
                    fields.Add(nf);
                    var res2 = await WebService.GetSingleDataWooCommercer(URLData.wcProducts, fields);
                    if (!res2.IsSuccessful)
                    {
                        MessageBox.Show($"Error al traer el producto {cC.Nombre} de la pagina WEB" +
                            $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }
                    else
                    {

                        List<WooCommerceProduct> aux = JsonConvert.DeserializeObject<List<WooCommerceProduct>>(res2.Content.ToString());
                        if (aux == null)
                        {
                            MessageBox.Show($"Error al traer el producto {cC.Nombre} de la pagina WEB" +
                            $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            VM.GettingData = false;
                            return;
                        }
                        if (aux.Count == 0)
                        {
                            MessageBox.Show($"Error al traer el producto {cC.Nombre} de la pagina WEB" +
                            $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            VM.GettingData = false;
                            return;
                        }
                        WooCommerceProduct product = aux[0];
                        WooCommerceUpdateSimple updatedProduct = new WooCommerceUpdateSimple();
                        updatedProduct.regular_price = cC.PrecioPublico.ToString();
                        updatedProduct.stock_quantity = (int)cC.Existencia;
                        string json = JsonConvert.SerializeObject(updatedProduct,
                                    Newtonsoft.Json.Formatting.None,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    });
                        if (product.Parent_id != 0)
                            res2 = await WebService.ModifyDataWooCommercer(URLData.wcProducts + $"/{product.Parent_id}/variations/{product.Id}", json);
                        else
                            res2 = await WebService.ModifyDataWooCommercer(URLData.wcProducts + $"/{product.Id}", json);
                        if (!res2.IsSuccessful)
                        {
                            MessageBox.Show($"Error al actualizar el producto {cC.Nombre} en la pagina WEB" +
                                $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            MessageBox.Show("Prodcuto Actualizado con exito", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    VM.GettingData = false;

                }
            }
        }
    }
}
