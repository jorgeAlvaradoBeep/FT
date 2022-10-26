using Bukimedia.PrestaSharp;
using Bukimedia.PrestaSharp.Entities;
using Bukimedia.PrestaSharp.Factories;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
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

                ProductFactory ArticuloFactory = new ProductFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
                product updateProduct = await ArticuloFactory.GetAsync(long.Parse(VM.Product.PrestashopID));

                //Actualizamos precio de producto directamente en la base de datos sin WS
                Response res = await WebService.InsertData(VM.Product, "https://tostatronic.com/store/NewWBST/updateProductSinglePrice.php");
                if (!res.succes)
                {
                    errMSG += "Error: " + res.message + Environment.NewLine + "No se actualizaron precios";
                    VM.GettingData = false;
                }

                //Omitimos la actualización por WEBService nativo para cambiarlo por webservice propio
                //updateProduct.wholesale_price = Convert.ToDecimal(VM.Product.BuyPrice);
                //updateProduct.price = Convert.ToDecimal(VM.Product.PublicPrice);

                //try
                //{
                //    await ArticuloFactory.UpdateAsync(updateProduct);
                //    messages += "Exito al actualizar precios en la pagina" + Environment.NewLine;
                //}
                //catch (PrestaSharpException e)
                //{
                //    errMSG += "Error al actualizar precios en la pagina: " + e.ResponseErrorMessage + Environment.NewLine;
                //}

                // Aqui actualizamos las cantidades en prestashop
                StockAvailableFactory stockAvailableFactory = new StockAvailableFactory(URLData.psBaseUrl, URLData.psAccount, URLData.psPassword);
                long stockAvailableId = updateProduct.associations.stock_availables[0].id;
                stock_available myStockAvailable = stockAvailableFactory.Get(stockAvailableId);
                myStockAvailable.quantity = (int)VM.Product.Existence; // Number of available products
                myStockAvailable.out_of_stock = 2; // Must enable orders

                try
                {
                    await stockAvailableFactory.UpdateAsync(myStockAvailable);
                    messages += "Exito al actualizar cantidades en la pagina" + Environment.NewLine;
                }
                catch (PrestaSharpException e)
                {
                    errMSG += "Error al actualizar cantidades en la pagina: " + e.ResponseErrorMessage + Environment.NewLine;
                }

                //Hacemos el procesod e actualizar los precios especificos en PS
                messages += await SetSpecificPrices();

                VM.GettingData = false;
                MessageBox.Show("Exitos: " + Environment.NewLine + messages + "Errores" + Environment.NewLine + errMSG);
                VM.Initialize();
                
            }

            
            

        }

        List<SpecificPrice> GetSpecificPrices()
        {
            float minPricePage = VM.Product.MinimumPrice * 1.06f;
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
