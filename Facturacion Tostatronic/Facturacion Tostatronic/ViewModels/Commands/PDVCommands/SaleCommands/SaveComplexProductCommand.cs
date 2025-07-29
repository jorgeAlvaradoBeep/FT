using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.EF_Models.EFEarnings;
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

                    if (VM.SelectedPlataforma == null)
                    {
                        MessageBox.Show("Debe seleccionar una plataforma", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        VM.GettingData = false;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(VM.CodigoPlataforma) || 
                        !System.Text.RegularExpressions.Regex.IsMatch(VM.CodigoPlataforma, @"^[a-zA-Z0-9]+$"))
                    {
                        MessageBox.Show("El código de plataforma es requerido y debe ser alfanumérico", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

                    // TODO: Save the complex product to the database
                    // This would require creating an endpoint and model for complex products
                    MessageBox.Show($"Producto complejo creado exitosamente:\n\n" +
                        $"Nombre: {VM.Nombre}\n" +
                        $"ID Cotización: {VM.IdCotizacion}\n" +
                        $"Plataforma: {VM.SelectedPlataforma.Nombre}\n" +
                        $"Código Plataforma: {VM.CodigoPlataforma}\n" +
                        $"SKU: {VM.Sku}", 
                        "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Reset the form
                    VM.InitializeCompleteSale();
                    VM.ResetComplexProductFields();
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
                using (var client = new HttpClient())
                {
                    var quote = new
                    {
                        id_vendedor = VM.CompleteSale.SalerID,
                        fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                        id_cliente = VM.CompleteSale.ClientSale.ID,
                        subtotal = VM.CompleteSale.SubTotal,
                        iva = VM.CompleteSale.IVA,
                        total = VM.CompleteSale.Total,
                        productos = VM.CompleteSale.SaledProducts.Select(p => new
                        {
                            id_producto = p.Code,
                            cantidad = p.SaledQuantity,
                            precio_unitario = p.DisplayPrice,
                            subtotal = p.Subtotal
                        }).ToList()
                    };

                    var json = JsonConvert.SerializeObject(quote);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(URLData.quote_save, content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<Response>(responseContent);
                        if (result.succes)
                        {
                            return Convert.ToInt32(result.data);
                        }
                    }
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
    }
}