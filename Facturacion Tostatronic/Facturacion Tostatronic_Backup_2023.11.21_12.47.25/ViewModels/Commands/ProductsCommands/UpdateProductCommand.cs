using Bukimedia.PrestaSharp;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
using GalaSoft.MvvmLight.Threading;
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
    public class UpdateProductCommand : ICommand
    {
        public DisscountPricesVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public UpdateProductCommand(DisscountPricesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (VM.DataUpdated)
            {
                if (VM.Product.MinimumQuantity<=0)
                {
                    MessageBox.Show($"Error: La cantidad minima no puede ser 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }else if (VM.Product.BuyPrice <= 0)
                {
                    MessageBox.Show($"Error: El precio de compra no puede ser 0", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }else if (VM.Product.PublicPrice <= VM.Product.DistributorPrice)
                {
                    MessageBox.Show($"Error: El precio publico no puede ser menor o igual al PD", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }else if (VM.Product.DistributorPrice <= VM.Product.MinimumPrice)
                {
                    MessageBox.Show($"Error: El precio distribuidor no puede ser menor o igual al PM", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (VM.Product.MinimumPrice <= VM.Product.BuyPrice)
                {
                    MessageBox.Show($"Error: El precio minimo no puede ser menor o igual al precio de costo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                VM.GettingData = true;
            
                Response res = await WebService.InsertData(VM.Product, URLData.product_update);
                if (!res.succes)
                {
                    MessageBox.Show("Error: " + res.message + Environment.NewLine + "No se encontrarion coincidencias", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    return;
                }
                VM.Product.SpecificPrices = GetSpecificPrices();
                VM.DataUpdated = false;
                VM.SpecificProductVisibility = Visibility.Visible;
                VM.GettingData = false;
            }
            else
            {
                VM.GettingData = true;
                string messages = string.Empty;
                string errMSG = string.Empty;
                List<Fields> fields = new List<Fields>();
                Fields nf = new Fields()
                {
                    FieldName = "sku",
                    FieldValue = VM.Product.Code
                };
                fields.Add(nf);
                var res2 = await WebService.GetSingleDataWooCommercer(URLData.wcProducts, fields);
                if (!res2.IsSuccessful)
                {
                    MessageBox.Show($"Error al traer el producto {VM.Product.Name} de la pagina WEB" +
                        $"{Environment.NewLine}Error: {res2.ErrorMessage}","Error",MessageBoxButton.OK, MessageBoxImage.Error);
                    VM.GettingData = false;
                    return;
                }
                else
                {

                    List<WooCommerceProduct> aux = JsonConvert.DeserializeObject<List<WooCommerceProduct>>(res2.Content.ToString());
                    if(aux==null)
                    {
                        MessageBox.Show($"Error al traer el producto {VM.Product.Name} de la pagina WEB" +
                        $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }
                    if(aux.Count==0)
                    {
                        MessageBox.Show($"Error al traer el producto {VM.Product.Name} de la pagina WEB" +
                        $"{Environment.NewLine}Error: {res2.ErrorMessage}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }
                    WooCommerceProduct product = aux[0];
                    WooCommerceUpdateSimple updatedProduct = new WooCommerceUpdateSimple();
                    updatedProduct.regular_price = VM.Product.PublicPrice.ToString();
                    updatedProduct.stock_quantity = (int)VM.Product.Existence;
                    updatedProduct.tiered_pricing_fixed_rules = string.Empty;
                    decimal d;
                    foreach (SpecificPrice sP in VM.Product.SpecificPrices)
                    {
                        d = Decimal.Round((decimal)sP.Price, 2);
                        updatedProduct.tiered_pricing_fixed_rules += $"{sP.Quantity}:{(float)d},";
                    }
                    updatedProduct.tiered_pricing_fixed_rules= updatedProduct.tiered_pricing_fixed_rules.Remove(updatedProduct.tiered_pricing_fixed_rules.Length-1,1);
                    string json = JsonConvert.SerializeObject(updatedProduct,
                                    Newtonsoft.Json.Formatting.None,
                                    new JsonSerializerSettings
                                    {
                                        NullValueHandling = NullValueHandling.Ignore
                                    });
                    if(product.Parent_id!=0)
                        res2 = await WebService.ModifyDataWooCommercer(URLData.wcProducts + $"/{product.Parent_id}/variations/{product.Id}", json);
                    else
                        res2 = await WebService.ModifyDataWooCommercer(URLData.wcProducts+$"/{product.Id}", json);
                    if (!res2.IsSuccessful)
                    {
                        errMSG = $"Error al actualizar el producto {VM.Product.Name} en la pagina WEB" +
                            $"{Environment.NewLine}Error: {res2.ErrorMessage}";
                    }
                    else
                    {
                        messages = "Prodcuto Actualizado con exito";
                    }
                }
                
                //messages += await SetSpecificPrices();

                VM.GettingData = false;
                MessageBox.Show("Exitos: " + Environment.NewLine + messages + "Errores" + Environment.NewLine + errMSG);
                VM.Initialize();
                
            }

            
            

        }

        List<SpecificPrice> GetSpecificPrices()
        {
            float minPricePage = VM.Product.MinimumPrice / 0.94f;
            float div = (VM.Product.PublicPrice - minPricePage) / 5f;
            float qtyDiv = VM.Product.Existence / 5f;
            List<SpecificPrice> specPrices = new List<SpecificPrice>();
            specPrices.Add(new SpecificPrice(qtyDiv, VM.Product.PublicPrice - div));
            specPrices.Add(new SpecificPrice(qtyDiv * 2, VM.Product.PublicPrice - div * 2));
            specPrices.Add(new SpecificPrice(qtyDiv * 3, VM.Product.PublicPrice - div * 3));
            specPrices.Add(new SpecificPrice(qtyDiv * 4, VM.Product.PublicPrice - div * 4));
            specPrices.Add(new SpecificPrice(qtyDiv * 5, VM.Product.PublicPrice - div * 5));
            return specPrices;
        }

        public async Task<string> SetSpecificPrices()
        {
            specific_price specialPriceRule = null;
            List<specific_price> specificPrices;
            SpecificPriceFactory specificPriceFactory = new SpecificPriceFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
            specificPrices = await GetSpecialPriceRule(VM.Product.PrestashopID, specificPriceFactory);
            string result = string.Empty;

            if (specificPrices.Count > 0)
            {
                foreach (specific_price sp in specificPrices)
                {
                    
                    try
                    {
                        await specificPriceFactory.DeleteAsync((long)sp.id);
                    }
                    catch (PrestaSharpException e)
                    {
                        result+= "Error al actualizar precios especificos: ." + e.ResponseErrorMessage + Environment.NewLine;
                    }
                }
                
            }
            specificPrices = new List<specific_price>();
            foreach (SpecificPrice sp in VM.Product.SpecificPrices)
            {
                specialPriceRule = new specific_price
                {
                    id_product = long.Parse(VM.Product.PrestashopID),
                    reduction = 0,
                    reduction_tax = 1,
                    reduction_type = "amount",
                    id_shop = 1,
                    id_shop_group = 0,
                    id_product_attribute = 0,
                    id_cart = 0,
                    id_currency = 0,
                    id_country = 0,
                    id_group = 0,
                    id_customer = 0,
                    from_quantity = (int)sp.Quantity,
                    price = Convert.ToDecimal(sp.Price),
                };
                specificPrices.Add(specialPriceRule);
            }


            try
            {
                await specificPriceFactory.AddListAsync(specificPrices);
                return $"Precios Especificos Dados de alta {Environment.NewLine}";
            }
            catch (PrestaSharpException e)
            {
                return "Error al dar de alta los precios especificos: ." + e.ResponseErrorMessage + Environment.NewLine;
            }
        }

        private async Task<List<specific_price>> GetSpecialPriceRule(string productID, SpecificPriceFactory factory)
        {
            var filter = new Dictionary<string, string> { { "id_product", Convert.ToString(productID) } };
            var specialPriceRule = await factory.GetByFilterAsync(filter, null, null);
            return specialPriceRule;
        }
    }
}
