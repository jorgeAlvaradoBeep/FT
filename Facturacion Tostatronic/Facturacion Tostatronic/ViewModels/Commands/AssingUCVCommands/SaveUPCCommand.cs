using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Facturacion_Tostatronic.ViewModels.Commands.AssingUCVCommands
{
    public class SaveUPCCommand : ICommand
    {
        private object Messagebox;

        public AssingUCVM VM { get; set; }
        public event EventHandler CanExecuteChanged;

        public SaveUPCCommand(AssingUCVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            if(string.IsNullOrEmpty(VM.SelectedProduct.universalCode))
            {
                MessageBox.Show("El codigo universal no puede ir vacio", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            WaitPlease wp = new WaitPlease();
            wp.Show();
            Response res = await WebService.InsertData(VM.SelectedProduct, URLData.product_complete_codes);
            if (res.succes)
            {
                MessageBox.Show("Exito al modificar el UPC", "Exito", MessageBoxButton.OK, MessageBoxImage.Information);
                VM.SelectedProduct = new Models.Products.ProductCodes();
            }
            else
                MessageBox.Show("Error al modificar: "+res.message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            wp.Close();
                
        }
    }
}
