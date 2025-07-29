using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Products;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.ComplexProductsCommands
{
    public class CPSearchProductsCommand : ICommand
    {
        private ComplexProductsVM VM;

        public CPSearchProductsCommand(ComplexProductsVM vm)
        {
            VM = vm;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if (string.IsNullOrEmpty(VM.ProductCriterialSearch))
            {
                return;
            }

            VM.GettingData = true;
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(URLData.product_sale_search + VM.ProductCriterialSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var products = JsonConvert.DeserializeObject<List<ProductSaleSearch>>(responseContent);
                        
                        if (products != null && products.Count > 0)
                        {
                            VM.CompleteSale.SearchedProducts = products;
                            // Apply price type to products
                            foreach (var product in VM.CompleteSale.SearchedProducts)
                            {
                                switch (VM.CompleteSale.PriceType)
                                {
                                    case 0:
                                        product.DisplayPrice = product.DistributorPrice;
                                        break;
                                    case 1:
                                        product.DisplayPrice = product.DistributorPrice;
                                        break;
                                    case 2:
                                        product.DisplayPrice = product.PublicPrice;
                                        break;
                                    default:
                                        product.DisplayPrice = product.PublicPrice;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("No se encontraron productos", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al buscar productos: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                VM.GettingData = false;
            }
        }
    }
}