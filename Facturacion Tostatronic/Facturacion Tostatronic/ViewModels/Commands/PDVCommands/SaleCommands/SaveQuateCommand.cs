using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Models.Sales;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Sales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.PDVCommands.SaleCommands
{
    public class SaveQuateCommand : ICommand
    {
        public SaleVM VM { get; set; }
        public event EventHandler CanExecuteChanged;
        public SaveQuateCommand(SaleVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }
        public async void Execute(object parameter)
        {
            if (VM.CompleteSale.SaledProducts.Count == 0)
            {
                MessageBox.Show("Carrito vacio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GettingData = true;

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
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            else
            {
                VM.CompleteSale.IDSale = int.Parse(res.message);
                MessageBox.Show($"Cotizacion Guardada con exito con el numero: {VM.CompleteSale.IDSale}", "Exitto", MessageBoxButton.OK, MessageBoxImage.Information);

                VM.GettingData = false;
                //VM.InitializeCompleteSale();//En caso de querer incializar la cotizacion.
            }
        }
        void printPDF(string pathF, string name)
        {
           

        }
        void EndSale()
        {
           
        }
    }
}
