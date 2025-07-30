using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SaveComplexProductCommand : ICommand
    {
        private ComplexProductsVM VM;

        public SaveComplexProductCommand(ComplexProductsVM vm)
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
            try
            {
                VM.GettingData = true;

                // First check if we're in the quote creation phase
                if (!VM.ShowComplexProductFields)
                {
                    // Perform the same validations as the original SaveQuoteCommand
                    if (VM.CompleteSale.SaledProducts.Count == 0)
                    {
                        MessageBox.Show("No hay productos en la venta", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }

                    if (string.IsNullOrEmpty(VM.CompleteSale.ClientSale.RFC))
                    {
                        MessageBox.Show("No hay cliente seleccionado", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }

                    // Create the quote (similar to SaveQuoteCommand)
                    var result = await SaveQuote();
                    if (result > 0)
                    {
                        VM.IdCotizacion = result;
                        // Load platforms
                        await LoadPlatforms();
                        VM.ShowComplexProductFields = true;
                        MessageBox.Show($"Cotización creada exitosamente. ID: {result}\n\nAhora complete los datos del producto complejo.", 
                            "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Error al crear la cotización", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // We're in the complex product creation phase
                    // Validate the complex product fields
                    if (string.IsNullOrWhiteSpace(VM.Nombre))
                    {
                        MessageBox.Show("El nombre es requerido", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }

                    // Validate CodigoPlataforma only if it's provided
                    if (!string.IsNullOrWhiteSpace(VM.CodigoPlataforma) && VM.CodigoPlataforma.Length > 350)
                    {
                        MessageBox.Show("El código de plataforma no puede exceder 350 caracteres", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(VM.Sku) || !VM.Sku.StartsWith("CMPLX") ||
                        !System.Text.RegularExpressions.Regex.IsMatch(VM.Sku, @"^CMPLX[a-zA-Z0-9]*$"))
                    {
                        MessageBox.Show("El SKU es requerido, debe comenzar con 'CMPLX' y ser alfanumérico", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }

                    // Save the complex product to the database
                    var result = await SaveComplexProduct();
                    if (result)
                    {
                        var platformInfo = VM.SelectedPlataforma != null 
                            ? $"Plataforma: {VM.SelectedPlataforma.Nombre}\n" 
                            : "";
                        var codeInfo = !string.IsNullOrWhiteSpace(VM.CodigoPlataforma) 
                            ? $"Código Plataforma: {VM.CodigoPlataforma}\n" 
                            : "";
                            
                        MessageBox.Show($"Producto complejo creado exitosamente:\n\n" +
                            $"Nombre: {VM.Nombre}\n" +
                            $"ID Cotización: {VM.IdCotizacion}\n" +
                            platformInfo +
                            codeInfo +
                            $"SKU: {VM.Sku}", 
                            "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Reset the form
                        VM.InitializeCompleteSale();
                        VM.ResetComplexProductFields();
                    }
                    else
                    {
                        MessageBox.Show("Error al guardar el producto complejo", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                VM.GettingData = false;
            }
        }

        private async Task<int> SaveQuote()
        {
            try
            {
                // Convert CompleteSaleM to VentasOM format
                var ventasOM = new VentasOM();
                ventasOM.IDSale = 1; // Always 1 as specified
                ventasOM.ClientSale = new Cliente();
                
                if (VM.CompleteSale.ClientSale != null)
                {
                    ventasOM.ClientSale.IdCliente = VM.CompleteSale.ClientSale.ID;
                    ventasOM.ClientSale.IdTipoCliente = VM.CompleteSale.ClientSale.ClientType;
                    ventasOM.ClientSale.Nombres = VM.CompleteSale.ClientSale.Name ?? "";
                    ventasOM.ClientSale.Rfc = VM.CompleteSale.ClientSale.RFC ?? "";
                    ventasOM.ClientSale.CorreoElectronico = VM.CompleteSale.ClientSale.Mail ?? "";
                }
                
                ventasOM.ClientSale.ApellidoPaterno = "";
                ventasOM.ClientSale.ApellidoMaterno = "";
                ventasOM.ClientSale.Telefono = "";
                ventasOM.ClientSale.Domicilio = "";
                ventasOM.ClientSale.CodigoPostal = 0;
                ventasOM.ClientSale.Colonia = "";
                ventasOM.ClientSale.Celular = "";
                ventasOM.ClientSale.Descripcion = "";
                ventasOM.ClientSale.RegimenFiscal = "";
                ventasOM.ClientSale.Eliminado = false;
                
                ventasOM.PriceType = VM.CompleteSale.PriceType;
                ventasOM.SaledProducts = new List<ProductoDeVentaOM>();
                if (VM.CompleteSale.SaledProducts != null)
                {
                    foreach (var p in VM.CompleteSale.SaledProducts)
                    {
                        if (p != null)
                        {
                            var product = new ProductoDeVentaOM();
                            product.IdProducto = p.Code ?? "";
                            product.PrecioAlMomento = p.DisplayPrice;
                            product.CantidadComprada = p.SaledQuantity;
                            product.Descuento = 0;
                            ventasOM.SaledProducts.Add(product);
                        }
                    }
                }
                ventasOM.NeedFactura = VM.CompleteSale.NeedFactura;
                ventasOM.FechaDeVenta = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                ventasOM.SalerID = VM.CompleteSale.SalerID;

                Response res = await WebService.InsertData(ventasOM, URLData.saveQuoteNET);
                if (res.succes)
                {
                    return int.Parse(res.message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la cotización: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return 0;
        }

        private async Task LoadPlatforms()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(URLData.Plataforma);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Response>(responseContent);
                        if (result.succes)
                        {
                            var platforms = JsonConvert.DeserializeObject<List<Plataforma>>(result.data.ToString());
                            VM.Plataformas = new ObservableCollection<Plataforma>(platforms);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar las plataformas: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> SaveComplexProduct()
        {
            try
            {
                // Create the ProductosComplejos object
                var productosComplejos = new ProductosComplejos();
                productosComplejos.Id = 1; // Will be set by the server
                productosComplejos.Nombre = VM.Nombre;
                productosComplejos.IdCotizacion = VM.IdCotizacion;
                productosComplejos.Sku = VM.Sku;
                productosComplejos.IdPlataforma = VM.SelectedPlataforma?.Id;
                productosComplejos.CodigoPlataforma = VM.CodigoPlataforma;

                Response res = await WebService.InsertData(productosComplejos, URLData.ProductosComplejos);
                return res.succes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar el producto complejo: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}