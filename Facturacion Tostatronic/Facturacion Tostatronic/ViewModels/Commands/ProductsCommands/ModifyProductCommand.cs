using Facturacion_Tostatronic.Models.Clients;
using Facturacion_Tostatronic.Models;
using Facturacion_Tostatronic.Services;
using Facturacion_Tostatronic.ViewModels.Clients;
using Facturacion_Tostatronic.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using Facturacion_Tostatronic.Models.Products;

namespace Facturacion_Tostatronic.ViewModels.Commands.ProductsCommands
{
    public class ModifyProductCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public SeeProductVM VM { get; set; }
        public ModifyProductCommand(SeeProductVM vM)
        {
            VM = vM;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            UpdateProductM cC = (UpdateProductM)((RadTabItem)parameter).DataContext;
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
                    VM.GettingData = false;
                    MessageBox.Show($"producto Modificado Con Exito", "Correcto", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
