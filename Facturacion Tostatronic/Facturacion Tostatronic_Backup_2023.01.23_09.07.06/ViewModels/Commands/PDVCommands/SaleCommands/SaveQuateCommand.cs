using Facturacion_Tostatronic.Models;
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
            VM.CompleteSale.SearchedProducts = new List<Models.Products.ProductSaleSearch>();
            Response res = await WebService.InsertData(VM.CompleteSale, URLData.quote_save);
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
