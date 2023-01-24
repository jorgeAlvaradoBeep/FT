using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Products;
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
    public class UpdateSucursalPriceCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SucursalPricesVM VM { get; set; }

        public UpdateSucursalPriceCommand(SucursalPricesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(VM.Product.SucursalQuantity == 0)
            {
                MessageBox.Show("Debe establecer una cantidad de producto", "Error",MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(VM.Product.SucursalPrice == 0)
            {
                MessageBox.Show("Debe establecer un precio de producto", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if(VM.Product.SucursalPrice <= VM.Product.Product.BuyPrice)
            {
                MessageBox.Show($"El precio de venta debe ser mayor al de compra: {VM.Product.Product.BuyPrice}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            VM.GettingData=true;
            Response res = await WebService.InsertData(VM.Product, URLData.addSucursalPrice);
            if (!res.succes)
            {
                MessageBox.Show("Error: " + res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                VM.GettingData = false;
                return;
            }
            else
            {
                VM.GettingData = false;
                MessageBox.Show("Exito al agregar: ", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                VM.Product = new Models.Products.SucursalProduct.SucursalProduct();
                //Aqui viene la parte para limpiar el formulario
            }
        }
    }
}
